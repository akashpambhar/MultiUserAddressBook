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

public partial class MasterPanel_City_CityAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                FillCountryDDL(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["CityID"] != null)
                {
                    LoadControls(Page.RouteData.Values["CityID"].ToString());
                    FillStateDDL(Convert.ToInt32(Session["UserID"].ToString()));
                }
            }

            #endregion
        }
    }
    private void FillCountryDDL(Int32 UserID)
    {
        #region Get All Countries By UserID

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Country_SelectAllByUserID";

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        ddlCountry.DataSource = objSDR;
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();

        objConnection.Close();

        #endregion

        ddlCountry.Items.Insert(0, new ListItem("Select Country...", "-1"));
    }
    private void FillStateDDL(Int32 UserID)
    {
        #region Get All States By UserID

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_State_SelectDropDownListByUserID";

        objCommand.Parameters.AddWithValue("@CountryID", ddlCountry.SelectedValue);
        objCommand.Parameters.AddWithValue("@UserID", UserID);

        SqlDataReader objSDR = objCommand.ExecuteReader();
        ddlState.DataSource = objSDR;
        ddlState.DataTextField = "StateName";
        ddlState.DataValueField = "StateID";
        ddlState.DataBind();

        objConnection.Close();

        #endregion

        ddlState.Items.Insert(0, new ListItem("Select State...", "-1"));
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
        {
            FillStateDDL(Convert.ToInt32(Session["UserID"].ToString()));
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region Server Side Validation

        String strErrorMessage = "";

        if (txtCityName.Text.Trim() == "")
        {
            strErrorMessage += "Enter City Name<br/>";
        }
        if (ddlCountry.SelectedIndex == 0)
        {
            strErrorMessage += "Select Country<br/>";
        }
        if (ddlState.SelectedIndex == 0)
        {
            strErrorMessage += "Select State<br/>";
        }
        if (strErrorMessage.Trim() != "")
        {
            lblErrorMessage.Text = strErrorMessage;
            return;
        }

        #endregion Server Side Validation

        #region Local Variables

        SqlString strCityName = SqlString.Null;
        SqlString strPincode = SqlString.Null;
        SqlString strSTDCode = SqlString.Null;
        SqlInt32 strStateID = SqlInt32.Null;

        #endregion Local Variables

        #region Gather Information

        if (txtCityName.Text.Trim() != "")
        {
            strCityName = txtCityName.Text.Trim();
        }
        if (txtPincode.Text.Trim() != "")
        {
            strPincode = txtPincode.Text.Trim();
        }
        if (txtSTDCode.Text.Trim() != "")
        {
            strSTDCode = txtSTDCode.Text.Trim();
        }
        if (ddlState.SelectedIndex > 0)
        {
            strStateID = Convert.ToInt32(ddlState.SelectedValue);
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

            #region Common Parameters to pass

            objCommand.Parameters.AddWithValue("@CityName", strCityName);
            objCommand.Parameters.AddWithValue("@Pincode", strPincode);
            objCommand.Parameters.AddWithValue("@STDCode", strSTDCode);
            objCommand.Parameters.AddWithValue("@StateID", strSTDCode);

            #endregion

            #region Check and Perform Insert or Update City

            if (Page.RouteData.Values["CityID"] != null)
            {
                objCommand.CommandText = "PR_City_UpdateByPK";
                if (Page.RouteData.Values["CityID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@CityID", Page.RouteData.Values["CityID"]);
                }
            }
            else
            {
                objCommand.CommandText = "PR_City_Insert";
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
                lblErrorMessage.Text = "You have already created a City with same name in same State";
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

        if (Page.RouteData.Values["CityID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/City");
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

    private void DBNullOrStringValue(DropDownList element, Object val)
    {
        if (!val.Equals(DBNull.Value))
        {
            element.SelectedValue = val.ToString();
        }
    }

    private void clearFields()
    {
        txtCityName.Text = "";
        txtPincode.Text = "";
        txtSTDCode.Text = "";
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/City");
    }
    private void LoadControls(String CityID)
    {
        #region Get City By PK

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_City_SelectByPK";

        objCommand.Parameters.AddWithValue("@CityID", CityID);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        #endregion

        #region Set obtained values to controls and Close connection

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                DBNullOrStringValue(txtCityName, objSDR["CityName"]);
                DBNullOrStringValue(txtPincode, objSDR["Pincode"]);
                DBNullOrStringValue(txtSTDCode, objSDR["STDCode"]);
                DBNullOrStringValue(ddlCountry, objSDR["CountryID"]);
                DBNullOrStringValue(ddlState, objSDR["StateID"]);
            }
        }

        objConnection.Close();

        #endregion
    }
}