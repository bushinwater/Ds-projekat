using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeLocationsModule()
        {
            if (dgvLocations == null)
                return;

            dgvLocations.AutoGenerateColumns = true;
            dgvLocations.SelectionChanged += dgvLocations_SelectionChanged;

            btnLocationAdd.Click += btnLocationAdd_Click;
            btnLocationUpdate.Click += btnLocationUpdate_Click;
            btnLocationDelete.Click += btnLocationDelete_Click;
            btnLocationClear.Click += btnLocationClear_Click;
            btnLocationRefresh.Click += btnLocationRefresh_Click;

            LoadLocations();
            ClearLocationForm();
        }

        private void LoadLocations()
        {
            try
            {
                _allLocations = _locationFacade.GetAll();

                dgvLocations.DataSource = null;
                dgvLocations.DataSource = _allLocations;

                FormatLocationsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju lokacija:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void FormatLocationsGrid()
        {
            if (dgvLocations.Columns.Count == 0) return;

            if (dgvLocations.Columns.Contains("LocationID"))
                dgvLocations.Columns["LocationID"].HeaderText = "ID";

            if (dgvLocations.Columns.Contains("LocationName"))
                dgvLocations.Columns["LocationName"].HeaderText = "Location Name";

            if (dgvLocations.Columns.Contains("AddressName"))
                dgvLocations.Columns["AddressName"].HeaderText = "Address";

            if (dgvLocations.Columns.Contains("City"))
                dgvLocations.Columns["City"].HeaderText = "City";

            if (dgvLocations.Columns.Contains("WorkingHours"))
                dgvLocations.Columns["WorkingHours"].HeaderText = "Working Hours";

            if (dgvLocations.Columns.Contains("MaxUsers"))
                dgvLocations.Columns["MaxUsers"].HeaderText = "Max Users";
        }

        private void dgvLocations_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLocations.CurrentRow == null) return;
            if (dgvLocations.CurrentRow.DataBoundItem is not Location location) return;

            FillLocationForm(location);
        }

        private void FillLocationForm(Location location)
        {
            txtLocationId.Text = location.LocationID.ToString();
            txtLocationName.Text = location.LocationName;
            txtLocationAddress.Text = location.AddressName;
            txtLocationCity.Text = location.City;
            txtWorkingHours.Text = location.WorkingHours;
            txtMaxUsers.Text = location.MaxUsers.ToString();
        }

        private Location ReadLocationFromForm()
        {
            if (string.IsNullOrWhiteSpace(txtLocationName.Text))
                throw new Exception("Location Name je obavezno.");

            if (string.IsNullOrWhiteSpace(txtLocationAddress.Text))
                throw new Exception("Address je obavezan.");

            if (string.IsNullOrWhiteSpace(txtLocationCity.Text))
                throw new Exception("City je obavezan.");

            if (string.IsNullOrWhiteSpace(txtWorkingHours.Text))
                throw new Exception("Working Hours je obavezan.");

            if (!int.TryParse(txtMaxUsers.Text.Trim(), out int maxUsers))
                throw new Exception("Max Users mora biti ceo broj.");

            Location location = new Location
            {
                LocationName = txtLocationName.Text.Trim(),
                AddressName = txtLocationAddress.Text.Trim(),
                City = txtLocationCity.Text.Trim(),
                WorkingHours = txtWorkingHours.Text.Trim(),
                MaxUsers = maxUsers
            };

            if (int.TryParse(txtLocationId.Text, out int locationId))
                location.LocationID = locationId;

            return location;
        }

        private void ClearLocationForm()
        {
            txtLocationId.Clear();
            txtLocationName.Clear();
            txtLocationAddress.Clear();
            txtLocationCity.Clear();
            txtWorkingHours.Clear();
            txtMaxUsers.Clear();

            dgvLocations.ClearSelection();
        }

        private void btnLocationAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Location location = ReadLocationFromForm();
                location.LocationID = 0;

                _locationFacade.AddLocation(location);

                MessageBox.Show(
                    "Lokacija je uspešno dodata.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadLocations();
                ClearLocationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri dodavanju lokacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnLocationUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtLocationId.Text, out _))
                    throw new Exception("Odaberite lokaciju za izmenu.");

                Location location = ReadLocationFromForm();
                _locationFacade.UpdateLocation(location);

                MessageBox.Show(
                    "Lokacija je uspešno izmenjena.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadLocations();
                ClearLocationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri izmeni lokacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnLocationDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtLocationId.Text, out int locationId))
                    throw new Exception("Odaberite lokaciju za brisanje.");

                DialogResult result = MessageBox.Show(
                    "Da li sigurno želite da obrišete lokaciju?",
                    "Potvrda brisanja",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;

                _locationFacade.DeleteLocation(locationId);

                MessageBox.Show(
                    "Lokacija je uspešno obrisana.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadLocations();
                ClearLocationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri brisanju lokacije:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnLocationClear_Click(object sender, EventArgs e)
        {
            ClearLocationForm();
        }

        private void btnLocationRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadLocations();
                ClearLocationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri osvežavanju lokacija:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}