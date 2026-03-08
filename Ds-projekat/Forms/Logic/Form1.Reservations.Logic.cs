using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeReservationsModule()
        {
            if (dgvReservations == null)
                return;

            dgvReservations.AutoGenerateColumns = true;
            dgvReservations.SelectionChanged += dgvReservations_SelectionChanged;

            btnReservationCreate.Click += btnReservationCreate_Click;
            btnReservationCancel.Click += btnReservationCancel_Click;
            btnReservationFinish.Click += btnReservationFinish_Click;
            btnReservationCheck.Click += btnReservationCheck_Click;
            btnReservationClear.Click += btnReservationClear_Click;
            btnReservationRefresh.Click += btnReservationRefresh_Click;

            cbReservationFilterUser.SelectedIndexChanged += (s, e) => ApplyReservationUserFilter();
            cbReservationFilterLocation.SelectedIndexChanged += (s, e) => ApplyReservationOccupancyFilter();
            dtpReservationFilterDate.ValueChanged += (s, e) => ApplyReservationOccupancyFilter();
            cbReservationUser.SelectedIndexChanged += (s, e) => SyncReservationUserFilterFromForm();

            LoadReservationUsers();
            LoadReservationResources();
            LoadReservationLocationFilters();
            LoadReservations();
            ClearReservationForm();
        }

        private void LoadReservationUsers()
        {
            try
            {
                _allReservationUsers = _userFacade.GetAll();

                cbReservationUser.DataSource = null;
                cbReservationUser.DisplayMember = "Email";
                cbReservationUser.ValueMember = "UserID";
                cbReservationUser.DataSource = _allReservationUsers;
                cbReservationUser.SelectedIndex = -1;

                cbReservationFilterUser.DataSource = null;
                var filterUsers = new List<User> { new User { UserID = 0, FirstName = "All", LastName = "Users", Email = "All users" } };
                filterUsers.AddRange(_allReservationUsers);
                cbReservationFilterUser.DisplayMember = "Email";
                cbReservationFilterUser.ValueMember = "UserID";
                cbReservationFilterUser.DataSource = filterUsers;
                cbReservationFilterUser.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju korisnika za rezervacije:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReservationResources()
        {
            try
            {
                _allReservationResources = _resourceFacade.GetAllResources();
                cbReservationResource.DataSource = null;
                cbReservationResource.DisplayMember = "ResourceName";
                cbReservationResource.ValueMember = "ResourceID";
                cbReservationResource.DataSource = _allReservationResources;
                cbReservationResource.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju resursa za rezervacije:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReservationLocationFilters()
        {
            var locations = _locationFacade.GetAll();
            cbReservationFilterLocation.DataSource = null;
            var filterLocations = new List<Location> { new Location { LocationID = 0, LocationName = "All" } };
            filterLocations.AddRange(locations);
            cbReservationFilterLocation.DisplayMember = "LocationName";
            cbReservationFilterLocation.ValueMember = "LocationID";
            cbReservationFilterLocation.DataSource = filterLocations;
            cbReservationFilterLocation.SelectedValue = 0;
        }

        private void LoadReservations()
        {
            try
            {
                _allReservations = _reservationFacade.GetAll();
                _allReservationUsers = _userFacade.GetAll();
                _allReservationResources = _resourceFacade.GetAllResources();
                BindReservationsGrid(_allReservations);
                lblReservationOccupancy.Text = "Occupancy: prikaz svih rezervacija";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju rezervacija:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindReservationsGrid(IEnumerable<Reservation> reservations)
        {
            _isBindingReservations = true;
            try
            {
                var locations = _locationFacade.GetAll();
                var rows = reservations.Select(r =>
            {
                var user = _allReservationUsers.FirstOrDefault(u => u.UserID == r.UserID);
                var resource = _allReservationResources.FirstOrDefault(res => res.ResourceID == r.ResourceID);
                var location = resource != null ? locations.FirstOrDefault(l => l.LocationID == resource.LocationID) : null;
                string occupancy = r.ReservationStatus == "Canceled" ? "Canceled" : "Occupied";

                if (r.ReservationStatus == "Finished") occupancy = "Finished";
                if (r.EndDateTime < DateTime.Now && r.ReservationStatus == "Active") occupancy = "Past";

                return new ReservationGridRow
                {
                    ReservationID = r.ReservationID,
                    UserID = r.UserID,
                    LocationID = location?.LocationID ?? 0,
                    User = user != null ? user.FirstName + " " + user.LastName + " (" + user.UserID + ")" : "N/A",
                    Resource = resource != null ? resource.ResourceName + " (" + resource.ResourceID + ")" : "N/A",
                    Location = location != null ? location.LocationName : "N/A",
                    UsersCount = r.UsersCount?.ToString() ?? "-",
                    Start = r.StartDateTime.ToString("dd.MM.yyyy HH:mm"),
                    End = r.EndDateTime.ToString("dd.MM.yyyy HH:mm"),
                    Status = r.ReservationStatus,
                    Occupancy = occupancy
                };
            }).OrderBy(x => x.Start).ToList();

                dgvReservations.DataSource = null;
                dgvReservations.DataSource = rows;
                FormatReservationsGrid();
                dgvReservations.ClearSelection();
            }
            finally
            {
                _isBindingReservations = false;
            }
        }

        private void ApplyReservationUserFilter()
        {
            if (cbReservationFilterUser.SelectedValue == null || Convert.ToInt32(cbReservationFilterUser.SelectedValue) == 0)
            {
                BindReservationsGrid(_allReservations);
                lblReservationOccupancy.Text = "Occupancy: prikaz svih rezervacija";
                return;
            }

            int userId = Convert.ToInt32(cbReservationFilterUser.SelectedValue);
            var reservations = _allReservations.Where(r => r.UserID == userId).OrderByDescending(r => r.StartDateTime).ToList();
            BindReservationsGrid(reservations);
            lblReservationOccupancy.Text = "Occupancy: rezervacije za izabranog korisnika = " + reservations.Count;
        }

        private void ApplyReservationOccupancyFilter()
        {
            if (cbReservationFilterLocation.SelectedValue == null || Convert.ToInt32(cbReservationFilterLocation.SelectedValue) == 0)
                return;

            int locationId = Convert.ToInt32(cbReservationFilterLocation.SelectedValue);
            DateTime dayStart = dtpReservationFilterDate.Value.Date;
            DateTime dayEnd = dayStart.AddDays(1);

            var resourceIdsInLocation = _allReservationResources.Where(r => r.LocationID == locationId).Select(r => r.ResourceID).ToHashSet();
            var reservations = _allReservations
                .Where(r => resourceIdsInLocation.Contains(r.ResourceID))
                .Where(r => r.StartDateTime < dayEnd && r.EndDateTime > dayStart)
                .OrderBy(r => r.StartDateTime)
                .ToList();

            BindReservationsGrid(reservations);

            int totalResources = resourceIdsInLocation.Count;
            int occupiedResources = reservations.Where(r => r.ReservationStatus != "Canceled").Select(r => r.ResourceID).Distinct().Count();
            lblReservationOccupancy.Text = totalResources == 0
                ? "Occupancy: nema resursa za izabranu lokaciju"
                : $"Occupancy: {occupiedResources}/{totalResources} resursa zauzeto za {dayStart:dd.MM.yyyy}";
        }

        private void SyncReservationUserFilterFromForm()
        {
            if (_isBindingReservations || _isFillingReservationForm) return;
            if (cbReservationUser.SelectedValue == null) return;
            if (cbReservationFilterUser.DataSource != null)
                cbReservationFilterUser.SelectedValue = Convert.ToInt32(cbReservationUser.SelectedValue);
        }

        private void FormatReservationsGrid()
        {
            if (dgvReservations.Columns.Count == 0) return;
            if (dgvReservations.Columns.Contains("ReservationID")) dgvReservations.Columns["ReservationID"].HeaderText = "ID";
            if (dgvReservations.Columns.Contains("UserID")) dgvReservations.Columns["UserID"].Visible = false;
            if (dgvReservations.Columns.Contains("LocationID")) dgvReservations.Columns["LocationID"].Visible = false;
            if (dgvReservations.Columns.Contains("User")) dgvReservations.Columns["User"].HeaderText = "User";
            if (dgvReservations.Columns.Contains("Resource")) dgvReservations.Columns["Resource"].HeaderText = "Resource";
            if (dgvReservations.Columns.Contains("Location")) dgvReservations.Columns["Location"].HeaderText = "Location";
            if (dgvReservations.Columns.Contains("UsersCount")) dgvReservations.Columns["UsersCount"].HeaderText = "Users Count";
            if (dgvReservations.Columns.Contains("Start")) dgvReservations.Columns["Start"].HeaderText = "Start";
            if (dgvReservations.Columns.Contains("End")) dgvReservations.Columns["End"].HeaderText = "End";
            if (dgvReservations.Columns.Contains("Status")) dgvReservations.Columns["Status"].HeaderText = "Status";
            if (dgvReservations.Columns.Contains("Occupancy")) dgvReservations.Columns["Occupancy"].HeaderText = "Occupancy";
        }

        private void dgvReservations_SelectionChanged(object sender, EventArgs e)
        {
            if (_isBindingReservations) return;
            if (dgvReservations.CurrentRow == null) return;
            if (dgvReservations.CurrentRow.Index < 0) return;
            if (dgvReservations.CurrentRow.DataBoundItem is not ReservationGridRow row) return;
            Reservation reservation = _allReservations.FirstOrDefault(x => x.ReservationID == row.ReservationID);
            if (reservation != null) FillReservationForm(reservation);
        }

        private void FillReservationForm(Reservation reservation)
        {
            _isFillingReservationForm = true;
            try
            {
                txtReservationId.Text = reservation.ReservationID.ToString();
                cbReservationUser.SelectedValue = reservation.UserID;
                cbReservationResource.SelectedValue = reservation.ResourceID;
                txtReservationUsersCount.Text = reservation.UsersCount?.ToString() ?? "";
                dtpReservationStart.Value = reservation.StartDateTime;
                dtpReservationEnd.Value = reservation.EndDateTime;
                cbReservationStatus.SelectedItem = reservation.ReservationStatus;
            }
            finally
            {
                _isFillingReservationForm = false;
            }
        }

        private void ClearReservationForm()
        {
            _isFillingReservationForm = true;
            try
            {
                txtReservationId.Clear();
                cbReservationUser.SelectedIndex = -1;
                cbReservationResource.SelectedIndex = -1;
                txtReservationUsersCount.Clear();
                dtpReservationStart.Value = DateTime.Now;
                dtpReservationEnd.Value = DateTime.Now.AddHours(1);
                cbReservationStatus.SelectedIndex = -1;
                dgvReservations.ClearSelection();
            }
            finally
            {
                _isFillingReservationForm = false;
            }
        }

        private void btnReservationCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbReservationUser.SelectedValue == null) throw new Exception("User je obavezan.");
                if (cbReservationResource.SelectedValue == null) throw new Exception("Resource je obavezan.");
                if (dtpReservationEnd.Value <= dtpReservationStart.Value) throw new Exception("End DateTime mora biti posle Start DateTime.");

                int userId = Convert.ToInt32(cbReservationUser.SelectedValue);
                int resourceId = Convert.ToInt32(cbReservationResource.SelectedValue);
                int? usersCount = null;
                if (!string.IsNullOrWhiteSpace(txtReservationUsersCount.Text))
                {
                    if (!int.TryParse(txtReservationUsersCount.Text.Trim(), out int parsedUsersCount)) throw new Exception("Users Count mora biti ceo broj.");
                    usersCount = parsedUsersCount;
                }

                ServiceResult result = _reservationFacade.CreateReservation(userId, resourceId, dtpReservationStart.Value, dtpReservationEnd.Value, usersCount);
                if (!result.Success) throw new Exception(result.Message);

                MessageBox.Show(result.Message, "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations();
                if (cbReservationFilterUser.SelectedValue != null && Convert.ToInt32(cbReservationFilterUser.SelectedValue) > 0) ApplyReservationUserFilter();
                else if (cbReservationFilterLocation.SelectedValue != null && Convert.ToInt32(cbReservationFilterLocation.SelectedValue) > 0) ApplyReservationOccupancyFilter();
                ClearReservationForm();
                NotifyObservers("Reservations", "Create", "Kreirana je nova rezervacija za korisnika ID=" + userId + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri kreiranju rezervacije:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReservationCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtReservationId.Text, out int reservationId)) throw new Exception("Odaberite rezervaciju za otkazivanje.");
                _reservationFacade.CancelReservation(reservationId);
                MessageBox.Show("Rezervacija je uspešno otkazana.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations();
                ClearReservationForm();
                NotifyObservers("Reservations", "Cancel", "Otkazana je rezervacija ID=" + reservationId + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri otkazivanju rezervacije:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReservationFinish_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtReservationId.Text, out int reservationId)) throw new Exception("Odaberite rezervaciju za završavanje.");
                _reservationFacade.FinishReservation(reservationId);
                MessageBox.Show("Rezervacija je uspešno završena.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations();
                ClearReservationForm();
                NotifyObservers("Reservations", "Finish", "Završena je rezervacija ID=" + reservationId + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri završavanju rezervacije:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReservationCheck_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbReservationUser.SelectedValue == null) throw new Exception("Odaberite korisnika.");
                if (cbReservationResource.SelectedValue == null) throw new Exception("Odaberite resurs.");
                if (dtpReservationEnd.Value <= dtpReservationStart.Value) throw new Exception("End DateTime mora biti posle Start DateTime.");

                int userId = Convert.ToInt32(cbReservationUser.SelectedValue);
                int resourceId = Convert.ToInt32(cbReservationResource.SelectedValue);
                int? usersCount = null;
                if (!string.IsNullOrWhiteSpace(txtReservationUsersCount.Text))
                {
                    if (!int.TryParse(txtReservationUsersCount.Text.Trim(), out int parsedUsersCount)) throw new Exception("Users Count mora biti ceo broj.");
                    usersCount = parsedUsersCount;
                }

                ServiceResult result = _reservationFacade.CanReserve(userId, resourceId, dtpReservationStart.Value, dtpReservationEnd.Value, usersCount);
                MessageBox.Show(result.Message, "Provera termina", MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri proveri termina:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReservationClear_Click(object sender, EventArgs e) => ClearReservationForm();

        private void btnReservationRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadReservationUsers();
                LoadReservationResources();
                LoadReservationLocationFilters();
                LoadReservations();
                ClearReservationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri osvežavanju rezervacija:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
