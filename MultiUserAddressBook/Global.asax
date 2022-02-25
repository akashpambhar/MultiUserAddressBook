<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    public static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        routes.Ignore("{resource}.axd/{*pathInfo}");

        routes.MapPageRoute(
            "Route_Default_Login",
            "AB/AdminPanel/Login",
            "~/Default.aspx");

        routes.MapPageRoute(
            "Route_Register",
            "AB/AdminPanel/Register",
            "~/Register.aspx");

        //routes.MapPageRoute(
        //    "Route_Master_Default_Home",
        //    "AB/AdminPanel/Home",
        //    "~/AdminPanel/Default.aspx");

        //Contact Routes        
        routes.MapPageRoute(
            "Route_Master_ContactList_Contact",
            "AB/AdminPanel/Contact",
            "~/AdminPanel/Contact/ContactList.aspx");

        routes.MapPageRoute(
            "Route_Master_ContactAdd_Contact_Add",
            "AB/AdminPanel/Contact/Add",
            "~/AdminPanel/Contact/ContactAddEdit.aspx");

        routes.MapPageRoute(
            "Route_Master_ContactAdd_Contact_Edit",
            "AB/AdminPanel/Contact/Edit/{ContactID}",
            "~/AdminPanel/Contact/ContactAddEdit.aspx");

        //ContactCategory Routes        
        routes.MapPageRoute(
            "Route_Master_ContactCategoryList_ContactCategory",
            "AB/AdminPanel/ContactCategory",
            "~/AdminPanel/ContactCategory/ContactCategoryList.aspx");

        routes.MapPageRoute(
            "Route_Master_ContactCategoryAdd_ContactCategory_Add",
            "AB/AdminPanel/ContactCategory/Add",
            "~/AdminPanel/ContactCategory/ContactCategoryAddEdit.aspx");

        routes.MapPageRoute(
            "Route_Master_ContactCategoryAdd_ContactCategory_Edit",
            "AB/AdminPanel/ContactCategory/Edit/{ContactCategoryID}",
            "~/AdminPanel/ContactCategory/ContactCategoryAddEdit.aspx");
        
        //City Routes        
        routes.MapPageRoute(
            "Route_Master_CityList_City",
            "AB/AdminPanel/City",
            "~/AdminPanel/City/CityList.aspx");

        routes.MapPageRoute(
            "Route_Master_CityAdd_City_Add",
            "AB/AdminPanel/City/Add",
            "~/AdminPanel/City/CityAddEdit.aspx");

        routes.MapPageRoute(
            "Route_Master_CityAdd_City_Edit",
            "AB/AdminPanel/City/Edit/{CityID}",
            "~/AdminPanel/City/CityAddEdit.aspx");

        //State Routes
        routes.MapPageRoute(
            "Route_Master_StateList_State",
            "AB/AdminPanel/State",
            "~/AdminPanel/State/StateList.aspx");

        routes.MapPageRoute(
            "Route_Master_StateAdd_State_Add",
            "AB/AdminPanel/State/Add",
            "~/AdminPanel/State/StateAddEdit.aspx");

        routes.MapPageRoute(
            "Route_Master_StateAdd_State_Edit",
            "AB/AdminPanel/State/Edit/{StateID}",
            "~/AdminPanel/State/StateAddEdit.aspx");

        //Country Routes
        routes.MapPageRoute(
            "Route_Master_CountryList_Country",
            "AB/AdminPanel/Country",
            "~/AdminPanel/Country/CountryList.aspx");

        routes.MapPageRoute(
            "Route_Master_CountryAdd_Country_Add",
            "AB/AdminPanel/Country/Add",
            "~/AdminPanel/Country/CountryAddEdit.aspx");

        routes.MapPageRoute(
            "Route_Master_CountryAdd_Country_Edit",
            "AB/AdminPanel/Country/Edit/{CountryID}",
            "~/AdminPanel/Country/CountryAddEdit.aspx");
    }
       
</script>
