using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat.DBFactory
{
    internal class SqlServerProviderFactory : IdbProviderFactory
    {
        private readonly string _cs;

        public SqlServerProviderFactory(string connectionString) => _cs = connectionString;

        public IDbConnection CreateConnection() => new SqlConnection(_cs);

        public IDbCommand CreateCommand(string sql, IDbConnection connection)
            => new SqlCommand(sql, (SqlConnection)connection);

        public IDbDataParameter CreateParameter(string name, object value)
            => new SqlParameter(name, value ?? System.DBNull.Value);

        public string LastInsertIdSql => "SELECT CAST(SCOPE_IDENTITY() AS int);";
    }
}
