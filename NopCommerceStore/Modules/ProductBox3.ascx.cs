﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductBox3 : BaseNopFrontendUserControl
    {
        Product product = null;

        public override void DataBind()
        {
            base.DataBind();
            this.BindData();
        }

        private void BindData()
        {
            if (product != null)
            {
                string productURL = SEOHelper.GetProductUrl(product);

                hlProduct.NavigateUrl = productURL;
                var name = product.LocalizedName;
                if (name.Length > 38) { name = string.Concat(name.Substring(0, 34), "..."); }
                hlProduct.Text = Server.HtmlEncode(name);
                hlProduct.ToolTip = product.LocalizedName;

                var picture = product.DefaultPicture;
                if (picture != null)
                {
                    hlImageLink.ImageUrl = this.PictureService.GetPictureUrl(picture, this.ProductImageSize, true);
                    hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageLinkTitleFormat"), product.LocalizedName);
                    hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.LocalizedName);
                }
                else
                {
                    hlImageLink.ImageUrl = this.PictureService.GetDefaultPictureUrl(this.ProductImageSize);
                    hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageLinkTitleFormat"), product.LocalizedName);
                    hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.LocalizedName);
                }
                hlImageLink.NavigateUrl = productURL;

                //lShortDescription.Text = product.LocalizedShortDescription;

                var productVariantCollection = product.ProductVariants;
                if (productVariantCollection.Count > 0)
                {
                    if (!product.HasMultipleVariants)
                    {
                        var productVariant = productVariantCollection[0];
                        //btnAddToCart.Visible = (!productVariant.DisableBuyButton);
                        if (!this.SettingManager.GetSettingValueBoolean("Common.HidePricesForNonRegistered") ||
                            (NopContext.Current.User != null &&
                            !NopContext.Current.User.IsGuest))
                        {
                            //nothing
                        }
                        else
                        {
                            //btnAddToCart.Visible = false;
                        }
                    }
                    else
                    {
                        //btnAddToCart.Visible = false;
                    }
                }
                else
                {
                    //btnAddToCart.Visible = false;
                }
            }
        }

        protected void btnProductDetails_Click(object sender, CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            string productURL = SEOHelper.GetProductUrl(productId);
            Response.Redirect(productURL);
        }

        protected void btnAddToCart_Click(object sender, CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            int productVariantId = 0;
            if (this.ProductService.DirectAddToCartAllowed(productId, out productVariantId))
            {
                var addToCartWarnings = this.ShoppingCartService.AddToCart(ShoppingCartTypeEnum.ShoppingCart,
                    productVariantId, string.Empty, decimal.Zero, 1);
                if (addToCartWarnings.Count == 0)
                {
                    bool displayCart = true;
                    if (this.RedirectCartAfterAddingProduct.HasValue)
                        displayCart = this.RedirectCartAfterAddingProduct.Value;
                    else
                        displayCart = this.SettingManager.GetSettingValueBoolean("Display.Products.DisplayCartAfterAddingProduct");

                    if (displayCart)
                    {
                        //redirect to shopping cart page
                        Response.Redirect(SEOHelper.GetShoppingCartUrl());
                    }
                    else
                    {
                        //display notification message
                        this.DisplayAlertMessage(GetLocaleResourceString("Products.ProductHasBeenAddedToTheCart"));
                    }
                }
                else
                {
                    string productURL = SEOHelper.GetProductUrl(productId);
                    Response.Redirect(productURL);
                }
            }
            else
            {
                string productURL = SEOHelper.GetProductUrl(productId);
                Response.Redirect(productURL);
            }
        }

        public Product Product
        {
            get
            {
                return product;
            }
            set
            {
                product = value;
            }
        }

        public int ProductImageSize
        {
            get
            {
                if (ViewState["ProductImageSize"] == null)
                    return 250; //this.SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize", 125);
                else
                    return (int)ViewState["ProductImageSize"];
            }
            set
            {
                ViewState["ProductImageSize"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value whether we redirects a customer to shopping cart page after adding a product to the cart (overrides "Display.Products.DisplayCartAfterAddingProduct" settings)
        /// </summary>
        public bool? RedirectCartAfterAddingProduct
        {
            get
            {
                if (ViewState["RedirectCartAfterAddingProduct"] == null)
                    return null;
                else
                    return (bool)ViewState["RedirectCartAfterAddingProduct"];
            }
            set
            {
                ViewState["RedirectCartAfterAddingProduct"] = value;
            }
        }
    }
}