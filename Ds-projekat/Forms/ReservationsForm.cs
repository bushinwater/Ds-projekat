using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class ReservationsForm : SectionFormBase, IReloadableSection
    {
        private readonly ReservationFacade _reservationFacade;
        private readonly UserFacade _userFacade;
        private readonly ResourceFacade _resourceFacade;
        private readonly LocationFacade _locationFacade;

        private ComboBox _userComboBox;
        private ComboBox _resourceComboBox;
        private NumericUpDown _usersCountInput;
        private DateTimePicker _startPicker;
        private DateTimePicker _endPicker;
        private ComboBox _statusComboBox;

        private ComboBox _filterUserComboBox;
        private ComboBox _filterLocationComboBox;
        private DateTimePicker _filterDayPicker;
        private CheckBox _filterDayCheckBox;

        private DataGridView _reservationsGrid;
        private Label _statusLabel;
        private bool _suspendPreviewBinding;

        private List<User> _users;
        private List<Location> _locations;
        private List<Resource> _resources;
        private List<Reservation> _reservations;
        private int _selectedReservationId;

        public ReservationsForm()
        {
            _reservationFacade = new ReservationFacade();
            _userFacade = new UserFacade();
            _resourceFacade = new ResourceFacade();
            _locationFacade = new LocationFacade();
            _users = new List<User>();
            _locations = new List<Location>();
            _resources = new List<Resource>();
            _reservations = new List<Reservation>();
            ActiveLocationContext.Instance.ActiveLocationChanged += ActiveLocationContext_ActiveLocationChanged;

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Pregled rezervacije", 20, 20, 430, 610);

            formGroup.Controls.Add(CreateLabel("Korisnik", 20, 40));
            _userComboBox = CreateComboBox("cbReservationUser", 180, 35, 210);
            formGroup.Controls.Add(_userComboBox);

            formGroup.Controls.Add(CreateLabel("Resurs", 20, 85));
            _resourceComboBox = CreateComboBox("cbReservationResource", 180, 80, 210);
            formGroup.Controls.Add(_resourceComboBox);

            formGroup.Controls.Add(CreateLabel("Broj korisnika", 20, 130));
            _usersCountInput = CreateNumericUpDown("numReservationUsersCount", 180, 125, 210, 0, 500);
            formGroup.Controls.Add(_usersCountInput);

            formGroup.Controls.Add(CreateLabel("Pocetak", 20, 175));
            _startPicker = CreateDateTimePicker("dtpReservationStart", 180, 170, 210);
            formGroup.Controls.Add(_startPicker);

            formGroup.Controls.Add(CreateLabel("Kraj", 20, 220));
            _endPicker = CreateDateTimePicker("dtpReservationEnd", 180, 215, 210);
            formGroup.Controls.Add(_endPicker);

            formGroup.Controls.Add(CreateLabel("Status", 20, 265));
            _statusComboBox = CreateComboBox("cbReservationStatus", 180, 260, 210);
            _statusComboBox.Items.AddRange(new object[] { "Active", "Finished", "Canceled" });
            formGroup.Controls.Add(_statusComboBox);

            // Rezervacije se otvaraju kroz modalni dijalog da zahtev zadatka bude pokriven.
            Button createButton = CreateActionButton("Nova rezervacija", 20, 345);
            createButton.Click += CreateButton_Click;
            formGroup.Controls.Add(createButton);

            Button updateButton = CreateActionButton("Izmeni dijalog", 140, 345);
            updateButton.Click += UpdateButton_Click;
            formGroup.Controls.Add(updateButton);

            Button clearButton = CreateActionButton("Ocisti pregled", 260, 345);
            clearButton.Click += ClearButton_Click;
            formGroup.Controls.Add(clearButton);

            Button refreshButton = CreateActionButton("Osvezi", 20, 395);
            refreshButton.Click += RefreshButton_Click;
            formGroup.Controls.Add(refreshButton);

            _statusLabel = new Label
            {
                Left = 20,
                Top = 455,
                Width = 370,
                Height = 85,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Kreiranje, izmena, otkazivanje i zavrsavanje rade se kroz dialog formu."
            };
            formGroup.Controls.Add(_statusLabel);

            GroupBox listGroup = CreateGroupBox("Lista rezervacija", 470, 20, 650, 610);

            listGroup.Controls.Add(CreateLabel("Korisnik", 15, 38));
            _filterUserComboBox = CreateComboBox("cbReservationFilterUser", 70, 33, 180);
            listGroup.Controls.Add(_filterUserComboBox);

            listGroup.Controls.Add(CreateLabel("Lokacija", 270, 38));
            _filterLocationComboBox = CreateComboBox("cbReservationFilterLocation", 340, 33, 180);
            listGroup.Controls.Add(_filterLocationComboBox);

            _filterDayCheckBox = CreateCheckBox("chkReservationFilterDay", "Samo izabrani dan", 15, 76);
            listGroup.Controls.Add(_filterDayCheckBox);

            _filterDayPicker = CreateDatePicker("dtpReservationFilterDay", 160, 72, 150);
            listGroup.Controls.Add(_filterDayPicker);

            Button applyFiltersButton = CreateActionButton("Primeni filtere", 340, 69, 130);
            applyFiltersButton.Click += ApplyFiltersButton_Click;
            listGroup.Controls.Add(applyFiltersButton);

            Button clearFiltersButton = CreateActionButton("Ocisti filtere", 485, 69, 130);
            clearFiltersButton.Click += ClearFiltersButton_Click;
            listGroup.Controls.Add(clearFiltersButton);

            _reservationsGrid = CreateGrid("dgvReservations", 15, 120, 620, 470);
            _reservationsGrid.SelectionChanged += ReservationsGrid_SelectionChanged;
            listGroup.Controls.Add(_reservationsGrid);

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }

        public void LoadData()
        {
            try
            {
                int selectedReservationId = _selectedReservationId;
                _users = _userFacade.GetAll();
                _locations = _locationFacade.GetAll();
                _resources = _resourceFacade.GetAllResources();
                _reservations = _reservationFacade.GetAllReservations();

                BindUsers();
                BindResources();
                BindFilterUsers();
                BindFilterLocations();
                ApplyFilters();

                if (selectedReservationId > 0)
                    SelectReservation(selectedReservationId);
                else
                    ClearEditor();

                SetStatus("Rezervacije su ucitane iz baze.", true);
            }
            catch (Exception ex)
            {
                SetStatus("Greska pri ucitavanju rezervacija: " + ex.Message, false);
            }
        }

        private void BindUsers()
        {
            _userComboBox.DataSource = null;
            _userComboBox.DisplayMember = "DisplayText";
            _userComboBox.ValueMember = "UserID";
            _userComboBox.DataSource = _users
                .Select(u => new UserOption
                {
                    UserID = u.UserID,
                    DisplayText = u.FirstName + " " + u.LastName + " (" + u.Email + ")"
                })
                .ToList();
            _userComboBox.SelectedIndex = -1;
        }

        private void BindResources()
        {
            IEnumerable<Resource> resourcesForPicker = _resources;
            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
            {
                resourcesForPicker = resourcesForPicker.Where(r => r.LocationID == ActiveLocationContext.Instance.ActiveLocationId);
            }

            _resourceComboBox.DataSource = null;
            _resourceComboBox.DisplayMember = "DisplayText";
            _resourceComboBox.ValueMember = "ResourceID";
            _resourceComboBox.DataSource = resourcesForPicker
                .Select(r => new ResourceOption
                {
                    ResourceID = r.ResourceID,
                    DisplayText = r.ResourceName + " [" + r.ResourceType + "]"
                })
                .ToList();
            _resourceComboBox.SelectedIndex = -1;
        }

        private void BindFilterUsers()
        {
            _filterUserComboBox.DataSource = null;
            _filterUserComboBox.DisplayMember = "DisplayText";
            _filterUserComboBox.ValueMember = "UserID";
            _filterUserComboBox.DataSource = _users
                .Select(u => new UserOption
                {
                    UserID = u.UserID,
                    DisplayText = u.FirstName + " " + u.LastName + " (" + u.Email + ")"
                })
                .ToList();
            _filterUserComboBox.SelectedIndex = -1;
        }

        private void BindFilterLocations()
        {
            _filterLocationComboBox.DataSource = null;
            _filterLocationComboBox.DisplayMember = "LocationName";
            _filterLocationComboBox.ValueMember = "LocationID";
            _filterLocationComboBox.DataSource = _locations.ToList();

            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
                _filterLocationComboBox.SelectedValue = ActiveLocationContext.Instance.ActiveLocationId;
            else
                _filterLocationComboBox.SelectedIndex = -1;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            OpenReservationDialog(null);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (_selectedReservationId <= 0)
            {
                SetStatus("Izaberi rezervaciju za otvaranje dijaloga.", false);
                return;
            }

            OpenReservationDialog(_selectedReservationId);
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

        private void ApplyFiltersButton_Click(object sender, EventArgs e)
        {
            ApplyFilters();
            SetStatus("Prikaz rezervacija je filtriran.", true);
        }

        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            _filterUserComboBox.SelectedIndex = -1;
            _filterLocationComboBox.SelectedIndex = -1;
            _filterDayCheckBox.Checked = false;
            _filterDayPicker.Value = DateTime.Today;
            ApplyFilters();
            SetStatus("Filteri rezervacija su ocisceni.", true);
        }

        private void ReservationsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_suspendPreviewBinding)
                return;

            if (_reservationsGrid.CurrentRow == null)
            {
                ClearPreviewFields();
                return;
            }

            ReservationGridRow row = _reservationsGrid.CurrentRow.DataBoundItem as ReservationGridRow;
            if (row == null)
            {
                ClearPreviewFields();
                return;
            }

            _selectedReservationId = row.ReservationID;
            _usersCountInput.Value = row.UsersCount.HasValue && row.UsersCount.Value > 0 ? row.UsersCount.Value : 0;
            _startPicker.Value = row.StartDateTime;
            _endPicker.Value = row.EndDateTime;
            _statusComboBox.Text = row.ReservationStatus;

            if (_userComboBox.Items.Count > 0 && _users.Any(u => u.UserID == row.UserID))
                _userComboBox.SelectedValue = row.UserID;
            else
                _userComboBox.SelectedIndex = -1;

            if (_resourceComboBox.Items.Count > 0 && ((_resourceComboBox.DataSource as IEnumerable<ResourceOption>) ?? Enumerable.Empty<ResourceOption>()).Any(r => r.ResourceID == row.ResourceID))
                _resourceComboBox.SelectedValue = row.ResourceID;
            else
                _resourceComboBox.SelectedIndex = -1;
        }

        private void ApplyFilters()
        {
            Dictionary<int, string> userNames = _users.ToDictionary(u => u.UserID, u => u.FirstName + " " + u.LastName);
            Dictionary<int, string> resourceNames = _resources.ToDictionary(r => r.ResourceID, r => r.ResourceName + " [" + r.ResourceType + "]");
            Dictionary<int, int> resourceLocationIds = _resources.ToDictionary(r => r.ResourceID, r => r.LocationID);
            Dictionary<int, string> locationNames = _locations.ToDictionary(l => l.LocationID, l => l.LocationName);

            IEnumerable<Reservation> filtered = _reservations;

            if (_filterUserComboBox.SelectedValue != null)
            {
                int userId = Convert.ToInt32(_filterUserComboBox.SelectedValue);
                filtered = filtered.Where(r => r.UserID == userId);
            }

            if (_filterLocationComboBox.SelectedValue != null)
            {
                int locationId = Convert.ToInt32(_filterLocationComboBox.SelectedValue);
                filtered = filtered.Where(r =>
                    resourceLocationIds.ContainsKey(r.ResourceID) &&
                    resourceLocationIds[r.ResourceID] == locationId);
            }

            if (_filterDayCheckBox.Checked)
            {
                DateTime selectedDay = _filterDayPicker.Value.Date;
                filtered = filtered.Where(r => r.StartDateTime.Date == selectedDay);
            }

            _reservationsGrid.DataSource = null;
            _reservationsGrid.DataSource = filtered
                .Select(r => new ReservationGridRow
                {
                    ReservationID = r.ReservationID,
                    UserID = r.UserID,
                    User = userNames.ContainsKey(r.UserID) ? userNames[r.UserID] : "",
                    ResourceID = r.ResourceID,
                    Resource = resourceNames.ContainsKey(r.ResourceID) ? resourceNames[r.ResourceID] : "",
                    LocationID = resourceLocationIds.ContainsKey(r.ResourceID) ? resourceLocationIds[r.ResourceID] : 0,
                    Location = resourceLocationIds.ContainsKey(r.ResourceID) &&
                               locationNames.ContainsKey(resourceLocationIds[r.ResourceID])
                        ? locationNames[resourceLocationIds[r.ResourceID]]
                        : "",
                    UsersCount = r.UsersCount,
                    StartDateTime = r.StartDateTime,
                    EndDateTime = r.EndDateTime,
                    ReservationStatus = r.ReservationStatus
                })
                .OrderBy(r => r.StartDateTime)
                .ToList();
            SetGridHeader(_reservationsGrid, "ReservationID", "ID");
            SetGridHeader(_reservationsGrid, "User", "Korisnik");
            SetGridHeader(_reservationsGrid, "Resource", "Resurs");
            SetGridHeader(_reservationsGrid, "Location", "Lokacija");
            SetGridHeader(_reservationsGrid, "UsersCount", "Broj korisnika");
            SetGridHeader(_reservationsGrid, "StartDateTime", "Pocetak");
            SetGridHeader(_reservationsGrid, "EndDateTime", "Kraj");
            SetGridHeader(_reservationsGrid, "ReservationStatus", "Status");
        }

        private void HandleResult(ServiceResult result, bool clearAfterSuccess)
        {
            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            int reservationId = result.NewId > 0 ? result.NewId : _selectedReservationId;
            LoadData();
            SetStatus(result.Message, result.Success);

            if (clearAfterSuccess)
                ClearEditor();
            else if (reservationId > 0)
                SelectReservation(reservationId);
        }

        private void SelectReservation(int reservationId)
        {
            foreach (DataGridViewRow row in _reservationsGrid.Rows)
            {
                ReservationGridRow data = row.DataBoundItem as ReservationGridRow;
                if (data == null || data.ReservationID != reservationId)
                    continue;

                row.Selected = true;
                _reservationsGrid.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void ClearEditor()
        {
            _suspendPreviewBinding = true;
            ClearPreviewFields();
            _reservationsGrid.ClearSelection();
            _reservationsGrid.CurrentCell = null;
            _suspendPreviewBinding = false;
        }

        private void OpenReservationDialog(int? reservationId)
        {
            using (ReservationDialogForm dialog = new ReservationDialogForm(reservationId))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
            }

            LoadData();
            SetStatus("Rezervacije su osvezene posle rada u dijalogu.", true);
        }

        // Preview panel se prazni kada nema selekcije ili kada korisnik to trazi.
        private void ClearPreviewFields()
        {
            _selectedReservationId = 0;
            _userComboBox.SelectedIndex = -1;
            _resourceComboBox.SelectedIndex = -1;
            _usersCountInput.Value = 0;
            _startPicker.Value = DateTime.Now;
            _endPicker.Value = DateTime.Now.AddHours(1);
            _statusComboBox.SelectedIndex = -1;
        }

        private int GetSelectedUserId()
        {
            return _userComboBox.SelectedValue == null ? 0 : Convert.ToInt32(_userComboBox.SelectedValue);
        }

        private int GetSelectedResourceId()
        {
            return _resourceComboBox.SelectedValue == null ? 0 : Convert.ToInt32(_resourceComboBox.SelectedValue);
        }

        private int? GetUsersCount()
        {
            return _usersCountInput.Value > 0 ? Decimal.ToInt32(_usersCountInput.Value) : (int?)null;
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private void ActiveLocationContext_ActiveLocationChanged(object sender, EventArgs e)
        {
            BindResources();
            BindFilterLocations();
            ApplyFilters();
        }

        private sealed class UserOption
        {
            public int UserID { get; set; }
            public string DisplayText { get; set; } = "";
        }

        private sealed class ResourceOption
        {
            public int ResourceID { get; set; }
            public string DisplayText { get; set; } = "";
        }

        private sealed class ReservationGridRow
        {
            public int ReservationID { get; set; }
            public int UserID { get; set; }
            public string User { get; set; } = "";
            public int ResourceID { get; set; }
            public string Resource { get; set; } = "";
            public int LocationID { get; set; }
            public string Location { get; set; } = "";
            public int? UsersCount { get; set; }
            public DateTime StartDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
            public string ReservationStatus { get; set; } = "";
        }
    }
}
