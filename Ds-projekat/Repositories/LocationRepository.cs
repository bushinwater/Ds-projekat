using System;
using System.Collections.Generic;

namespace Ds_projekat
{
    internal class LocationRepository : BaseRepository, ILocationRepository
    {
        public int Add(Location l)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO Locations(LocationName, AddressName, City, WorkingHours, MaxUsers)
  VALUES(@n,@a,@c,@w,@m);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@n", l.LocationName));
            cmd.Parameters.Add(Factory.CreateParameter("@a", l.AddressName));
            cmd.Parameters.Add(Factory.CreateParameter("@c", l.City));
            cmd.Parameters.Add(Factory.CreateParameter("@w", l.WorkingHours));
            cmd.Parameters.Add(Factory.CreateParameter("@m", l.MaxUsers));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(Location l)
        {
            using var conn = Open();
            string sql =
@"UPDATE Locations
  SET LocationName=@n, AddressName=@a, City=@c, WorkingHours=@w, MaxUsers=@m
  WHERE LocationID=@id";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@n", l.LocationName));
            cmd.Parameters.Add(Factory.CreateParameter("@a", l.AddressName));
            cmd.Parameters.Add(Factory.CreateParameter("@c", l.City));
            cmd.Parameters.Add(Factory.CreateParameter("@w", l.WorkingHours));
            cmd.Parameters.Add(Factory.CreateParameter("@m", l.MaxUsers));
            cmd.Parameters.Add(Factory.CreateParameter("@id", l.LocationID));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM Locations WHERE LocationID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));
            return cmd.ExecuteNonQuery() > 0;
        }

        public Location? GetById(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Locations WHERE LocationID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Location
            {
                LocationID = Convert.ToInt32(r["LocationID"]),
                LocationName = r["LocationName"].ToString() ?? "",
                AddressName = r["AddressName"].ToString() ?? "",
                City = r["City"].ToString() ?? "",
                WorkingHours = r["WorkingHours"].ToString() ?? "",
                MaxUsers = Convert.ToInt32(r["MaxUsers"])
            };
        }

        public List<Location> GetAll()
        {
            var list = new List<Location>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Locations", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Location
                {
                    LocationID = Convert.ToInt32(r["LocationID"]),
                    LocationName = r["LocationName"].ToString() ?? "",
                    AddressName = r["AddressName"].ToString() ?? "",
                    City = r["City"].ToString() ?? "",
                    WorkingHours = r["WorkingHours"].ToString() ?? "",
                    MaxUsers = Convert.ToInt32(r["MaxUsers"])
                });
            }
            return list;
        }
    }
}