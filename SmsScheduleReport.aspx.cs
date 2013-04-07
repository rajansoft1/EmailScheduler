using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SmsScheduleReport : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
   
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
        ddlSheet.Items.Insert(0, "Select Sheet");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SearchResults();
    }

    private void SearchResults()
    {
        if (ddlSheet.SelectedIndex > 0)
        {
            if (ddlGroup.SelectedIndex > 0)
            {
                var smsReports = from smsSchedule in _dbScheduler.SmsScheduleViews
                                 where smsSchedule.SheetId == Convert.ToInt32(ddlSheet.SelectedValue) && smsSchedule.IsDelete == false && smsSchedule.Expr1 == Convert.ToInt32(ddlGroup.SelectedValue)
                                 select new { smsSchedule.Id, smsSchedule.Name, smsSchedule.MessageText, smsSchedule.SentOn };
                GridView1.DataSource = smsReports;
                GridView1.DataBind();
            }
            else
            {
                var smsReports = from smsSchedule in _dbScheduler.SmsScheduleViews
                                 where smsSchedule.SheetId == Convert.ToInt32(ddlSheet.SelectedValue) && smsSchedule.IsDelete == false
                                 select new { smsSchedule.Id, smsSchedule.Name, smsSchedule.MessageText, smsSchedule.SentOn };
                GridView1.DataSource = smsReports;
                GridView1.DataBind();
            }
        }
        else
        {
            var smsReports = from smsSchedule in _dbScheduler.SmsScheduleViews
                             where smsSchedule.Expr1 == Convert.ToInt32(ddlGroup.SelectedValue) && smsSchedule.IsDelete == false
                             select new { smsSchedule.Id, smsSchedule.Name, smsSchedule.MessageText, smsSchedule.SentOn };
            GridView1.DataSource = smsReports;
            GridView1.DataBind();
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);

        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.SmsSchedules.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            SearchResults();
        }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        SearchResults();
    }
}