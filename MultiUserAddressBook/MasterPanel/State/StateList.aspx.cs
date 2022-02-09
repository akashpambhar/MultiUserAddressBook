using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPanel_State_StateList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["UserID"] != null)
            {
                FillStateGridView(Convert.ToInt32(Session["UserID"].ToString()));
            }
        }
    }
    private void FillStateGridView(Int32 UserID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        objConnection.Open();

        SqlCommand objCommand = objConnection.CreateCommand();
        objCommand.CommandType = CommandType.StoredProcedure;
        objCommand.CommandText = "PR_State_SelectAllByUserID";

        objCommand.Parameters.AddWithValue("@UserID", UserID);

        SqlDataReader objSDR = objCommand.ExecuteReader();

        gvStateList.DataSource = objSDR;
        gvStateList.DataBind();

        objConnection.Close();
    }
    protected void gvStateList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRecord" && e.CommandArgument != null)
        {
            DeleteRecord(Convert.ToInt32(e.CommandArgument));
            if (Session["UserID"] != null)
            {
                FillStateGridView(Convert.ToInt32(Session["UserID"].ToString()));
            }
        }
    }
    private void DeleteRecord(Int32 StateID)
    {
        SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressBookConnectionString"].ConnectionString);
        try
        {
            objConnection.Open();

            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_State_DeleteByPK";
            objCommand.Parameters.AddWithValue("@StateID", StateID);

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