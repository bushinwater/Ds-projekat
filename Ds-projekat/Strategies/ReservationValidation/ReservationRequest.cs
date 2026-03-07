using System;

namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class ReservationRequest
    {
        public int UserID { get; set; }
        public int ResourceID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? UsersCount { get; set; }
        public int? IgnoreReservationId { get; set; }
    }
}