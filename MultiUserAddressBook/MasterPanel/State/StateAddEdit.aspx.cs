using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPanel_State_StateAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["UserID"] != null)
            {
                FillCountryDDL(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["StateID"] != null)
                {
                    LoadControls();
                }
            }
        }
    }
    private void FillCountryDDL(Int32 UserID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Country_SelectAllByUserID";

        if (Session["UserID"] != null)
        {
            objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Convert.ToInt32(Session["UserID"].ToString())));
        }

        SqlDataReader objSDR = objCommand.ExecuteReader();
        ddlCountry.DataSource = objSDR;
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();

        objConnection.Close();

        ddlCountry.Items.Insert(0, new ListItem("Select Country...", "-1"));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.Parameters.AddWithValue("@StateName", DBNullOrStringValue(txtStateName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@CountryID", DBNullOrStringValue(ddlCountry.SelectedValue));


            if (Page.RouteData.Values["StateID"] == null)
            {
                objCommand.CommandText = "PR_State_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Convert.ToInt32(Session["UserID"].ToString())));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_State_UpdateByPK";
                objCommand.Parameters.AddWithValue("@StateID", Page.RouteData.Values["StateID"]);
            }

            objCommand.ExecuteNonQuery();

            lblErrorMessage.Text = "Data recorded successfully!";
        }
        catch (SqlException sqlEx)
        {
            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "You have already created a State with same name with same Country";
                clearFields();
            }
        }
        finally
        {
            objConnection.Close();
        }

        if (Page.RouteData.Values["StateID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/State");
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
        ddlCountry.SelectedIndex = 0;
        txtStateName.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/State");
    }
    private void LoadControls()
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_State_SelectByPK";

        objCommand.Parameters.AddWithValue("@StateID", Page.RouteData.Values["StateID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                txtStateName.Text = DBNullOrStringValue(objSDR["StateName"]);
                ddlCountry.SelectedValue = DBNullOrStringValue(objSDR["CountryID"]);
            }
        }

        objConnection.Close();
    }
}