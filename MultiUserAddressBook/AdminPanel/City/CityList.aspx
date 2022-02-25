<%@ Page Title="" Language="C#" MasterPageFile="~/Content/AdminPanel.master" AutoEventWireup="true" CodeFile="CityList.aspx.cs" Inherits="AdminPanel_City_CityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" runat="Server">
    <div class="w-75 m-auto text-center mt-4">
        <div class="row">
            <div class="col-md-12">
                <h1>City List</h1>
            </div>
        </div>
        <div class="my-3">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="col-md-12 mx-auto alert-light rounded" Text=""></asp:Label>
        </div>
        <div class="row">
            <div class="col-md-12 text-end">
                <asp:HyperLink ID="hlAddCity" runat="server" Text="Add City" NavigateUrl="~/AB/AdminPanel/City/Add" CssClass="btn btn-primary" />
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12">
                <asp:GridView ID="gvCityList" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-bordered" OnRowCommand="gvCityList_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="CityID" HeaderText="ID" />
                        <asp:BoundField DataField="CityName" HeaderText="City Name" />
                        <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                        <asp:BoundField DataField="StateName" HeaderText="State Name" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlEdit" Text="Edit" NavigateUrl='<%# "~/AB/AdminPanel/City/Edit/" + Eval("CityID").ToString() %>' CssClass="btn btn-warning" runat="server" />
                                <asp:Button ID="btnDelete" Text="Delete" CommandName="DeleteRecord" CommandArgument='<%# Eval("CityID") %>' CssClass="btn btn-danger" runat="server" />
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

