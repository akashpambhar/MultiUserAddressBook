<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPanel.master" AutoEventWireup="true" CodeFile="CityAddEdit.aspx.cs" Inherits="MasterPanel_City_CityAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" runat="Server">
    <div class="w-50 m-auto mt-4">
        <h3 class="text-center">City Add/Edit</h3>
        <hr />
        <div class="col-12 mb-3">
            <label for="txtCityName" class="form-label">City Name</label>
            <asp:TextBox ID="txtCityName" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="rfvCityName" runat="server" ErrorMessage="Enter City Name" ControlToValidate="txtCityName" Display="Dynamic" ForeColor="#CC0000"></asp:RequiredFieldValidator>
        </div>
        <div class="row mb-3">
            <div class="col-md-6">
                <label for="txtPincode" class="form-label">Pincode</label>
                <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-6">
                <label for="txtSTDCode" class="form-label">STD Code</label>
                <asp:TextBox ID="txtSTDCode" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-md-6 mb-3">
                <label for="ddlCountry" class="form-label">Country</label>
                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Select a Country" Display="Dynamic" ForeColor="#CC0000" ControlToValidate="ddlCountry" InitialValue="-1"></asp:RequiredFieldValidator>
            </div>
            <div class="col-md-6 mb-3">
                <label for="ddlState" class="form-label">State</label>
                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-select">
                    <asp:ListItem Selected="True" Value="-1">Select State...</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvState" runat="server" ErrorMessage="Select a State" Display="Dynamic" ForeColor="#CC0000" ControlToValidate="ddlState" InitialValue="-1"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-12 text-center">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
        </div>
        <div class="my-3">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="col-md-12 mx-auto alert-light rounded" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPageLevelScript" runat="Server">
</asp:Content>

