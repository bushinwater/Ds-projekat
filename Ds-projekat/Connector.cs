using System;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using MySqlConnector;
namespace Ds_projekat
{
    public class Connector
    {
        // Singleton logika: čuvamo jednu instancu klase
        private static Connector _instance = null;
        private static readonly object _lock = new object();

        public string BrandName { get; private set; }
        public string ConnectionString { get; private set; }

        // Privatni konstruktor - čita config.txt samo jednom pri pokretanju
        private Connector()
        {
            // Putanja do config.txt fajla u Debug/Release folderu
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                // Prva linija je naziv brenda (npr. HubSpace)
                BrandName = lines.Length > 0 ? lines[0] : "HubSpace";

                // Druga linija je connection string za bazu
                ConnectionString = lines.Length > 1 ? lines[1] : "";
            }
            else
            {
                // Ako fajl ne postoji, aplikacija će javiti grešku odmah
                throw new FileNotFoundException("Kritična greška: config.txt nije pronađen!");
            }
        }

        // Property preko kojeg pristupamo konektoru bilo gde u aplikaciji
        public static Connector Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Connector();
                    return _instance;
                }
            }
        }

        // Factory Method: Na osnovu teksta u connection stringu bira drajver za bazu
        public IDbConnection CreateConnection()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new Exception("Connection string je prazan u config.txt!");

            // Prepoznavanje MSSQL-a (obično sadrži "Initial Catalog" ili "Data Source")
            if (ConnectionString.Contains("Initial Catalog") || ConnectionString.Contains("Data Source"))
            {
                return new SqlConnection(ConnectionString);
            }
            // Prepoznavanje MySQL-a (obično sadrži "server=" ili "uid=")
            else if (ConnectionString.Contains("server=") || ConnectionString.Contains("Database="))
            {
                return new MySqlConnection(ConnectionString);
            }
            else
            {
                throw new NotSupportedException("Format baze u config.txt nije prepoznat (mora biti MSSQL ili MySQL).");
            }
        }
    }
}
