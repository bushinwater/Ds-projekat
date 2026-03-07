using System;
using System.Collections.Generic;

namespace Ds_projekat.Export
{
    internal class CsvExporterAdapter : IReportExporter
    {
        private readonly SimpleCsvWriter _csvWriter;

        public CsvExporterAdapter()
        {
            _csvWriter = new SimpleCsvWriter();
        }

        public void ExportUsers(string filePath, List<User> users)
        {
            List<string> lines = new List<string>();
            lines.Add("UserID,FirstName,LastName,Email,Phone,MembershipTypeID,MembershipStartDate,MembershipEndDate,AccountStatus");

            foreach (User u in users)
            {
                lines.Add(
                    Escape(u.UserID.ToString()) + "," +
                    Escape(u.FirstName) + "," +
                    Escape(u.LastName) + "," +
                    Escape(u.Email) + "," +
                    Escape(u.Phone) + "," +
                    Escape(u.MembershipTypeID.ToString()) + "," +
                    Escape(u.MembershipStartDate.ToString("yyyy-MM-dd")) + "," +
                    Escape(u.MembershipEndDate.ToString("yyyy-MM-dd")) + "," +
                    Escape(u.AccountStatus)
                );
            }

            _csvWriter.SaveLines(filePath, lines);
        }

        public void ExportReservations(string filePath, List<Reservation> reservations)
        {
            List<string> lines = new List<string>();
            lines.Add("ReservationID,UserID,ResourceID,UsersCount,StartDateTime,EndDateTime,ReservationStatus");

            foreach (Reservation r in reservations)
            {
                lines.Add(
                    Escape(r.ReservationID.ToString()) + "," +
                    Escape(r.UserID.ToString()) + "," +
                    Escape(r.ResourceID.ToString()) + "," +
                    Escape(r.UsersCount.HasValue ? r.UsersCount.Value.ToString() : "") + "," +
                    Escape(r.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss")) + "," +
                    Escape(r.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss")) + "," +
                    Escape(r.ReservationStatus)
                );
            }

            _csvWriter.SaveLines(filePath, lines);
        }

        public void ExportResources(string filePath, List<Resource> resources)
        {
            List<string> lines = new List<string>();
            lines.Add("ResourceID,LocationID,ResourceName,ResourceType,IsActive,Description");

            foreach (Resource r in resources)
            {
                lines.Add(
                    Escape(r.ResourceID.ToString()) + "," +
                    Escape(r.LocationID.ToString()) + "," +
                    Escape(r.ResourceName) + "," +
                    Escape(r.ResourceType) + "," +
                    Escape(r.IsActive.ToString()) + "," +
                    Escape(r.Description ?? "")
                );
            }

            _csvWriter.SaveLines(filePath, lines);
        }

        public void ExportLocations(string filePath, List<Location> locations)
        {
            List<string> lines = new List<string>();
            lines.Add("LocationID,LocationName,AddressName,City,WorkingHours,MaxUsers");

            foreach (Location l in locations)
            {
                lines.Add(
                    Escape(l.LocationID.ToString()) + "," +
                    Escape(l.LocationName) + "," +
                    Escape(l.AddressName) + "," +
                    Escape(l.City) + "," +
                    Escape(l.WorkingHours) + "," +
                    Escape(l.MaxUsers.ToString())
                );
            }

            _csvWriter.SaveLines(filePath, lines);
        }

        public void ExportMembershipTypes(string filePath, List<MembershipType> membershipTypes)
        {
            List<string> lines = new List<string>();
            lines.Add("MembershipTypeID,PackageName,Price,DurationDays,MaxReservationHoursPerMonth,MeetingRoomAccess,MeetingRoomHoursPerMonth");

            foreach (MembershipType m in membershipTypes)
            {
                lines.Add(
                    Escape(m.MembershipTypeID.ToString()) + "," +
                    Escape(m.PackageName) + "," +
                    Escape(m.Price.ToString()) + "," +
                    Escape(m.DurationDays.ToString()) + "," +
                    Escape(m.MaxReservationHoursPerMonth.ToString()) + "," +
                    Escape(m.MeetingRoomAccess.ToString()) + "," +
                    Escape(m.MeetingRoomHoursPerMonth.HasValue ? m.MeetingRoomHoursPerMonth.Value.ToString() : "")
                );
            }

            _csvWriter.SaveLines(filePath, lines);
        }

        private string Escape(string value)
        {
            if (value == null)
                return "\"\"";

            string escaped = value.Replace("\"", "\"\"");

            return "\"" + escaped + "\"";
        }
    }
}