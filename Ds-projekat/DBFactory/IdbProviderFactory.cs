using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat
{
    internal interface IdbProviderFactory
    {
        IDbConnection CreateConnection();
        IDbCommand CreateCommand(string sql, IDbConnection connection);
        IDbDataParameter CreateParameter(string name, object value);
        string LastInsertIdSql {  get; } // Sql za uzimanje poslednjeg ID-a
    }
}
 