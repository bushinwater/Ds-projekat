using Ds_projekat.Repositories.Interfaces;
using System;

namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class MembershipHoursLimitValidationStrategy : IReservationValidationStrategy
    {
        private readonly IUserRepository _userRepository;
        private readonly IMembershipTypeRepository _membershipTypeRepository;
        private readonly IReservationRepository _reservationRepository;

        public MembershipHoursLimitValidationStrategy()
        {
            _userRepository = new UserRepository();
            _membershipTypeRepository = new MembershipTypeRepository();
            _reservationRepository = new ReservationRepository();
        }

        public ReservationValidationResult Validate(ReservationRequest request)
        {
            User user = _userRepository.GetById(request.UserID);
            if (user == null)
                return ReservationValidationResult.Fail("Korisnik ne postoji.");

            MembershipType membership = _membershipTypeRepository.GetById(user.MembershipTypeID);
            if (membership == null)
                return ReservationValidationResult.Fail("Tip članarine ne postoji.");

            double alreadyReserved = _reservationRepository.GetReservedHoursForUserInMonth(
                request.UserID,
                request.StartDateTime
            );

            double newReservationHours = (request.EndDateTime - request.StartDateTime).TotalHours;
            double total = alreadyReserved + newReservationHours;

            if (total > membership.MaxReservationHoursPerMonth)
            {
                return ReservationValidationResult.Fail(
                    "Korisnik bi prekoračio dozvoljen broj sati za svoj paket članstva. " +
                    "Dozvoljeno: " + membership.MaxReservationHoursPerMonth +
                    ", već rezervisano: " + alreadyReserved.ToString("0.##") +
                    ", nova rezervacija: " + newReservationHours.ToString("0.##")
                );
            }

            return ReservationValidationResult.Success();
        }
    }
}