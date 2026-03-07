using System;
using Ds_projekat.Repositories.Interfaces;

namespace Ds_projekat.Repositories
{
    internal class RoomDetailsRepository : BaseRepository, IRoomDetailsRepository
    {
        public int Add(RoomDetails r)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO RoomDetails(ResourceID, Capacity, HasProjector, HasTV, HasBoard, HasOnlineEquipment)
  VALUES(@rid, @cap, @proj, @tv, @board, @online);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));
            cmd.Parameters.Add(Factory.CreateParameter("@cap", r.Capacity));
            cmd.Parameters.Add(Factory.CreateParameter("@proj", r.HasProjector));
            cmd.Parameters.Add(Factory.CreateParameter("@tv", r.HasTV));
            cmd.Parameters.Add(Factory.CreateParameter("@board", r.HasBoard));
            cmd.Parameters.Add(Factory.CreateParameter("@online", r.HasOnlineEquipment));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(RoomDetails r)
        {
            using var conn = Open();

            string sql =
@"UPDATE RoomDetails
  SET Capacity=@cap, HasProjector=@proj, HasTV=@tv, HasBoard=@board, HasOnlineEquipment=@online
  WHERE ResourceID=@rid";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@cap", r.Capacity));
            cmd.Parameters.Add(Factory.CreateParameter("@proj", r.HasProjector));
            cmd.Parameters.Add(Factory.CreateParameter("@tv", r.HasTV));
            cmd.Parameters.Add(Factory.CreateParameter("@board", r.HasBoard));
            cmd.Parameters.Add(Factory.CreateParameter("@online", r.HasOnlineEquipment));
            cmd.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int resourceId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM RoomDetails WHERE ResourceID=@rid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
            return cmd.ExecuteNonQuery() > 0;
        }

        public RoomDetails GetByResourceId(int resourceId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM RoomDetails WHERE ResourceID=@rid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new RoomDetails
            {
                ResourceID = Convert.ToInt32(r["ResourceID"]),
                Capacity = Convert.ToInt32(r["Capacity"]),
                HasProjector = DbHelpers.ReadBool(r["HasProjector"]),
                HasTV = DbHelpers.ReadBool(r["HasTV"]),
                HasBoard = DbHelpers.ReadBool(r["HasBoard"]),
                HasOnlineEquipment = DbHelpers.ReadBool(r["HasOnlineEquipment"])
            };
        }
    }
}