using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRMDataManager.Library.Models
{
    /// <summary>
    /// A model for posting sale data to the db.
    /// </summary>
    public class SaleModel
    {
        //wont initialise it so we check when we receive it from the front. If is null problem.
        public List<SaleDetailModel> SaleDetails { get; set; }
    }
}