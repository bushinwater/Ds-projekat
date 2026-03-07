using Ds_projekat.Ds_projekat;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat.DBFactory
{
    internal sealed class DatabaseContext
    {
        private static readonly Lazy<DatabaseContext> _instance =
            new Lazy<DatabaseContext>(() => new DatabaseContext());

        public static DatabaseContext Instance => _instance.Value;

        public IdbProviderFactory Factory { get; }

        private DatabaseContext()
        {
            AppConfig.Instance.Load("config.txt");
            Factory = DbFactory.CreateProviderFactory(AppConfig.Instance.ConnectionString);
        }
    }
}
