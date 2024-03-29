﻿using Microsoft.Extensions.Configuration;
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
    /// Abstracts away the specifics of the db implementaion from the consumer.
    /// </summary>
    public class UserData
    {
        private readonly IConfiguration _config;

        public UserData(IConfiguration config)
        {
            _config = config;
        }

        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var p = new { Id = id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");

            return output;
        }
    }
}
