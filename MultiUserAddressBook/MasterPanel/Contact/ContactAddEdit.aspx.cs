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
            if (Session["UserID"] != null)
            {
                FillCountryAndContactCategoryDDL(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["ContactID"] != null)
                {
                    FillStateDDL(Convert.ToInt32(Session["UserID"].ToString()));
                    FillCityDDL(Convert.ToInt32(Session["UserID"].ToString()));
                    LoadControls(Convert.ToInt32(Session["UserID"].ToString()));
                }
            }
        }
    }
    private void FillCountryAndContactCategoryDDL(Int32 UserID)
    {
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

        objSDR.Close();

        ddlCountry.Items.Insert(0, new ListItem("Select Country...", "-1"));

        objCommand.CommandText = "PR_ContactCategory_SelectAllByUserID";

        objSDR = objCommand.ExecuteReader();
        ddlContactCategory.DataSource = objSDR;
        ddlContactCategory.DataTextField = "ContactCategoryName";
        ddlContactCategory.DataValueField = "ContactCategoryID";
        ddlContactCategory.DataBind();

        objConnection.Close();

        ddlContactCategory.Items.Insert(0, new ListItem("Select Contact Category...", "-1"));
    }
    private void FillStateDDL(Int32 UserID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        if (ddlCountry.SelectedValue == "-1")
        {
            objCommand.CommandText = "PR_State_SelectAllByUserID";
        }
        else
        {
            objCommand.CommandText = "PR_State_SelectDropDownListByUserID";
            objCommand.Parameters.AddWithValue("@CountryID", ddlCountry.SelectedValue);
        }

        SqlDataReader objSDR = objCommand.ExecuteReader();
        ddlState.DataSource = objSDR;
        ddlState.DataTextField = "StateName";
        ddlState.DataValueField = "StateID";
        ddlState.DataBind();

        objConnection.Close();

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
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

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

            if (Page.RouteData.Values["ContactID"] == null)
            {
                objCommand.CommandText = "PR_Contact_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Convert.ToInt32(Session["UserID"].ToString())));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_Contact_UpdateByPK";
                objCommand.Parameters.AddWithValue("@ContactID", Page.RouteData.Values["ContactID"]);
            }

            objCommand.ExecuteNonQuery();

            lblErrorMessage.Text = "Data recorded successfully!";
        }
        catch (SqlException sqlEx)
        {
            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "You have already created a Contact with same name and Mobile number";
                clearFields();
            }
        }
        finally
        {
            objConnection.Close();
        }

        if (Page.RouteData.Values["ContactID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/Contact");
        }
    }
    private Object DBNullOrStringValue(String val)
    {
        if (String.IsNullOrEmpty(val) || val == "-1")
        {
            return DBNull.Value;
        }
        return val;
    }
    private String DBNullOrStringValue(Object val)
    {
        if (val.Equals(DBNull.Value))
        {
            return "";
        }
        return val.ToString();
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
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        if (ddlCountry.SelectedValue == "-1" && ddlState.SelectedValue == "-1")
        {
            objCommand.CommandText = "PR_City_SelectAllByUserID";
        }
        else
        {
            objCommand.CommandText = "PR_City_SelectDropDownListByUserID";
            objCommand.Parameters.AddWithValue("@StateID", ddlState.SelectedValue);
        }

        SqlDataReader objSDR = objCommand.ExecuteReader();
        ddlCity.DataSource = objSDR;
        ddlCity.DataTextField = "CityName";
        ddlCity.DataValueField = "CityID";
        ddlCity.DataBind();

        objConnection.Close();

        ddlCity.Items.Insert(0, new ListItem("Select City...", "-1"));
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/Contact");
    }
    private void LoadControls(Int32 UserID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Contact_SelectByPK";

        objCommand.Parameters.AddWithValue("@ContactID", Page.RouteData.Values["ContactID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                txtContactName.Text = DBNullOrStringValue(objSDR["ContactName"]);
                ddlContactCategory.SelectedValue = DBNullOrStringValue(objSDR["ContactCategoryID"]);
                txtMobileNo.Text = DBNullOrStringValue(objSDR["MobileNo"]);
                txtEmail.Text = DBNullOrStringValue(objSDR["EmailAddress"]);
                txtAddress.Text = DBNullOrStringValue(objSDR["Address"]);
                txtPincode.Text = DBNullOrStringValue(objSDR["Pincode"]);

                if (objSDR["CountryID"] == null)
                {
                    ddlCountry.SelectedValue = "-1";
                }
                else
                {
                    ddlCountry.SelectedValue = objSDR["CountryID"].ToString();
                }
                if (objSDR["StateID"] == null)
                {
                    ddlState.SelectedValue = "-1";
                }
                else
                {
                    FillStateDDL(UserID);
                    ddlState.SelectedValue = objSDR["StateID"].ToString();
                }
                if (objSDR["CityID"] == null)
                {
                    ddlCity.SelectedValue = "-1";
                }
                else
                {
                    FillCityDDL(UserID);
                    ddlCity.SelectedValue = objSDR["CityID"].ToString();
                }
                txtFacebookID.Text = DBNullOrStringValue(objSDR["FacebookID"]);
                txtLinkedInID.Text = DBNullOrStringValue(objSDR["LinkedInID"]);
            }
        }

        objConnection.Close();
    }
}