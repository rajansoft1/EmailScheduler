using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtUserName.Text == "admin" && txtPassword.Text == "password")
        {
            Session["uid"] = 1;
            Response.Redirect("Default.aspx");
        }
    }
}