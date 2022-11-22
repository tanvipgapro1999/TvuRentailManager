using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library.Internal.DataAccess
{
    // internal mean it can not be seen or used by anything outside a library  
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private bool isClosed = false;
        private readonly IConfiguration _config;
        private readonly ILogger<SqlDataAccess> _logger;
        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }
        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }
        // LoadData : Take data from database in sql
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }
        public void SaveData<T>(string storedProcedure, T parameters, string connectionstringName)
        {
            string connectionString = GetConnectionString(connectionstringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public void StartTransaction(string connectionstringName)
        {
            string connectionString = GetConnectionString(connectionstringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            isClosed = false;
        }
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {

            List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            return rows;

        }
        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure, transaction: _transaction);

        }

      

        public void CommitTransaction()
        {
            // Commit all data
            // ? mean if instance null they will not work
            _transaction?.Commit();
            _connection?.Close();

            isClosed = true;
        }

        public void RollBackTransaction()
        {
            // delete all change we made instead of committing them
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }
        // Will do something to clean method
        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Commit transaction failed in the dispose method.");
                }
            }

            _transaction = null;
            _connection = null;
        }
        // Open connect/Start transaction method
        // Load using the transaction 
        // Save using the transaction
        // Close connection/stop transaction method
        // Dispose
    }
}
