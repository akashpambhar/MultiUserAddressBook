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

public partial class AdminPanel_State_StateAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                CommonFillDropDown.FillCountryDropDownList(ddlCountryID, lblErrorMessage, Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["StateID"] != null)
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

        if (txtStateName.Text.Trim() == "")
        {
            strErrorMessage += "Enter State Name<br/>";
        }
        if (ddlCountryID.SelectedIndex == 0)
        {
            strErrorMessage += "Select Country<br/>";
        }
        if (strErrorMessage.Trim() != "")
        {
            lblErrorMessage.Text = strErrorMessage;
            return;
        }

        #endregion Server Side Validation

        #region Local Variables

        SqlString strStateName = SqlString.Null;
        SqlInt32 strCountryID = SqlInt32.Null;

        #endregion Local Variables

        #region Gather Information

        if (ddlCountryID.SelectedIndex > 0)
        {
            strCountryID = Convert.ToInt32(ddlCountryID.SelectedValue);
        }
        if (txtStateName.Text.Trim() != "")
        {
            strStateName = txtStateName.Text.Trim();
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

            objCmd.Parameters.AddWithValue("@StateName", strStateName);
            objCmd.Parameters.AddWithValue("@CountryID", strCountryID);

            #endregion

            #region Check and Perform Insert or Update State

            if (Page.RouteData.Values["StateID"] != null)
            {
                objCmd.CommandText = "PR_State_UpdateByPK";
                objCmd.Parameters.AddWithValue("@StateID", Page.RouteData.Values["StateID"]);
            }
            else
            {
                objCmd.CommandText = "PR_State_Insert";
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
                lblErrorMessage.Text = "You have already created a State with same name with same Country";
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

        if (Page.RouteData.Values["StateID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/State");
        }

        #endregion
    }
    private void clearFields()
    {
        ddlCountryID.SelectedIndex = 0;
        txtStateName.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/State");
    }
    private void LoadControls()
    {
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Get State By PK

            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectByPK";

            objCmd.Parameters.AddWithValue("@StateID", Page.RouteData.Values["StateID"]);

            SqlDataReader objSDR = objCmd.ExecuteReader();

            #endregion

            #region Set obtained values to controls and Close connection

            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    if (!objSDR["StateName"].Equals(DBNull.Value))
                    {
                        txtStateName.Text = objSDR["StateName"].ToString();
                    }
                    if (!objSDR["CountryID"].Equals(DBNull.Value))
                    {
                        ddlCountryID.SelectedValue = objSDR["CountryID"].ToString();
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