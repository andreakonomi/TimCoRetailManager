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
    internal class SqlDataAccess : IDisposable
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

        /// <summary>
        /// Holds an instance of IdbConnection for talking to the database.
        /// </summary>
        private IDbConnection _connection;

        /// <summary>
        /// Holds a transaction instance so it can be approachable by a set of methods in the class
        /// to complete the transaction.
        /// </summary>
        private IDbTransaction _transaction;

        /// <summary>
        /// Starts of the connection and sql transaction.
        /// </summary>
        /// <param name="connectionStringName"></param>
        public void StartTranscation(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();

            isClosed = false;
        }

        public void SaveDataInTranscation<T>(string storedProcedure, T parameters)
        {
            //last argument associates the command with the transaction we are using
            _connection.Execute(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public List<T> LoadDataInTranscation<T, U>(string storedProcedure, U parameters)
        {
                List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

                return rows;
        }

        private bool isClosed = false;

        /// <summary>
        /// Apply the changes to the db (successful transaction).
        /// </summary>
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            isClosed = true;
        }

        /// <summary>
        /// Rollback all changes made by the current transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            isClosed = true;
        }

        public void Dispose()
        {
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    //TODO: Log this issue
                }
            }

            _transaction = null;
            _connection = null;
        }

        // Open connect/start transcation method
        // load using the transaction
        // save using the transaction
        // Close connection/stop transaction method
        // Dispose **
    }
}
