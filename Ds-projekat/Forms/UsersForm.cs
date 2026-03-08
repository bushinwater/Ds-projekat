using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class UsersForm : SectionFormBase, IReloadableSection
    {
        private readonly UserFacade _userFacade;
        private readonly MembershipFacade _membershipFacade;
        private readonly LocationFacade _locationFacade;
        private readonly ResourceFacade _resourceFacade;
        private readonly ReservationFacade _reservationFacade;

        private TextBox _firstNameTextBox;
        private TextBox _lastNameTextBox;
        private TextBox _emailTextBox;
        private TextBox _phoneTextBox;
        private ComboBox _membershipComboBox;
        private DateTimePicker _membershipStartPicker;
        private DateTimePicker _membershipEndPicker;
        private ComboBox _statusComboBox;
        private TextBox _searchTextBox;
        private ComboBox _locationFilterComboBox;
        private ComboBox _membershipFilterComboBox;
        private ComboBox _statusFilterComboBox;
        private DataGridView _usersGrid;
        private Label _statusLabel;

        private List<User> _users;
        private List<MembershipType> _memberships;
        private List<Location> _locations;
        private List<Resource> _resources;
        private List<Reservation> _reservations;
        private int _selectedUserId;

        public UsersForm()
        {
            _userFacade = new UserFacade();
            _membershipFacade = new MembershipFacade();
            _locationFacade = new LocationFacade();
            _resourceFacade = new ResourceFacade();
            _reservationFacade = new ReservationFacade();
            _users = new List<User>();
            _memberships = new List<MembershipType>();
            _locations = new List<Location>();
            _resources = new List<Resource>();
            _reservations = new List<Reservation>();
            ActiveLocationContext.Instance.ActiveLocationChanged += ActiveLocationContext_ActiveLocationChanged;

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Detalji korisnika", 20, 20, 420, 610);

            formGroup.Controls.Add(CreateLabel("Ime", 20, 40));
            _firstNameTextBox = CreateTextBox("txtFirstName", 180, 35, 200);
            formGroup.Controls.Add(_firstNameTextBox);

            formGroup.Controls.Add(CreateLabel("Prezime", 20, 85));
            _lastNameTextBox = CreateTextBox("txtLastName", 180, 80, 200);
            formGroup.Controls.Add(_lastNameTextBox);

            formGroup.Controls.Add(CreateLabel("Email", 20, 130));
            _emailTextBox = CreateTextBox("txtUserEmail", 180, 125, 200);
            formGroup.Controls.Add(_emailTextBox);

            formGroup.Controls.Add(CreateLabel("Telefon", 20, 175));
            _phoneTextBox = CreateTextBox("txtUserPhone", 180, 170, 200);
            formGroup.Controls.Add(_phoneTextBox);

            formGroup.Controls.Add(CreateLabel("Tip clanarine", 20, 220));
            _membershipComboBox = CreateComboBox("cbUserMembershipType", 180, 215, 200);
            formGroup.Controls.Add(_membershipComboBox);

            formGroup.Controls.Add(CreateLabel("Datum pocetka", 20, 265));
            _membershipStartPicker = CreateDatePicker("dtpMembershipStart", 180, 260, 200);
            formGroup.Controls.Add(_membershipStartPicker);

            formGroup.Controls.Add(CreateLabel("Datum isteka", 20, 310));
            _membershipEndPicker = CreateDatePicker("dtpMembershipEnd", 180, 305, 200);
            formGroup.Controls.Add(_membershipEndPicker);

            formGroup.Controls.Add(CreateLabel("Status", 20, 355));
            _statusComboBox = CreateComboBox("cbUserStatus", 180, 350, 200);
            _statusComboBox.Items.AddRange(new object[] { "Active", "Paused", "Expired" });
            formGroup.Controls.Add(_statusComboBox);

            Button addButton = CreateActionButton("Dodaj", 20, 430);
            addButton.Click += AddButton_Click;
            formGroup.Controls.Add(addButton);

            Button updateButton = CreateActionButton("Azuriraj", 140, 430);
            updateButton.Click += UpdateButton_Click;
            formGroup.Controls.Add(updateButton);

            Button deleteButton = CreateActionButton("Obrisi", 260, 430);
            deleteButton.Click += DeleteButton_Click;
            formGroup.Controls.Add(deleteButton);

            Button clearButton = CreateActionButton("Ocisti", 20, 480);
            clearButton.Click += ClearButton_Click;
            formGroup.Controls.Add(clearButton);

            Button filterFromEditorButton = CreateActionButton("Pretrazi", 140, 480);
            filterFromEditorButton.Click += SearchButton_Click;
            formGroup.Controls.Add(filterFromEditorButton);

            Button refreshButton = CreateActionButton("Osvezi", 260, 480);
            refreshButton.Click += RefreshButton_Click;
            formGroup.Controls.Add(refreshButton);

            _statusLabel = new Label
            {
                Left = 20,
                Top = 535,
                Width = 360,
                Height = 58,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Pregled i izmena korisnika iz baze."
            };
            formGroup.Controls.Add(_statusLabel);

            GroupBox listGroup = CreateGroupBox("Lista korisnika", 460, 20, 660, 610);
            // Filteri liste su odvojeni od forme za unos da admin lakse pretrazuje korisnike.
            listGroup.Controls.Add(CreateLabel("Pretraga", 15, 40));
            _searchTextBox = CreateTextBox("txtUserSearch", 75, 35, 180);
            listGroup.Controls.Add(_searchTextBox);

            listGroup.Controls.Add(CreateLabel("Lokacija", 275, 40));
            _locationFilterComboBox = CreateComboBox("cbUserLocationFilter", 340, 35, 140);
            listGroup.Controls.Add(_locationFilterComboBox);

            listGroup.Controls.Add(CreateLabel("Clanarina", 15, 80));
            _membershipFilterComboBox = CreateComboBox("cbUserMembershipFilter", 95, 75, 160);
            listGroup.Controls.Add(_membershipFilterComboBox);

            listGroup.Controls.Add(CreateLabel("Status", 275, 80));
            _statusFilterComboBox = CreateComboBox("cbUserStatusFilter", 340, 75, 140);
            _statusFilterComboBox.Items.AddRange(new object[] { "Active", "Paused", "Expired" });
            listGroup.Controls.Add(_statusFilterComboBox);

            Button applyFiltersButton = CreateActionButton("Primeni filtere", 500, 35, 130);
            applyFiltersButton.Click += SearchButton_Click;
            listGroup.Controls.Add(applyFiltersButton);

            Button clearFiltersButton = CreateActionButton("Ocisti filtere", 500, 75, 130);
            clearFiltersButton.Click += ClearFiltersButton_Click;
            listGroup.Controls.Add(clearFiltersButton);

            _usersGrid = CreateGrid("dgvUsers", 15, 130, 630, 460);
            _usersGrid.SelectionChanged += UsersGrid_SelectionChanged;
            listGroup.Controls.Add(_usersGrid);

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }

        public void LoadData()
        {
            try
            {
                int selectedUserId = _selectedUserId;
                _memberships = _membershipFacade.GetAll();
                _locations = _locationFacade.GetAll();
                _resources = _resourceFacade.GetAllResources();
                _reservations = _reservationFacade.GetAllReservations();
                _users = _userFacade.GetAll();

                BindMemberships();
                BindLocationFilters();
                BindUsers(_users);

                if (selectedUserId > 0)
                    SelectUser(selectedUserId);
                else
                    ClearEditor();

                SetStatus("Korisnici su ucitani iz baze.", true);
            }
            catch (Exception ex)
            {
                SetStatus("Greska pri ucitavanju korisnika: " + ex.Message, false);
            }
        }

        private void BindMemberships()
        {
            _membershipComboBox.DataSource = null;
            _membershipComboBox.DisplayMember = "PackageName";
            _membershipComboBox.ValueMember = "MembershipTypeID";
            _membershipComboBox.DataSource = _memberships.ToList();
            _membershipComboBox.SelectedIndex = -1;

            _membershipFilterComboBox.DataSource = null;
            _membershipFilterComboBox.DisplayMember = "PackageName";
            _membershipFilterComboBox.ValueMember = "MembershipTypeID";
            _membershipFilterComboBox.DataSource = _memberships.ToList();
            _membershipFilterComboBox.SelectedIndex = -1;
        }

        private void BindUsers(List<User> users)
        {
            Dictionary<int, string> membershipNames = _memberships.ToDictionary(m => m.MembershipTypeID, m => m.PackageName);

            List<UserGridRow> rows = users.Select(u => new UserGridRow
            {
                UserID = u.UserID,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Phone = u.Phone,
                Membership = membershipNames.ContainsKey(u.MembershipTypeID) ? membershipNames[u.MembershipTypeID] : "",
                MembershipTypeID = u.MembershipTypeID,
                MembershipStartDate = u.MembershipStartDate,
                MembershipEndDate = u.MembershipEndDate,
                AccountStatus = u.AccountStatus
            }).ToList();

            _usersGrid.DataSource = null;
            _usersGrid.DataSource = rows;
            SetGridHeader(_usersGrid, "UserID", "ID");
            SetGridHeader(_usersGrid, "FirstName", "Ime");
            SetGridHeader(_usersGrid, "LastName", "Prezime");
            SetGridHeader(_usersGrid, "Email", "Email");
            SetGridHeader(_usersGrid, "Phone", "Telefon");
            SetGridHeader(_usersGrid, "Membership", "Clanarina");
            SetGridHeader(_usersGrid, "MembershipStartDate", "Pocetak");
            SetGridHeader(_usersGrid, "MembershipEndDate", "Istek");
            SetGridHeader(_usersGrid, "AccountStatus", "Status");
        }

        private void BindLocationFilters()
        {
            _locationFilterComboBox.DataSource = null;
            _locationFilterComboBox.DisplayMember = "LocationName";
            _locationFilterComboBox.ValueMember = "LocationID";
            _locationFilterComboBox.DataSource = _locations.ToList();

            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
                _locationFilterComboBox.SelectedValue = ActiveLocationContext.Instance.ActiveLocationId;
            else
                _locationFilterComboBox.SelectedIndex = -1;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            User user = BuildUserFromForm();
            ServiceResult result = _userFacade.AddUser(user);
            HandleResult(result, true);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (_selectedUserId <= 0)
            {
                SetStatus("Izaberi korisnika za azuriranje.", false);
                return;
            }

            User user = BuildUserFromForm();
            user.UserID = _selectedUserId;

            ServiceResult result = _userFacade.UpdateUser(user);
            HandleResult(result, false);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedUserId <= 0)
            {
                SetStatus("Izaberi korisnika za brisanje.", false);
                return;
            }

            DialogResult dialog = MessageBox.Show(
                "Da li zelis da obrises izabranog korisnika?",
                "Delete user",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog != DialogResult.Yes)
                return;

            ServiceResult result = _userFacade.DeleteUser(_selectedUserId);
            HandleResult(result, true);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearEditor();
            SetStatus("Forma je ociscena.", true);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            IEnumerable<User> filtered = _users;

            if (!string.IsNullOrWhiteSpace(_searchTextBox.Text))
            {
                string search = _searchTextBox.Text.Trim();
                filtered = filtered.Where(u =>
                    u.FirstName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    u.LastName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    u.Email.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    u.Phone.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (_statusFilterComboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(_statusFilterComboBox.Text))
            {
                filtered = filtered.Where(u => string.Equals(u.AccountStatus, _statusFilterComboBox.Text, StringComparison.OrdinalIgnoreCase));
            }

            if (_membershipFilterComboBox.SelectedValue != null)
            {
                int membershipTypeId = Convert.ToInt32(_membershipFilterComboBox.SelectedValue);
                filtered = filtered.Where(u => u.MembershipTypeID == membershipTypeId);
            }

            if (_locationFilterComboBox.SelectedValue != null)
            {
                int locationId = Convert.ToInt32(_locationFilterComboBox.SelectedValue);
                HashSet<int> resourceIds = new HashSet<int>(
                    _resources.Where(r => r.LocationID == locationId).Select(r => r.ResourceID));

                HashSet<int> userIds = new HashSet<int>(
                    _reservations.Where(r => resourceIds.Contains(r.ResourceID)).Select(r => r.UserID));

                filtered = filtered.Where(u => userIds.Contains(u.UserID));
            }

            List<User> rows = filtered.ToList();
            BindUsers(rows);
            SetStatus("Prikazano korisnika: " + rows.Count, true);
        }

        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            _searchTextBox.Clear();
            _membershipFilterComboBox.SelectedIndex = -1;
            _statusFilterComboBox.SelectedIndex = -1;

            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
                _locationFilterComboBox.SelectedValue = ActiveLocationContext.Instance.ActiveLocationId;
            else
                _locationFilterComboBox.SelectedIndex = -1;

            BindUsers(_users);
            SetStatus("Filteri korisnika su ocisceni.", true);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void UsersGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_usersGrid.CurrentRow == null)
                return;

            UserGridRow row = _usersGrid.CurrentRow.DataBoundItem as UserGridRow;
            if (row == null)
                return;

            _selectedUserId = row.UserID;
            _firstNameTextBox.Text = row.FirstName;
            _lastNameTextBox.Text = row.LastName;
            _emailTextBox.Text = row.Email;
            _phoneTextBox.Text = row.Phone;
            _membershipStartPicker.Value = row.MembershipStartDate;
            _membershipEndPicker.Value = row.MembershipEndDate;
            _statusComboBox.Text = row.AccountStatus;

            if (_membershipComboBox.Items.Count > 0)
                _membershipComboBox.SelectedValue = row.MembershipTypeID;
        }

        private User BuildUserFromForm()
        {
            return new User
            {
                FirstName = _firstNameTextBox.Text.Trim(),
                LastName = _lastNameTextBox.Text.Trim(),
                Email = _emailTextBox.Text.Trim(),
                Phone = _phoneTextBox.Text.Trim(),
                MembershipTypeID = _membershipComboBox.SelectedValue == null ? 0 : Convert.ToInt32(_membershipComboBox.SelectedValue),
                MembershipStartDate = _membershipStartPicker.Value.Date,
                MembershipEndDate = _membershipEndPicker.Value.Date,
                AccountStatus = string.IsNullOrWhiteSpace(_statusComboBox.Text) ? "Active" : _statusComboBox.Text
            };
        }

        private void HandleResult(ServiceResult result, bool clearAfterSuccess)
        {
            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            int selectedUserId = result.NewId > 0 ? result.NewId : _selectedUserId;
            LoadData();
            SetStatus(result.Message, result.Success);

            if (clearAfterSuccess)
            {
                ClearEditor();
            }
            else if (selectedUserId > 0)
            {
                SelectUser(selectedUserId);
            }
        }

        private void SelectUser(int userId)
        {
            foreach (DataGridViewRow row in _usersGrid.Rows)
            {
                UserGridRow data = row.DataBoundItem as UserGridRow;
                if (data == null || data.UserID != userId)
                    continue;

                row.Selected = true;
                _usersGrid.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void ClearEditor()
        {
            _selectedUserId = 0;
            _firstNameTextBox.Clear();
            _lastNameTextBox.Clear();
            _emailTextBox.Clear();
            _phoneTextBox.Clear();
            _membershipStartPicker.Value = DateTime.Today;
            _membershipEndPicker.Value = DateTime.Today;
            _statusComboBox.SelectedIndex = -1;
            _membershipComboBox.SelectedIndex = -1;
            _usersGrid.ClearSelection();
            _usersGrid.CurrentCell = null;
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private void ActiveLocationContext_ActiveLocationChanged(object sender, EventArgs e)
        {
            if (_locationFilterComboBox.DataSource == null)
                return;

            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
                _locationFilterComboBox.SelectedValue = ActiveLocationContext.Instance.ActiveLocationId;
            else
                _locationFilterComboBox.SelectedIndex = -1;

            SearchButton_Click(sender, e);
        }

        private sealed class UserGridRow
        {
            public int UserID { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Membership { get; set; } = "";
            public int MembershipTypeID { get; set; }
            public DateTime MembershipStartDate { get; set; }
            public DateTime MembershipEndDate { get; set; }
            public string AccountStatus { get; set; } = "";
        }
    }
}
