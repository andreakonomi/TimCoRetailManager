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
    /// The interface from api to db, hiding the specific implementation of the database.
    /// ProductData interface.
    /// </summary>
    public class ProductData
    {
        /// <summary>
        /// Gets all products from the database.
        /// </summary>
        /// <returns></returns>
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");

            return output;
        }

        /// <summary>
        /// Gets the specific product from the database matching the given id. Or null if no product is found.
        /// </summary>
        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();

            return output;

        }
    }
}
