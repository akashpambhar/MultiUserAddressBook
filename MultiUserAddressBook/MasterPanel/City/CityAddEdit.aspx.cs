using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPanel_City_CityAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["UserID"] != null)
            {
                FillCountryDDL(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["CityID"] != null)
                {
                    LoadControls(Page.RouteData.Values["CityID"].ToString());
                    FillStateDDL(Convert.ToInt32(Session["UserID"].ToString()));
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

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        ddlCountry.DataSource = objSDR;
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();

        objConnection.Close();

        ddlCountry.Items.Insert(0, new ListItem("Select Country...", "-1"));
    }
    private void FillStateDDL(Int32 UserID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_State_SelectDropDownListByUserID";

        objCommand.Parameters.AddWithValue("@CountryID", ddlCountry.SelectedValue);
        objCommand.Parameters.AddWithValue("@UserID", UserID);

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

            objCommand.Parameters.AddWithValue("@CityName", DBNullOrStringValue(txtCityName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@Pincode", DBNullOrStringValue(txtPincode.Text.Trim()));
            objCommand.Parameters.AddWithValue("@STDCode", DBNullOrStringValue(txtSTDCode.Text.Trim()));
            objCommand.Parameters.AddWithValue("@StateID", DBNullOrStringValue(ddlState.SelectedValue));

            if (Page.RouteData.Values["CityID"] == null)
            {
                objCommand.CommandText = "PR_City_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Convert.ToInt32(Session["UserID"].ToString())));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_City_UpdateByPK";
                if (Page.RouteData.Values["CityID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@CityID", Page.RouteData.Values["CityID"]);
                }
            }

            objCommand.ExecuteNonQuery();

            lblErrorMessage.Text = "Data recorded successfully!";
        }
        catch (SqlException sqlEx)
        {
            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "You have already created a City with same name in same State";
                clearFields();
            }
        }
        finally
        {
            objConnection.Close();
        }

        if (Page.RouteData.Values["CityID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/City");
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
        txtCityName.Text = "";
        txtPincode.Text = "";
        txtSTDCode.Text = "";
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedValue = "-1";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/City");
    }
    private void LoadControls(String CityID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_City_SelectByPK";

        objCommand.Parameters.AddWithValue("@CityID", CityID);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                txtCityName.Text = DBNullOrStringValue(objSDR["CityName"]);
                txtPincode.Text = DBNullOrStringValue(objSDR["Pincode"]);
                txtSTDCode.Text = DBNullOrStringValue(objSDR["STDCode"]);
                ddlCountry.SelectedValue = DBNullOrStringValue(objSDR["CountryID"]);
                ddlState.SelectedValue = DBNullOrStringValue(objSDR["StateID"]);
            }
        }

        objConnection.Close();
    }
}