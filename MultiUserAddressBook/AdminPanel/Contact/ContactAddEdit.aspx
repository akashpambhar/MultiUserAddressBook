<%@ Page Title="" Language="C#" MasterPageFile="~/Content/AdminPanel.master" AutoEventWireup="true" CodeFile="ContactAddEdit.aspx.cs" Inherits="AdminPanel_Contact_ContactAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" runat="Server">
    <div class="w-50 m-auto mt-4">
        <h3 class="text-center">Contact Add/Edit</h3>
        <hr />
        <div class="row mb-3">
            <div class="col-md-12">
                <label for="txtContactName" class="form-label">Contact Name</label>
                <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvContactName" runat="server" ErrorMessage="Enter Contact Name" ControlToValidate="txtContactName" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <label for="cblContactCategoryID" class="form-label">Contact Category</label>
                <asp:CheckBoxList ID="cblContactCategoryID" runat="server" CssClass="form-control" RepeatDirection="Vertical">
                </asp:CheckBoxList>
                <br />
                <asp:Label ID="lblContactCategoryEmptyMessage" runat="server" CssClass="form-control mx-auto mb-3" Text="" ForeColor="#CC0000" EnableViewState="False" Visible="False"></asp:Label>
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-md-6">
                <label for="txtMobileNo" class="form-label">Mobile No.</label>
                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvMobileNo" runat="server" ErrorMessage="Enter Mobile Number" ControlToValidate="txtMobileNo" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
            </div>
            <div class="col-md-6">
                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
            </div>
        </div>
        <div class="col-12 mb-3">
            <label for="txtAddress" class="form-label">Address</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" />
        </div>
        <div class="row mb-3">
            <div class="col-md-6">
                <label for="txtPincode" class="form-label">Pincode</label>
                <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-md-4">
                <label for="ddlCountryID" class="form-label">Country</label>
                <asp:DropDownList ID="ddlCountryID" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlCountryID_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Selected="True" Value="-1">Select Country...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-4">
                <label for="ddlStateID" class="form-label">State</label>
                <asp:DropDownList ID="ddlStateID" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlStateID_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Selected="True" Value="-1">Select State...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-4">
                <label for="ddlCityID" class="form-label">City</label>
                <asp:DropDownList ID="ddlCityID" runat="server" CssClass="form-select">
                    <asp:ListItem Selected="True" Value="-1">Select City...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-md-6">
                <label for="txtFacebookID" class="form-label">Facebook ID</label>
                <asp:TextBox ID="txtFacebookID" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-6">
                <label for="txtLinkedInID" class="form-label">LinkedIn ID</label>
                <asp:TextBox ID="txtLinkedInID" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-12 text-center mb-3">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
        </div>
        <div class="my-3">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="col-md-12 mx-auto alert-light rounded" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPageLevelScript" runat="Server">
    <script>
        $(document).ready(function () {
            $("#cphMainBody_cblContactCategoryID td").addClass("form-check");
            $("#cphMainBody_cblContactCategoryID input[type='checkbox']").addClass("form-check-input");
            $("#cphMainBody_cblContactCategoryID input[type='checkbox']").addClass("form-check-label");
        });
    </script>
</asp:Content>

