using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SheetMaster : System.Web.UI.Page
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
                     where var.IsDelete == false
                     select new { var.Id,var.Name};
        GridView1.DataSource = sheets;
        GridView1.DataBind();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var sheet = new Sheet();
        sheet.Name = txtName.Text;
        sheet.IsDelete = false;
        sheet.CreatedOn = DateTime.Now;
        sheet.OwnerId = Convert.ToInt32(Session["uid"]);
        _dbScheduler.Sheets.InsertOnSubmit(sheet);
        _dbScheduler.SubmitChanges();
        txtName.Text = "";
        BindSheet();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var sheet = _dbScheduler.Sheets.Single(p => p.Id == Id);
            txtName.Text = sheet.Name;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();

        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.Sheets.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            BindSheet();
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var sheet = _dbScheduler.Sheets.Single(p => p.Id == Id);
        sheet.Name = txtName.Text;
        _dbScheduler.SubmitChanges();
        BindSheet();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;
        txtName.Text = "";

    }
}