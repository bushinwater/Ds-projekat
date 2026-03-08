using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class DashboardForm : SectionFormBase, IReloadableSection
    {
        private readonly UserFacade _userFacade;
        private readonly LocationFacade _locationFacade;
        private readonly ResourceFacade _resourceFacade;
        private readonly ReservationFacade _reservationFacade;

        private Label _usersValueLabel;
        private Label _locationsValueLabel;
        private Label _resourcesValueLabel;
        private Label _reservationsValueLabel;
        private DataGridView _recentReservationsGrid;
        private Label _notesLabel;

        public DashboardForm()
        {
            _userFacade = new UserFacade();
            _locationFacade = new LocationFacade();
            _resourceFacade = new ResourceFacade();
            _reservationFacade = new ReservationFacade();

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            Panel card1 = CreateStatCard("Korisnici", "0", 20, 20);
            Panel card2 = CreateStatCard("Lokacije", "0", 290, 20);
            Panel card3 = CreateStatCard("Resursi", "0", 560, 20);
            Panel card4 = CreateStatCard("Rezervacije", "0", 830, 20);

            _usersValueLabel = card1.Controls.OfType<Label>().Last();
            _locationsValueLabel = card2.Controls.OfType<Label>().Last();
            _resourcesValueLabel = card3.Controls.OfType<Label>().Last();
            _reservationsValueLabel = card4.Controls.OfType<Label>().Last();

            GroupBox recentGroup = CreateGroupBox("Poslednje rezervacije", 20, 170, 1080, 250);
            _recentReservationsGrid = CreateGrid("dgvRecentReservations", 15, 30, 1050, 200);
            recentGroup.Controls.Add(_recentReservationsGrid);

            GroupBox notesGroup = CreateGroupBox("Napomene sistema", 20, 440, 1080, 180);
            _notesLabel = new Label
            {
                Left = 20,
                Top = 40,
                Width = 900,
                Height = 90,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent
            };
            notesGroup.Controls.Add(_notesLabel);

            Controls.Add(card1);
            Controls.Add(card2);
            Controls.Add(card3);
            Controls.Add(card4);
            Controls.Add(recentGroup);
            Controls.Add(notesGroup);
        }

        public void LoadData()
        {
            try
            {
                List<User> users = _userFacade.GetAll();
                List<Location> locations = _locationFacade.GetAll();
                List<Resource> resources = _resourceFacade.GetAllResources();
                List<Reservation> reservations = _reservationFacade.GetAllReservations();

                _usersValueLabel.Text = users.Count.ToString();
                _locationsValueLabel.Text = locations.Count.ToString();
                _resourcesValueLabel.Text = resources.Count.ToString();
                _reservationsValueLabel.Text = reservations.Count.ToString();

                Dictionary<int, string> userNames = users.ToDictionary(u => u.UserID, u => u.FirstName + " " + u.LastName);
                Dictionary<int, string> resourceNames = resources.ToDictionary(r => r.ResourceID, r => r.ResourceName);

                _recentReservationsGrid.DataSource = null;
                _recentReservationsGrid.DataSource = reservations
                    .Take(10)
                    .Select(r => new
                    {
                        r.ReservationID,
                        User = userNames.ContainsKey(r.UserID) ? userNames[r.UserID] : "",
                        Resource = resourceNames.ContainsKey(r.ResourceID) ? resourceNames[r.ResourceID] : "",
                        r.StartDateTime,
                        r.EndDateTime,
                        r.ReservationStatus
                    })
                    .ToList();
                SetGridHeader(_recentReservationsGrid, "ReservationID", "ID");
                SetGridHeader(_recentReservationsGrid, "User", "Korisnik");
                SetGridHeader(_recentReservationsGrid, "Resource", "Resurs");
                SetGridHeader(_recentReservationsGrid, "StartDateTime", "Pocetak");
                SetGridHeader(_recentReservationsGrid, "EndDateTime", "Kraj");
                SetGridHeader(_recentReservationsGrid, "ReservationStatus", "Status");

                int activeReservations = reservations.Count(r => string.Equals(r.ReservationStatus, "Active", StringComparison.OrdinalIgnoreCase));
                int activeResources = resources.Count(r => r.IsActive);

                _notesLabel.Text =
                    "Aktivnih rezervacija: " + activeReservations + Environment.NewLine +
                    "Aktivnih resursa: " + activeResources + Environment.NewLine +
                    "Podaci se ucitavaju direktno iz baze pri otvaranju sekcije.";
            }
            catch (Exception ex)
            {
                _notesLabel.Text = "Greska pri ucitavanju dashboard podataka: " + ex.Message;
                _notesLabel.ForeColor = AppTheme.DangerColor;
            }
        }
    }
}
