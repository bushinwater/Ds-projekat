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
    }
}