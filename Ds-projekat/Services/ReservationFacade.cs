using System;
using System.Collections.Generic;
using Ds_projekat.Repositories;
using Ds_projekat.Strategies.ReservationValidation;

namespace Ds_projekat.Services
{
    internal class ReservationFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IReservationRepository _reservationRepository;

        public ReservationFacade()
        {
            _userRepository = new UserRepository();
            _resourceRepository = new ResourceRepository();
            _reservationRepository = new ReservationRepository();
        }

        public ServiceResult CreateReservation(int userId, int resourceId, DateTime start, DateTime end, int? usersCount = null)
        {
            try
            {
                ReservationRequest request = new ReservationRequest
                {
                    UserID = userId,
                    ResourceID = resourceId,
                    StartDateTime = start,
                    EndDateTime = end,
                    UsersCount = usersCount,
                    IgnoreReservationId = null
                };

                ReservationValidator validator = new ReservationValidator();
                validator.AddStrategy(new UserActiveValidationStrategy());
                validator.AddStrategy(new ResourceActiveValidationStrategy());
                validator.AddStrategy(new OverlapValidationStrategy());
                validator.AddStrategy(new RoomCapacityValidationStrategy());

                ReservationValidationResult validationResult = validator.Validate(request);

                if (!validationResult.IsValid)
                    return ServiceResult.Fail(validationResult.Message);

                Reservation reservation = new Reservation();
                reservation.UserID = userId;
                reservation.ResourceID = resourceId;
                reservation.UsersCount = usersCount;
                reservation.StartDateTime = start;
                reservation.EndDateTime = end;
                reservation.ReservationStatus = "Active";

                int newId = _reservationRepository.Add(reservation);

                return ServiceResult.Ok("Rezervacija je uspesno kreirana.", newId);
            }
            catch (System.Exception ex)
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

        public List<Reservation> GetReservationsForResource(int resourceId)
        {
            return _reservationRepository.GetByResource(resourceId);
        }

        public ServiceResult CanReserve(int userId, int resourceId, DateTime start, DateTime end, int? usersCount = null)
        {
            try
            {
                ReservationRequest request = new ReservationRequest
                {
                    UserID = userId,
                    ResourceID = resourceId,
                    StartDateTime = start,
                    EndDateTime = end,
                    UsersCount = usersCount,
                    IgnoreReservationId = null
                };

                ReservationValidator validator = new ReservationValidator();
                validator.AddStrategy(new UserActiveValidationStrategy());
                validator.AddStrategy(new ResourceActiveValidationStrategy());
                validator.AddStrategy(new OverlapValidationStrategy());
                validator.AddStrategy(new RoomCapacityValidationStrategy());

                ReservationValidationResult validationResult = validator.Validate(request);

                if (!validationResult.IsValid)
                    return ServiceResult.Fail(validationResult.Message);

                return ServiceResult.Ok("Rezervacija moze da se napravi.");
            }
            catch (System.Exception ex)
            {
                return ServiceResult.Fail("Greska pri proveri rezervacije: " + ex.Message);
            }
        }
        public List<Reservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }
    }
}