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

        #region Local Variables

        SqlString strCountryName = SqlString.Null;
        SqlString strCountryCode = SqlString.Null;

        #endregion Local Variables

        #region Gather Information

        if (txtCountryName.Text.Trim() != "")
        {
            strCountryName = txtCountryName.Text.Trim();
        }
        if (txtCountryCode.Text.Trim() != "")
        {
            strCountryCode = txtCountryCode.Text.Trim();
        }

        #endregion

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Open Connection and Set up Command

            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

            #endregion

            #region Common Parameters to pass

            objCommand.Parameters.AddWithValue("@CountryName", strCountryName);
            objCommand.Parameters.AddWithValue("@CountryCode", strCountryCode);

            #endregion

            #region Check and Perform Insert or Update Country

            if (Page.RouteData.Values["CountryID"] != null)
            {
                objCommand.CommandText = "PR_Country_UpdateByPK";
                objCommand.Parameters.AddWithValue("@CountryID", Page.RouteData.Values["CountryID"]);
            }
            else
            {
                objCommand.CommandText = "PR_Country_Insert";
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
    private void DBNullOrStringValue(TextBox element, Object val)
    {
        if (!val.Equals(DBNull.Value))
        {
            element.Text = val.ToString();
        }
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