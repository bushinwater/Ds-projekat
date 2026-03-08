using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class LocationsForm : SectionFormBase, IReloadableSection
    {
        private readonly LocationFacade _locationFacade;
        private readonly ResourceFacade _resourceFacade;
        private readonly ReservationFacade _reservationFacade;

        private TextBox _locationNameTextBox;
        private TextBox _locationAddressTextBox;
        private TextBox _locationCityTextBox;
        private TextBox _workingHoursTextBox;
        private NumericUpDown _maxUsersInput;
        private DataGridView _locationsGrid;
        private Label _statusLabel;
        private Label _activeLocationLabel;

        private List<Location> _locations;
        private List<Resource> _resources;
        private List<Reservation> _reservations;
        private int _selectedLocationId;

        public LocationsForm()
        {
            _locationFacade = new LocationFacade();
            _locations = new List<Location>();
            _resourceFacade = new ResourceFacade();
            _reservationFacade = new ReservationFacade();
            _resources = new List<Resource>();
            _reservations = new List<Reservation>();

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Detalji lokacije", 20, 20, 420, 610);

            formGroup.Controls.Add(CreateLabel("Naziv lokacije", 20, 40));
            _locationNameTextBox = CreateTextBox("txtLocationName", 180, 35, 200);
            formGroup.Controls.Add(_locationNameTextBox);

            formGroup.Controls.Add(CreateLabel("Adresa", 20, 85));
            _locationAddressTextBox = CreateTextBox("txtLocationAddress", 180, 80, 200);
            formGroup.Controls.Add(_locationAddressTextBox);

            formGroup.Controls.Add(CreateLabel("Grad", 20, 130));
            _locationCityTextBox = CreateTextBox("txtLocationCity", 180, 125, 200);
            formGroup.Controls.Add(_locationCityTextBox);

            formGroup.Controls.Add(CreateLabel("Radno vreme", 20, 175));
            _workingHoursTextBox = CreateTextBox("txtWorkingHours", 180, 170, 200);
            formGroup.Controls.Add(_workingHoursTextBox);

            Label workingHoursHint = new Label
            {
                Left = 180,
                Top = 200,
                Width = 220,
                Height = 32,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Primer: Mon-Fri 08:00-20:00; Sat 10:00-14:00"
            };
            formGroup.Controls.Add(workingHoursHint);

            formGroup.Controls.Add(CreateLabel("Maks. korisnika", 20, 245));
            _maxUsersInput = CreateNumericUpDown("numMaxUsers", 180, 240, 200, 0, 100000);
            formGroup.Controls.Add(_maxUsersInput);

            Button addButton = CreateActionButton("Dodaj", 20, 320);
            addButton.Click += AddButton_Click;
            formGroup.Controls.Add(addButton);

            Button updateButton = CreateActionButton("Azuriraj", 140, 320);
            updateButton.Click += UpdateButton_Click;
            formGroup.Controls.Add(updateButton);

            Button deleteButton = CreateActionButton("Obrisi", 260, 320);
            deleteButton.Click += DeleteButton_Click;
            formGroup.Controls.Add(deleteButton);

            Button clearButton = CreateActionButton("Ocisti", 20, 370);
            clearButton.Click += ClearButton_Click;
            formGroup.Controls.Add(clearButton);

            Button refreshButton = CreateActionButton("Osvezi", 140, 370);
            refreshButton.Click += RefreshButton_Click;
            formGroup.Controls.Add(refreshButton);

            // Ova lokacija postaje podrazumevana za filtere i nove rezervacije.
            Button setActiveButton = CreateActionButton("Koristi za filtere", 260, 370);
            setActiveButton.Click += SetActiveButton_Click;
            formGroup.Controls.Add(setActiveButton);

            _statusLabel = new Label
            {
                Left = 20,
                Top = 430,
                Width = 360,
                Height = 58,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Lokacije se citaju i menjaju direktno iz baze. Dugme ispod postavlja lokaciju kao podrazumevanu za filtere."
            };
            formGroup.Controls.Add(_statusLabel);

            _activeLocationLabel = new Label
            {
                Left = 20,
                Top = 485,
                Width = 360,
                Height = 45,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
                Text = "Lokacija za filtere: nije izabrana"
            };
            formGroup.Controls.Add(_activeLocationLabel);

            GroupBox listGroup = CreateGroupBox("Lista lokacija", 460, 20, 660, 610);
            _locationsGrid = CreateGrid("dgvLocations", 15, 30, 630, 560);
            _locationsGrid.SelectionChanged += LocationsGrid_SelectionChanged;
            listGroup.Controls.Add(_locationsGrid);

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }

        public void LoadData()
        {
            try
            {
                int selectedLocationId = _selectedLocationId;
                _locations = _locationFacade.GetAll();
                _resources = _resourceFacade.GetAllResources();
                _reservations = _reservationFacade.GetAllReservations();

                _locationsGrid.DataSource = null;
                _locationsGrid.DataSource = _locations
                    .Select(l => new LocationGridRow
                    {
                        LocationID = l.LocationID,
                        LocationName = l.LocationName,
                        AddressName = l.AddressName,
                        City = l.City,
                        WorkingHours = l.WorkingHours,
                        MaxUsers = l.MaxUsers,
                        TotalResources = _resources.Count(r => r.LocationID == l.LocationID),
                        ActiveReservations = GetActiveReservationCount(l.LocationID),
                        OccupancyPercent = GetOccupancyPercent(l.LocationID)
                    })
                    .ToList();
                SetGridHeader(_locationsGrid, "LocationID", "ID");
                SetGridHeader(_locationsGrid, "LocationName", "Naziv");
                SetGridHeader(_locationsGrid, "AddressName", "Adresa");
                SetGridHeader(_locationsGrid, "City", "Grad");
                SetGridHeader(_locationsGrid, "WorkingHours", "Radno vreme");
                SetGridHeader(_locationsGrid, "MaxUsers", "Maks. korisnika");
                SetGridHeader(_locationsGrid, "TotalResources", "Ukupno resursa");
                SetGridHeader(_locationsGrid, "ActiveReservations", "Aktivne rezervacije");
                SetGridHeader(_locationsGrid, "OccupancyPercent", "Zauzetost");

                if (selectedLocationId > 0)
                    SelectLocation(selectedLocationId);
                else
                    ClearEditor();

                UpdateActiveLocationLabel();
                SetStatus("Lokacije su ucitane iz baze.", true);
            }
            catch (Exception ex)
            {
                SetStatus("Greska pri ucitavanju lokacija: " + ex.Message, false);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Location location = BuildLocationFromForm();
            ServiceResult result = _locationFacade.AddLocation(location);
            HandleResult(result, true);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (_selectedLocationId <= 0)
            {
                SetStatus("Izaberi lokaciju za azuriranje.", false);
                return;
            }

            Location location = BuildLocationFromForm();
            location.LocationID = _selectedLocationId;

            ServiceResult result = _locationFacade.UpdateLocation(location);
            HandleResult(result, false);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedLocationId <= 0)
            {
                SetStatus("Izaberi lokaciju za brisanje.", false);
                return;
            }

            DialogResult dialog = MessageBox.Show(
                "Da li zelis da obrises izabranu lokaciju?",
                "Delete location",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog != DialogResult.Yes)
                return;

            ServiceResult result = _locationFacade.DeleteLocation(_selectedLocationId);
            HandleResult(result, true);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearEditor();
            SetStatus("Forma je ociscena.", true);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SetActiveButton_Click(object sender, EventArgs e)
        {
            if (_selectedLocationId <= 0)
            {
                SetStatus("Izaberi lokaciju koju zelis da postavis kao aktivnu.", false);
                return;
            }

            ActiveLocationContext.Instance.SetActiveLocation(_selectedLocationId, _locationNameTextBox.Text.Trim());
            UpdateActiveLocationLabel();
            SetStatus("Lokacija je postavljena kao podrazumevana za filtere.", true);
        }

        private void LocationsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_locationsGrid.CurrentRow == null)
                return;

            LocationGridRow row = _locationsGrid.CurrentRow.DataBoundItem as LocationGridRow;
            if (row == null)
                return;

            _selectedLocationId = row.LocationID;
            _locationNameTextBox.Text = row.LocationName;
            _locationAddressTextBox.Text = row.AddressName;
            _locationCityTextBox.Text = row.City;
            _workingHoursTextBox.Text = row.WorkingHours;
            _maxUsersInput.Value = row.MaxUsers;
        }

        private Location BuildLocationFromForm()
        {
            return new Location
            {
                LocationName = _locationNameTextBox.Text.Trim(),
                AddressName = _locationAddressTextBox.Text.Trim(),
                City = _locationCityTextBox.Text.Trim(),
                WorkingHours = _workingHoursTextBox.Text.Trim(),
                MaxUsers = Decimal.ToInt32(_maxUsersInput.Value)
            };
        }

        private void HandleResult(ServiceResult result, bool clearAfterSuccess)
        {
            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            int selectedLocationId = result.NewId > 0 ? result.NewId : _selectedLocationId;
            LoadData();
            SetStatus(result.Message, result.Success);

            if (clearAfterSuccess)
                ClearEditor();
            else if (selectedLocationId > 0)
                SelectLocation(selectedLocationId);
        }

        private void SelectLocation(int locationId)
        {
            foreach (DataGridViewRow row in _locationsGrid.Rows)
            {
                LocationGridRow data = row.DataBoundItem as LocationGridRow;
                if (data == null || data.LocationID != locationId)
                    continue;

                row.Selected = true;
                _locationsGrid.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void ClearEditor()
        {
            _selectedLocationId = 0;
            _locationNameTextBox.Clear();
            _locationAddressTextBox.Clear();
            _locationCityTextBox.Clear();
            _workingHoursTextBox.Clear();
            _maxUsersInput.Value = 0;
            _locationsGrid.ClearSelection();
            _locationsGrid.CurrentCell = null;
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private int GetActiveReservationCount(int locationId)
        {
            HashSet<int> resourceIds = new HashSet<int>(_resources
                .Where(r => r.LocationID == locationId)
                .Select(r => r.ResourceID));

            return _reservations.Count(r =>
                resourceIds.Contains(r.ResourceID) &&
                string.Equals(r.ReservationStatus, "Active", StringComparison.OrdinalIgnoreCase));
        }

        private string GetOccupancyPercent(int locationId)
        {
            int totalResources = _resources.Count(r => r.LocationID == locationId);
            if (totalResources <= 0)
                return "0%";

            int activeReservations = GetActiveReservationCount(locationId);
            double percent = (double)activeReservations / totalResources * 100;
            return percent.ToString("0.##") + "%";
        }

        private void UpdateActiveLocationLabel()
        {
            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
            {
                _activeLocationLabel.Text = "Lokacija za filtere: " + ActiveLocationContext.Instance.ActiveLocationName;
                _activeLocationLabel.ForeColor = AppTheme.PrimaryColor;
                return;
            }

            _activeLocationLabel.Text = "Lokacija za filtere: nije izabrana";
            _activeLocationLabel.ForeColor = MutedTextColor;
        }

        private sealed class LocationGridRow
        {
            public int LocationID { get; set; }
            public string LocationName { get; set; } = "";
            public string AddressName { get; set; } = "";
            public string City { get; set; } = "";
            public string WorkingHours { get; set; } = "";
            public int MaxUsers { get; set; }
            public int TotalResources { get; set; }
            public int ActiveReservations { get; set; }
            public string OccupancyPercent { get; set; } = "";
        }
    }
}
