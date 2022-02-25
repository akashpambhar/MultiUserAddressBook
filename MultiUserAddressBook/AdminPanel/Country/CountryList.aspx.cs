using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminPanel_Country_CountryList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                FillCountryGridView(Convert.ToInt32(Session["UserID"].ToString()));
            }

            #endregion
        }
    }
    private void FillCountryGridView(Int32 UserID)
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

            gvCountryList.DataSource = objSDR;
            gvCountryList.DataBind();
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
    }

    protected void gvCountryList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        #region Handle Delete Action from GridView

        if (e.CommandName == "DeleteRecord" && e.CommandArgument != null)
        {
            DeleteRecord(Convert.ToInt32(e.CommandArgument));
            FillCountryGridView(Convert.ToInt32(Session["UserID"].ToString()));
        }

        #endregion
    }
    private void DeleteRecord(Int32 CountryID)
    {
        #region Delete Country By PK

        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            if (objConn.State != ConnectionState.Open)
                objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);

            objCmd.ExecuteNonQuery();
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
    }
}