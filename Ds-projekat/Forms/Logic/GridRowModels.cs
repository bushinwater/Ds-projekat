namespace Ds_projekat
{
    internal class UserGridRow
    {
        public int UserID { get; set; }
        public string User { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Membership { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    internal class MembershipGridRow
    {
        public int MembershipTypeID { get; set; }
        public string Membership { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int MaxReservationHoursPerMonth { get; set; }
        public bool MeetingRoomAccess { get; set; }
        public string MeetingRoomHours { get; set; }
    }

    internal class LocationGridRow
    {
        public int LocationID { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string WorkingHours { get; set; }
        public int MaxUsers { get; set; }
    }

    internal class ResourceGridRow
    {
        public int ResourceID { get; set; }
        public string ResourceTypeGroup { get; set; }
        public string Resource { get; set; }
        public string Location { get; set; }
        public string ResourceType { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }

    internal class ReservationGridRow
    {
        public int ReservationID { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public string User { get; set; }
        public string Resource { get; set; }
        public string Location { get; set; }
        public string UsersCount { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Status { get; set; }
        public string Occupancy { get; set; }
    }

    internal class AdminGridRow
    {
        public int UserID { get; set; }
        public string AdminUser { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
    }
}