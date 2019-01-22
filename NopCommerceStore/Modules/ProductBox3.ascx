<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductBox3.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductBox3" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductPrice2" Src="~/Modules/ProductPrice2.ascx" %>
<div class="product-item">
    <h2 class="product-title">
        <asp:HyperLink ID="hlProduct" runat="server" />
    </h2>
    <div class="picture">
        <asp:HyperLink ID="hlImageLink" runat="server" />
    </div>
<%--    <div class="add-info">
        <div class="prices">
            <nopCommerce:ProductPrice2 ID="ctrlProductPrice" runat="server" ProductID='<%#Eval("ProductId") %>' />
        </div>
        <div class="buttons">
            <asp:Button runat="server" ID="btnAddToCart" OnCommand="btnAddToCart_Click" Text="<% $NopResources:Products.AddToCart %>"
                ValidationGroup="ProductDetails" CommandArgument='<%# Eval("ProductId") %>' CssClass="productgridaddtocartbutton" />
        </div>
    --%>
</div>
