using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPanel_ContactCategory_ContactCategoryAddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Page.RouteData.Values["ContactCategoryID"] != null)
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
            objCommand.CommandText = "PR_ContactCategory_Insert";

            objCommand.Parameters.AddWithValue("@ContactCategoryName", DBNullOrStringValue(txtContactCategoryName.Text.Trim()));

            if (Page.RouteData.Values["ContactCategoryID"] == null)
            {
                objCommand.CommandText = "PR_ContactCategory_Insert";
                if (Session["UserID"] != null)
                {
                    objCommand.Parameters.AddWithValue("@UserID", DBNullOrStringValue(Convert.ToInt32(Session["UserID"].ToString())));
                }
                objCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            }
            else
            {
                objCommand.CommandText = "PR_ContactCategory_UpdateByPK";
                objCommand.Parameters.AddWithValue("@ContactCategoryID", Page.RouteData.Values["ContactCategoryID"]);
            }

            objCommand.ExecuteNonQuery();

            lblErrorMessage.Text = "Data recorded successfully!";
        }
        catch (SqlException sqlEx)
        {
            if (sqlEx.Number == 2627)
            {
                lblErrorMessage.Text = "You have already created a Contact Category with same name";
                clearFields();
            }
        }
        finally
        {
            objConnection.Close();
        }

        if (Page.RouteData.Values["ContactCategoryID"] == null)
        {
            clearFields();
        }
        else
        {
            Response.Redirect("~/AB/AdminPanel/ContactCategory");
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
        txtContactCategoryName.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AB/AdminPanel/ContactCategory");
    }
    private void LoadControls()
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_ContactCategory_SelectByPK";

        objCommand.Parameters.AddWithValue("@ContactCategoryID", Page.RouteData.Values["ContactCategoryID"]);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        if (objSDR.HasRows)
        {
            while (objSDR.Read())
            {
                txtContactCategoryName.Text = DBNullOrStringValue(objSDR["ContactCategoryName"]);
            }
        }

        objConnection.Close();
    }
}