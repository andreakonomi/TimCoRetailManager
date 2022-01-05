using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;

        public BindingList<string> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private BindingList<string> _cart;

        public BindingList<string> Cart
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
