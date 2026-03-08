using Ds_projekat.Repositories;
using System;
using System.Collections.Generic;

namespace Ds_projekat
{
    internal class ReservationRepository : BaseRepository, IReservationRepository
    {
        public int Add(Reservation r)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO Reservations(UserID, ResourceID, UsersCount, StartDateTime, EndDateTime, ReservationStatus)
  VALUES(@uid,@rid,@uc,@s,@e,@st);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@uid", r.UserID));
            cmd.Parameters.Add(Factory.CreateParameter("@rid", r.ResourceID));
            cmd.Parameters.Add(Factory.CreateParameter("@uc", (object?)r.UsersCount ?? DBNull.Value));
            cmd.Parameters.Add(Factory.CreateParameter("@s", r.StartDateTime));
            cmd.Parameters.Add(Factory.CreateParameter("@e", r.EndDateTime));
            cmd.Parameters.Add(Factory.CreateParameter("@st", r.ReservationStatus));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool UpdateStatus(int reservationId, string status)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand(
                "UPDATE Reservations SET ReservationStatus=@st WHERE ReservationID=@id", conn);

            cmd.Parameters.Add(Factory.CreateParameter("@st", status));
            cmd.Parameters.Add(Factory.CreateParameter("@id", reservationId));
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int reservationId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM Reservations WHERE ReservationID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", reservationId));
            return cmd.ExecuteNonQuery() > 0;
        }

        public Reservation? GetById(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Reservations WHERE ReservationID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public List<Reservation> GetByUser(int userId)
        {
            var list = new List<Reservation>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Reservations WHERE UserID=@uid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@uid", userId));
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public List<Reservation> GetByResource(int resourceId)
        {
            var list = new List<Reservation>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Reservations WHERE ResourceID=@rid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        // Overlap logika: (Start < existingEnd) AND (End > existingStart)
        public bool HasOverlap(int resourceId, DateTime start, DateTime end, int? ignoreReservationId = null)
        {
            using var conn = Open();

            string sql =
@"SELECT COUNT(*) 
  FROM Reservations
  WHERE ResourceID=@rid
    AND ReservationStatus <> 'Canceled'
    AND (@start < EndDateTime) AND (@end > StartDateTime)";

            if (ignoreReservationId.HasValue)
                sql += " AND ReservationID <> @ignoreId";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
            cmd.Parameters.Add(Factory.CreateParameter("@start", start));
            cmd.Parameters.Add(Factory.CreateParameter("@end", end));
            if (ignoreReservationId.HasValue)
                cmd.Parameters.Add(Factory.CreateParameter("@ignoreId", ignoreReservationId.Value));

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        private static Reservation Map(dynamic r)
        {
            return new Reservation
            {
                ReservationID = Convert.ToInt32(r["ReservationID"]),
                UserID = Convert.ToInt32(r["UserID"]),
                ResourceID = Convert.ToInt32(r["ResourceID"]),
                UsersCount = DbHelpers.ReadNullableInt(r["UsersCount"]),
                StartDateTime = Convert.ToDateTime(r["StartDateTime"]),
                EndDateTime = Convert.ToDateTime(r["EndDateTime"]),
                ReservationStatus = r["ReservationStatus"].ToString() ?? "Active"
            };
        }
        public List<Reservation> GetAll()
        {
            List<Reservation> list = new List<Reservation>();

            using var con = Open();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
        SELECT ReservationID, UserID, ResourceID, UsersCount,
               StartDateTime, EndDateTime, ReservationStatus
        FROM Reservations
        ORDER BY ReservationID DESC";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Reservation r = new Reservation
                {
                    ReservationID = Convert.ToInt32(reader["ReservationID"]),
                    UserID = Convert.ToInt32(reader["UserID"]),
                    ResourceID = Convert.ToInt32(reader["ResourceID"]),
                    UsersCount = reader["UsersCount"] == DBNull.Value ? null : Convert.ToInt32(reader["UsersCount"]),
                    StartDateTime = Convert.ToDateTime(reader["StartDateTime"]),
                    EndDateTime = Convert.ToDateTime(reader["EndDateTime"]),
                    ReservationStatus = reader["ReservationStatus"].ToString()
                };

                list.Add(r);
            }

            return list;
        }



        public bool UserHasReservations(int userId)
        {
            using (var conn = Open())
            using (var cmd = Factory.CreateCommand("SELECT COUNT(*) FROM Reservations WHERE UserID=@uid", conn))
            {
                cmd.Parameters.Add(Factory.CreateParameter("@uid", userId));
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public bool DeleteByUserId(int userId)
        {
            using (var conn = Open())
            using (var cmd = Factory.CreateCommand("DELETE FROM Reservations WHERE UserID=@uid", conn))
            {
                cmd.Parameters.Add(Factory.CreateParameter("@uid", userId));
                return cmd.ExecuteNonQuery() >= 0;
            }
        }

        public bool LocationResourcesHaveReservations(int locationId)
        {
            using (var conn = Open())
            using (var cmd = Factory.CreateCommand(
                @"SELECT COUNT(*)
          FROM Reservations
          WHERE ResourceID IN (
              SELECT ResourceID FROM Resources WHERE LocationID=@lid
          )", conn))
            {
                cmd.Parameters.Add(Factory.CreateParameter("@lid", locationId));
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public bool DeleteByLocationId(int locationId)
        {
            using (var conn = Open())
            using (var cmd = Factory.CreateCommand(
                @"DELETE FROM Reservations
          WHERE ResourceID IN (
              SELECT ResourceID FROM Resources WHERE LocationID=@lid
          )", conn))
            {
                cmd.Parameters.Add(Factory.CreateParameter("@lid", locationId));
                return cmd.ExecuteNonQuery() >= 0;
            }
        }

        public bool ResourceHasReservations(int resourceId)
        {
            using (var conn = Open())
            using (var cmd = Factory.CreateCommand("SELECT COUNT(*) FROM Reservations WHERE ResourceID=@rid", conn))
            {
                cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public bool DeleteByResourceId(int resourceId)
        {
            using (var conn = Open())
            using (var cmd = Factory.CreateCommand("DELETE FROM Reservations WHERE ResourceID=@rid", conn))
            {
                cmd.Parameters.Add(Factory.CreateParameter("@rid", resourceId));
                return cmd.ExecuteNonQuery() >= 0;
            }
        }

        public double GetReservedHoursForUserInMonth(int userId, DateTime monthDate)
        {
            using (var conn = Open())
            {
                DateTime monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);
                DateTime nextMonthStart = monthStart.AddMonths(1);

                string sql =
        @"SELECT StartDateTime, EndDateTime
  FROM Reservations
  WHERE UserID=@uid
    AND ReservationStatus <> 'Canceled'
    AND StartDateTime >= @monthStart
    AND StartDateTime < @nextMonthStart";

                using (var cmd = Factory.CreateCommand(sql, conn))
                {
                    cmd.Parameters.Add(Factory.CreateParameter("@uid", userId));
                    cmd.Parameters.Add(Factory.CreateParameter("@monthStart", monthStart));
                    cmd.Parameters.Add(Factory.CreateParameter("@nextMonthStart", nextMonthStart));

                    using (var reader = cmd.ExecuteReader())
                    {
                        double totalHours = 0;

                        while (reader.Read())
                        {
                            DateTime start = Convert.ToDateTime(reader["StartDateTime"]);
                            DateTime end = Convert.ToDateTime(reader["EndDateTime"]);
                            totalHours += (end - start).TotalHours;
                        }

                        return totalHours;
                    }
                }
            }
        }
    }
}