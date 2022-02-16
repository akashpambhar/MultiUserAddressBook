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

public partial class MasterPanel_Contact_ContactAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                FillCountryAndContactCategoryDDL(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["ContactID"] != null)
                {
                    LoadControls(Convert.ToInt32(Session["UserID"].ToString()));
                }
            }

            #endregion
        }
    }
    private void FillCountryAndContactCategoryDDL(Int32 UserID)
    {
        #region Set up Connection and Command

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;

        #endregion

        #region Get All Countries By UserID

        objCommand.CommandText = "PR_Country_SelectAllByUserID";

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        SqlDataReader objSDR = objCommand.ExecuteReader();
        ddlCountry.DataSource = objSDR;
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();

        objSDR.Close();

        ddlCountry.Items.Insert(0, new ListItem("Select Country...", "-1"));

        #endregion

        #region Get All Contact Categories By UserID

        objCommand.CommandText = "PR_ContactCategory_SelectAllByUserID";

        objSDR = objCommand.ExecuteReader();
        ddlContactCategory.DataSource = objSDR;
        ddlContactCategory.DataTextField = "ContactCategoryName";
        ddlContactCategory.DataValueField = "ContactCategoryID";
        ddlContactCategory.DataBind();

        objConnection.Close();

        ddlContactCategory.Items.Insert(0, new ListItem("Select Contact Category...", "-1"));

        #endregion
    }
    private void FillStateDDL(Int32 UserID)
    {
        if (ddlCountry.SelectedIndex == 0)
        {
            ddlState.Items.Clear();
        }
        else
        {
            #region Get All States By CountryID and UserID

            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.Parameters.AddWithValue("@UserID", UserID);

            objCommand.CommandText = "PR_State_SelectDropDownListByUserID";
            objCommand.Parameters.AddWithValue("@CountryID", ddlCountry.SelectedValue);

            SqlDataReader objSDR = objCommand.ExecuteReader();
            ddlState.DataSource = objSDR;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateID";
            ddlState.DataBind();

            objConnection.Close();

            #endregion
        }

        ddlState.Items.Insert(0, new ListItem("Select State...", "-1"));
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
        {
            FillStateDDL(Convert.ToInt32(Session["UserID"].ToString()));
            FillCityDDL(Convert.ToInt32(Session["UserID"].ToString()));
        }
        if (ddlCountry.SelectedIndex == 0)
        {
            ddlState.Items.Clear();
            ddlCity.Items.Clear();
            ddlState.Items.Insert(0, new ListItem("Select City...", "-1"));
            ddlCity.Items.Insert(0, new ListItem("Select City...", "-1"));
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region Server Side Validation

        String strErrorMessage = "";

        if (txtContactName.Text.Trim() == "")
        {
            strErrorMessage += "Enter Contact Name<br/>";
        }
        if (ddlContactCategory.SelectedIndex == 0)
        {
            strErrorMessage += "Select Contact category<br/>";
        }
        if (txtMobileNo.Text.Trim() == "")
        {
            strErrorMessage += "Enter Mobile Number<br/>";
        }
        if (strErrorMessage.Trim() != "")
        {
            lblErrorMessage.Text = strErrorMessage;
            return;
        }

        #endregion Server Side Validation

        #region Local Variables

        SqlInt32 strContactCategoryID = SqlInt32.Null;
        SqlString strContactName = SqlString.Null;
        SqlString strAddress = SqlString.Null;
        SqlString strPincode = SqlString.Null;
        SqlInt32 strCityID = SqlInt32.Null;
        SqlInt32 strStateID = SqlInt32.Null;
        SqlInt32 strCountryID = SqlInt32.Null;
        SqlString strEmailAddress = SqlString.Null;
        SqlString strMobileNo = SqlString.Null;
        SqlString strFacebookID = SqlString.Null;
        SqlString strLinkedInID = SqlString.Null;

        #endregion Local Variables

        #region Gather Information

        if (ddlContactCategory.SelectedIndex > 0)
        {
            strContactCategoryID = Convert.ToInt32(ddlContactCategory.SelectedValue);
        }
        if (txtContactName.Text.Trim() != "")
        {
            strContactName = txtContactName.Text.Trim();
        }
        if (txtAddress.Text.Trim() != "")
        {
            strAddress = txtAddress.Text.Trim();
        }
        if (txtPincode.Text.Trim() != "")
        {
            strPincode = txtPincode.Text.Trim();
        }
        if (ddlCity.SelectedIndex > 0)
        {
            strCityID = Convert.ToInt32(ddlCity.SelectedValue);
        }
        if (ddlState.SelectedIndex > 0)
        {
            strStateID = Convert.ToInt32(ddlState.SelectedValue);
        }
        if (ddlCountry.SelectedIndex > 0)
        {
            strCountryID = Convert.ToInt32(ddlCountry.SelectedValue);
        }
        if (txtEmail.Text.Trim() != "")
        {
            strEmailAddress = txtEmail.Text.Trim();
        }
        if (txtMobileNo.Text.Trim() != "")
        {
            strMobileNo = txtMobileNo.Text.Trim();
        }
        if (txtFacebookID.Text.Trim() != "")
        {
            strFacebookID = txtFacebookID.Text.Trim();
        }
        if (txtLinkedInID.Text.Trim() != "")
        {
            strLinkedInID = txtLinkedInID.Text.Trim();
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

            objCommand.Parameters.AddWithValue("@ContactCategoryID", strContactCategoryID);
            objCommand.Parameters.AddWithValue("@ContactName", strContactName);
            objCommand.Parameters.AddWithValue("@Address", strAddress);
            objCommand.Parameters.AddWithValue("@Pincode", strPincode);
            objCommand.Parameters.AddWithValue("@CityID", strCityID);
            objCommand.Parameters.AddWithValue("@StateID", strStateID);
            objCommand.Parameters.AddWithValue("@CountryID", strCountryID);
            objCommand.Parameters.AddWithValue("@EmailAddress", strEmailAddress);
            objCommand.Parameters.AddWithValue("@MobileNo", strMobileNo);
            objCommand.Parameters.AddWithValue("@FacebookID", strFacebookID);
            objCommand.Parameters.AddWithValue("@LinkedInID", strLinkedInID);

            #endregion

            #region Check and Perform Insert or Update Contact

            if (Page.RouteData.Values["ContactID"] != null)
            {
                objCommand.CommandText = "PR_Contact_UpdateByPK";
                objCommand.Parameters.AddWithValue("@ContactID", Page.RouteData.Values["ContactID"]);
            }
            else
            {
                objCommand.CommandText = "PR_Contact_Insert";
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
                lblErrorMessage.Text = "You have already created a Contact with same name and Mobile number";
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

        if (Page.RouteData.Values["ContactID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/Contact");
        }

        #endregion
    }
    private Object DBNullOrStringValue(String val)
    {
        if (String.IsNullOrEmpty(val) || val == "-1")
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
        txtContactName.Text = "";
        ddlContactCategory.SelectedIndex = 0;
        txtMobileNo.Text = "";
        txtEmail.Text = "";
        txtAddress.Text = "";
        txtPincode.Text = "";
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        txtFacebookID.Text = "";
        txtLinkedInID.Text = "";
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
        {
            FillCityDDL(Convert.ToInt32(Session["UserID"].ToString()));
        }
    }
    private void FillCityDDL(Int32 UserID)
    {
        if (ddlCountry.SelectedIndex == 0 && ddlState.SelectedIndex == 0)
        {
            ddlCity.Items.Clear();
        }
        else
        {
            #region Get All Cities By StateID and UserID

            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.Parameters.AddWithValue("@UserID", UserID);

            objCommand.CommandText = "PR_City_SelectDropDownListByUserID";
            objCommand.Parameters.AddWithValue("@StateID", ddlState.SelectedValue);

            SqlDataReader objSDR = objCommand.ExecuteReader();
            ddlCity.DataSource = objSDR;
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataBind();

            objConnection.Close();

            #endregion
        }

        ddlCity.Items.Insert(0, new ListItem("Select City...", "-1"));
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/Contact");
    }
    private void LoadControls(Int32 UserID)
    {
        #region Get Contact By PK

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Contact_SelectByPK";

        objCommand.Parameters.AddWithValue("@ContactID", Page.RouteData.Values["ContactID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        #endregion

        #region Set obtained values to controls and Close connection

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                DBNullOrStringValue(txtContactName, objSDR["ContactName"]);
                DBNullOrStringValue(ddlContactCategory, objSDR["ContactCategoryID"]);
                DBNullOrStringValue(txtMobileNo, objSDR["MobileNo"]);
                DBNullOrStringValue(txtEmail, objSDR["EmailAddress"]);
                DBNullOrStringValue(txtAddress, objSDR["Address"]);
                DBNullOrStringValue(txtPincode, objSDR["Pincode"]);

                if (!objSDR["CountryID"].Equals(DBNull.Value))
                {
                    ddlCountry.SelectedValue = objSDR["CountryID"].ToString();
                }
                FillStateDDL(UserID);
                if (!objSDR["StateID"].Equals(DBNull.Value))
                {
                    ddlState.SelectedValue = objSDR["StateID"].ToString();
                }
                FillCityDDL(UserID);
                if (!objSDR["CityID"].Equals(DBNull.Value))
                {
                    ddlCity.SelectedValue = objSDR["CityID"].ToString();
                }
                DBNullOrStringValue(txtFacebookID, objSDR["FacebookID"]);
                DBNullOrStringValue(txtLinkedInID, objSDR["LinkedInID"]);
            }
        }

        objConnection.Close();

        #endregion
    }
}