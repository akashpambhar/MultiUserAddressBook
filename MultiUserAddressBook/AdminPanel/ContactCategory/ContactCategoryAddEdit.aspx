<%@ Page Title="" Language="C#" MasterPageFile="~/Content/AdminPanel.master" AutoEventWireup="true" CodeFile="ContactCategoryAddEdit.aspx.cs" Inherits="AdminPanel_ContactCategory_ContactCategoryAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" runat="Server">
    <div class="w-50 m-auto mt-4">
        <h3 class="text-center">Contact Category Add/Edit</h3>
        <hr />
        <div class="col-12 mb-3">
            <label for="txtContactCategoryName" class="form-label">Contact Category Name</label>
            <asp:TextBox ID="txtContactCategoryName" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="rfvContactCategoryName" runat="server" ErrorMessage="Enter Contact Category Name" ControlToValidate="txtContactCategoryName" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
        </div>
        <div class="col-12 text-center">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" OnClick="btnCancel_Click" />
        </div>
        <div class="my-3">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="col-md-12 mx-auto alert-light rounded" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPageLevelScript" runat="Server">
</asp:Content>

