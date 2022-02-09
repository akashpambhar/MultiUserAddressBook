<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Content/Asset/Content/css/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Content/Asset/Content/js/jquery.min.js"></script>
    <script src="~/Content/Asset/Content/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server" class="">
        <div class="card w-50 m-auto mt-5">
            <div class="card-header">
                <h3 class="text-center">Login</h3>
            </div>

            <div class="card-body">
                <div class="col-md-6 mx-auto alert-danger mb-2">
                    <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-6 mx-auto">
                    <label for="txtUserName" class="form-label">User Name</label>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Enter User Name" ControlToValidate="txtUserName" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-6 mx-auto">
                    <label for="txtPassword" class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Enter Password" ControlToValidate="txtPassword" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                </div>
                <div class="col-12 text-center mt-3">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                </div>
                <div class="col-12 text-center mt-1">
                    <hr />
                    New User?
                <asp:HyperLink ID="hlRegister" NavigateUrl="~/AB/AdminPanel/Register" runat="server">Register</asp:HyperLink>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
