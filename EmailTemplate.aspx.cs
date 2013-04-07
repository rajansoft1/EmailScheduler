using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using school;

public partial class Email : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindSheet();
            BindEmailTemplate();
        }
    }
    private void BindEmailTemplate()
    {
        if (Request.QueryString["q"] != null)
        {
            var emailTemplate = _dbScheduler.EmailTemplates.Single(p => p.Id == Convert.ToInt32(Request.QueryString["q"]));
            txtName.Text = emailTemplate.Name;
            txtSubjectt.Text = emailTemplate.Subject;
            txtMailTemplate.InnerText = emailTemplate.EmailText;
            ddlSheet.SelectedValue = emailTemplate.SheetId.ToString();
            btnUpdate.Visible = true;
            btnAdd.Visible = false;
        }
    }
    public string UploadHtmlFile()
    {
        string path = Server.MapPath("HtmlFiles") + "\\" + FileUpload1.FileName;
        FileUpload1.SaveAs(path);
        string fileData = file.getdata(path);
        return fileData;
    }
    public void BindSheet()
    {
        var sheets = from var in _dbScheduler.Sheets
                     select  var;
        ddlSheet.DataSource = sheets;
        ddlSheet.DataTextField = "Name";
        ddlSheet.DataValueField = "ID";
        ddlSheet.DataBind();
        ddlSheet.Items.Insert(0, "Select Sheet");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        var  emailTemplate = new EmailTemplate();
        emailTemplate.CreatedOn = DateTime.Now;
        if (!FileUpload1.HasFile)
        {
            emailTemplate.EmailText = txtMailTemplate.InnerText;
        }
        else
        {
            emailTemplate.EmailText = UploadHtmlFile();
        }
        emailTemplate.IsDelete = false;
        emailTemplate.Name = txtName.Text;
        emailTemplate.SheetId = Convert.ToInt32(ddlSheet.SelectedValue);
        emailTemplate.Subject = txtSubjectt.Text;
        _dbScheduler.EmailTemplates.InsertOnSubmit(emailTemplate);
        _dbScheduler.SubmitChanges();
        txtMailTemplate.InnerText = "";
        txtName.Text = "";
        txtSubjectt.Text = "";
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        var emailTemplate = _dbScheduler.EmailTemplates.Single(p => p.Id == Convert.ToInt32(Request.QueryString["q"]));
        if (!FileUpload1.HasFile)
        {
            emailTemplate.EmailText = txtMailTemplate.InnerText;
        }
        else
        {
            emailTemplate.EmailText = UploadHtmlFile();
        }
        emailTemplate.Name = txtName.Text;
        emailTemplate.SheetId = Convert.ToInt32(ddlSheet.SelectedValue);
        emailTemplate.Subject = txtSubjectt.Text;
        _dbScheduler.SubmitChanges();
        txtMailTemplate.InnerText = "";
        txtName.Text = "";
        txtSubjectt.Text = "";
        btnUpdate.Visible = false;
        btnAdd.Visible = true;
    }
}