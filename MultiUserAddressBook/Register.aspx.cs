using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtFullName.Focus();
        }
    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        #region Server Side Validation

        String strErrorMessage = "";

        if (txtUserName.Text.Trim() == "")
        {
            strErrorMessage += "Enter User Name <br/>";
        }
        if (txtPassword.Text.Trim() == "")
        {
            strErrorMessage += "Enter Password <br/>";
        }
        String strUserPhotoPath = MakePhotoPath();
        if (strUserPhotoPath == "Please upload file of size less than 1 MB" || strUserPhotoPath == "Please select a jpg, jpeg or png file")
        {
            strErrorMessage += strUserPhotoPath + " <br/>";
        }
        if (strErrorMessage != "")
        {
            lblErrorMessage.Text = strErrorMessage;
            return;
        }

        #endregion Server Side Validation

        #region Local Variables

        SqlString strUserName = SqlString.Null;
        SqlString strPassword = SqlString.Null;
        SqlString strFullName = SqlString.Null;
        SqlString strAddress = SqlString.Null;
        SqlString strMobileNo = SqlString.Null;
        SqlString strEmail = SqlString.Null;
        SqlString strFacebookID = SqlString.Null;
        SqlString strBirthDate = SqlString.Null;
        SqlString strPhotoPath = SqlString.Null;

        #endregion Local Variables

        #region Gather Information

        if (txtUserName.Text.Trim() != "")
        {
            strUserName = txtUserName.Text.Trim();
        }
        if (txtPassword.Text.Trim() != "")
        {
            strPassword = txtPassword.Text.Trim();
        }
        if (txtFullName.Text.Trim() != "")
        {
            strFullName = txtFullName.Text.Trim();
        }
        if (txtAddress.Text.Trim() != "")
        {
            strAddress = txtAddress.Text.Trim();
        }
        if (txtMobileNo.Text.Trim() != "")
        {
            strMobileNo = txtMobileNo.Text.Trim();
        }
        if (txtEmail.Text.Trim() != "")
        {
            strEmail = txtEmail.Text.Trim();
        }
        if (txtFacebookID.Text.Trim() != "")
        {
            strFacebookID = txtFacebookID.Text.Trim();
        }
        if (txtBirthDate.Text.Trim() != "")
        {
            strBirthDate = txtBirthDate.Text.Trim();
        }
        if (strUserPhotoPath.Trim() != "")
        {
            strPhotoPath = strUserPhotoPath.Trim();
        }

        #endregion Gather Information

        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Open Connection and Set up Command

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            #endregion

            #region Parameters to pass

            objCmd.Parameters.AddWithValue("@UserName", strUserName);
            objCmd.Parameters.AddWithValue("@Password", strPassword);
            objCmd.Parameters.AddWithValue("@FullName", strFullName);
            objCmd.Parameters.AddWithValue("@Address", strAddress);
            objCmd.Parameters.AddWithValue("@MobileNo", strMobileNo);
            objCmd.Parameters.AddWithValue("@EmailID", strEmail);
            objCmd.Parameters.AddWithValue("@FacebookID", strFacebookID);
            objCmd.Parameters.AddWithValue("@BirthDate", strBirthDate);
            objCmd.Parameters.AddWithValue("@PhotoPath", strPhotoPath);
            objCmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);

            #endregion

            objCmd.CommandText = "PR_UserMaster_Insert";

            objCmd.ExecuteNonQuery();
        }
        catch (SqlException sqlEx)
        {
            #region Set Error Message

            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "UserName already in use. Please enter another";
                txtUserName.Text = "";
                txtUserName.Focus();
                objConn.Close();
                return;
            }
            else
            {
                lblErrorMessage.Text = sqlEx.Message.ToString();
            }

            #endregion
        }
        finally
        {
            if (objConn.State != ConnectionState.Closed)
                objConn.Close();
        }

        Response.Redirect("~/AB/AdminPanel/Login");
    }
    private String MakePhotoPath()
    {
        #region Perform Photo Validation and Return PhotoPath

        if (fuUserPic.HasFile)
        {
            String fileExt = System.IO.Path.GetExtension(fuUserPic.FileName);
            if (fileExt == ".jpg" || fileExt == ".jpeg" || fileExt == ".png")
            {
                HttpPostedFile file = fuUserPic.PostedFile;
                int iFileSize = file.ContentLength;
                if (iFileSize > 1048576)
                {
                    return "Please upload file of size less than 1 MB";
                }
                else
                {
                    String strPhotoPath = "~/Content/Asset/Content/images/";
                    if (!Directory.Exists(Server.MapPath(strPhotoPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(strPhotoPath));
                    }

                    fuUserPic.SaveAs(Server.MapPath(strPhotoPath + fuUserPic.FileName));
                    return strPhotoPath + fuUserPic.FileName;
                }
            }
            else
            {
                return "Please select a jpg, jpeg or png file";
            }
        }
        return "";

        #endregion
    }
}