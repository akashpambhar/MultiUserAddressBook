using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for CommonFillDropDown
/// </summary>
public static class CommonFillDropDown
{
    public static void FillCountryDropDownList(DropDownList ddlCountryID, Label lblErrorMessage, Int32 UserID)
    {
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            #region Get All Countries By UserID

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

        ddlCountryID.Items.Insert(0, new ListItem("Select Country...", "-1"));
    }

    public static void FillStateDropDownList(DropDownList ddlStateID, Label lblErrorMessage, Int32 UserID, Int32 CountryID)
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

            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
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

    public static void FillCityDropDownList(DropDownList ddlCityID, Label lblErrorMessage, Int32 UserID, Int32 CountryID, Int32 StateID)
    {
        if (CountryID == 0 && StateID == 0)
        {
            ddlCityID.Items.Clear();
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
                objCmd.Parameters.AddWithValue("@StateID", StateID);

                SqlDataReader objSDR = objCmd.ExecuteReader();
                ddlCityID.DataSource = objSDR;
                ddlCityID.DataTextField = "CityName";
                ddlCityID.DataValueField = "CityID";
                ddlCityID.DataBind();

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

        ddlCityID.Items.Insert(0, new ListItem("Select City...", "-1"));
    }
}