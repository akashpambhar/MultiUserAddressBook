<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPanel.master" AutoEventWireup="true" CodeFile="StateAddEdit.aspx.cs" Inherits="MasterPanel_State_StateAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" runat="Server">
    <div class="w-50 m-auto mt-4">
        <h3 class="text-center">State Add/Edit</h3>
        <hr />
        <div class="row mb-3">

            <div class="col-md-6">
                <label for="ddlCountry" class="form-label">Country</label>
                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-select">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Select a Country" Display="Dynamic" ForeColor="#CC0000" ControlToValidate="ddlCountry" InitialValue="-1"></asp:RequiredFieldValidator>
            </div>
            <div class="col-md-6">
                <label for="txtStateName" class="form-label">State Name</label>
                <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvStateName" runat="server" ErrorMessage="Enter State Name" Display="Dynamic" ForeColor="#CC0000" ControlToValidate="txtStateName"></asp:RequiredFieldValidator>
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

