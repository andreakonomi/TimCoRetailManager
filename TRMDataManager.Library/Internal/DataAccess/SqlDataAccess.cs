using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace TRMDataManager.Library.Internal.DataAccess
{
    /// <summary>
    /// Implements the low level connection to the db. Which will be hidden from the consumer.
    /// </summary>
    internal class SqlDataAccess
    {
        /// <summary>
        /// Gets the connection string from the web.config for the given name. Grabs the config file
        /// of the caller application that has summoned it.
        /// </summary>
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Allows us to load data from the database.
        /// </summary>
        /// <param name="connectionStringName">The name of the config section for the conn string.</param>
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                //remember you pass to dapper parameters as anonymus objects
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
