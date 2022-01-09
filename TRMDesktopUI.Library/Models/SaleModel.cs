using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// Includes the list of details of one sale transaction.
    /// </summary>
    public class SaleModel
    {
        public List<SaleDetailModel> SaleDetails { get; set; } = new List<SaleDetailModel>();
    }
}
