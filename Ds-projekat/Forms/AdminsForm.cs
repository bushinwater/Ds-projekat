using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class AdminsForm : SectionFormBase, IReloadableSection
    {
        private readonly AdminFacade _adminFacade;
        private readonly UserFacade _userFacade;

        private ComboBox _adminUserComboBox;
        private TextBox _roleNameTextBox;
        private TextBox _adminUsernameTextBox;
        private TextBox _adminPasswordTextBox;
        private DataGridView _adminsGrid;
        private Label _statusLabel;

        private int _selectedUserId;
        private List<User> _users;
        private List<Admin> _admins;

        public AdminsForm()
        {
            _adminFacade = new AdminFacade();
            _userFacade = new UserFacade();
            _users = new List<User>();
            _admins = new List<Admin>();

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            GroupBox adminGroup = CreateGroupBox("Detalji administratora", 20, 20, 420, 610);

            adminGroup.Controls.Add(CreateLabel("Korisnik", 20, 40));
            _adminUserComboBox = CreateComboBox("cbAdminUser", 180, 35, 200);
            adminGroup.Controls.Add(_adminUserComboBox);

            adminGroup.Controls.Add(CreateLabel("Naziv role", 20, 85));
            _roleNameTextBox = CreateTextBox("txtAdminRoleName", 180, 80, 200);
            adminGroup.Controls.Add(_roleNameTextBox);

            adminGroup.Controls.Add(CreateLabel("Korisnicko ime", 20, 130));
            _adminUsernameTextBox = CreateTextBox("txtAdminUsername", 180, 125, 200);
            adminGroup.Controls.Add(_adminUsernameTextBox);

            adminGroup.Controls.Add(CreateLabel("Lozinka", 20, 175));
            _adminPasswordTextBox = CreateTextBox("txtAdminPassword", 180, 170, 200);
            _adminPasswordTextBox.PasswordChar = '*';
            adminGroup.Controls.Add(_adminPasswordTextBox);

            Button registerButton = CreateActionButton("Dodaj", 20, 250);
            registerButton.Click += RegisterButton_Click;
            adminGroup.Controls.Add(registerButton);

            Button updateButton = CreateActionButton("Azuriraj", 140, 250);
            updateButton.Click += UpdateButton_Click;
            adminGroup.Controls.Add(updateButton);

            Button deleteButton = CreateActionButton("Obrisi", 260, 250);
            deleteButton.Click += DeleteButton_Click;
            adminGroup.Controls.Add(deleteButton);

            Button clearButton = CreateActionButton("Ocisti", 20, 300);
            clearButton.Click += ClearButton_Click;
            adminGroup.Controls.Add(clearButton);

            Button refreshButton = CreateActionButton("Osvezi", 140, 300);
            refreshButton.Click += RefreshButton_Click;
            adminGroup.Controls.Add(refreshButton);

            _statusLabel = new Label
            {
                Left = 20,
                Top = 380,
                Width = 360,
                Height = 150,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Izaberi korisnika, unesi role sa recju admin i sacuvaj nalog." + Environment.NewLine +
                       "Za novi unos prikazuju se samo korisnici koji jos nemaju admin nalog." + Environment.NewLine +
                       "Prijava za admin panel radi se na posebnoj Login formi pri pokretanju aplikacije."
            };
            adminGroup.Controls.Add(_statusLabel);

            GroupBox listGroup = CreateGroupBox("Lista administratora", 460, 20, 660, 610);
            _adminsGrid = CreateGrid("dgvAdmins", 15, 30, 630, 560);
            _adminsGrid.SelectionChanged += AdminsGrid_SelectionChanged;
            listGroup.Controls.Add(_adminsGrid);

            Controls.Add(adminGroup);
            Controls.Add(listGroup);
        }

        public void LoadData()
        {
            try
            {
                int previousSelection = _selectedUserId;
                _users = _userFacade.GetAll();
                _admins = _adminFacade.GetAllAdmins();
                BindUsers(previousSelection);
                BindAdmins();

                if (previousSelection > 0)
                    SelectAdmin(previousSelection);
                else
                    ClearEditor();

                SetStatus("Admin podaci su ucitani iz baze.", true);
            }
            catch (Exception ex)
            {
                SetStatus("Greska pri ucitavanju admin podataka: " + ex.Message, false);
            }
        }

        private void BindUsers(int includedUserId = 0)
        {
            HashSet<int> existingAdminIds = new HashSet<int>(_admins.Select(a => a.UserID));

            List<UserOption> items = _users
                .Where(u => !existingAdminIds.Contains(u.UserID) || u.UserID == includedUserId)
                .Select(u => new UserOption
                {
                    UserID = u.UserID,
                    DisplayText = u.UserID + " - " + u.FirstName + " " + u.LastName + " (" + u.Email + ")"
                })
                .ToList();

            _adminUserComboBox.DataSource = null;
            _adminUserComboBox.DisplayMember = "DisplayText";
            _adminUserComboBox.ValueMember = "UserID";
            _adminUserComboBox.DataSource = items;
            _adminUserComboBox.SelectedIndex = -1;
        }

        private void BindAdmins()
        {
            Dictionary<int, User> usersById = _users.ToDictionary(u => u.UserID, u => u);

            List<AdminGridRow> rows = _admins
                .Select(a =>
                {
                    User user;
                    usersById.TryGetValue(a.UserID, out user);

                    return new AdminGridRow
                    {
                        UserID = a.UserID,
                        FullName = user == null ? "(user missing)" : user.FirstName + " " + user.LastName,
                        Email = user == null ? "" : user.Email,
                        RoleName = a.RoleName,
                        Username = a.Username,
                        Password = a.HashedPass
                    };
                })
                .ToList();

            _adminsGrid.DataSource = null;
            _adminsGrid.DataSource = rows;
            SetGridHeader(_adminsGrid, "UserID", "ID korisnika");
            SetGridHeader(_adminsGrid, "FullName", "Ime i prezime");
            SetGridHeader(_adminsGrid, "Email", "Email");
            SetGridHeader(_adminsGrid, "RoleName", "Rola");
            SetGridHeader(_adminsGrid, "Username", "Korisnicko ime");
            SetGridHeader(_adminsGrid, "Password", "Lozinka");
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            ServiceResult result = _adminFacade.AddAdmin(CreateAdminFromForm());
            HandleResult(result, true);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId <= 0)
            {
                SetStatus("Izaberi admin nalog za azuriranje.", false);
                return;
            }

            Admin admin = CreateAdminFromForm();
            admin.UserID = userId;

            ServiceResult result = _adminFacade.UpdateAdmin(admin);
            HandleResult(result, false);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId <= 0)
            {
                SetStatus("Izaberi admin nalog za brisanje.", false);
                return;
            }

            DialogResult dialogResult = MessageBox.Show(
                "Da li zelis da obrises izabrani admin nalog?",
                "Delete admin",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
                return;

            ServiceResult result = _adminFacade.DeleteAdmin(userId);
            HandleResult(result, false);
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

        private void AdminsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_adminsGrid.CurrentRow == null)
                return;

            AdminGridRow row = _adminsGrid.CurrentRow.DataBoundItem as AdminGridRow;
            if (row == null)
                return;

            _selectedUserId = row.UserID;
            BindUsers(row.UserID);

            if (_adminUserComboBox.Items.Count > 0)
                _adminUserComboBox.SelectedValue = row.UserID;

            _roleNameTextBox.Text = row.RoleName;
            _adminUsernameTextBox.Text = row.Username;
            _adminPasswordTextBox.Text = row.Password;
        }

        private void HandleResult(ServiceResult result, bool clearAfterSuccess)
        {
            SetStatus(result.Message, result.Success);

            if (!result.Success)
                return;

            int selectedUserId = GetSelectedUserId();
            LoadData();
            SetStatus(result.Message, result.Success);

            if (clearAfterSuccess)
            {
                ClearEditor();
            }
            else if (selectedUserId > 0)
            {
                SelectAdmin(selectedUserId);
            }
        }

        private void SelectAdmin(int userId)
        {
            foreach (DataGridViewRow row in _adminsGrid.Rows)
            {
                AdminGridRow data = row.DataBoundItem as AdminGridRow;
                if (data == null || data.UserID != userId)
                    continue;

                row.Selected = true;
                _adminsGrid.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void ClearEditor()
        {
            _selectedUserId = 0;
            BindUsers();
            _adminUserComboBox.SelectedIndex = -1;

            _roleNameTextBox.Clear();
            _adminUsernameTextBox.Clear();
            _adminPasswordTextBox.Clear();
            _adminsGrid.ClearSelection();
            _adminsGrid.CurrentCell = null;
        }

        private Admin CreateAdminFromForm()
        {
            return new Admin
            {
                UserID = GetSelectedUserId(),
                RoleName = _roleNameTextBox.Text.Trim(),
                Username = _adminUsernameTextBox.Text.Trim(),
                HashedPass = _adminPasswordTextBox.Text
            };
        }

        private int GetSelectedUserId()
        {
            if (_adminUserComboBox.SelectedValue != null)
                return Convert.ToInt32(_adminUserComboBox.SelectedValue);

            return _selectedUserId;
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private sealed class UserOption
        {
            public int UserID { get; set; }
            public string DisplayText { get; set; } = "";
        }

        private sealed class AdminGridRow
        {
            public int UserID { get; set; }
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string RoleName { get; set; } = "";
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
        }
    }
}
