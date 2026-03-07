using Ds_projekat.Ds_projekat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat
{
    internal sealed class DataBase
    {
        private static readonly Lazy<DataBase> _instance = new Lazy<DataBase>(() => new DataBase());
        public static DataBase Instance = _instance.Value;

        private DataBase() { }

        public IDbConnection OpenConnection()
        {
            AppConfig.Instance.Load("config.txt");

            var factory = DbFactory.CreateProviderFactory(AppConfig.Instance.ConnectionString);
            var conn = factory.CreateConnection();
            conn.Open();
            return conn;
        }

        public bool TestConnection(out string message)
        {
            try
            {
                // using pravi objekat i automatski ga zatvara i oslobadja kada izadje iz bloka
                using (var conn = OpenConnection())
                {
                    message = "Connection successful";
                    return true;
                }
            }
            catch (Exception e)
            {
                message = "Error: " + e.Message;
                return false;
            }
        }
    }
}
