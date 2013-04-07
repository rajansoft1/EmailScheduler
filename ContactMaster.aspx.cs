using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContactMaster : System.Web.UI.Page
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
                     select var;
        ddlGroup.DataSource = groups;
        ddlGroup.DataTextField = "Name";
        ddlGroup.DataValueField = "Id";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0,"Select Group");
        
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var messages = from mapping in _dbScheduler.RecursiveContactGroupSheetMapings
                       join recusiveMessageTemplate in _dbScheduler.RecursiveSmsTemplates on mapping.RecursiveSheetId equals recusiveMessageTemplate.RecursiveSheetId
                       where mapping.ContactGroupId == Convert.ToInt32(ddlGroup.SelectedValue) && mapping.IsDelete == false
                       select new { recusiveMessageTemplate.Id, recusiveMessageTemplate.DaysAfter };
        var contact = new Contact();
        if (!FileUpload1.HasFile)
        {
            contact.ContactGroupId = Convert.ToInt32(ddlGroup.SelectedValue);
            contact.CreatedDate = DateTime.Now;
            contact.EmailId = txtEmail.Text;
            contact.IsDelete = false;
            contact.MobileNo = txtMobile.Text;
            contact.Name = txtName.Text;
            contact.OwnerId = Convert.ToInt32(Session["uid"]);
            _dbScheduler.Contacts.InsertOnSubmit(contact);
            _dbScheduler.SubmitChanges();
            txtEmail.Text = "";
            txtMobile.Text = "";
            txtName.Text = "";
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
                var contacts =  new List<Contact>();
                if (ds.Tables[0].Rows.Count > 0)
                { 
                    for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i][0] != null && ds.Tables[0].Rows[i][2] != null && ds.Tables[0].Rows[i][3] != null)
                        {
                            try
                            {
                                contacts.Add( new Contact()
                                    {
                                        EmailId = Convert.ToString(ds.Tables[0].Rows[i][3]),
                                        MobileNo = Convert.ToString(ds.Tables[0].Rows[i][2]),
                                        Name = Convert.ToString(ds.Tables[0].Rows[i][1]),
                                        IsDelete = false,
                                        ContactGroupId = Convert.ToInt32(ddlGroup.SelectedValue),
                                        CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i][0]),
                                        OwnerId = Convert.ToInt32(Session["uid"])
                                    });
                                
                            }
                            catch (Exception ex)
                            {
                                Response.Write(ex.Message);
                            }

                        }
                    }
                    _dbScheduler.Contacts.InsertAllOnSubmit(contacts);
                    _dbScheduler.SubmitChanges();
                }
                BindContact();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);

            }
        }
    }
    public void BindContact()
    {
        try
        {
            var contacts = from var in _dbScheduler.Contacts
                           where (var.IsDelete == false && var.ContactGroupId == Convert.ToInt32(ddlGroup.SelectedValue))
                           select new { var.Id, var.MobileNo, var.Name,var.EmailId };
            GridView1.DataSource = contacts;
            GridView1.DataBind();
        }
        catch
        {
        }
    }
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindContact();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32( GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var contact = _dbScheduler.Contacts.Single(p => p.Id == Id);
            txtEmail.Text = contact.EmailId;
            txtMobile.Text = contact.MobileNo;
            txtName.Text = contact.Name;
            ddlGroup.SelectedValue = contact.ContactGroupId.ToString();
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();
        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.Contacts.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            BindContact();
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var contact = _dbScheduler.Contacts.Single(p => p.Id == Id);
        contact.ContactGroupId = Convert.ToInt32(ddlGroup.SelectedValue);
        contact.EmailId = txtEmail.Text;
        contact.MobileNo = txtMobile.Text;
        contact.Name = txtName.Text;
        contact.OwnerId = Convert.ToInt32(Session["uid"]);
        _dbScheduler.SubmitChanges();
        BindContact();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;
        txtEmail.Text = "";
        txtMobile.Text = "";
        txtName.Text = "";


    }
}