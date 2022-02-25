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

public partial class AdminPanel_City_CityAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                FillCountryDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["CityID"] != null)
                {
                    LoadControls(Page.RouteData.Values["CityID"].ToString());
                    FillStateDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
                }
            }

            #endregion
        }
    }
    private void FillCountryDropDownList(Int32 UserID)
    {
        #region Get All Countries By UserID

        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_SelectAllByUserID";

            objCmd.Parameters.AddWithValue("@UserID", UserID);

            SqlDataReader objSDR = objCmd.ExecuteReader();

            ddlCountryID.DataSource = objSDR;
            ddlCountryID.DataTextField = "CountryName";
            ddlCountryID.DataValueField = "CountryID";
            ddlCountryID.DataBind();
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

        #endregion

        ddlCountryID.Items.Insert(0, new ListItem("Select Country...", "-1"));
    }
    private void FillStateDropDownList(Int32 UserID)
    {
        #region Get All States By UserID

        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectDropDownListByUserID";

            objCmd.Parameters.AddWithValue("@CountryID", ddlCountryID.SelectedValue);
            objCmd.Parameters.AddWithValue("@UserID", UserID);

            SqlDataReader objSDR = objCmd.ExecuteReader();
            ddlStateID.DataSource = objSDR;
            ddlStateID.DataTextField = "StateName";
            ddlStateID.DataValueField = "StateID";
            ddlStateID.DataBind();
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

        #endregion

        ddlStateID.Items.Insert(0, new ListItem("Select State...", "-1"));
    }
    protected void ddlCountryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
        {
            FillStateDropDownList(Convert.ToInt32(Session["UserID"].ToString()));
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
        if (ddlCountryID.SelectedIndex == 0)
        {
            strErrorMessage += "Select Country<br/>";
        }
        if (ddlStateID.SelectedIndex == 0)
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
        if (ddlStateID.SelectedIndex > 0)
        {
            strStateID = Convert.ToInt32(ddlStateID.SelectedValue);
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

            objCmd.Parameters.AddWithValue("@CityName", strCityName);
            objCmd.Parameters.AddWithValue("@Pincode", strPincode);
            objCmd.Parameters.AddWithValue("@STDCode", strSTDCode);
            objCmd.Parameters.AddWithValue("@StateID", strStateID);

            #endregion

            #region Check and Perform Insert or Update City

            if (Page.RouteData.Values["CityID"] != null)
            {
                objCmd.CommandText = "PR_City_UpdateByPK";
                if (Page.RouteData.Values["CityID"] != null)
                {
                    objCmd.Parameters.AddWithValue("@CityID", Page.RouteData.Values["CityID"]);
                }
            }
            else
            {
                objCmd.CommandText = "PR_City_Insert";
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
            if (objConn.State != ConnectionState.Closed)
                objConn.Close();
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
    private void clearFields()
    {
        txtCityName.Text = "";
        txtPincode.Text = "";
        txtSTDCode.Text = "";
        ddlCountryID.SelectedIndex = 0;
        ddlStateID.SelectedIndex = 0;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/City");
    }
    private void LoadControls(String CityID)
    {
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Get City By PK

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_SelectByPK";

            objCmd.Parameters.AddWithValue("@CityID", CityID);

            SqlDataReader objSDR = objCmd.ExecuteReader();

            #endregion

            #region Set obtained values to controls

            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    if (!objSDR["CityName"].Equals(DBNull.Value))
                    {
                        txtCityName.Text = objSDR["CityName"].ToString();
                    }
                    if (!objSDR["Pincode"].Equals(DBNull.Value))
                    {
                        txtPincode.Text = objSDR["Pincode"].ToString();
                    }
                    if (!objSDR["STDCode"].Equals(DBNull.Value))
                    {
                        txtSTDCode.Text = objSDR["STDCode"].ToString();
                    }
                    if (!objSDR["CountryID"].Equals(DBNull.Value))
                    {
                        ddlCountryID.SelectedValue = objSDR["CountryID"].ToString();
                    }
                    if (!objSDR["StateID"].Equals(DBNull.Value))
                    {
                        ddlStateID.SelectedValue = objSDR["StateID"].ToString();
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