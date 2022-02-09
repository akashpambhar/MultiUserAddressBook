using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReadAPI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PopulateGridView();
        }
    }
    private void PopulateGridView()
    {
        String serviceUrl = "http://localhost/AddressBook-API/Api/AB/AddressBook/GetStateListByCountryID/3002";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl);
        request.ContentType = "application/json; charset=utf-8";
        request.Method = "GET";

        HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
        String strResponse = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
        String json = strResponse;

        EntityWrapper ent = JsonConvert.DeserializeObject<EntityWrapper>(json);

        gvState.DataSource = ent.ResultList;
        gvState.DataBind();
    }
}