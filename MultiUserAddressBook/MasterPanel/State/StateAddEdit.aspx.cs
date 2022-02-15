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
            #region Check Session UserID and Load Controls

            if (Session["UserID"] != null)
            {
                FillCountryDDL(Convert.ToInt32(Session["UserID"].ToString()));
                if (Page.RouteData.Values["StateID"] != null)
                {
                    LoadControls();
                }
            }

            #endregion
        }
    }
    private void FillCountryDDL(Int32 UserID)
    {
        #region Get All Countries By UserID

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Country_SelectAllByUserID";

        if (Session["UserID"] != null)
        {
            objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Session["UserID"].ToString()));
        }

        SqlDataReader objSDR = objCommand.ExecuteReader();
        ddlCountry.DataSource = objSDR;
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();

        objConnection.Close();

        #endregion

        ddlCountry.Items.Insert(0, new ListItem("Select Country...", "-1"));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region Server Side Validation

        String strErrorMessage = "";

        if (txtStateName.Text.Trim() == "")
        {
            strErrorMessage += "Enter State Name<br/>";
        }
        if (ddlCountry.SelectedIndex == 0)
        {
            strErrorMessage += "Select Country<br/>";
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

            objCommand.Parameters.AddWithValue("@StateName", DBNullOrStringValue(txtStateName.Text.Trim()));
            objCommand.Parameters.AddWithValue("@CountryID", DBNullOrStringValue(ddlCountry.SelectedValue));

            #endregion

            #region Check and Perform Insert or Update State

            if (Page.RouteData.Values["StateID"] == null)
            {
                objCommand.CommandText = "PR_State_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Session["UserID"].ToString()));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_State_UpdateByPK";
                objCommand.Parameters.AddWithValue("@StateID", Page.RouteData.Values["StateID"]);
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
            objConnection.Close();
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
    private Object DBNullOrStringValue(String val)
    {
        if (String.IsNullOrEmpty(val))
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
        ddlCountry.SelectedIndex = 0;
        txtStateName.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/State");
    }
    private void LoadControls()
    {
        #region Get State By PK

        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_State_SelectByPK";

        objCommand.Parameters.AddWithValue("@StateID", Page.RouteData.Values["StateID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        #endregion

        #region Set obtained values to controls and Close connection

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                DBNullOrStringValue(txtStateName, objSDR["StateName"]);
                DBNullOrStringValue(ddlCountry, objSDR["CountryID"]);
            }
        }

        objConnection.Close();

        #endregion
    }
}