namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class OverlapValidationStrategy : IReservationValidationStrategy
    {
        private readonly IReservationRepository _reservationRepository;

        public OverlapValidationStrategy()
        {
            _reservationRepository = new ReservationRepository();
        }

        public ReservationValidationResult Validate(ReservationRequest request)
        {
            if (request.StartDateTime >= request.EndDateTime)
                return ReservationValidationResult.Fail("Datum kraja mora biti posle datuma pocetka.");

            bool overlap = _reservationRepository.HasOverlap(
                request.ResourceID,
                request.StartDateTime,
                request.EndDateTime,
                request.IgnoreReservationId
            );

            if (overlap)
                return ReservationValidationResult.Fail("Resurs je vec rezervisan u tom terminu.");

            return ReservationValidationResult.Success();
        }
    }
}