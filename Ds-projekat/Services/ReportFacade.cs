using System.Collections.Generic;
using System.Text;

namespace Ds_projekat.Services
{
    internal class ReportFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IResourceRepository _resourceRepository;

        public ReportFacade()
        {
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
            _locationRepository = new LocationRepository();
            _resourceRepository = new ResourceRepository();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public List<Location> GetAllLocations()
        {
            return _locationRepository.GetAll();
        }

        public List<Resource> GetAllResources()
        {
            return _resourceRepository.GetAllResources();
        }

        public List<Reservation> GetReservationsByUser(int userId)
        {
            return _reservationRepository.GetByUser(userId);
        }

        public List<Reservation> GetReservationsByResource(int resourceId)
        {
            return _reservationRepository.GetByResource(resourceId);
        }

        public string BuildUsersCsv()
        {
            List<User> users = _userRepository.GetAll();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UserID,FirstName,LastName,Email,Phone,MembershipTypeID,MembershipStartDate,MembershipEndDate,AccountStatus");

            foreach (User u in users)
            {
                sb.AppendLine(
                    u.UserID + "," +
                    u.FirstName + "," +
                    u.LastName + "," +
                    u.Email + "," +
                    u.Phone + "," +
                    u.MembershipTypeID + "," +
                    u.MembershipStartDate.ToShortDateString() + "," +
                    u.MembershipEndDate.ToShortDateString() + "," +
                    u.AccountStatus
                );
            }

            return sb.ToString();
        }
    }
}