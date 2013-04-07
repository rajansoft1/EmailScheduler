using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web.UI.WebControls;
public partial class RecursiveEmailMaster : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindRecursiveSheet();
            BindSheet();
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
    public void BindSheet()
    {
        var sheets = from var in _dbScheduler.Sheets
                     select var;
        ddlSheetTemplate.DataSource = sheets;
        ddlSheetTemplate.DataTextField = "Name";
        ddlSheetTemplate.DataValueField = "ID";
        ddlSheetTemplate.DataBind();
        ddlSheetTemplate.Items.Insert(0, "Select Sheet");
    }
    public void BindRecursiveEmailTemplate()
    {
        var emailTemplates = _dbScheduler.EmailTemplates.Where(p => p.IsDelete == false && p.SheetId == Convert.ToInt32(ddlSheetTemplate.SelectedValue));
        ddlEmailTemplate.DataSource = emailTemplates;
        ddlEmailTemplate.DataTextField = "Name";
        ddlEmailTemplate.DataValueField = "Id";
        ddlEmailTemplate.DataBind();
        ddlEmailTemplate.Items.Insert(0, "Select Email Template");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
       
            var recursiveEmailTemplate = new RecursiveEmailTemplate();
            recursiveEmailTemplate.IsDelete = false;
            recursiveEmailTemplate.RecursiveSheetId = Convert.ToInt32(ddlSheet.SelectedValue);
            recursiveEmailTemplate.EmailTemplateId = Convert.ToInt16(ddlEmailTemplate.SelectedValue);
            recursiveEmailTemplate.DaysAfter = Convert.ToInt32(txtDaysAfter.Text);
            recursiveEmailTemplate.CreatedOn = DateTime.Now;
            _dbScheduler.RecursiveEmailTemplates.InsertOnSubmit(recursiveEmailTemplate);
            _dbScheduler.SubmitChanges();
            EmailTextBinding();
        
    }
   
    void EmailTextBinding()
    {
        var emailTexts = from emailTemplates in _dbScheduler.RecursiveEmailTemplates
                       join emailTemplate in _dbScheduler.EmailTemplates on emailTemplates.EmailTemplateId equals emailTemplate.Id
                       where (emailTemplates.IsDelete == false && emailTemplates.RecursiveSheetId == Convert.ToInt32(ddlSheet.SelectedValue))
                       select new { emailTemplates.Id,emailTemplates.DaysAfter,emailTemplate.Name };
        GridView1.DataSource = emailTexts;
        GridView1.DataBind();
    }
    protected void ddlSheet_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmailTextBinding();

    }
    protected void ddlSheetTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindRecursiveEmailTemplate();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var recursiveEmailTemplate = _dbScheduler.RecursiveEmailTemplates.Single(p => p.Id == Id);
            txtDaysAfter.Text = recursiveEmailTemplate.DaysAfter.ToString();
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();
        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.RecursiveEmailTemplates.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
           
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var recursiveEmailTemplate = _dbScheduler.RecursiveEmailTemplates.Single(p => p.Id == Id);
        recursiveEmailTemplate.DaysAfter = Convert.ToInt32(txtDaysAfter.Text);
        _dbScheduler.SubmitChanges();
        EmailTextBinding();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;
        

    }
    
}