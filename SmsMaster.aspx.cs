using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

public partial class SmsMaster : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindSheet();
        }
    }
    public void BindSheet()
    {
        var sheets = from var in _dbScheduler.Sheets
                     select var;
        ddlSheet.DataSource = sheets;
        ddlSheet.DataTextField = "Name";
        ddlSheet.DataValueField = "ID";
        ddlSheet.DataBind();
        ddlSheet.Items.Insert(0, "Select Sheet");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!FileUpload1.HasFile)
        {
            var smsTemplate = new SmsTemplate();
            smsTemplate.IsDelete = false;
            smsTemplate.SheetId = Convert.ToInt32(ddlSheet.SelectedValue);
            smsTemplate.MessageText = txtSms.Text;
            smsTemplate.NoOfTimes = Convert.ToInt32(txtNoOfTimes.Text);
            smsTemplate.RepeatAfter = Convert.ToInt32(txtRepeatAfter.Text);
            smsTemplate.SendingDate = Convert.ToDateTime(txtSendingDate.Text);
            _dbScheduler.SmsTemplates.InsertOnSubmit(smsTemplate);
            _dbScheduler.SubmitChanges();
            SmsTextBinding();
            txtSms.Text = "";
            txtSendingDate.Text = "";
            txtRepeatAfter.Text = "";
            txtNoOfTimes.Text = "";
        }
        else
        {
            try
            {
                string fPath = Server.MapPath("ExcelFolder") + "\\" + Session.SessionID + FileUpload1.FileName;
                FileUpload1.SaveAs(fPath);

                var ds = new DataSet();

                var da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fPath + "; Extended Properties='Excel 8.0;HDR=YES'");
                da.Fill(ds);
                var smsTemplates = new List<SmsTemplate>();

                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {

                            smsTemplates.Add(new SmsTemplate()
                                {
                                    SendingDate = Convert.ToDateTime(ds.Tables[0].Rows[i][0]),
                                    IsDelete = false,
                                    MessageText = Convert.ToString(ds.Tables[0].Rows[i][4]),
                                    RepeatAfter = Convert.ToInt32(getdays(ds.Tables[0].Rows[i][1].ToString())),
                                    NoOfTimes = Convert.ToInt32(ds.Tables[0].Rows[i][2]),
                                    SheetId = Convert.ToInt32(ddlSheet.SelectedValue)
                                });


                        }
                        catch (Exception ex)
                        {
                            Response.Write("<br>Their is some error in row " + (i + 1));

                        }
                    }
                    _dbScheduler.SmsTemplates.InsertAllOnSubmit(smsTemplates);
                    _dbScheduler.SubmitChanges();
                }
                else
                {



                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);

            }
        }
    }
    int getdays(string format)
    {
        string[] date = format.Split('-');
        int years = Convert.ToInt32(date[0]);
        DateTime newdate = DateTime.Now.AddDays(years);
        int x = Convert.ToInt32((newdate - DateTime.Now).TotalMinutes);
        return x;
    }
    void SmsTextBinding()
    {
        var smsTexts = from smsTemplate in _dbScheduler.SmsTemplates
                       where (smsTemplate.IsDelete == false && smsTemplate.SheetId == Convert.ToInt32(ddlSheet.SelectedValue))
                       select new { smsTemplate.Id,smsTemplate.MessageText , smsTemplate.NoOfTimes , smsTemplate.RepeatAfter};
        GridView1.DataSource = smsTexts;
        GridView1.DataBind();
    }
    protected void ddlSheet_SelectedIndexChanged(object sender, EventArgs e)
    {
        SmsTextBinding();

    }
    protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var sms = _dbScheduler.SmsTemplates.Single(p => p.Id == Id);
            txtNoOfTimes.Text = sms.NoOfTimes.ToString();
            txtRepeatAfter.Text = sms.RepeatAfter.ToString();
            txtSendingDate.Text = sms.SendingDate.ToString();
            ddlSheet.SelectedValue = sms.SheetId.ToString();
            txtSms.Text = sms.MessageText;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();
        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.Contacts.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            SmsTextBinding();
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var smsTemplate = _dbScheduler.SmsTemplates.Single(p => p.Id == Id);
        smsTemplate.SheetId = Convert.ToInt32(ddlSheet.SelectedValue);
        smsTemplate.MessageText = txtSms.Text;
        smsTemplate.NoOfTimes = Convert.ToInt32(txtNoOfTimes.Text);
        smsTemplate.RepeatAfter = Convert.ToInt32(txtRepeatAfter.Text);
        smsTemplate.SendingDate = Convert.ToDateTime(txtSendingDate.Text);
        _dbScheduler.SubmitChanges();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;
        SmsTextBinding();
        txtSms.Text = "";
        txtSendingDate.Text = "";
        txtRepeatAfter.Text = "";
        txtNoOfTimes.Text = "";
    }
    protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        SmsTextBinding();
    }
}