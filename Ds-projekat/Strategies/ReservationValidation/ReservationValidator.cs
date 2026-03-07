using System.Collections.Generic;

namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class ReservationValidator
    {
        private readonly List<IReservationValidationStrategy> _strategies;

        public ReservationValidator()
        {
            _strategies = new List<IReservationValidationStrategy>();
        }

        public void AddStrategy(IReservationValidationStrategy strategy)
        {
            _strategies.Add(strategy);
        }

        public ReservationValidationResult Validate(ReservationRequest request)
        {
            foreach (IReservationValidationStrategy strategy in _strategies)
            {
                ReservationValidationResult result = strategy.Validate(request);

                if (!result.IsValid)
                    return result;
            }

            return ReservationValidationResult.Success();
        }
    }
}