<%@ Page Title="" Language="C#" MasterPageFile="~/Content/AdminPanel.master" AutoEventWireup="true" CodeFile="CountryList.aspx.cs" Inherits="AdminPanel_Country_CountryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" runat="Server">
    <div class="w-75 m-auto text-center mt-4">
        <div class="row">
            <div class="col-md-12">
                <h1>Country List</h1>
            </div>
        </div>
        <div class="my-3">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="col-md-12 mx-auto alert-light rounded" Text=""></asp:Label>
        </div>
        <div class="row">
            <div class="col-md-12 text-end">
                <asp:HyperLink ID="hlAddCountry" runat="server" Text="Add Country" NavigateUrl="~/AB/AdminPanel/Country/Add" CssClass="btn btn-primary" />
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12">
                <asp:GridView ID="gvCountryList" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-bordered" OnRowCommand="gvCountryList_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="CountryID" HeaderText="ID" />
                        <asp:BoundField DataField="CountryName" HeaderText="Country Name" />
                        <asp:BoundField DataField="CountryCode" HeaderText="Code" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlEdit" Text="Edit" NavigateUrl='<%# "~/AB/AdminPanel/Country/Edit/" + EncryptDecrypt.Base64Encode(Eval("CountryID").ToString()) %>' CssClass="btn btn-warning" runat="server" />
                                <asp:Button ID="btnDelete" Text="Delete" CommandName="DeleteRecord" CommandArgument='<%# Eval("CountryID") %>' CssClass="btn btn-danger" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPageLevelScript" runat="Server">
</asp:Content>

