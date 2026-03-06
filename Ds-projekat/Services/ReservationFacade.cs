using System;
using System.Collections.Generic;
using Ds_projekat.Repositories;

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
                if (start >= end)
                    return ServiceResult.Fail("End date must be after start date.");

                User user = _userRepository.GetById(userId);
                if (user == null)
                    return ServiceResult.Fail("User doesnt exist.");

                if (user.AccountStatus != "Active")
                    return ServiceResult.Fail("User is not active.");

                Resource resource = _resourceRepository.GetResource(resourceId);
                if (resource == null)
                    return ServiceResult.Fail("Resource doesnt exist.");

                if (!resource.IsActive)
                    return ServiceResult.Fail("Resource is not active.");

                bool overlap = _reservationRepository.HasOverlap(resourceId, start, end, null);
                if (overlap)
                    return ServiceResult.Fail("Resource is already reserved in that time.");

                if (resource.ResourceType == "Room")
                {
                    RoomDetails room = _resourceRepository.GetRoomDetails(resourceId);
                    if (room != null && usersCount.HasValue && usersCount.Value > room.Capacity)
                        return ServiceResult.Fail("Number of people is greater then supported for that room.");
                }

                Reservation reservation = new Reservation();
                reservation.UserID = userId;
                reservation.ResourceID = resourceId;
                reservation.UsersCount = usersCount;
                reservation.StartDateTime = start;
                reservation.EndDateTime = end;
                reservation.ReservationStatus = "Active";

                int newId = _reservationRepository.Add(reservation);

                return ServiceResult.Ok("Reservation created successfuly.", newId);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Error while creating reservation: " + ex.Message);
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
                if (start >= end)
                    return ServiceResult.Fail("End date must be after start date.");

                User user = _userRepository.GetById(userId);
                if (user == null)
                    return ServiceResult.Fail("User doesnt exist.");

                if (user.AccountStatus != "Active")
                    return ServiceResult.Fail("User is not active.");

                Resource resource = _resourceRepository.GetResource(resourceId);
                if (resource == null)
                    return ServiceResult.Fail("Resource doesnt exist.");

                if (!resource.IsActive)
                    return ServiceResult.Fail("Resource is not active.");

                bool overlap = _reservationRepository.HasOverlap(resourceId, start, end, null);
                if (overlap)
                    return ServiceResult.Fail("Resource is already reserved in that time.");

                if (resource.ResourceType == "Room")
                {
                    RoomDetails room = _resourceRepository.GetRoomDetails(resourceId);
                    if (room != null && usersCount.HasValue && usersCount.Value > room.Capacity)
                        return ServiceResult.Fail("Number of user is greater then capacity of a room.");
                }

                return ServiceResult.Ok("Reservation can be created.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Error while checking reservation: " + ex.Message);
            }
        }
    }
}