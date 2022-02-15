using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPanel_Country_CountryAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                if (Page.RouteData.Values["CountryID"] != null)
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

        if (txtCountryName.Text.Trim() == "")
        {
            strErrorMessage += "Enter Country Name <br/>";
        }

        if (strErrorMessage != "")
        {
            lblErrorMessage.Text = strErrorMessage;
            return;
        }

        #endregion Server Side Validation

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Open Connection and Set up Command

            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

            #endregion

            #region Common Parameters to pass

            objCommand.Parameters.AddWithValue("@CountryName", DBNullOrStringValue(txtCountryName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@CountryCode", DBNullOrStringValue(txtCountryCode.Text.Trim()));

            #endregion

            #region Check and Perform Insert or Update Country

            if (Page.RouteData.Values["CountryID"] == null)
            {
                objCommand.CommandText = "PR_Country_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Session["UserID"].ToString()));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_Country_UpdateByPK";
                objCommand.Parameters.AddWithValue("@CountryID", Page.RouteData.Values["CountryID"]);
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
                lblErrorMessage.Text = "You have already created a Country with same name";
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

        if (Page.RouteData.Values["CountryID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/Country");
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
    private String DBNullOrStringValue(TextBox element, Object val)
    {
        if (!val.Equals(DBNull.Value))
        {
            element.Text = val.ToString();
            return val.ToString();
        }
        return "";
    }

    private void clearFields()
    {
        txtCountryName.Text = "";
        txtCountryCode.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/Country");
    }
    private void LoadControls()
    {
        #region Get Country By PK

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Country_SelectByPK";

        objCommand.Parameters.AddWithValue("@CountryID", Page.RouteData.Values["CountryID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        #endregion

        #region Set obtained values to controls and Close connection

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                DBNullOrStringValue(txtCountryName, objSDR["CountryName"]);
                DBNullOrStringValue(txtCountryCode, objSDR["CountryCode"]);
            }
        }

        objConnection.Close();

        #endregion
    }
}