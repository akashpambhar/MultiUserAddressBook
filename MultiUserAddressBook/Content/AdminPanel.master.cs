using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Content_AdminPanel : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/AB/AdminPanel/Login");
            }
            if (Session["UserName"] != null)
            {
                hlUserName.Text = Session["UserName"].ToString();
            }
            if (Session["PhotoPath"] != null)
            {
                imgProfile.Visible = true;
                imgProfile.ImageUrl = Session["PhotoPath"].ToString();
            }
        }
    }
    protected void lbLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("~/AB/AdminPanel/Login");
    }
}
