using Ds_projekat.DBFactory;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat
{

    namespace Ds_projekat
    {
        internal static class DbFactory
        {
            public static IdbProviderFactory CreateProviderFactory(string connectionString)
            {
                var s = connectionString.ToLowerInvariant();

                // MySQL tipično ima user/uid + password/pwd
                if (s.Contains("server=") && (s.Contains("user=") || s.Contains("uid=")) &&
                    (s.Contains("password=") || s.Contains("pwd=")))
                {
                    return new MySqlProviderFactory(connectionString);
                }

                // SQL Server / LocalDB
                return new SqlServerProviderFactory(connectionString);
            }
        }
    }
}
