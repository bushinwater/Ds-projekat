namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class UserActiveValidationStrategy : IReservationValidationStrategy
    {
        private readonly IUserRepository _userRepository;

        public UserActiveValidationStrategy()
        {
            _userRepository = new UserRepository();
        }

        public ReservationValidationResult Validate(ReservationRequest request)
        {
            User user = _userRepository.GetById(request.UserID);

            if (user == null)
                return ReservationValidationResult.Fail("Korisnik ne postoji.");

            if (user.AccountStatus != "Active")
                return ReservationValidationResult.Fail("Korisnik nije aktivan.");

            return ReservationValidationResult.Success();
        }
    }
}