using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmailTemplateLits : System.Web.UI.Page
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
    protected void ddlSheet_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindEmails();
    }

    private void BindEmails()
    {
        try
        {
            var emailTemplates = from emailTemplate in _dbScheduler.EmailTemplates
                                 where emailTemplate.IsDelete == false && emailTemplate.SheetId == Convert.ToInt32(ddlSheet.SelectedValue)
                                 select new { emailTemplate.Id, emailTemplate.Name, emailTemplate.Subject };
            GridView1.DataSource = emailTemplates;
            GridView1.DataBind();
        }
        catch
        {
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            Response.Redirect("EmailTemplate.aspx?q=" + Id);

        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.EmailTemplates.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            BindEmails();
            
        }
    }
}