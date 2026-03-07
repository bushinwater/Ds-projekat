using System;
using System.Collections.Generic;
using Ds_projekat.Repositories.Interfaces;

namespace Ds_projekat.Repositories
{
    internal class ResourceRepository : BaseRepository, IResourceRepository
    {
        public int Add(Resource r)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO Resources(LocationID, ResourceName, ResourceType, IsActive, Description)
  VALUES(@lid, @name, @type, @active, @desc);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@lid", r.LocationID));
            cmd.Parameters.Add(Factory.CreateParameter("@name", r.ResourceName));
            cmd.Parameters.Add(Factory.CreateParameter("@type", r.ResourceType));
            cmd.Parameters.Add(Factory.CreateParameter("@active", r.IsActive));
            cmd.Parameters.Add(Factory.CreateParameter("@desc", (object)r.Description ?? DBNull.Value));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(Resource r)
        {
            using var conn = Open();

            string sql =
@"UPDATE Resources
  SET LocationID=@lid, ResourceName=@name, ResourceType=@type, IsActive=@active, Description=@desc
  WHERE ResourceID=@id";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@lid", r.LocationID));
            cmd.Parameters.Add(Factory.CreateParameter("@name", r.ResourceName));
            cmd.Parameters.Add(Factory.CreateParameter("@type", r.ResourceType));
            cmd.Parameters.Add(Factory.CreateParameter("@active", r.IsActive));
            cmd.Parameters.Add(Factory.CreateParameter("@desc", (object)r.Description ?? DBNull.Value));
            cmd.Parameters.Add(Factory.CreateParameter("@id", r.ResourceID));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM Resources WHERE ResourceID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));
            return cmd.ExecuteNonQuery() > 0;
        }

        public Resource GetById(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Resources WHERE ResourceID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Resource
            {
                ResourceID = Convert.ToInt32(r["ResourceID"]),
                LocationID = Convert.ToInt32(r["LocationID"]),
                ResourceName = r["ResourceName"].ToString() ?? "",
                ResourceType = r["ResourceType"].ToString() ?? "",
                IsActive = DbHelpers.ReadBool(r["IsActive"]),
                Description = r["Description"].ToString()
            };
        }

        public List<Resource> GetAll()
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
                    Description = r["Description"].ToString()
                });
            }
            return list;
        }

        public List<Resource> GetByLocation(int locationId)
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
                    Description = r["Description"].ToString()
                });
            }
            return list;
        }

        public List<Resource> GetByType(string resourceType)
        {
            var list = new List<Resource>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Resources WHERE ResourceType=@type", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@type", resourceType));
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
                    Description = r["Description"].ToString()
                });
            }
            return list;
        }
    }
}