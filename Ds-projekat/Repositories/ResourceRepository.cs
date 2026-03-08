using Ds_projekat.Repositories;
using System;
using System.Collections.Generic;
using System.Data;

namespace Ds_projekat
{
    internal class ResourceRepository : BaseRepository, IResourceRepository
    {
        public int AddResource(Resource r, DeskDetails desk, RoomDetails room)
        {
            using var conn = Open();
            using var tx = conn.BeginTransaction();

            try
            {
                string sqlRes =
@"INSERT INTO Resources(LocationID, ResourceName, ResourceType, IsActive, Description)
  VALUES(@lid,@n,@t,@a,@d);
  " + Factory.LastInsertIdSql;

                int resourceId;
                using (var cmd = Factory.CreateCommand(sqlRes, conn))
                {
                    cmd.Transaction = tx;
                    cmd.Parameters.Add(Factory.CreateParameter("@lid", r.LocationID));
                    cmd.Parameters.Add(Factory.CreateParameter("@n", r.ResourceName));
                    cmd.Parameters.Add(Factory.CreateParameter("@t", r.ResourceType));
                    cmd.Parameters.Add(Factory.CreateParameter("@a", r.IsActive));
                    cmd.Parameters.Add(Factory.CreateParameter("@d", (object)r.Description ?? DBNull.Value));
                    resourceId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (desk != null)
                {
                    string sqlDesk = "INSERT INTO DeskDetails(ResourceID, DeskSubType) VALUES(@rid,@sub)";
                    using var cmdDesk = Factory.CreateCommand(sqlDesk, conn);
                    cmdDesk.Transaction = tx;
                    cmdDesk.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
                    cmdDesk.Parameters.Add(Factory.CreateParameter("@sub", desk.DeskSubType));
                    cmdDesk.ExecuteNonQuery();
                }

                if (room != null)
                {
                    string sqlRoom =
@"INSERT INTO RoomDetails(ResourceID, Capacity, HasProjector, HasTV, HasBoard, HasOnlineEquipment)
  VALUES(@rid,@cap,@p,@tv,@b,@oe)";
                    using var cmdRoom = Factory.CreateCommand(sqlRoom, conn);
                    cmdRoom.Transaction = tx;
                    cmdRoom.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
                    cmdRoom.Parameters.Add(Factory.CreateParameter("@cap", room.Capacity));
                    cmdRoom.Parameters.Add(Factory.CreateParameter("@p", room.HasProjector));
                    cmdRoom.Parameters.Add(Factory.CreateParameter("@tv", room.HasTV));
                    cmdRoom.Parameters.Add(Factory.CreateParameter("@b", room.HasBoard));
                    cmdRoom.Parameters.Add(Factory.CreateParameter("@oe", room.HasOnlineEquipment));
                    cmdRoom.ExecuteNonQuery();
                }

                tx.Commit();
                return resourceId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public bool UpdateResource(Resource r, DeskDetails desk, RoomDetails room)
        {
            using var conn = Open();
            using var tx = conn.BeginTransaction();

            try
            {
                string sqlRes =
@"UPDATE Resources
  SET LocationID=@lid, ResourceName=@n, ResourceType=@t, IsActive=@a, Description=@d
  WHERE ResourceID=@rid";

                using (var cmd = Factory.CreateCommand(sqlRes, conn))
                {
                    cmd.Transaction = tx;
                    cmd.Parameters.Add(Factory.CreateParameter("@lid", r.LocationID));
                    cmd.Parameters.Add(Factory.CreateParameter("@n", r.ResourceName));
                    cmd.Parameters.Add(Factory.CreateParameter("@t", r.ResourceType));
                    cmd.Parameters.Add(Factory.CreateParameter("@a", r.IsActive));
                    cmd.Parameters.Add(Factory.CreateParameter("@d", (object)r.Description ?? DBNull.Value));
                    cmd.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));
                    cmd.ExecuteNonQuery();
                }

                // Obriši stare detalje pa ubaci nove (najjednostavnije i stabilno)
                using (var delDesk = Factory.CreateCommand("DELETE FROM DeskDetails WHERE ResourceID=@rid", conn))
                {
                    delDesk.Transaction = tx;
                    delDesk.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));
                    delDesk.ExecuteNonQuery();
                }
                using (var delRoom = Factory.CreateCommand("DELETE FROM RoomDetails WHERE ResourceID=@rid", conn))
                {
                    delRoom.Transaction = tx;
                    delRoom.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));
                    delRoom.ExecuteNonQuery();
                }

                if (desk != null)
                {
                    using var insDesk = Factory.CreateCommand("INSERT INTO DeskDetails(ResourceID, DeskSubType) VALUES(@rid,@sub)", conn);
                    insDesk.Transaction = tx;
                    insDesk.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));
                    insDesk.Parameters.Add(Factory.CreateParameter("@sub", desk.DeskSubType));
                    insDesk.ExecuteNonQuery();
                }

                if (room != null)
                {
                    string sqlRoom =
@"INSERT INTO RoomDetails(ResourceID, Capacity, HasProjector, HasTV, HasBoard, HasOnlineEquipment)
  VALUES(@rid,@cap,@p,@tv,@b,@oe)";
                    using var insRoom = Factory.CreateCommand(sqlRoom, conn);
                    insRoom.Transaction = tx;
                    insRoom.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));
                    insRoom.Parameters.Add(Factory.CreateParameter("@cap", room.Capacity));
                    insRoom.Parameters.Add(Factory.CreateParameter("@p", room.HasProjector));
                    insRoom.Parameters.Add(Factory.CreateParameter("@tv", room.HasTV));
                    insRoom.Parameters.Add(Factory.CreateParameter("@b", room.HasBoard));
                    insRoom.Parameters.Add(Factory.CreateParameter("@oe", room.HasOnlineEquipment));
                    insRoom.ExecuteNonQuery();
                }

                tx.Commit();
                return true;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public bool DeleteResource(int resourceId)
        {
            using var conn = Open();
            using var tx = conn.BeginTransaction();
            try
            {
                // prvo detail tabele (FK)
                using (var cmd = Factory.CreateCommand("DELETE FROM DeskDetails WHERE ResourceID=@rid", conn))
                {
                    cmd.Transaction = tx;
                    cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = Factory.CreateCommand("DELETE FROM RoomDetails WHERE ResourceID=@rid", conn))
                {
                    cmd.Transaction = tx;
                    cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = Factory.CreateCommand("DELETE FROM Resources WHERE ResourceID=@rid", conn))
                {
                    cmd.Transaction = tx;
                    cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
                    int rows = cmd.ExecuteNonQuery();
                    tx.Commit();
                    return rows > 0;
                }
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public Resource GetResource(int resourceId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Resources WHERE ResourceID=@rid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Resource
            {
                ResourceID = Convert.ToInt32(r["ResourceID"]),
                LocationID = Convert.ToInt32(r["LocationID"]),
                ResourceName = r["ResourceName"].ToString() ?? "",
                ResourceType = r["ResourceType"].ToString() ?? "",
                IsActive = DbHelpers.ReadBool(r["IsActive"]),
                Description = DbHelpers.ReadNullableString(r["Description"])
            };
        }

        public DeskDetails GetDeskDetails(int resourceId)
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

        public RoomDetails GetRoomDetails(int resourceId)
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

        public List<Resource> GetAllResources()
        {
            var list = new List<Resource>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Resources", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Resource
                {
                    ResourceID = Convert.ToInt32(r["ResourceID"]),
                    LocationID = Convert.ToInt32(r["LocationID"]),
                    ResourceName = r["ResourceName"].ToString() ?? "",
                    ResourceType = r["ResourceType"].ToString() ?? "",
                    IsActive = DbHelpers.ReadBool(r["IsActive"]),
                    Description = DbHelpers.ReadNullableString(r["Description"])
                });
            }
            return list;
        }

        public List<Resource> GetResourcesByLocation(int locationId)
        {
            var list = new List<Resource>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Resources WHERE LocationID=@lid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@lid", locationId));
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Resource
                {
                    ResourceID = Convert.ToInt32(r["ResourceID"]),
                    LocationID = Convert.ToInt32(r["LocationID"]),
                    ResourceName = r["ResourceName"].ToString() ?? "",
                    ResourceType = r["ResourceType"].ToString() ?? "",
                    IsActive = DbHelpers.ReadBool(r["IsActive"]),
                    Description = DbHelpers.ReadNullableString(r["Description"])
                });
            }
            return list;
        }
    }
}
