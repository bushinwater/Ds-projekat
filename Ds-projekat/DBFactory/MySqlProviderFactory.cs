using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat.DBFactory
{
    internal class MySqlProviderFactory : IdbProviderFactory
    {
        private readonly string _cs;

        public MySqlProviderFactory(string connectionString) => _cs = connectionString;

        public IDbConnection CreateConnection() => new MySqlConnection(_cs);

        public IDbCommand CreateCommand(string sql, IDbConnection connection)
            => new MySqlCommand(sql, (MySqlConnection)connection);

        public IDbDataParameter CreateParameter(string name, object value)
            => new MySqlParameter(name, value ?? System.DBNull.Value);

        public string LastInsertIdSql => "SELECT LAST_INSERT_ID();";
    }
}
