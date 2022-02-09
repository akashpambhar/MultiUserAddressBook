﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPanel.master" AutoEventWireup="true" CodeFile="StateList.aspx.cs" Inherits="MasterPanel_State_StateList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainBody" Runat="Server">
    <div class="w-75 m-auto text-center mt-4">
            <div class="row">
                <div class="col-md-12">
                    <h1>State List</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 text-end">
                    <asp:HyperLink ID="hlAddState" runat="server" Text="Add State" NavigateUrl="~/AB/AdminPanel/State/Add" CssClass="btn btn-primary" />
                </div>
            </div>
            <div class="row mt-4">
                <div class="col-md-12">
                    <asp:GridView ID="gvStateList" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-bordered" OnRowCommand="gvStateList_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="StateID" HeaderText="ID" />
                            <asp:BoundField DataField="StateName" HeaderText="State" />
                            <asp:BoundField DataField="CountryName" HeaderText="Country" />
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlEdit" Text="Edit" NavigateUrl='<%# "~/AB/AdminPanel/State/Edit/" + Eval("StateID").ToString() %>' CssClass="btn btn-warning" runat="server" />
                                    <asp:Button ID="btnDelete" Text="Delete" CommandName="DeleteRecord" CommandArgument='<%# Eval("StateID") %>' CssClass="btn btn-danger" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPageLevelScript" Runat="Server">
</asp:Content>
