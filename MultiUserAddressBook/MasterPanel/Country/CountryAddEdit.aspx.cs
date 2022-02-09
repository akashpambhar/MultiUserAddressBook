using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            if (Page.RouteData.Values["CountryID"] != null)
            {
                LoadControls();
            }
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

            objCommand.Parameters.AddWithValue("@CountryName", DBNullOrStringValue(txtCountryName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@CountryCode", DBNullOrStringValue(txtCountryCode.Text.Trim()));


            if (Page.RouteData.Values["CountryID"] == null)
            {
                objCommand.CommandText = "PR_Country_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Convert.ToInt32(Session["UserID"].ToString())));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_Country_UpdateByPK";
                objCommand.Parameters.AddWithValue("@CountryID", Page.RouteData.Values["CountryID"]);
            }


            objCommand.ExecuteNonQuery();

            lblErrorMessage.Text = "Data recorded successfully!";
        }
        catch (SqlException sqlEx)
        {
            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "You have already created a Country with same name";
                clearFields();
            }
        }
        finally
        {
            objConnection.Close();
        }

        if (Page.RouteData.Values["CountryID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/Country");
        }
    }
    private Object DBNullOrStringValue(String val)
    {
        if (String.IsNullOrEmpty(val))
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
        txtCountryName.Text = "";
        txtCountryCode.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/Country");
    }
    private void LoadControls()
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Country_SelectByPK";

        objCommand.Parameters.AddWithValue("@CountryID", Page.RouteData.Values["CountryID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                txtCountryName.Text = DBNullOrStringValue(objSDR["CountryName"]);
                txtCountryCode.Text = DBNullOrStringValue(objSDR["CountryCode"]);
            }
        }

        objConnection.Close();
    }
}