using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

                var rows = _allLocations.Select(l => new LocationGridRow
                {
                    LocationID = l.LocationID,
                    Location = l.LocationName + " (" + l.LocationID + ")",
                    Address = l.AddressName,
                    City = l.City,
                    WorkingHours = l.WorkingHours,
                    MaxUsers = l.MaxUsers
                }).ToList();

                dgvLocations.DataSource = null;
                dgvLocations.DataSource = rows;

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
                dgvLocations.Columns["LocationID"].Visible = false;

            if (dgvLocations.Columns.Contains("Location"))
                dgvLocations.Columns["Location"].HeaderText = "Location";

            if (dgvLocations.Columns.Contains("Address"))
                dgvLocations.Columns["Address"].HeaderText = "Address";

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
            if (dgvLocations.CurrentRow.DataBoundItem is not LocationGridRow row) return;

            Location location = _allLocations.FirstOrDefault(x => x.LocationID == row.LocationID);
            if (location == null) return;

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
                NotifyObservers("Locations", "Add", "Dodata je nova lokacija: " + location.LocationName + ".");
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
                NotifyObservers("Locations", "Update", "Izmenjena je lokacija ID=" + location.LocationID + ".");
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

                DialogResult result1 = MessageBox.Show(
                    "Da li sigurno želite da obrišete lokaciju?",
                    "Potvrda brisanja",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result1 != DialogResult.Yes)
                    return;

                //int locationId = int.Parse(txtLocationId.Text);

                bool hasResources = _locationFacade.LocationHasResources(locationId);

                if (hasResources)
                {
                    DialogResult result = MessageBox.Show(
                        "Ova lokacija ima resurse. Da li želiš da obrišeš i te resurse?",
                        "Potvrda brisanja",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.Cancel)
                        return;

                    if (result == DialogResult.No)
                    {
                        MessageBox.Show("Lokacija nije obrisana.");
                        return;
                    }

                    ServiceResult srWithResources = _locationFacade.DeleteLocationWithCheck(locationId, true);
                    MessageBox.Show(srWithResources.Message);
                    if (srWithResources.Success)
                    {
                        LoadLocations();
                        ClearLocationForm();
                        NotifyObservers("Locations", "Delete", "Obrisana je lokacija ID=" + locationId + " zajedno sa resursima.");
                    }
                    return;
                }

                ServiceResult sr = _locationFacade.DeleteLocationWithCheck(locationId, false);
                MessageBox.Show(sr.Message);

                LoadLocations();
                ClearLocationForm();
                NotifyObservers("Locations", "Delete", "Obrisana je lokacija ID=" + locationId + ".");
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