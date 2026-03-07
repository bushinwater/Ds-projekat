using System;
using System.Collections.Generic;

namespace Ds_projekat
{
    internal interface IReservationRepository
    {
        int Add(Reservation r);
        bool UpdateStatus(int reservationId, string status); // Active/Finished/Canceled
        bool Delete(int reservationId);

        Reservation? GetById(int id);
        List<Reservation> GetByUser(int userId);
        List<Reservation> GetByResource(int resourceId);

        bool HasOverlap(int resourceId, DateTime start, DateTime end, int? ignoreReservationId = null);
    }
}