using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

            #region Parameters to pass

            objCommand.Parameters.AddWithValue("@UserName", DBNullOrStringValue(txtUserName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@Password", DBNullOrStringValue(txtPassword.Text.Trim()));
            objCommand.Parameters.AddWithValue("@FullName", DBNullOrStringValue(txtFullName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@Address", DBNullOrStringValue(txtAddress.Text.Trim()));
            objCommand.Parameters.AddWithValue("@MobileNo", DBNullOrStringValue(txtMobileNo.Text.Trim()));
            objCommand.Parameters.AddWithValue("@EmailID", DBNullOrStringValue(txtEmail.Text.Trim()));
            objCommand.Parameters.AddWithValue("@FacebookID", DBNullOrStringValue(txtFacebookID.Text.Trim()));
            objCommand.Parameters.AddWithValue("@BirthDate", DBNullOrStringValue(txtBirthDate.Text.Trim()));

            #region Validate Photo and Set as Parameter

            String strUserPhotoPath = MakePhotoPath();
            if (strUserPhotoPath == "Please upload file of size less than 1 MB" || strUserPhotoPath == "Please select a jpg, jpeg or png file")
            {
                lblErrorMessage.Text = strUserPhotoPath;
                return;
            }

            objCommand.Parameters.AddWithValue("@PhotoPath", DBNullOrStringValue(strUserPhotoPath));

            #endregion

            objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);

            #endregion

            objCommand.CommandText = "PR_UserMaster_Insert";

            objCommand.ExecuteNonQuery();
        }
        catch (SqlException sqlEx)
        {
            #region Set Error Message

            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "UserName already in use. Please enter another";
                txtUserName.Text = "";
                txtUserName.Focus();
                objConnection.Close();
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
            objConnection.Close();
        }

        Response.Redirect("~/AB/AdminPanel/Login");
    }
    private Object DBNullOrStringValue(String val)
    {
        if (String.IsNullOrEmpty(val))
        {
            return DBNull.Value;
        }
        return val;
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