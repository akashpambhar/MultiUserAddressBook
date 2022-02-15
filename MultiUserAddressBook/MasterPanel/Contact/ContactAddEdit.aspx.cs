using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        if (ddlCountry.SelectedValue == "-1")
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
        if (ddlCountry.SelectedValue == "-1")
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

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Open Connection and Set up Command

            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

            #endregion

            #region Common Parameters to pass

            objCommand.Parameters.AddWithValue("@ContactCategoryID", DBNullOrStringValue(ddlContactCategory.SelectedValue));
            objCommand.Parameters.AddWithValue("@ContactName", DBNullOrStringValue(txtContactName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@Address", DBNullOrStringValue(txtAddress.Text.Trim()));
            objCommand.Parameters.AddWithValue("@Pincode", DBNullOrStringValue(txtPincode.Text.Trim()));
            objCommand.Parameters.AddWithValue("@CityID", DBNullOrStringValue(ddlCity.SelectedValue));
            objCommand.Parameters.AddWithValue("@StateID", DBNullOrStringValue(ddlState.SelectedValue));
            objCommand.Parameters.AddWithValue("@CountryID", DBNullOrStringValue(ddlCountry.SelectedValue));
            objCommand.Parameters.AddWithValue("@EmailAddress", DBNullOrStringValue(txtEmail.Text.Trim()));
            objCommand.Parameters.AddWithValue("@MobileNo", DBNullOrStringValue(txtMobileNo.Text.Trim()));
            objCommand.Parameters.AddWithValue("@FacebookID", DBNullOrStringValue(txtFacebookID.Text.Trim()));
            objCommand.Parameters.AddWithValue("@LinkedInID", DBNullOrStringValue(txtLinkedInID.Text.Trim()));

            #endregion

            #region Check and Perform Insert or Update Contact

            if (Page.RouteData.Values["ContactID"] == null)
            {
                objCommand.CommandText = "PR_Contact_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Session["UserID"].ToString()));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_Contact_UpdateByPK";
                objCommand.Parameters.AddWithValue("@ContactID", Page.RouteData.Values["ContactID"]);
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
    private String DBNullOrStringValue(TextBox element, Object val)
    {
        if (!val.Equals(DBNull.Value))
        {
            element.Text = val.ToString();
            return val.ToString();
        }
        return "";
    }

    private String DBNullOrStringValue(DropDownList element, Object val)
    {
        if (!val.Equals(DBNull.Value))
        {
            element.SelectedValue = val.ToString();
            return val.ToString();
        }
        return "";
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
        if (ddlCountry.SelectedValue == "-1" && ddlState.SelectedValue == "-1")
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