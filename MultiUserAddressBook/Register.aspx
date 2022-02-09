<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Content/Asset/Content/css/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Content/Asset/Content/js/jquery.min.js"></script>
    <script src="~/Content/Asset/Content/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card w-50 m-auto mt-5">
            <div class="card-header">
                <h3 class="text-center">Register</h3>
            </div>
            <div class="card-body">
                <div class="col-md-6 mx-auto alert-danger mb-2">
                    <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                </div>
                <div class="row mb-2">
                    <div class="col-md-6">
                        <label for="txtFullName" class="form-label">Full Name</label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-6">
                        <label for="txtUserName" class="form-label">User Name</label>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Enter User Name" ControlToValidate="txtUserName" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-6">
                        <label for="txtPassword" class="form-label">Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Enter Password<br />" ControlToValidate="txtPassword" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revPassword" runat="server" ErrorMessage="Password must contain one uppercase, lowercase, special character and digit. Min. length should be 8 characters. Max. length 50 characters" ControlToValidate="txtPassword" Display="Dynamic" ForeColor="#CC0000" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,50}$"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-6">
                        <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:CompareValidator ID="cvConfirmPassword" runat="server" ErrorMessage="Passwords do not match" ControlToCompare="txtConfirmPassword" ControlToValidate="txtPassword" Display="Dynamic" ForeColor="#CC0000"></asp:CompareValidator>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-6">
                        <label for="txtMobileNo" class="form-label">Mobile No.</label>
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-6">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    </div>
                </div>
                <div class="col-12 mx-auto mb-2">
                    <label for="txtAddress" class="form-label">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" />
                </div>
                <div class="row mb-3">
                    <div class="col-md-6">
                        <label for="txtBirthDate" class="form-label">Birth Date</label>
                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>
                    <div class="col-md-6">
                        <label for="txtFacebookID" class="form-label">Facebook ID</label>
                        <asp:TextBox ID="txtFacebookID" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-12 mx-auto mb-2">
                    <div class="input-group">
                        <label for="fuUserPic" class="input-group-text">Profile picture</label>
                        <asp:FileUpload ID="fuUserPic" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-12 text-center mt-3">
                    <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                </div>
                <div class="col-12 text-center mt-1">
                    <hr />
                    Already registered?
                <asp:HyperLink ID="hlLogin" NavigateUrl="~/AB/AdminPanel/Login" runat="server">Login</asp:HyperLink>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
