namespace Ds_projekat.Strategies.ReservationValidation
{
    internal interface IReservationValidationStrategy
    {
        ReservationValidationResult Validate(ReservationRequest request);
    }
}