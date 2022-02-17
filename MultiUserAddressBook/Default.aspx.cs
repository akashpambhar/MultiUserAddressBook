using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtUserName.Focus();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Get User By UserName and Password

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_UserMaster_SelectByUserNamePassword";

            objCmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
            objCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

            SqlDataReader objSDR = objCmd.ExecuteReader();

            DataTable dtUser = new DataTable();
            dtUser.Load(objSDR);
            
            #endregion

            #region Validate User

            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                foreach (DataRow drUser in dtUser.Rows)
                {
                    if (!drUser["UserID"].Equals(DBNull.Value))
                    {
                        Session["UserID"] = drUser["UserID"].ToString();
                    }
                    if (!drUser["UserName"].Equals(DBNull.Value))
                    {
                        Session["UserName"] = drUser["UserName"].ToString();
                    }
                    if (!drUser["PhotoPath"].Equals(DBNull.Value))
                    {
                        Session["PhotoPath"] = drUser["PhotoPath"].ToString();
                    }
                    break;
                }
                Response.Redirect("~/AB/AdminPanel/Contact");
            }
            else
            {
                lblErrorMessage.Text = "Either Username or Password is not wrong, please try again";
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtUserName.Focus();
            }

            #endregion
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message.ToString();
        }
        finally
        {
            if (objConn.State != ConnectionState.Closed)
                objConn.Close();
        }
    }
}