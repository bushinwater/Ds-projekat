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

            LoadReservationUsers();
            LoadReservationResources();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju korisnika za rezervacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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
                MessageBox.Show(
                    "Greška pri učitavanju resursa za rezervacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadReservations()
        {
            try
            {
                _allReservations = _reservationFacade.GetAll();

                dgvReservations.DataSource = null;
                dgvReservations.DataSource = _allReservations;

                FormatReservationsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju rezervacija:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void FormatReservationsGrid()
        {
            if (dgvReservations.Columns.Count == 0) return;

            if (dgvReservations.Columns.Contains("ReservationID"))
                dgvReservations.Columns["ReservationID"].HeaderText = "ID";

            if (dgvReservations.Columns.Contains("UserID"))
                dgvReservations.Columns["UserID"].HeaderText = "User ID";

            if (dgvReservations.Columns.Contains("ResourceID"))
                dgvReservations.Columns["ResourceID"].HeaderText = "Resource ID";

            if (dgvReservations.Columns.Contains("UsersCount"))
                dgvReservations.Columns["UsersCount"].HeaderText = "Users Count";

            if (dgvReservations.Columns.Contains("StartDateTime"))
                dgvReservations.Columns["StartDateTime"].HeaderText = "Start";

            if (dgvReservations.Columns.Contains("EndDateTime"))
                dgvReservations.Columns["EndDateTime"].HeaderText = "End";

            if (dgvReservations.Columns.Contains("ReservationStatus"))
                dgvReservations.Columns["ReservationStatus"].HeaderText = "Status";
        }

        private void dgvReservations_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReservations.CurrentRow == null) return;
            if (dgvReservations.CurrentRow.DataBoundItem is not Reservation reservation) return;

            FillReservationForm(reservation);
        }

        private void FillReservationForm(Reservation reservation)
        {
            txtReservationId.Text = reservation.ReservationID.ToString();
            cbReservationUser.SelectedValue = reservation.UserID;
            cbReservationResource.SelectedValue = reservation.ResourceID;
            txtReservationUsersCount.Text = reservation.UsersCount?.ToString() ?? "";
            dtpReservationStart.Value = reservation.StartDateTime;
            dtpReservationEnd.Value = reservation.EndDateTime;
            cbReservationStatus.SelectedItem = reservation.ReservationStatus;
        }

        private Reservation ReadReservationFromForm()
        {
            if (cbReservationUser.SelectedValue == null)
                throw new Exception("User je obavezan.");

            if (cbReservationResource.SelectedValue == null)
                throw new Exception("Resource je obavezan.");

            int? usersCount = null;
            if (!string.IsNullOrWhiteSpace(txtReservationUsersCount.Text))
            {
                if (!int.TryParse(txtReservationUsersCount.Text.Trim(), out int parsedUsersCount))
                    throw new Exception("Users Count mora biti ceo broj.");

                usersCount = parsedUsersCount;
            }

            if (dtpReservationEnd.Value <= dtpReservationStart.Value)
                throw new Exception("End DateTime mora biti posle Start DateTime.");

            Reservation reservation = new Reservation
            {
                UserID = Convert.ToInt32(cbReservationUser.SelectedValue),
                ResourceID = Convert.ToInt32(cbReservationResource.SelectedValue),
                UsersCount = usersCount,
                StartDateTime = dtpReservationStart.Value,
                EndDateTime = dtpReservationEnd.Value,
                ReservationStatus = cbReservationStatus.SelectedItem?.ToString() ?? "Active"
            };

            if (int.TryParse(txtReservationId.Text, out int reservationId))
                reservation.ReservationID = reservationId;

            return reservation;
        }

        private void ClearReservationForm()
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

        private void btnReservationCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbReservationUser.SelectedValue == null)
                    throw new Exception("User je obavezan.");

                if (cbReservationResource.SelectedValue == null)
                    throw new Exception("Resource je obavezan.");

                if (dtpReservationEnd.Value <= dtpReservationStart.Value)
                    throw new Exception("End DateTime mora biti posle Start DateTime.");

                int userId = Convert.ToInt32(cbReservationUser.SelectedValue);
                int resourceId = Convert.ToInt32(cbReservationResource.SelectedValue);

                int? usersCount = null;
                if (!string.IsNullOrWhiteSpace(txtReservationUsersCount.Text))
                {
                    if (!int.TryParse(txtReservationUsersCount.Text.Trim(), out int parsedUsersCount))
                        throw new Exception("Users Count mora biti ceo broj.");

                    usersCount = parsedUsersCount;
                }

                ServiceResult result = _reservationFacade.CreateReservation(
                    userId,
                    resourceId,
                    dtpReservationStart.Value,
                    dtpReservationEnd.Value,
                    usersCount
                );

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadReservations();
                ClearReservationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri kreiranju rezervacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnReservationCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtReservationId.Text, out int reservationId))
                    throw new Exception("Odaberite rezervaciju za otkazivanje.");

                _reservationFacade.CancelReservation(reservationId);

                MessageBox.Show(
                    "Rezervacija je uspešno otkazana.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadReservations();
                ClearReservationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri otkazivanju rezervacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnReservationFinish_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtReservationId.Text, out int reservationId))
                    throw new Exception("Odaberite rezervaciju za završavanje.");

                _reservationFacade.FinishReservation(reservationId);

                MessageBox.Show(
                    "Rezervacija je uspešno završena.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadReservations();
                ClearReservationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri završavanju rezervacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnReservationCheck_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbReservationUser.SelectedValue == null)
                    throw new Exception("Odaberite korisnika.");

                if (cbReservationResource.SelectedValue == null)
                    throw new Exception("Odaberite resurs.");

                if (dtpReservationEnd.Value <= dtpReservationStart.Value)
                    throw new Exception("End DateTime mora biti posle Start DateTime.");

                int userId = Convert.ToInt32(cbReservationUser.SelectedValue);
                int resourceId = Convert.ToInt32(cbReservationResource.SelectedValue);

                int? usersCount = null;
                if (!string.IsNullOrWhiteSpace(txtReservationUsersCount.Text))
                {
                    if (!int.TryParse(txtReservationUsersCount.Text.Trim(), out int parsedUsersCount))
                        throw new Exception("Users Count mora biti ceo broj.");

                    usersCount = parsedUsersCount;
                }

                ServiceResult result = _reservationFacade.CanReserve(
                    userId,
                    resourceId,
                    dtpReservationStart.Value,
                    dtpReservationEnd.Value,
                    usersCount
                );

                MessageBox.Show(
                    result.Message,
                    "Provera termina",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri proveri termina:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnReservationClear_Click(object sender, EventArgs e)
        {
            ClearReservationForm();
        }

        private void btnReservationRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadReservationUsers();
                LoadReservationResources();
                LoadReservations();
                ClearReservationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri osvežavanju rezervacija:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}