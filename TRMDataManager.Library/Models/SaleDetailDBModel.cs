using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library.Models
{
    /// <summary>
    /// This model will be used as input to the store procedures. Has direct mapping to the 
    /// database fields on the table we are interested in.
    /// </summary>
    public class SaleDetailDBModel
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Tax { get; set; }
    }
}
