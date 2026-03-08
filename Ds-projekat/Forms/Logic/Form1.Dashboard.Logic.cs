using System;
using System.Linq;
using System.Windows.Forms;
using Ds_projekat.Repositories;

namespace Ds_projekat
{
    public partial class Form1
    {
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly ILocationRepository _locationRepository = new LocationRepository();
        private readonly IResourceRepository _resourceRepository = new ResourceRepository();
        private readonly IReservationRepository _reservationRepository = new ReservationRepository();

        private void InitializeDashboard()
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                var users = _userRepository.GetAll();
                var locations = _locationRepository.GetAll();
                var resources = _resourceRepository.GetAllResources();
                var reservations = _reservationRepository.GetAll();

                lblUsersCount.Text = users.Count.ToString();
                lblLocationsCount.Text = locations.Count.ToString();
                lblResourcesCount.Text = resources.Count.ToString();
                lblReservationsCount.Text = reservations.Count.ToString();

                dgvRecentReservations.DataSource = reservations
                    .OrderByDescending(r => r.StartDateTime)
                    .Take(10)
                    .Select(r => new
                    {
                        r.ReservationID,
                        r.UserID,
                        r.ResourceID,
                        r.StartDateTime,
                        r.EndDateTime,
                        r.ReservationStatus
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju dashboard podataka:\n" + ex.Message
                );
            }
        }
    }
}