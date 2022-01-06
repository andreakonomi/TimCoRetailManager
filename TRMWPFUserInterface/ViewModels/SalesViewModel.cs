using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndpoint _productEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
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

        private BindingList<ProductModel> _cart;

        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        //Eventhough it is bound on a textbox it is automatically converted to int from Caliburn Micro
        private int _itemQuantity;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public string SubTotal => "$0.00";  // add calculation
        public string Total => "$0.00";  // add calculation
        public string Tax => "$0.00";  // add calculation

        public bool CanAddToCart => true;   // add check sth is selected, item quantity is okay

        public void AddToCart()
        {
             
        }

        public bool CanRemoveFromCart => true;  // add check

        public void RemoveFromCart()
        {

        }

        public bool CanCheckOut => true;  // add check


        public void CheckOut()
        {

        }

    }
}
