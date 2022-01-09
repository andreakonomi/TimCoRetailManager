using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// A model for sending only the neccessary information from the front end to the back.
    /// It sends only the minimal information for each product to buy.
    /// The other data are calculated on the back.
    /// </summary>
    public class SaleDetailModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
