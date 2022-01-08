using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// Model to capture and package/separate the items in the cart
    /// from the items in the stock/db
    /// Model to send data back to the api for buying products
    /// </summary>
    public class CartItemModel
    {
        public ProductModel Product { get; set; }
        public int QuantityInCart { get; set; }
        /// <summary>
        /// Display property for the listbox.
        /// </summary>
        public string DisplayText => $"{Product.ProductName} ({QuantityInCart})";
    }
}
