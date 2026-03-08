using System;

namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class WorkingHoursValidationStrategy : IReservationValidationStrategy
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ILocationRepository _locationRepository;

        public WorkingHoursValidationStrategy()
        {
            _resourceRepository = new ResourceRepository();
            _locationRepository = new LocationRepository();
        }

        public ReservationValidationResult Validate(ReservationRequest request)
        {
            Resource resource = _resourceRepository.GetResource(request.ResourceID);
            if (resource == null)
                return ReservationValidationResult.Fail("Resurs ne postoji.");

            Location location = _locationRepository.GetById(resource.LocationID);
            if (location == null)
                return ReservationValidationResult.Fail("Lokacija ne postoji.");

            if (string.IsNullOrWhiteSpace(location.WorkingHours))
                return ReservationValidationResult.Fail("Radno vreme lokacije nije definisano.");

            string workingHours = location.WorkingHours.Trim();

            // Očekivan format:
            // Mon-Fri 08:00-20:00
            // Sat-Sun 10:00-18:00
            // Mon-Sun 00:00-23:59

            int firstSpace = workingHours.IndexOf(' ');
            if (firstSpace <= 0 || firstSpace >= workingHours.Length - 1)
            {
                return ReservationValidationResult.Fail(
                    "Radno vreme lokacije nije u ispravnom formatu. Očekivano npr: Mon-Fri 08:00-20:00"
                );
            }

            string daysPart = workingHours.Substring(0, firstSpace).Trim();
            string timePart = workingHours.Substring(firstSpace + 1).Trim();

            string[] timeParts = timePart.Split('-');
            if (timeParts.Length != 2)
            {
                return ReservationValidationResult.Fail(
                    "Radno vreme lokacije nije u ispravnom formatu vremena. Očekivano: HH:mm-HH:mm"
                );
            }

            TimeSpan openTime;
            TimeSpan closeTime;

            if (!TimeSpan.TryParse(timeParts[0].Trim(), out openTime) ||
                !TimeSpan.TryParse(timeParts[1].Trim(), out closeTime))
            {
                return ReservationValidationResult.Fail(
                    "Radno vreme lokacije nije u ispravnom formatu vremena. Očekivano: HH:mm-HH:mm"
                );
            }

            DayOfWeek reservationDay = request.StartDateTime.DayOfWeek;

            if (!IsDayAllowed(daysPart, reservationDay))
            {
                return ReservationValidationResult.Fail(
                    "Rezervacija nije dozvoljena tog dana. Radno vreme lokacije je: " + location.WorkingHours
                );
            }

            // Ako rezervacija prelazi u drugi dan, odmah odbij
            if (request.StartDateTime.Date != request.EndDateTime.Date)
            {
                return ReservationValidationResult.Fail(
                    "Rezervacija mora biti u okviru jednog radnog dana lokacije."
                );
            }

            TimeSpan reservationStart = request.StartDateTime.TimeOfDay;
            TimeSpan reservationEnd = request.EndDateTime.TimeOfDay;

            if (reservationStart < openTime || reservationEnd > closeTime)
            {
                return ReservationValidationResult.Fail(
                    "Rezervacija je van radnog vremena lokacije. Radno vreme je: " + location.WorkingHours
                );
            }

            return ReservationValidationResult.Success();
        }

        private bool IsDayAllowed(string daysPart, DayOfWeek day)
        {
            // Podržani formati:
            // Mon-Fri
            // Sat-Sun
            // Mon-Sun
            // Mon
            // Tue
            // Wed
            // Thu
            // Fri
            // Sat
            // Sun

            string[] rangeParts = daysPart.Split('-');

            if (rangeParts.Length == 1)
            {
                return ParseDay(rangeParts[0].Trim()) == day;
            }

            if (rangeParts.Length == 2)
            {
                DayOfWeek startDay = ParseDay(rangeParts[0].Trim());
                DayOfWeek endDay = ParseDay(rangeParts[1].Trim());

                int dayValue = NormalizeDay(day);
                int startValue = NormalizeDay(startDay);
                int endValue = NormalizeDay(endDay);

                if (startValue <= endValue)
                {
                    return dayValue >= startValue && dayValue <= endValue;
                }
                else
                {
                    // npr Fri-Mon
                    return dayValue >= startValue || dayValue <= endValue;
                }
            }

            return false;
        }

        private DayOfWeek ParseDay(string dayText)
        {
            switch (dayText)
            {
                case "Mon": return DayOfWeek.Monday;
                case "Tue": return DayOfWeek.Tuesday;
                case "Wed": return DayOfWeek.Wednesday;
                case "Thu": return DayOfWeek.Thursday;
                case "Fri": return DayOfWeek.Friday;
                case "Sat": return DayOfWeek.Saturday;
                case "Sun": return DayOfWeek.Sunday;
                default:
                    throw new Exception("Nepoznat dan u radnom vremenu: " + dayText);
            }
        }

        private int NormalizeDay(DayOfWeek day)
        {
            // DayOfWeek enum je:
            // Sunday=0, Monday=1, ..., Saturday=6
            // Ovde prebacujemo da Monday=1 ... Sunday=7
            if (day == DayOfWeek.Sunday)
                return 7;

            return (int)day;
        }
    }
}