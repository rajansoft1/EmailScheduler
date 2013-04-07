using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web.UI.WebControls;

public partial class SmsMaster : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindRecursiveSheet();
        }
    }
    public void BindRecursiveSheet()
    {
        var sheets = from var in _dbScheduler.RecursiveSheets
                     select var;
        ddlSheet.DataSource = sheets;
        ddlSheet.DataTextField = "Name";
        ddlSheet.DataValueField = "ID";
        ddlSheet.DataBind();
        ddlSheet.Items.Insert(0, "Select Sheet");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
       
            var smsTemplate = new RecursiveSmsTemplate();
            smsTemplate.IsDelete = false;
            smsTemplate.RecursiveSheetId = Convert.ToInt32(ddlSheet.SelectedValue);
            smsTemplate.MessageText = txtSms.Text;
            smsTemplate.DaysAfter = Convert.ToInt32(txtDaysAfter.Text);
            _dbScheduler.RecursiveSmsTemplates.InsertOnSubmit(smsTemplate);
            _dbScheduler.SubmitChanges();
            SmsTextBinding();
        
    }
   
    void SmsTextBinding()
    {
        var smsTexts = from smsTemplate in _dbScheduler.RecursiveSmsTemplates
                       where (smsTemplate.IsDelete == false && smsTemplate.RecursiveSheetId == Convert.ToInt32(ddlSheet.SelectedValue))
                       select smsTemplate;
        GridView1.DataSource = smsTexts;
        GridView1.DataBind();
    }
    protected void ddlSheet_SelectedIndexChanged(object sender, EventArgs e)
    {
        SmsTextBinding();

    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var smsTemplate = _dbScheduler.RecursiveSmsTemplates.Single(p => p.Id == Id);
            txtSms.Text = smsTemplate.MessageText;
            txtDaysAfter.Text = smsTemplate.DaysAfter.ToString();
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();
        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.RecursiveSmsTemplates.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();

        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var smsTemplate = _dbScheduler.RecursiveSmsTemplates.Single(p => p.Id == Id);
        smsTemplate.IsDelete = false;
        smsTemplate.RecursiveSheetId = Convert.ToInt32(ddlSheet.SelectedValue);
        smsTemplate.MessageText = txtSms.Text;
        smsTemplate.DaysAfter = Convert.ToInt32(txtDaysAfter.Text);
        _dbScheduler.SubmitChanges();
        txtSms.Text = "";
        txtDaysAfter.Text = "";
        BindRecursiveSheet();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;


    }
}