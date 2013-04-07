using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StartAfterMapping : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindRecursiveSheet();
            BindGroup();
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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            var getData = _dbScheduler.RecursiveContactGroupSheetMapings.Where(p => p.ContactGroupId == Convert.ToInt32(ddlGroup.SelectedValue) && p.IsDelete == false);
            if (!getData.Any())
            {

                RecursiveContactGroupSheetMaping recursiveContactGroupSheetMaping = new RecursiveContactGroupSheetMaping();
                recursiveContactGroupSheetMaping.ContactGroupId = Convert.ToInt32(ddlGroup.SelectedValue);
                recursiveContactGroupSheetMaping.IsDelete = false;
                recursiveContactGroupSheetMaping.RecursiveSheetId = Convert.ToInt32(ddlSheet.SelectedValue);
                _dbScheduler.RecursiveContactGroupSheetMapings.InsertOnSubmit(recursiveContactGroupSheetMaping);
                _dbScheduler.SubmitChanges();
                BindMapping();
            }
        }
        catch
        {

        }
    }
    public void BindMapping()
    {
        var mappings = from mapping in _dbScheduler.RecursiveContactGroupSheetMapings
                       join contactGroup in _dbScheduler.ContactGroups on mapping.ContactGroupId equals contactGroup.Id
                       join recursiveSheet in _dbScheduler.RecursiveSheets on mapping.RecursiveSheetId equals recursiveSheet.Id
                       where mapping.IsDelete == false && mapping.RecursiveSheetId == Convert.ToInt32(ddlSheet.SelectedValue)
                       select new { mapping.Id, contactGroup.Name
                           //, recursiveSheet.Name
                       };
        GridView1.DataSource = mappings;
        GridView1.DataBind();


    }
    protected void ddlSheet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindMapping();   
        }
        catch
        {
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.RecursiveContactGroupSheetMapings.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            BindMapping();
        }
    }
}