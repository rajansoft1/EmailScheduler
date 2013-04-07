using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SmsConnectionMaster : System.Web.UI.Page
{
    readonly DbSchedulerDataContext _dbScheduler = new DbSchedulerDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindConnections();
    }
    void BindConnections()
    {
        var connections = from connection in _dbScheduler.SmsConnections
                          where connection.IsDelete == false
                          select new { connection.Id, connection.Name, connection.Proirity, connection.Url };
        GridView1.DataSource = connections;
        GridView1.DataBind();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            var smsConnection = new SmsConnection();
            smsConnection.CreatedOn = DateTime.Now;
            smsConnection.DayLimit = Convert.ToInt32(txtDailyLimit.Text);
            smsConnection.IsDelete = false;
            smsConnection.Name = txtName.Text;
            smsConnection.OwnerId = Convert.ToInt32(Session["uid"]);
            smsConnection.Proirity = Convert.ToInt32(txtPriority.Text);
            smsConnection.Url = txtUrl.Text;
            _dbScheduler.SmsConnections.InsertOnSubmit(smsConnection);
            _dbScheduler.SubmitChanges();
            BindConnections();
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
            var smsConnection = _dbScheduler.SmsConnections.Single(p => p.Id == Id);
            txtName.Text = smsConnection.Name;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            hfId.Value = Id.ToString();

        }
        if (e.CommandName == "myDelete")
        {
            var contact = _dbScheduler.SmsConnections.Single(p => p.Id == Id);
            contact.IsDelete = true;
            _dbScheduler.SubmitChanges();
            BindConnections();
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Id = Convert.ToInt32(hfId.Value);
        var smsConnection = _dbScheduler.SmsConnections.Single(p => p.Id == Id);
        smsConnection.DayLimit = Convert.ToInt32(txtDailyLimit.Text);
        smsConnection.Name = txtName.Text;
        smsConnection.Proirity = Convert.ToInt32(txtPriority.Text);
        smsConnection.Url = txtUrl.Text;
        _dbScheduler.SubmitChanges();
        BindConnections();
        btnUpdate.Visible = false;
        btnSubmit.Visible = true;
        txtName.Text = "";

    }
}