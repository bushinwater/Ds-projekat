using Ds_projekat.Export;
using Ds_projekat.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Ds_projekat.Services
{
    internal class ReportFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IMembershipTypeRepository _membershipTypeRepository;
        private readonly IReportExporter _reportExporter;

        public ReportFacade()
        {
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
            _locationRepository = new LocationRepository();
            _resourceRepository = new ResourceRepository();
            _membershipTypeRepository = new MembershipTypeRepository();
            _reportExporter = new CsvExporterAdapter();
        }

        public ServiceResult ExportUsersToCsv(string filePath)
        {
            try
            {
                List<User> users = _userRepository.GetAll();
                _reportExporter.ExportUsers(filePath, users);
                return ServiceResult.Ok("Users su uspesno eksportovani u CSV.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri exportu users: " + ex.Message);
            }
        }

        public ServiceResult ExportReservationsByUserToCsv(int userId, string filePath)
        {
            try
            {
                List<Reservation> reservations = _reservationRepository.GetByUser(userId);
                _reportExporter.ExportReservations(filePath, reservations);
                return ServiceResult.Ok("Reservations su uspesno eksportovane u CSV.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri exportu reservations: " + ex.Message);
            }
        }

        public ServiceResult ExportResourcesToCsv(string filePath)
        {
            try
            {
                List<Resource> resources = _resourceRepository.GetAllResources();
                _reportExporter.ExportResources(filePath, resources);
                return ServiceResult.Ok("Resources su uspesno eksportovani u CSV.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri exportu resources: " + ex.Message);
            }
        }

        public ServiceResult ExportLocationsToCsv(string filePath)
        {
            try
            {
                List<Location> locations = _locationRepository.GetAll();
                _reportExporter.ExportLocations(filePath, locations);
                return ServiceResult.Ok("Locations su uspesno eksportovane u CSV.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri exportu locations: " + ex.Message);
            }
        }

        public ServiceResult ExportMembershipTypesToCsv(string filePath)
        {
            try
            {
                List<MembershipType> membershipTypes = _membershipTypeRepository.GetAll();
                _reportExporter.ExportMembershipTypes(filePath, membershipTypes);
                return ServiceResult.Ok("Membership types su uspesno eksportovani u CSV.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri exportu membership types: " + ex.Message);
            }
        }
    }
}