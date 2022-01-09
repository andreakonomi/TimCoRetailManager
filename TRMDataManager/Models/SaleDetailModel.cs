using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRMDataManager.Models
{
    /// <summary>
    /// A model for posting sale detail to the db from the api.
    /// </summary>
    public class SaleDetailModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}