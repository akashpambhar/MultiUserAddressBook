using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPanel_Contact_ContactList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["UserID"] != null)
            {
                FillContactGridView(Convert.ToInt32(Session["UserID"].ToString()));
            }
        }
    }
    private void FillContactGridView(Int32 UserID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_Contact_SelectAllByUserID";

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        gvContactList.DataSource = objSDR;
        gvContactList.DataBind();

        objConnection.Close();
    }
    protected void gvContactList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRecord" && e.CommandArgument != null)
        {
            DeleteRecord(Convert.ToInt32(e.CommandArgument));
            if (Session["UserID"] != null)
            {
                FillContactGridView(Convert.ToInt32(Session["UserID"].ToString()));
            }
        }
    }
    private void DeleteRecord(Int32 ContactID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_Contact_DeleteByPK";
            objCommand.Parameters.AddWithValue("@ContactID", ContactID);

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