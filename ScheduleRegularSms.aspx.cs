using System;
using System.Collections.Generic;
using System.Linq;

public partial class ScheduleRegularSms : System.Web.UI.Page
{
    private readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindGroup();
            BindSheet();
        }
    }
    public void BindGroup()
    {
        var groups = from var in _dbScheduler.ContactGroups
                     select var;
        ddlGroup.DataSource = groups;
        ddlGroup.DataTextField = "Name";
        ddlGroup.DataValueField = "Id";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, "Select Group");

    }
    public void BindSheet()
    {
        var sheets = from var in _dbScheduler.Sheets
                     select var;
        ddlSheet.DataSource = sheets;
        ddlSheet.DataTextField = "Name";
        ddlSheet.DataValueField = "ID";
        ddlSheet.DataBind();
        ddlSheet.Items.Insert(0,"Select Sheet");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (GetLastContactGroupMapingId() == 0)
        {
            var contactGroupSheetMaping = new ContactGroupSheetMaping();
            contactGroupSheetMaping.ContactGroupId = Convert.ToInt32(ddlGroup.SelectedValue);
            contactGroupSheetMaping.SheetId = Convert.ToInt32(ddlSheet.SelectedValue);
            contactGroupSheetMaping.IsDelete = false;
            contactGroupSheetMaping.CreatedOn = DateTime.Now;
            _dbScheduler.ContactGroupSheetMapings.InsertOnSubmit(contactGroupSheetMaping);
            _dbScheduler.SubmitChanges();
            int id = GetLastContactGroupMapingId();
            var contacts = from contact in _dbScheduler.Contacts
                           where
                               (contact.ContactGroupId == Convert.ToInt32(ddlSheet.SelectedValue) &&
                                contact.IsDelete == false)
                           select contact;
            var messages = from sms in _dbScheduler.SmsTemplates
                           where sms.IsDelete == false && sms.SheetId == Convert.ToInt32(ddlSheet.SelectedValue)
                           select sms;
            var smsSchedules = new List<SmsSchedule>();
            foreach (var message in messages)
            {
                
                    smsSchedules.Add(new SmsSchedule()
                        {
                            IsDelete = false,
                            ContactId = Convert.ToInt32(ddlGroup.SelectedValue),
                            IsRecursive = 0,
                            SmsTemplateId = message.Id,
                            IsSent = false,
                            MapingId = id,
                            SentOn = message.SendingDate

                        });
                    for (int i = 0; i < message.NoOfTimes; i++)
                    {

                        if (message.SendingDate != null)
                            smsSchedules.Add(new SmsSchedule()
                                {
                                    IsDelete = false,
                                    ContactId = Convert.ToInt32(ddlSheet.SelectedValue),
                                    IsRecursive = 0,
                                    SmsTemplateId = message.Id,
                                    IsSent = false,
                                    MapingId = id,
                                    SentOn = message.SendingDate.Value.AddDays(Convert.ToDouble(message.RepeatAfter)),
                                });
                    }
                
            }
            _dbScheduler.SmsSchedules.InsertAllOnSubmit(smsSchedules);
            _dbScheduler.SubmitChanges();
        }
    }

    private int GetLastContactGroupMapingId()
    {
        var contactGroupSheetMapings = from contactGroupSheetMaping in _dbScheduler.ContactGroupSheetMapings
                                       where contactGroupSheetMaping.ContactGroupId == Convert.ToInt32(ddlGroup.SelectedValue) && 
                                       contactGroupSheetMaping.SheetId == Convert.ToInt32(ddlSheet.SelectedValue)
                                       select contactGroupSheetMaping.Id;
        if (contactGroupSheetMapings.Any())
        {
            return  contactGroupSheetMapings.Max();
           
        }
        return 0;
    }
}