using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    /// <summary>
    /// The interface from api to db layer, hiding the specific implementation of the database.
    /// SaleData interface.
    /// </summary>
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this SOLID/DRY/Better
            // Separate the bussiness logic

            #region \Comment - Steps for the proccess
            // (We didnt trust the front end so we will need to gather all the following informations)
            // --------------------------------------------------------------------
            // Start filling in the sale detail models we will save to the database
            // Fill in the available information
            // Create the sale model
            // Save the save model
            // Get the Id from the sale model
            // Finish filling in the sale detail models
            // Save the sale detail models
            #endregion
            ProductData products = new ProductData();
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            decimal sumSubTotal = 0, sumTax = 0;

            // the taxRate is stored as x% not 0.x % TODO: Fix on the config file
            var taxRate = ConfigHelper.GetTaxRate() / 100;  

            // mapping the ui models to the db models to be sent
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // Get the information about this product
                var productInfo = products.GetProductById(detail.ProductId);

                if (productInfo is null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;
                sumSubTotal += detail.PurchasePrice;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * taxRate;
                    sumTax += detail.Tax;
                }

                details.Add(detail);
            }

            // Create the Sale model
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = sumSubTotal,
                Tax = sumTax,
                Total = sumSubTotal + sumTax,
                CashierId = cashierId
            };

            // Save the sale model
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "TRMData");

            // Get the ID from the sale mode

            // Here the issue is that the stored procedure above should return the created Id but we dont capture it
            // instead of creating another stored procedure we can create another version of the generic LoadData that
            // has the stored procedure with a parameter specified as return type. Dapper gives the solution,
            // any other orm should give as well I suppose.
            sale.Id = sql.LoadData<int, dynamic>("spSale_Lookup", new {sale.CashierId, sale.SaleDate }, "TRMData").FirstOrDefault();

            // Finish filling in the sale detail models
            details.ForEach(x =>
            {
                x.SaleId = sale.Id;
                // Save the sale detail model
                sql.SaveData("dbo.spSaleDetail_Insert", x, "TRMData");
            });
        }
    }
}
