//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductSpecifications: BaseNopFrontendUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindData();
        }

        protected void BindData()
        {
            var product = this.ProductService.GetProductById(this.ProductId);
            if (product != null)
            {
                var productSpecificationAttributes = this.SpecificationAttributeService.GetProductSpecificationAttributesByProductId(product.ProductId, null, true);
                if (productSpecificationAttributes.Count > 0)
                {
                    this.Visible = true;
                    rptrProductSpecification.DataSource = productSpecificationAttributes;
                    rptrProductSpecification.DataBind();
                }
                else
                    this.Visible = false;
            }
            else
                this.Visible = false;
        }

        // Value Spec Mod 
        protected void rptrProductSpecification_DataBound(object sender, RepeaterItemEventArgs e)
        {
            ProductSpecificationAttribute theSpecAttribute = (ProductSpecificationAttribute)e.Item.DataItem;
            PlaceHolder specPH1 = (PlaceHolder)e.Item.FindControl("specPH1");

            switch (theSpecAttribute.SpecificationAttributeOption.Name)
            {
                case "Value":
                    {
                        Label theLabel = new Label();
                        theLabel.Text = Server.HtmlEncode(theSpecAttribute.Value);
                        specPH1.Controls.Add(theLabel);
                        break;
                    }

                case "Image":
                    {
                        Image theImage = new Image();
                        theImage.ImageUrl = Server.HtmlEncode(theSpecAttribute.Value);
                        specPH1.Controls.Add(theImage);
                        break;
                    }

                case "File":
                    {
                        // Just grab the file name and pull out the extension then
                        // Show the proper file icon

                        // Get just the file Name
                        string[] file = theSpecAttribute.Value.Split('/');
                        string fileName = file[file.Length - 1];

                        // Get just the file extension
                        string fileExt = fileName.Substring(fileName.Length - 4);

                        // remove the file extension
                        string fnNoExt = fileName.Replace(fileExt, "");


                        // Show the correct file image based on the file extension
                        switch (fileExt)
                        {
                            case ".pdf":
                                {
                                    Image extImage = new Image();
                                    extImage.ImageUrl = Server.HtmlEncode("~/images/adobeFileIcon.jpg");
                                    specPH1.Controls.Add(extImage);
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }


                        HyperLink theLink = new HyperLink();
                        theLink.NavigateUrl = Server.HtmlEncode(theSpecAttribute.Value);
                        theLink.Text = Server.HtmlEncode(fnNoExt);
                        specPH1.Controls.Add(theLink);
                        break;
                    }

                case "Html":
                    {
                        Literal theLiteral = new Literal();
                        theLiteral.Text = theSpecAttribute.Value;
                        specPH1.Controls.Add(theLiteral);
                        break;
                    }

                default:
                    {
                        // spec value is nops just add the label control to the placeholder, but pull the
                        // value from the actual spec att option name
                        Label theLabel = new Label();
                        theLabel.Text = Server.HtmlEncode(theSpecAttribute.SpecificationAttributeOption.Name);
                        specPH1.Controls.Add(theLabel);
                        break;
                    }
            }

        }

        public int ProductId
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductId");
            }
        }
    }
}