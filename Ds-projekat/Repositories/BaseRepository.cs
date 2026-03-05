using Ds_projekat.DBFactory;
using System.Data;
using System.Data.Common;

namespace Ds_projekat
{
    internal abstract class BaseRepository
    {
        protected readonly IdbProviderFactory Factory;

        protected BaseRepository()
        {
            Factory = DatabaseContext.Instance.Factory;
        }

        protected IDbConnection Open()
        {
            var conn = Factory.CreateConnection();
            conn.Open();
            return conn;
        }
    }
}