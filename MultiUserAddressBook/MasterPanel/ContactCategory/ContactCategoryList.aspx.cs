using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPanel_ContactCategory_ContactCategoryList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["UserID"] != null)
            {
                FillContactCategoryGridView(Convert.ToInt32(Session["UserID"].ToString()));
            }
        }
    }
    private void FillContactCategoryGridView(Int32 UserID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_ContactCategory_SelectAllByUserID";

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        gvContactCategoryList.DataSource = objSDR;
        gvContactCategoryList.DataBind();

        objConnection.Close();
    }
    protected void gvContactCategoryList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRecord" && e.CommandArgument != null)
        {
            DeleteRecord(Convert.ToInt32(e.CommandArgument));
            if (Session["UserID"] != null)
            {
                FillContactCategoryGridView(Convert.ToInt32(Session["UserID"].ToString()));
            }
        }
    }
    private void DeleteRecord(Int32 ContactCategoryID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_ContactCategory_DeleteByPK";
            objCommand.Parameters.AddWithValue("@ContactCategoryID", ContactCategoryID);

            objCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            objConnection.Close();
        }
    }
}