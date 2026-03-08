using Ds_projekat.Repositories;
using Ds_projekat.Repositories.Interfaces;
using Ds_projekat.Strategies.ReservationValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ds_projekat.Services
{
    internal class ReservationFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMembershipTypeRepository _membershipRepository;

        public ReservationFacade()
        {
            _userRepository = new UserRepository();
            _resourceRepository = new ResourceRepository();
            _reservationRepository = new ReservationRepository();
            _locationRepository = new LocationRepository();
            _membershipRepository = new MembershipTypeRepository();
        }

        public ServiceResult CreateReservation(int userId, int resourceId, DateTime start, DateTime end, int? usersCount = null)
        {
            try
            {
                ServiceResult timingResult = ValidateReservationTiming(userId, resourceId, start, end, usersCount, null);
                if (!timingResult.Success)
                    return timingResult;

                ReservationRequest request = BuildRequest(userId, resourceId, start, end, usersCount, null);
                ReservationValidationResult validationResult = ValidateRequest(request);

                if (!validationResult.IsValid)
                    return ServiceResult.Fail(validationResult.Message);

                Reservation reservation = new Reservation
                {
                    UserID = userId,
                    ResourceID = resourceId,
                    UsersCount = usersCount,
                    StartDateTime = start,
                    EndDateTime = end,
                    ReservationStatus = "Active"
                };

                int newId = _reservationRepository.Add(reservation);

                return ServiceResult.Ok("Rezervacija je uspesno kreirana.", newId);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri kreiranju rezervacije: " + ex.Message);
            }
        }

        public ServiceResult CancelReservation(int reservationId)
        {
            try
            {
                Reservation reservation = _reservationRepository.GetById(reservationId);
                if (reservation == null)
                    return ServiceResult.Fail("Reservation doesnt exist.");

                bool updated = _reservationRepository.UpdateStatus(reservationId, "Canceled");

                if (!updated)
                    return ServiceResult.Fail("Reservation is not canceled.");

                return ServiceResult.Ok("Reservation canceled successfuly.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Error while canceling reservatin: " + ex.Message);
            }
        }

        public ServiceResult FinishReservation(int reservationId)
        {
            try
            {
                Reservation reservation = _reservationRepository.GetById(reservationId);
                if (reservation == null)
                    return ServiceResult.Fail("Reservation doesnt exist.");

                bool updated = _reservationRepository.UpdateStatus(reservationId, "Finished");

                if (!updated)
                    return ServiceResult.Fail("Reservation is not completed.");

                return ServiceResult.Ok("Reservation is marked completed.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Error while finishing reservation: " + ex.Message);
            }
        }

        public List<Reservation> GetReservationsForUser(int userId)
        {
            return _reservationRepository.GetByUser(userId);
        }

        public ServiceResult UpdateReservation(int reservationId, int userId, int resourceId, DateTime start, DateTime end, int? usersCount, string status)
        {
            try
            {
                Reservation existing = _reservationRepository.GetById(reservationId);
                if (existing == null)
                    return ServiceResult.Fail("Rezervacija ne postoji.");

                ServiceResult timingResult = ValidateReservationTiming(userId, resourceId, start, end, usersCount, reservationId);
                if (!timingResult.Success)
                    return timingResult;

                ReservationRequest request = BuildRequest(userId, resourceId, start, end, usersCount, reservationId);
                ReservationValidationResult validationResult = ValidateRequest(request);

                if (!validationResult.IsValid)
                    return ServiceResult.Fail(validationResult.Message);

                Reservation reservation = new Reservation
                {
                    ReservationID = reservationId,
                    UserID = userId,
                    ResourceID = resourceId,
                    UsersCount = usersCount,
                    StartDateTime = start,
                    EndDateTime = end,
                    ReservationStatus = NormalizeStatus(status, existing.ReservationStatus)
                };

                bool updated = _reservationRepository.Update(reservation);
                if (!updated)
                    return ServiceResult.Fail("Rezervacija nije azurirana.");

                return ServiceResult.Ok("Rezervacija je uspesno azurirana.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju rezervacije: " + ex.Message);
            }
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationRepository.GetAll();
        }

        public Reservation GetReservationById(int reservationId)
        {
            return _reservationRepository.GetById(reservationId);
        }

        public List<Reservation> GetReservationsForResource(int resourceId)
        {
            return _reservationRepository.GetByResource(resourceId);
        }

        public ServiceResult CanReserve(int userId, int resourceId, DateTime start, DateTime end, int? usersCount = null)
        {
            try
            {
                ServiceResult timingResult = ValidateReservationTiming(userId, resourceId, start, end, usersCount, null);
                if (!timingResult.Success)
                    return timingResult;

                ReservationRequest request = BuildRequest(userId, resourceId, start, end, usersCount, null);
                ReservationValidationResult validationResult = ValidateRequest(request);

                if (!validationResult.IsValid)
                    return ServiceResult.Fail(validationResult.Message);

                return ServiceResult.Ok("Rezervacija moze da se napravi.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri proveri rezervacije: " + ex.Message);
            }
        }

        private ReservationRequest BuildRequest(int userId, int resourceId, DateTime start, DateTime end, int? usersCount, int? ignoreReservationId)
        {
            return new ReservationRequest
            {
                UserID = userId,
                ResourceID = resourceId,
                StartDateTime = start,
                EndDateTime = end,
                UsersCount = usersCount,
                IgnoreReservationId = ignoreReservationId
            };
        }

        private ReservationValidationResult ValidateRequest(ReservationRequest request)
        {
            ReservationValidator validator = new ReservationValidator();
            validator.AddStrategy(new UserActiveValidationStrategy());
            validator.AddStrategy(new ResourceActiveValidationStrategy());
            validator.AddStrategy(new OverlapValidationStrategy());
            validator.AddStrategy(new RoomCapacityValidationStrategy());
            return validator.Validate(request);
        }

        // Osnovna poslovna pravila za vreme, clanarinu i radno vreme pre upisa rezervacije.
        private ServiceResult ValidateReservationTiming(int userId, int resourceId, DateTime start, DateTime end, int? usersCount, int? ignoreReservationId)
        {
            if (userId <= 0)
                return ServiceResult.Fail("Izabrani korisnik nije validan.");

            if (resourceId <= 0)
                return ServiceResult.Fail("Izabrani resurs nije validan.");

            if (end <= start)
                return ServiceResult.Fail("Vreme zavrsetka mora biti posle vremena pocetka.");

            if (usersCount.HasValue && usersCount.Value <= 0)
                return ServiceResult.Fail("Broj korisnika mora biti veci od 0.");

            if (start.Date != end.Date)
                return ServiceResult.Fail("Rezervacija mora biti u okviru jednog dana.");

            Resource resource = _resourceRepository.GetResource(resourceId);
            if (resource == null)
                return ServiceResult.Fail("Resurs ne postoji.");

            Location location = _locationRepository.GetById(resource.LocationID);
            if (location == null)
                return ServiceResult.Fail("Lokacija za izabrani resurs ne postoji.");

            ServiceResult workingHoursResult = ValidateWorkingHours(location, start, end);
            if (!workingHoursResult.Success)
                return workingHoursResult;

            ServiceResult meetingRoomResult = ValidateMeetingRoomAccess(userId, resource, start, end, ignoreReservationId);
            if (!meetingRoomResult.Success)
                return meetingRoomResult;

            ServiceResult monthlyHoursResult = ValidateMonthlyHours(userId, start, end, ignoreReservationId);
            if (!monthlyHoursResult.Success)
                return monthlyHoursResult;

            return ServiceResult.Ok("");
        }

        private ServiceResult ValidateWorkingHours(Location location, DateTime start, DateTime end)
        {
            TimeSpan workStart;
            TimeSpan workEnd;

            if (!TryGetWorkingHoursForDate(location.WorkingHours, start.DayOfWeek, out workStart, out workEnd))
                return ServiceResult.Fail("Lokacija nema definisano radno vreme za izabrani dan.");

            if (start.TimeOfDay < workStart || end.TimeOfDay > workEnd)
                return ServiceResult.Fail("Rezervacija mora biti u okviru radnog vremena lokacije.");

            return ServiceResult.Ok("");
        }

        private ServiceResult ValidateMonthlyHours(int userId, DateTime start, DateTime end, int? ignoreReservationId)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
                return ServiceResult.Fail("Korisnik ne postoji.");

            MembershipType membership = _membershipRepository.GetById(user.MembershipTypeID);
            if (membership == null)
                return ServiceResult.Fail("Tip clanarine ne postoji.");

            if (membership.MaxReservationHoursPerMonth <= 0)
                return ServiceResult.Ok("");

            double requestedHours = (end - start).TotalHours;
            double currentMonthHours = _reservationRepository
                .GetByUser(userId)
                .Where(r =>
                    (!ignoreReservationId.HasValue || r.ReservationID != ignoreReservationId.Value) &&
                    !string.Equals(r.ReservationStatus, "Canceled", StringComparison.OrdinalIgnoreCase) &&
                    r.StartDateTime.Year == start.Year &&
                    r.StartDateTime.Month == start.Month)
                .Sum(r => Math.Max(0, (r.EndDateTime - r.StartDateTime).TotalHours));

            if (currentMonthHours + requestedHours > membership.MaxReservationHoursPerMonth)
                return ServiceResult.Fail("Korisnik bi prekoracio dozvoljeni broj sati rezervacija za ovaj mesec.");

            return ServiceResult.Ok("");
        }

        private ServiceResult ValidateMeetingRoomAccess(int userId, Resource resource, DateTime start, DateTime end, int? ignoreReservationId)
        {
            if (!string.Equals(resource.ResourceType, "Room", StringComparison.OrdinalIgnoreCase))
                return ServiceResult.Ok("");

            User user = _userRepository.GetById(userId);
            if (user == null)
                return ServiceResult.Fail("Korisnik ne postoji.");

            MembershipType membership = _membershipRepository.GetById(user.MembershipTypeID);
            if (membership == null)
                return ServiceResult.Fail("Tip clanarine ne postoji.");

            if (!membership.MeetingRoomAccess)
                return ServiceResult.Fail("Izabrani tip clanarine ne dozvoljava rezervaciju sala.");

            if (!membership.MeetingRoomHoursPerMonth.HasValue || membership.MeetingRoomHoursPerMonth.Value <= 0)
                return ServiceResult.Ok("");

            HashSet<int> roomResourceIds = new HashSet<int>(_resourceRepository
                .GetAllResources()
                .Where(r => string.Equals(r.ResourceType, "Room", StringComparison.OrdinalIgnoreCase))
                .Select(r => r.ResourceID));

            double requestedHours = (end - start).TotalHours;
            double currentRoomHours = _reservationRepository
                .GetByUser(userId)
                .Where(r =>
                    (!ignoreReservationId.HasValue || r.ReservationID != ignoreReservationId.Value) &&
                    !string.Equals(r.ReservationStatus, "Canceled", StringComparison.OrdinalIgnoreCase) &&
                    r.StartDateTime.Year == start.Year &&
                    r.StartDateTime.Month == start.Month &&
                    roomResourceIds.Contains(r.ResourceID))
                .Sum(r => Math.Max(0, (r.EndDateTime - r.StartDateTime).TotalHours));

            if (currentRoomHours + requestedHours > membership.MeetingRoomHoursPerMonth.Value)
                return ServiceResult.Fail("Korisnik bi prekoracio dozvoljeni broj sati za sale u ovom mesecu.");

            return ServiceResult.Ok("");
        }

        private static bool TryGetWorkingHoursForDate(string workingHours, DayOfWeek day, out TimeSpan startTime, out TimeSpan endTime)
        {
            startTime = TimeSpan.Zero;
            endTime = TimeSpan.Zero;

            if (string.IsNullOrWhiteSpace(workingHours))
                return false;

            string[] segments = workingHours
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string rawSegment in segments)
            {
                string segment = rawSegment.Trim();
                if (!SegmentMatchesDay(segment, day))
                    continue;

                if (TryParseHoursFromSegment(segment, out startTime, out endTime))
                    return true;
            }

            foreach (string rawSegment in segments)
            {
                string segment = rawSegment.Trim();
                if (ContainsKnownDayToken(segment))
                    continue;

                if (TryParseHoursFromSegment(segment, out startTime, out endTime))
                    return true;
            }

            return false;
        }

        private static string NormalizeTimeToken(string token)
        {
            return token.Contains(":") ? token : token + ":00";
        }

        private static bool TryParseHoursFromSegment(string segment, out TimeSpan startTime, out TimeSpan endTime)
        {
            startTime = TimeSpan.Zero;
            endTime = TimeSpan.Zero;

            string[] tokens = Regex.Matches(segment, @"\d{1,2}(:\d{2})?")
                .Cast<Match>()
                .Select(m => NormalizeTimeToken(m.Value))
                .Take(2)
                .ToArray();

            if (tokens.Length < 2)
                return false;

            if (!TimeSpan.TryParse(tokens[0], out startTime))
                return false;

            if (!TimeSpan.TryParse(tokens[1], out endTime))
                return false;

            return endTime > startTime;
        }

        private static bool SegmentMatchesDay(string segment, DayOfWeek day)
        {
            string normalized = segment.ToLowerInvariant();
            string dayToken = GetDayToken(day);

            if (normalized.Contains(dayToken))
                return true;

            if (normalized.Contains("mon-fri"))
                return day >= DayOfWeek.Monday && day <= DayOfWeek.Friday;

            if (normalized.Contains("sat-sun"))
                return day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;

            if (normalized.Contains("mon-sat"))
                return day >= DayOfWeek.Monday && day <= DayOfWeek.Saturday;

            if (normalized.Contains("mon-sun"))
                return true;

            return false;
        }

        private static bool ContainsKnownDayToken(string segment)
        {
            string normalized = segment.ToLowerInvariant();
            return normalized.Contains("mon") ||
                   normalized.Contains("tue") ||
                   normalized.Contains("wed") ||
                   normalized.Contains("thu") ||
                   normalized.Contains("fri") ||
                   normalized.Contains("sat") ||
                   normalized.Contains("sun");
        }

        private static string GetDayToken(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return "mon";
                case DayOfWeek.Tuesday:
                    return "tue";
                case DayOfWeek.Wednesday:
                    return "wed";
                case DayOfWeek.Thursday:
                    return "thu";
                case DayOfWeek.Friday:
                    return "fri";
                case DayOfWeek.Saturday:
                    return "sat";
                default:
                    return "sun";
            }
        }

        private static string NormalizeStatus(string status, string fallbackStatus)
        {
            if (string.IsNullOrWhiteSpace(status))
                return string.IsNullOrWhiteSpace(fallbackStatus) ? "Active" : fallbackStatus;

            if (string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase))
                return "Active";

            if (string.Equals(status, "Finished", StringComparison.OrdinalIgnoreCase))
                return "Finished";

            if (string.Equals(status, "Canceled", StringComparison.OrdinalIgnoreCase))
                return "Canceled";

            return string.IsNullOrWhiteSpace(fallbackStatus) ? "Active" : fallbackStatus;
        }
    }
}
