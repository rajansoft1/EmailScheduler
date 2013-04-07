using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Smtp : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            
            BindConnections();
        }
    }
    public void BindConnections()
    {
        var smtp = from smtpConnections in _dbScheduler.SmtpConnections
                   where smtpConnections.IsDelete == false
                   select new {smtpConnections.Id, smtpConnections.Name, smtpConnections.UserName, smtpConnections.EmailId };
        GridView1.DataSource = smtp;
        GridView1.DataBind();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var smtpConnection = new SmtpConnection();
        smtpConnection.IsDelete = false;
        smtpConnection.CreatedDate = System.DateTime.Now;
        smtpConnection.DailyLimit = Convert.ToInt32(txtDailyLimit.Text);
        smtpConnection.EmailId = txtEmail.Text;
        smtpConnection.Name = txtName.Text;
        smtpConnection.UserName = txtUserName.Text;
        smtpConnection.Url = txtSmtpUrl.Text;
        smtpConnection.OwnerId = Convert.ToInt32(Session["uid"]);
        smtpConnection.Password = txtPassword.Text;
        smtpConnection.Port = txtPortNo.Text;
        smtpConnection.Priority = Convert.ToInt32(txtPriority.Text);
        _dbScheduler.SmtpConnections.InsertOnSubmit(smtpConnection);
        _dbScheduler.SubmitChanges();
        BindConnections();
        txtEmail.Text = "";
        txtName.Text = "";
        txtPassword.Text = "";
        txtSmtpUrl.Text = "";
        txtPriority.Text = "";
        txtPortNo.Text = "";
        txtUserName.Text = "";
        txtDailyLimit.Text = "";
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Id = Convert.ToInt32(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
        if (e.CommandName == "myEdit")
        {
            var smtpConnections = _dbScheduler.SmtpConnections.Single(p => p.Id == Id);
            txtEmail.Text = smtpConnections.EmailId;
            txtName.Text = smtpConnections.Name;
            txtSmtpUrl.Text = smtpConnections.Url;
            txtPriority.Text = smtpConnections.Priority.ToString();
            txtPortNo.Text = smtpConnections.Port;
            txtPassword.Text = smtpConnections.Password;
            txtUserName.Text = smtpConnections.UserName;
            txtDailyLimit.Text = smtpConnections.DailyLimit.ToString() ;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();
        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.Contacts.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            BindConnections();
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var smtpConnection = _dbScheduler.SmtpConnections.Single(p => p.Id == Id);
        smtpConnection.DailyLimit = Convert.ToInt32(txtDailyLimit.Text);
        smtpConnection.EmailId = txtEmail.Text;
        smtpConnection.Name = txtName.Text;
        smtpConnection.UserName = txtUserName.Text;
        smtpConnection.Url = txtSmtpUrl.Text;
        smtpConnection.Password = txtPassword.Text;
        smtpConnection.Port = txtPortNo.Text;
        smtpConnection.Priority = Convert.ToInt32(txtPriority.Text);
        _dbScheduler.SubmitChanges();
        BindConnections();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;
        txtEmail.Text = "";
        txtPassword.Text = "";
        txtName.Text = "";
        txtName.Text = "";
        txtSmtpUrl.Text = "";
        txtPriority.Text = "";
        txtPortNo.Text = "";
        txtUserName.Text = "";
        txtDailyLimit.Text = "";

    }
}