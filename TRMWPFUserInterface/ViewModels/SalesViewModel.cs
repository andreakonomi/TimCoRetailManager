using AutoMapper;
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
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndpoint _productEndpoint;
        ISaleEndpoint _saleEndpoint;
        IConfigHelper _configHelper;
        IMapper _mapper;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper,
            ISaleEndpoint saleEndpoint, IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
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

            // map ProdcutModel that comes from the api to out Display Model.
            var products = _mapper.Map<List<ProductDisplayModel>>(prodctList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _products;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        /// <summary>
        /// Whenever the selected product is changed check to see if the condition for 
        /// enabling the AddToCart button are satisfied.
        /// The set is triggered by the binding we put on the ListBox Products SelectedItem
        /// </summary>
        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private CartItemDisplayModel _selectedCartItem;

        /// <summary>
        /// Captures the selected item on the shopping cart.
        /// </summary>
        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        /// <summary>
        /// The collection of items on the shopping cart of the client. This is bound with a 
        /// cart specific model.
        /// </summary>
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public BindingList<CartItemDisplayModel> Cart
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

            //foreach (CartItemDisplayModel item in Cart)
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
        /// Takes the selected item of type ProductDisplayModel and transforms it
        /// to CartItemDisplayModel that is better adapted for the cart.
        /// </summary>
        public void AddToCart()
        {
            // * tricky why its the same object from the two persepectives!
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            // if this is the second or more time we add the same type on the cart dont add new update the quantity in the cart
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;    //both hold a reference to that specific address of ProductDisplayModel, only one loaded on memory
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
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
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        /// <summary>
        /// Enables/disables the RemoveFromCart button.
        /// </summary>
        public bool CanRemoveFromCart => SelectedCartItem != null && SelectedCartItem.Product.QuantityInStock > 0;

        /// <summary>
        /// Removes the Selected Cart Item from the cart.
        /// </summary>
        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock++;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        /// <summary>
        /// Enables/disables the CheckOut button.
        /// </summary>
        public bool CanCheckOut => Cart.Count > 0;


        public async Task CheckOut()
        {
            // create the model that will be sent
            SaleModel sale = new SaleModel();

            //populate the model with the data
            //TODO: Convert this to linq
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            // post to api
            await _saleEndpoint.PostSale(sale);
        }

    }
}
