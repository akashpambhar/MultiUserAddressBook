using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminPanel_ContactCategory_ContactCategoryAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                if (Page.RouteData.Values["ContactCategoryID"] != null)
                {
                    LoadControls();
                }
            }

            #endregion
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region Server Side Validation

        String strErrorMessage = "";

        if (txtContactCategoryName.Text.Trim() == "")
        {
            strErrorMessage += "Enter Contact Category<br/>";
        }
        if (strErrorMessage != "")
        {
            lblErrorMessage.Text = strErrorMessage;
            return;
        }

        #endregion Server Side Validation

        #region Local Variables

        SqlString strContactCategoryName = SqlString.Null;

        #endregion Local Variables

        #region Gather Information

        if (txtContactCategoryName.Text.Trim() != "")
        {
            strContactCategoryName = txtContactCategoryName.Text.Trim();
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

            #region Common parameters to pass

            objCmd.Parameters.AddWithValue("@ContactCategoryName", strContactCategoryName);

            #endregion

            #region Check and Perform Insert or Update ContactCategory

            if (Page.RouteData.Values["ContactCategoryID"] != null)
            {
                objCmd.CommandText = "PR_ContactCategory_UpdateByPK";
                objCmd.Parameters.AddWithValue("@ContactCategoryID", Page.RouteData.Values["ContactCategoryID"]);
            }
            else
            {
                objCmd.CommandText = "PR_ContactCategory_Insert";
                if (Session["UserID"] != null)
                {
                    objCmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"].ToString().Trim()));
                }
                objCmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }

            objCmd.ExecuteNonQuery();

            #endregion

            lblErrorMessage.Text = "Data recorded successfully!";
        }
        catch (SqlException sqlEx)
        {
            #region Set Error Message

            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "You have already created a Contact Category with same name";
                clearFields();
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

        #region Clear Fields or Redirect

        if (Page.RouteData.Values["ContactCategoryID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/ContactCategory");
        }

        #endregion
    }
    private void clearFields()
    {
        txtContactCategoryName.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/ContactCategory");
    }
    private void LoadControls()
    {
        string ContactCategoryID = EncryptDecrypt.Base64Decode(Page.RouteData.Values["ContactCategoryID"].ToString());

        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Get ContactCategory By PK

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_ContactCategory_SelectByPK";

            objCmd.Parameters.AddWithValue("@ContactCategoryID", ContactCategoryID);

            SqlDataReader objSDR = objCmd.ExecuteReader();

            #endregion

            #region Set obtained values to controls

            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    if (!objSDR["ContactCategoryName"].Equals(DBNull.Value))
                    {
                        txtContactCategoryName.Text = objSDR["ContactCategoryName"].ToString();
                    }
                    break;
                }
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