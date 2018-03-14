﻿<%@ Page Title="Transaction History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RefCart.aspx.cs" Inherits="E_CommerceApp.FrmViewRefCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" RenderMode="Block">
            <ContentTemplate>
                <div class="container">
                    <div class="row">
                        <div class="col col-sm-8">
                            <div class="container">
                                <asp:ListView ID="lvw_items" runat="server" OnPagePropertiesChanging="lvw_items_PagePropertiesChanging">
                                    <LayoutTemplate>
                                        <tr id="itemPlaceholderContainer" runat="server">
                                            <td id="itemPlaceholder" runat="server"></td>
                                        </tr>
                                        <div class="container">
                                            <asp:DataPager ID="DataPager1" runat="server" PageSize="3">
                                                <Fields>
                                                    <asp:NextPreviousPagerField
                                                        ButtonCssClass="btn btn-outline-primary"
                                                        ShowPreviousPageButton="true"
                                                        ShowNextPageButton="false"
                                                        PreviousPageText="&laquo; Prev." />
                                                    <asp:NumericPagerField
                                                        NumericButtonCssClass="NumericButtonCSS"
                                                        NextPreviousButtonCssClass="btn btn-outline-primary" />
                                                    <asp:NextPreviousPagerField
                                                        ButtonCssClass="btn btn-outline-primary"
                                                        ShowNextPageButton="true"
                                                        ShowPreviousPageButton="false"
                                                        NextPageText="Next &raquo;" />
                                                </Fields>
                                            </asp:DataPager>
                                        </div>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <div class="card text-center">
                                            <img class="card-img-top" src="Content/Images/dino.jpg" alt="Card image cap">
                                            <div class="card-body">
                                                <p class="card-text">Your cart is empty.</p>
                                                <asp:Button ID="btn_shpNow" runat="server" CssClass="btn btn-outline-success" PostBackUrl="~/Products.aspx" Text="Shop now" />
                                            </div>
                                        </div>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <td runat="server">
                                            <div class="card">
                                                <div class="row align-items-center">
                                                    <div class="col col-md-4">
                                                        <img src="https://www.tmonews.com/wp-content/uploads/2017/03/rediphone7tmo-1-660x544.jpg" class="img-thumbnail" />
                                                    </div>
                                                    <div class="col col-md-4">
                                                        <asp:Label ID="lbl_item" runat="server" Text='<%# Eval("item") %>' CssClass="h5"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lbl_sku" runat="server" Text='<%# Eval("sku") %>' CssClass="h6 text-muted"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lbl_price" runat="server" Text='<%# Eval("price","{0:c}") %>' CssClass="h6 text-muted"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="container">
                                                            <div class="form-group">
                                                                <asp:Label ID="Label5" runat="server" Text="Quantity (Max: 99)" CssClass="h6 text-muted"></asp:Label>
                                                                <br />
                                                                <asp:TextBox ID="tbx_qty" runat="server" CssClass="form-control" TextMode="Number" Text='<%# Eval("quantity") %>' min="1" max="99" step="1" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>

                                        <br />
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                        <div class="col-6 col-sm-4 align-self-start">
                            <div class="card">
                                <h3 class="card-header">Totals</h3>
                                <div class="card-body">
                                    <asp:ListView ID="lvw_totals" runat="server">
                                        <LayoutTemplate>
                                            <table runat="server" class="table table-hover">
                                                <tr id="ItemPlaceholder" runat="server"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <EmptyDataTemplate>
                                            <p>Empty.</p>
                                        </EmptyDataTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <th scope="row">Quantity</th>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("totalQuantity") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th scope="row">Price</th>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("totalPrice", "{0:c}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                            <br />
                            <div class="card">
                                <h5 class="card-header">View Previous Transactions</h5>
                                <div class="card-body">
                                    <div class="form-group">
                                        <asp:TextBox ID="tbx_refNum" runat="server" CssClass="form-control" Placeholder="Reference Numer" ClientIDMode="Static" CausesValidation="true" />
                                    </div>
                                    <asp:Button ID="btn_goRefCart" runat="server" CssClass="btn btn-outline-success btn-block" Text="View Transaction" ClientIDMode="Static" OnClick="btn_goRefCart_Click" CausesValidation="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
