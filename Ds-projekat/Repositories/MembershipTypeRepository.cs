using Ds_projekat.Repositories;
using Ds_projekat.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Ds_projekat
{
    internal class MembershipTypeRepository : BaseRepository, IMemershipTypeRepository
    {
        public int Add(MembershipType mt)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO MembershipTypes(PackageName, Price, DurationDays, MaxReservationHoursPerMonth, MeetingRoomAccess, MeetingRoomHoursPerMonth)
  VALUES(@name,@price,@dur,@maxh,@mra,@mrh);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@name", mt.PackageName));
            cmd.Parameters.Add(Factory.CreateParameter("@price", mt.Price));
            cmd.Parameters.Add(Factory.CreateParameter("@dur", mt.DurationDays));
            cmd.Parameters.Add(Factory.CreateParameter("@maxh", mt.MaxReservationHoursPerMonth));
            cmd.Parameters.Add(Factory.CreateParameter("@mra", mt.MeetingRoomAccess));
            cmd.Parameters.Add(Factory.CreateParameter("@mrh", (object)mt.MeetingRoomHoursPerMonth ?? DBNull.Value));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(MembershipType mt)
        {
            using var conn = Open();

            string sql =
@"UPDATE MembershipTypes
  SET PackageName=@name, Price=@price, DurationDays=@dur,
      MaxReservationHoursPerMonth=@maxh, MeetingRoomAccess=@mra, MeetingRoomHoursPerMonth=@mrh
  WHERE MembershipTypeID=@id";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@name", mt.PackageName));
            cmd.Parameters.Add(Factory.CreateParameter("@price", mt.Price));
            cmd.Parameters.Add(Factory.CreateParameter("@dur", mt.DurationDays));
            cmd.Parameters.Add(Factory.CreateParameter("@maxh", mt.MaxReservationHoursPerMonth));
            cmd.Parameters.Add(Factory.CreateParameter("@mra", mt.MeetingRoomAccess));
            cmd.Parameters.Add(Factory.CreateParameter("@mrh", (object?)mt.MeetingRoomHoursPerMonth ?? DBNull.Value));
            cmd.Parameters.Add(Factory.CreateParameter("@id", mt.MembershipTypeID));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM MembershipTypes WHERE MembershipTypeID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));
            return cmd.ExecuteNonQuery() > 0;
        }

        public MembershipType GetById(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM MembershipTypes WHERE MembershipTypeID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new MembershipType
            {
                MembershipTypeID = Convert.ToInt32(r["MembershipTypeID"]),
                PackageName = r["PackageName"].ToString() ?? "",
                Price = Convert.ToDecimal(r["Price"]),
                DurationDays = Convert.ToInt32(r["DurationDays"]),
                MaxReservationHoursPerMonth = Convert.ToInt32(r["MaxReservationHoursPerMonth"]),
                MeetingRoomAccess = DbHelpers.ReadBool(r["MeetingRoomAccess"]),
                MeetingRoomHoursPerMonth = DbHelpers.ReadNullableInt(r["MeetingRoomHoursPerMonth"])
            };
        }

        public List<MembershipType> GetAll()
        {
            var list = new List<MembershipType>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM MembershipTypes", conn);
            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new MembershipType
                {
                    MembershipTypeID = Convert.ToInt32(r["MembershipTypeID"]),
                    PackageName = r["PackageName"].ToString() ?? "",
                    Price = Convert.ToDecimal(r["Price"]),
                    DurationDays = Convert.ToInt32(r["DurationDays"]),
                    MaxReservationHoursPerMonth = Convert.ToInt32(r["MaxReservationHoursPerMonth"]),
                    MeetingRoomAccess = DbHelpers.ReadBool(r["MeetingRoomAccess"]),
                    MeetingRoomHoursPerMonth = DbHelpers.ReadNullableInt(r["MeetingRoomHoursPerMonth"])
                });
            }
            return list;
        }
    }
}