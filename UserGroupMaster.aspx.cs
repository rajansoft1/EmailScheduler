using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserGroupMaster : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindGroup();
        }
    }
    public void BindGroup()
    {
        var groups = from var in _dbScheduler.ContactGroups
                     where var.IsDelete == false
                     select new { var.Id,var.Name};
        GridView1.DataSource = groups;
        GridView1.DataBind();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var contactGroup = new ContactGroup();
        contactGroup.Name = txtName.Text;
        contactGroup.OwnerId = Convert.ToInt32(Session["uid"]);
        contactGroup.IsDelete = false;
        contactGroup.CreatedOn = DateTime.Now;
        _dbScheduler.ContactGroups.InsertOnSubmit(contactGroup);
        _dbScheduler.SubmitChanges();
        txtName.Text = "";
        BindGroup();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var sheet = _dbScheduler.ContactGroups.Single(p => p.Id == Id);
            txtName.Text = sheet.Name;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();

        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.ContactGroups.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            BindGroup();
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var sheet = _dbScheduler.ContactGroups.Single(p => p.Id == Id);
        sheet.Name = txtName.Text;
        _dbScheduler.SubmitChanges();
        BindGroup();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;
        txtName.Text = "";

    }
}