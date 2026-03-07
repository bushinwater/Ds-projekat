using System;
using System.Collections.Generic;
using Ds_projekat.Repositories.Interfaces;

namespace Ds_projekat.Repositories
{
    internal class DeskDetailsRepository : BaseRepository, IDeskDetailsRepository
    {
        public int Add(DeskDetails d)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO DeskDetails(ResourceID, DeskSubType)
  VALUES(@rid, @subtype);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", d.ResourceID));
            cmd.Parameters.Add(Factory.CreateParameter("@subtype", d.DeskSubType));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(DeskDetails d)
        {
            using var conn = Open();

            string sql =
@"UPDATE DeskDetails
  SET DeskSubType=@subtype
  WHERE ResourceID=@rid";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@subtype", d.DeskSubType));
            cmd.Parameters.Add(Factory.CreateParameter("@rid", d.ResourceID));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int resourceId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM DeskDetails WHERE ResourceID=@rid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
            return cmd.ExecuteNonQuery() > 0;
        }

        public DeskDetails GetByResourceId(int resourceId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM DeskDetails WHERE ResourceID=@rid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new DeskDetails
            {
                ResourceID = Convert.ToInt32(r["ResourceID"]),
                DeskSubType = r["DeskSubType"].ToString() ?? ""
            };
        }
    }
}