using System;

namespace Ds_projekat
{
    internal class MembershipType
    {
        public int MembershipTypeID { get; set; }
        public string PackageName { get; set; } = "";
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int MaxReservationHoursPerMonth { get; set; }
        public bool MeetingRoomAccess { get; set; }
        public int? MeetingRoomHoursPerMonth { get; set; }
    }

    internal class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public int MembershipTypeID { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipEndDate { get; set; }
        public string AccountStatus { get; set; } = "Active"; // Active/Paused/Expired
    }

    internal class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; } = "";
        public string AddressName { get; set; } = "";
        public string City { get; set; } = "";
        public string WorkingHours { get; set; } = "";
        public int MaxUsers { get; set; }
    }

    internal class Resource
    {
        public int ResourceID { get; set; }
        public int LocationID { get; set; }
        public string ResourceName { get; set; } = "";
        public string ResourceType { get; set; } = ""; // npr "Desk" ili "Room"
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }

    internal class DeskDetails
    {
        public int ResourceID { get; set; }
        public string DeskSubType { get; set; } = ""; // Hot/Dedicated
    }

    internal class RoomDetails
    {
        public int ResourceID { get; set; }
        public int Capacity { get; set; }
        public bool HasProjector { get; set; }
        public bool HasTV { get; set; }
        public bool HasBoard { get; set; }
        public bool HasOnlineEquipment { get; set; }
    }

    internal class Reservation
    {
        public int ReservationID { get; set; }
        public int UserID { get; set; }
        public int ResourceID { get; set; }
        public int? UsersCount { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string ReservationStatus { get; set; } = "Active"; // Active/Finished/Canceled
    }

    internal class Admin
    {
        public int UserID { get; set; }
        public string RoleName { get; set; } = "";
        public string Username { get; set; } = "";
        public string HashedPass { get; set; } = "";
    }
}