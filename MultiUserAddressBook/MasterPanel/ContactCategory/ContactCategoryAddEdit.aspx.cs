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

public partial class MasterPanel_ContactCategory_ContactCategoryAddEdit : System.Web.UI.Page
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

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Open Connection and Set up Command

            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure; 

            #endregion

            #region Common parameters to pass

            objCommand.Parameters.AddWithValue("@ContactCategoryName", strContactCategoryName);
            
            #endregion            

            #region Check and Perform Insert or Update ContactCategory

            if (Page.RouteData.Values["ContactCategoryID"] != null)
            {
                objCommand.CommandText = "PR_ContactCategory_UpdateByPK";
                objCommand.Parameters.AddWithValue("@ContactCategoryID", Page.RouteData.Values["ContactCategoryID"]);
            }
            else
            {
                objCommand.CommandText = "PR_ContactCategory_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"].ToString().Trim()));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }

            objCommand.ExecuteNonQuery();
            
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
            objConnection.Close();
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
    private Object DBNullOrStringValue(String val)
    {
        if (String.IsNullOrEmpty(val))
        {
            return DBNull.Value;
        }
        return val;
    }
    private void DBNullOrStringValue(TextBox element, Object val)
    {
        if (!val.Equals(DBNull.Value))
        {
            element.Text = val.ToString();
        }
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
        #region Get ContactCategory By PK

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_ContactCategory_SelectByPK";

        objCommand.Parameters.AddWithValue("@ContactCategoryID", Page.RouteData.Values["ContactCategoryID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();
        
        #endregion

        #region Set obtained values to controls and Close connection

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                DBNullOrStringValue(txtContactCategoryName, objSDR["ContactCategoryName"]);
            }
        }

        objConnection.Close();
        
        #endregion
    }
}