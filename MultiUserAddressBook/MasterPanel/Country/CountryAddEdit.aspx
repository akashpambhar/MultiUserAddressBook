<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPanel.master" AutoEventWireup="true" CodeFile="CountryAddEdit.aspx.cs" Inherits="MasterPanel_Country_CountryAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" runat="Server">
    <div class="w-50 m-auto mt-4">
        <h3 class="text-center">Country Add/Edit</h3>
        <hr />
        <div class="row mb-3">
            <div class="col-md-6">
                <label for="txtCountryName" class="form-label">Country Name</label>
                <asp:TextBox ID="txtCountryName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvCountryName" runat="server" ErrorMessage="Enter Country Name" ControlToValidate="txtCountryName" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
            </div>
            <div class="col-md-6">
                <label for="txtCountryCode" class="form-label">Country Code</label>
                <asp:TextBox ID="txtCountryCode" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-12 text-center">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
        </div>
        <div class="my-3">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="col-md-12 mx-auto alert-light rounded" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPageLevelScript" runat="Server">
</asp:Content>

