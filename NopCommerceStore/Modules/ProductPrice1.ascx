<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductPrice1Control"
    CodeBehind="ProductPrice1.ascx.cs" %>
<asp:PlaceHolder runat="server" ID="phOldPrice">
    <%=GetLocaleResourceString("Products.OldPrice")%>&nbsp;
    <asp:Label ID="lblOldPrice" runat="server" CssClass="oldProductPrice" />
</asp:PlaceHolder>
<asp:Label ID="lblCustomerEnterPrice" runat="server" Visible="false" />
<asp:Label ID="lblPrice" runat="server" Visible="false" />
<asp:Label ID="lblPriceValue" runat="server" CssClass="productPrice" />
<asp:PlaceHolder runat="server" ID="phDiscount">
    <asp:Label ID="lblFinalPriceWithDiscount" runat="server" CssClass="productPrice" />
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="phDiscountPercent">
    <div class="discountPercent">
        <asp:Label ID="lblDiscountPercent" runat="server" CssClass="productDiscountPercent" />
        <div class="burst" />
    </div>
    <div class="freeShipping">
        <asp:LinkButton runat="server" ID="UILinkButtonFreeShipping" PostBackUrl="~/conditionsinfo.aspx">
            <asp:Label ID="lblFreeShipping" runat="server" CssClass="productFreeShipping" Text="Portes Grátis" />
        </asp:LinkButton>
        <div class="burstGray" />
    </div>
</asp:PlaceHolder>
