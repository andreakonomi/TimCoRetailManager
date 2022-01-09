using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndpoint _productEndpoint;
        IConfigHelper _configHelper;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        /// <summary>
        /// Calls the product data from the instance facing the api endpoint for the ui layer.
        /// Loads the data on the binding list of products.
        /// </summary>
        /// <returns></returns>
        private async Task LoadProducts()
        {
            var prodctList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(prodctList);
        }

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        /// <summary>
        /// Whenever the selected product is changed check to see if the condition for 
        /// enabling the AddToCart button are satisfied.
        /// The set is triggered by the binding we put on the ListBox Products SelectedItem
        /// </summary>
        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        /// <summary>
        /// The collection of items on the shopping cart of the client. This is bound with a 
        /// cart specific model.
        /// </summary>
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        //Eventhough it is bound on a textbox it is automatically converted to int from Caliburn Micro
        private int _itemQuantity = 1;

        /// <summary>
        /// Whenever the ItemQuantity is changed which is linked with the textbox
        /// on the UI for quanity to buy, Check if the Button AddToCart should be enabled.
        /// </summary>
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        /// <summary>
        /// Calculates the subtotal of the items in the cart. This is called to be updated everytime a change
        /// in the cart happens.
        /// </summary>
        public string SubTotal => CalculateSubTotal().ToString("C", new CultureInfo("en-US"));

        private decimal CalculateSubTotal()
        {
            return Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart);
        }

        /// <summary>
        /// in my humble opinion this is wrong, you redo once again the looping through items when you have
        /// already done that to calculate the subtotal and tax. CAn just get it from string convert and add
        /// it again. Or store it somewhere. No need to redo the loop again.
        /// </summary>
        public string Total => (CalculateSubTotal() + CalculateTax()).ToString("C", new CultureInfo("en-US"));

        public string Tax => CalculateTax().ToString("C", new CultureInfo("en-US"));

        private decimal CalculateTax()
        {
            //old implementation before tax included
            //return Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart).ToString("C", new CultureInfo("en-US"));

            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable == true)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            //foreach (CartItemModel item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += item.Product.RetailPrice * item.QuantityInCart * taxRate;
            //    }
            //}

            return taxAmount;
        }


        /// <summary>
        /// Enables disables the AddToCart button depending on the condition of QuantityInStock
        /// if it is more than requested to buy.
        /// </summary>
        public bool CanAddToCart => ItemQuantity != 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;

        /// <summary>
        /// Takes the selected item of type ProductModel and transforms it
        /// to CartItemModel that is better adapted for the cart.
        /// </summary>
        public void AddToCart()
        {
            // * tricky why its the same object from the two persepectives!
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            // if this is the second or more time we add the same type on the cart dont add new update the quantity in the cart
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;    //both hold a reference to that specific address of ProductModel, only one loaded on memory
                // HACK - Since we are updating a property of the bounded object the list doesnt get updated on the view
                //it sees the same references as before no need to update myself everything ok, doesnt look at the properties
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart => true;  // add check

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanCheckOut => true;  // add check


        public void CheckOut()
        {

        }

    }
}
