﻿using System;
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
                FillCountryDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
                FillContactCategoryDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["ContactID"] != null)
                {
                    LoadControls(Convert.ToInt32(Session["UserID"].ToString()));
                }
            }

            #endregion
        }
    }
    private void FillCountryDropDownList(Int32 UserID)
    {
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Set up Connection and Command

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            #endregion

            #region Get All Countries By UserID

            objCmd.CommandText = "PR_Country_SelectAllByUserID";

            objCmd.Parameters.AddWithValue("@UserID", UserID);

            SqlDataReader objSDR = objCmd.ExecuteReader();
            ddlCountry.DataSource = objSDR;
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataValueField = "CountryID";
            ddlCountry.DataBind();

            objSDR.Close();

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

        ddlCountry.Items.Insert(0, new ListItem("Select Country...", "-1"));
    }
    private void FillContactCategoryDropDownList(Int32 UserID)
    {
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Set up Connection and Command

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            #endregion

            #region Get All Contact Categories By UserID

            objCmd.CommandText = "PR_ContactCategory_SelectAllByUserID";

            objCmd.Parameters.AddWithValue("@UserID", UserID);

            SqlDataReader objSDR = objCmd.ExecuteReader();

            ddlContactCategory.DataSource = objSDR;
            ddlContactCategory.DataTextField = "ContactCategoryName";
            ddlContactCategory.DataValueField = "ContactCategoryID";
            ddlContactCategory.DataBind();

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

        ddlContactCategory.Items.Insert(0, new ListItem("Select Contact Category...", "-1"));
    }
    private void FillStateDropDownList(Int32 UserID)
    {
        if (ddlCountry.SelectedIndex == 0)
        {
            ddlState.Items.Clear();
        }
        else
        {
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
            try
            {
                #region Get All States By CountryID and UserID

                if (objConn.State != ConnectionState.Open)
                    objConn.Open();

                SqlCommand objCmd = objConn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.AddWithValue("@UserID", UserID);

                objCmd.CommandText = "PR_State_SelectDropDownListByUserID";
                objCmd.Parameters.AddWithValue("@CountryID", ddlCountry.SelectedValue);

                SqlDataReader objSDR = objCmd.ExecuteReader();
                ddlState.DataSource = objSDR;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateID";
                ddlState.DataBind();

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

        ddlState.Items.Insert(0, new ListItem("Select State...", "-1"));
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
        {
            FillStateDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
            FillCityDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
        }
        if (ddlCountry.SelectedIndex == 0)
        {
            ddlState.Items.Clear();
            ddlCity.Items.Clear();
            ddlState.Items.Insert(0, new ListItem("Select State...", "-1"));
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

        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Open Connection and Set up Command

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            #endregion

            #region Common Parameters to pass

            objCmd.Parameters.AddWithValue("@ContactCategoryID", strContactCategoryID);
            objCmd.Parameters.AddWithValue("@ContactName", strContactName);
            objCmd.Parameters.AddWithValue("@Address", strAddress);
            objCmd.Parameters.AddWithValue("@Pincode", strPincode);
            objCmd.Parameters.AddWithValue("@CityID", strCityID);
            objCmd.Parameters.AddWithValue("@StateID", strStateID);
            objCmd.Parameters.AddWithValue("@CountryID", strCountryID);
            objCmd.Parameters.AddWithValue("@EmailAddress", strEmailAddress);
            objCmd.Parameters.AddWithValue("@MobileNo", strMobileNo);
            objCmd.Parameters.AddWithValue("@FacebookID", strFacebookID);
            objCmd.Parameters.AddWithValue("@LinkedInID", strLinkedInID);

            #endregion

            #region Check and Perform Insert or Update Contact

            if (Page.RouteData.Values["ContactID"] != null)
            {
                objCmd.CommandText = "PR_Contact_UpdateByPK";
                objCmd.Parameters.AddWithValue("@ContactID", Page.RouteData.Values["ContactID"]);
            }
            else
            {
                objCmd.CommandText = "PR_Contact_Insert";
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
            if (objConn.State != ConnectionState.Closed)
                objConn.Close();
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
            FillCityDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
        }
    }
    private void FillCityDropDownList(Int32 UserID)
    {
        if (ddlCountry.SelectedIndex == 0 && ddlState.SelectedIndex == 0)
        {
            ddlCity.Items.Clear();
        }
        else
        {
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
            try
            {
                #region Get All Cities By StateID and UserID

                if (objConn.State != ConnectionState.Open)
                    objConn.Open();

                SqlCommand objCmd = objConn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.AddWithValue("@UserID", UserID);

                objCmd.CommandText = "PR_City_SelectDropDownListByUserID";
                objCmd.Parameters.AddWithValue("@StateID", ddlState.SelectedValue);

                SqlDataReader objSDR = objCmd.ExecuteReader();
                ddlCity.DataSource = objSDR;
                ddlCity.DataTextField = "CityName";
                ddlCity.DataValueField = "CityID";
                ddlCity.DataBind();

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

        ddlCity.Items.Insert(0, new ListItem("Select City...", "-1"));
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/Contact");
    }
    private void LoadControls(Int32 UserID)
    {
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Get Contact By PK

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Contact_SelectByPK";

            objCmd.Parameters.AddWithValue("@ContactID", Page.RouteData.Values["ContactID"]);

            SqlDataReader objSDR = objCmd.ExecuteReader();

            #endregion

            #region Set obtained values to controls and Close connection

            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    if (!objSDR["ContactName"].Equals(DBNull.Value))
                    {
                        txtContactName.Text = objSDR["ContactName"].ToString();
                    }
                    if (!objSDR["ContactCategoryID"].Equals(DBNull.Value))
                    {
                        ddlContactCategory.SelectedValue = objSDR["ContactCategoryID"].ToString();
                    }
                    if (!objSDR["MobileNo"].Equals(DBNull.Value))
                    {
                        txtMobileNo.Text = objSDR["MobileNo"].ToString();
                    }
                    if (!objSDR["EmailAddress"].Equals(DBNull.Value))
                    {
                        txtEmail.Text = objSDR["EmailAddress"].ToString();
                    }
                    if (!objSDR["Address"].Equals(DBNull.Value))
                    {
                        txtAddress.Text = objSDR["Address"].ToString();
                    }
                    if (!objSDR["Pincode"].Equals(DBNull.Value))
                    {
                        txtPincode.Text = objSDR["Pincode"].ToString();
                    }
                    if (!objSDR["CountryID"].Equals(DBNull.Value))
                    {
                        ddlCountry.SelectedValue = objSDR["CountryID"].ToString();
                    }
                    FillStateDropDownList(UserID);
                    if (!objSDR["StateID"].Equals(DBNull.Value))
                    {
                        ddlState.SelectedValue = objSDR["StateID"].ToString();
                    }
                    FillCityDropDownList(UserID);
                    if (!objSDR["CityID"].Equals(DBNull.Value))
                    {
                        ddlCity.SelectedValue = objSDR["CityID"].ToString();
                    }
                    if (!objSDR["FacebookID"].Equals(DBNull.Value))
                    {
                        txtFacebookID.Text = objSDR["FacebookID"].ToString();
                    }
                    if (!objSDR["LinkedInID"].Equals(DBNull.Value))
                    {
                        txtLinkedInID.Text = objSDR["LinkedInID"].ToString();
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