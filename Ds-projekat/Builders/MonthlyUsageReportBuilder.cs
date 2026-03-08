using System;
using System.Collections.Generic;
using System.Linq;

namespace Ds_projekat.Builders
{
    // Builder sklapa jednostavan mesecni izvestaj iz podataka koji vec postoje u sistemu.
    internal class MonthlyUsageReportBuilder
    {
        private List<User> _users = new List<User>();
        private List<MembershipType> _memberships = new List<MembershipType>();
        private List<Location> _locations = new List<Location>();
        private List<Resource> _resources = new List<Resource>();
        private List<Reservation> _reservations = new List<Reservation>();
        private int _year;
        private int _month;

        public MonthlyUsageReportBuilder ForMonth(int year, int month)
        {
            _year = year;
            _month = month;
            return this;
        }

        public MonthlyUsageReportBuilder WithUsers(List<User> users)
        {
            _users = users ?? new List<User>();
            return this;
        }

        public MonthlyUsageReportBuilder WithMemberships(List<MembershipType> memberships)
        {
            _memberships = memberships ?? new List<MembershipType>();
            return this;
        }

        public MonthlyUsageReportBuilder WithLocations(List<Location> locations)
        {
            _locations = locations ?? new List<Location>();
            return this;
        }

        public MonthlyUsageReportBuilder WithResources(List<Resource> resources)
        {
            _resources = resources ?? new List<Resource>();
            return this;
        }

        public MonthlyUsageReportBuilder WithReservations(List<Reservation> reservations)
        {
            _reservations = reservations ?? new List<Reservation>();
            return this;
        }

        public List<string> BuildLines()
        {
            string monthLabel = new DateTime(_year, _month, 1).ToString("MM.yyyy");
            List<string> lines = new List<string>();
            lines.Add("Section,Month,ID,Name,Category,Hours,Count,Percent,Details");

            // Sekcija po korisniku i clanarini.
            Dictionary<int, string> membershipNames = _memberships.ToDictionary(m => m.MembershipTypeID, m => m.PackageName);
            List<Reservation> monthlyReservations = _reservations
                .Where(r =>
                    !string.Equals(r.ReservationStatus, "Canceled", StringComparison.OrdinalIgnoreCase) &&
                    r.StartDateTime.Year == _year &&
                    r.StartDateTime.Month == _month)
                .ToList();

            foreach (User user in _users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName))
            {
                List<Reservation> userReservations = monthlyReservations.Where(r => r.UserID == user.UserID).ToList();
                double totalHours = userReservations.Sum(r => Math.Max(0, (r.EndDateTime - r.StartDateTime).TotalHours));
                string membershipName = membershipNames.ContainsKey(user.MembershipTypeID) ? membershipNames[user.MembershipTypeID] : "";

                lines.Add(
                    Escape("UserUsage") + "," +
                    Escape(monthLabel) + "," +
                    Escape(user.UserID.ToString()) + "," +
                    Escape(user.FirstName + " " + user.LastName) + "," +
                    Escape(membershipName) + "," +
                    Escape(totalHours.ToString("0.##")) + "," +
                    Escape(userReservations.Count.ToString()) + "," +
                    Escape("") + "," +
                    Escape(user.AccountStatus));
            }

            // Sekcija po resursu i lokaciji.
            Dictionary<int, string> locationNames = _locations.ToDictionary(l => l.LocationID, l => l.LocationName);
            int daysInMonth = DateTime.DaysInMonth(_year, _month);
            double totalMonthHours = daysInMonth * 24.0;

            foreach (Resource resource in _resources.OrderBy(r => r.ResourceType).ThenBy(r => r.ResourceName))
            {
                List<Reservation> resourceReservations = monthlyReservations.Where(r => r.ResourceID == resource.ResourceID).ToList();
                double usedHours = resourceReservations.Sum(r => Math.Max(0, (r.EndDateTime - r.StartDateTime).TotalHours));
                double percent = totalMonthHours <= 0 ? 0 : usedHours / totalMonthHours * 100;
                string locationName = locationNames.ContainsKey(resource.LocationID) ? locationNames[resource.LocationID] : "";

                lines.Add(
                    Escape("ResourceUsage") + "," +
                    Escape(monthLabel) + "," +
                    Escape(resource.ResourceID.ToString()) + "," +
                    Escape(resource.ResourceName) + "," +
                    Escape(resource.ResourceType) + "," +
                    Escape(usedHours.ToString("0.##")) + "," +
                    Escape(resourceReservations.Count.ToString()) + "," +
                    Escape(percent.ToString("0.##")) + "," +
                    Escape(locationName));
            }

            return lines;
        }

        private static string Escape(string value)
        {
            if (value == null)
                return "\"\"";

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
