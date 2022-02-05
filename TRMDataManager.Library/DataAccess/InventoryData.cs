using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        private readonly IConfiguration _config;

        public InventoryData(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Gets all inventory records from the database.
        /// </summary>
        /// <returns>A list of all inventory items.</returns>
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "TRMData");

            return output;
        }

        /// <summary>
        /// Insert an inventory record to the database.
        /// </summary>
        /// <param name="item">The item to be added on the inventory.</param>
        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            sql.SaveData("dbo.spInventory_Insert", item, "TRMData");
        }
    }
}
