using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RecursiveSheetMaster : System.Web.UI.Page
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
        GridView1.DataSource = sheets;
        GridView1.DataBind();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var sheet = new RecursiveSheet();
        sheet.Name = txtName.Text;
        sheet.IsDelete = false;
        sheet.CreatedOn = DateTime.Now;
        sheet.OwnerId = Convert.ToInt32(Session["uid"]);
        _dbScheduler.RecursiveSheets.InsertOnSubmit(sheet);
        _dbScheduler.SubmitChanges();
        txtName.Text = "";
        BindRecursiveSheet();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var sheet = _dbScheduler.RecursiveSheets.Single(p => p.Id == Id);
            txtName.Text = sheet.Name; ;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();
        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.RecursiveSheets.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();

        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var sheet = _dbScheduler.RecursiveSheets.Single(p => p.Id == Id);
        sheet.Name = txtName.Text;
         _dbScheduler.SubmitChanges();
        txtName.Text = "";
        BindRecursiveSheet();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;


    }
}