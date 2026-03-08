using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeUsersModule()
        {
            if (dgvUsers == null)
                return;

            dgvUsers.AutoGenerateColumns = true;
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;

            btnUserAdd.Click += btnUserAdd_Click;
            btnUserUpdate.Click += btnUserUpdate_Click;
            btnUserDelete.Click += btnUserDelete_Click;
            btnUserClear.Click += btnUserClear_Click;
            btnUserSearch.Click += btnUserSearch_Click;
            btnUserRefresh.Click += btnUserRefresh_Click;

            cbUserMembershipType.SelectedIndexChanged += cbUserMembershipType_SelectedIndexChanged;
            dtpMembershipStart.ValueChanged += dtpMembershipStart_ValueChanged;
            cbUserFilterMembershipType.SelectedIndexChanged += (s, e) => ApplyUserListFilters();
            cbUserFilterStatus.SelectedIndexChanged += (s, e) => ApplyUserListFilters();

            LoadUserMembershipTypes();
            LoadUserFilterOptions();
            LoadUsers();
            ClearUserForm();
        }

        private void LoadUserMembershipTypes()
        {
            try
            {
                _allMembershipTypes = _membershipFacade.GetAll();

                cbUserMembershipType.DataSource = null;
                cbUserMembershipType.DisplayMember = "PackageName";
                cbUserMembershipType.ValueMember = "MembershipTypeID";
                cbUserMembershipType.DataSource = _allMembershipTypes;
                cbUserMembershipType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju tipova članstva:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserFilterOptions()
        {
            cbUserFilterMembershipType.DataSource = null;
            var membershipFilterItems = new List<MembershipType> { new MembershipType { MembershipTypeID = 0, PackageName = "All" } };
            membershipFilterItems.AddRange(_allMembershipTypes);
            cbUserFilterMembershipType.DisplayMember = "PackageName";
            cbUserFilterMembershipType.ValueMember = "MembershipTypeID";
            cbUserFilterMembershipType.DataSource = membershipFilterItems;
            cbUserFilterMembershipType.SelectedValue = 0;

            if (cbUserFilterStatus.Items.Count == 0)
                cbUserFilterStatus.Items.AddRange(new object[] { "All", "Active", "Paused", "Expired" });
            cbUserFilterStatus.SelectedItem = "All";
        }

        private void LoadUsers()
        {
            try
            {
                _allUsers = _userFacade.GetAll();
                _allMembershipTypes = _membershipFacade.GetAll();
                BindUsersGrid(_allUsers);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju korisnika:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindUsersGrid(IEnumerable<User> users)
        {
            var rows = users.Select(u =>
            {
                var membership = _allMembershipTypes.FirstOrDefault(m => m.MembershipTypeID == u.MembershipTypeID);
                return new UserGridRow
                {
                    UserID = u.UserID,
                    User = u.FirstName + " " + u.LastName + " (" + u.UserID + ")",
                    Email = u.Email,
                    Phone = u.Phone,
                    Membership = membership != null ? membership.PackageName + " (" + membership.MembershipTypeID + ")" : "N/A",
                    Status = u.AccountStatus,
                    StartDate = u.MembershipStartDate.ToString("dd.MM.yyyy"),
                    EndDate = u.MembershipEndDate.ToString("dd.MM.yyyy")
                };
            }).ToList();

            dgvUsers.DataSource = null;
            dgvUsers.DataSource = rows;
            FormatUsersGrid();
        }

        private void ApplyUserListFilters()
        {
            IEnumerable<User> query = _allUsers;

            if (cbUserFilterMembershipType.SelectedValue != null && Convert.ToInt32(cbUserFilterMembershipType.SelectedValue) > 0)
            {
                int membershipTypeId = Convert.ToInt32(cbUserFilterMembershipType.SelectedValue);
                query = query.Where(x => x.MembershipTypeID == membershipTypeId);
            }

            string status = cbUserFilterStatus.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(status) && status != "All")
                query = query.Where(x => string.Equals(x.AccountStatus, status, StringComparison.OrdinalIgnoreCase));

            BindUsersGrid(query);
        }

        private void FormatUsersGrid()
        {
            if (dgvUsers.Columns.Count == 0) return;
            if (dgvUsers.Columns.Contains("UserID")) dgvUsers.Columns["UserID"].Visible = false;
            if (dgvUsers.Columns.Contains("User")) dgvUsers.Columns["User"].HeaderText = "User";
            if (dgvUsers.Columns.Contains("Email")) dgvUsers.Columns["Email"].HeaderText = "Email";
            if (dgvUsers.Columns.Contains("Phone")) dgvUsers.Columns["Phone"].HeaderText = "Phone";
            if (dgvUsers.Columns.Contains("Membership")) dgvUsers.Columns["Membership"].HeaderText = "Membership";
            if (dgvUsers.Columns.Contains("Status")) dgvUsers.Columns["Status"].HeaderText = "Status";
            if (dgvUsers.Columns.Contains("StartDate")) dgvUsers.Columns["StartDate"].HeaderText = "Start Date";
            if (dgvUsers.Columns.Contains("EndDate")) dgvUsers.Columns["EndDate"].HeaderText = "End Date";
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            if (dgvUsers.CurrentRow.DataBoundItem is not UserGridRow row) return;
            User user = _allUsers.FirstOrDefault(x => x.UserID == row.UserID);
            if (user != null) FillUserForm(user);
        }

        private void FillUserForm(User user)
        {
            txtUserId.Text = user.UserID.ToString();
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtUserEmail.Text = user.Email;
            txtUserPhone.Text = user.Phone;
            cbUserMembershipType.SelectedValue = user.MembershipTypeID;
            dtpMembershipStart.Value = user.MembershipStartDate;
            dtpMembershipEnd.Value = user.MembershipEndDate;
            cbUserStatus.SelectedItem = user.AccountStatus;
        }

        private User ReadUserFromForm()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text)) throw new Exception("First Name je obavezno.");
            if (string.IsNullOrWhiteSpace(txtLastName.Text)) throw new Exception("Last Name je obavezno.");
            if (string.IsNullOrWhiteSpace(txtUserEmail.Text)) throw new Exception("Email je obavezan.");
            if (string.IsNullOrWhiteSpace(txtUserPhone.Text)) throw new Exception("Phone je obavezan.");
            if (cbUserMembershipType.SelectedValue == null) throw new Exception("Membership Type je obavezan.");
            if (cbUserStatus.SelectedItem == null) throw new Exception("Status je obavezan.");

            User user = new User
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtUserEmail.Text.Trim(),
                Phone = txtUserPhone.Text.Trim(),
                MembershipTypeID = Convert.ToInt32(cbUserMembershipType.SelectedValue),
                MembershipStartDate = dtpMembershipStart.Value.Date,
                MembershipEndDate = dtpMembershipEnd.Value.Date,
                AccountStatus = cbUserStatus.SelectedItem.ToString()
            };

            if (int.TryParse(txtUserId.Text, out int userId))
                user.UserID = userId;

            return user;
        }

        private void ClearUserForm()
        {
            txtUserId.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtUserEmail.Clear();
            txtUserPhone.Clear();
            cbUserMembershipType.SelectedIndex = -1;
            cbUserStatus.SelectedIndex = -1;
            dtpMembershipStart.Value = DateTime.Today;
            dtpMembershipEnd.Value = DateTime.Today;
            dgvUsers.ClearSelection();
        }

        private void ApplyUserSearch()
        {
            IEnumerable<User> query = _allUsers;
            if (int.TryParse(txtUserId.Text.Trim(), out int userId)) query = query.Where(x => x.UserID == userId);
            if (!string.IsNullOrWhiteSpace(txtFirstName.Text)) query = query.Where(x => x.FirstName != null && x.FirstName.Contains(txtFirstName.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(txtLastName.Text)) query = query.Where(x => x.LastName != null && x.LastName.Contains(txtLastName.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(txtUserEmail.Text)) query = query.Where(x => x.Email != null && x.Email.Contains(txtUserEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(txtUserPhone.Text)) query = query.Where(x => x.Phone != null && x.Phone.Contains(txtUserPhone.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            if (cbUserMembershipType.SelectedValue != null) query = query.Where(x => x.MembershipTypeID == Convert.ToInt32(cbUserMembershipType.SelectedValue));
            if (cbUserStatus.SelectedItem != null) query = query.Where(x => x.AccountStatus == cbUserStatus.SelectedItem.ToString());
            BindUsersGrid(query);
        }

        private void UpdateEndDateFromMembership()
        {
            if (cbUserMembershipType.SelectedValue == null) return;
            int membershipTypeId = Convert.ToInt32(cbUserMembershipType.SelectedValue);
            var membership = _allMembershipTypes.FirstOrDefault(x => x.MembershipTypeID == membershipTypeId);
            if (membership != null) dtpMembershipEnd.Value = dtpMembershipStart.Value.Date.AddDays(membership.DurationDays);
        }

        private void cbUserMembershipType_SelectedIndexChanged(object sender, EventArgs e) => UpdateEndDateFromMembership();
        private void dtpMembershipStart_ValueChanged(object sender, EventArgs e) => UpdateEndDateFromMembership();

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                User user = ReadUserFromForm();
                user.UserID = 0;
                _userFacade.AddUser(user);
                MessageBox.Show("Korisnik je uspešno dodat.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ApplyUserListFilters();
                ClearUserForm();
                NotifyObservers("Users", "Add", "Dodat je novi korisnik: " + user.FirstName + " " + user.LastName + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri dodavanju korisnika:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUserUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtUserId.Text, out _)) throw new Exception("Odaberite korisnika za izmenu.");
                User user = ReadUserFromForm();
                _userFacade.UpdateUser(user);
                MessageBox.Show("Korisnik je uspešno izmenjen.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ApplyUserListFilters();
                ClearUserForm();
                NotifyObservers("Users", "Update", "Izmenjen je korisnik ID=" + user.UserID + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri izmeni korisnika:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUserDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtUserId.Text, out int userId)) throw new Exception("Odaberite korisnika za brisanje.");
                DialogResult result = MessageBox.Show("Da li sigurno želite da obrišete korisnika?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;

                bool hasReservations = new ReservationRepository().UserHasReservations(userId);
                if (hasReservations)
                {
                    DialogResult result1 = MessageBox.Show("Ovaj korisnik ima rezervacije. Da li želiš da obrišeš i te rezervacije?", "Potvrda brisanja", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result1 == DialogResult.Cancel) return;
                    bool deleteReservationsToo = (result1 == DialogResult.Yes);
                    ServiceResult sr = _userFacade.DeleteUserWithCheck(userId, deleteReservationsToo);
                    MessageBox.Show(sr.Message);
                    if (sr.Success)
                    {
                        LoadUsers();
                        ApplyUserListFilters();
                        ClearUserForm();
                        NotifyObservers("Users", "Delete", "Obrisan je korisnik ID=" + userId + (deleteReservationsToo ? " zajedno sa rezervacijama." : "."));
                    }
                    return;
                }

                ServiceResult normalDelete = _userFacade.DeleteUserWithCheck(userId, false);
                if (!normalDelete.Success) throw new Exception(normalDelete.Message);
                MessageBox.Show(normalDelete.Message, "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ApplyUserListFilters();
                ClearUserForm();
                NotifyObservers("Users", "Delete", "Obrisan je korisnik ID=" + userId + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri brisanju korisnika:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUserClear_Click(object sender, EventArgs e) => ClearUserForm();

        private void btnUserSearch_Click(object sender, EventArgs e)
        {
            try { ApplyUserSearch(); }
            catch (Exception ex) { MessageBox.Show("Greška pri pretrazi korisnika:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnUserRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadUserMembershipTypes();
                LoadUserFilterOptions();
                LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri osvežavanju korisnika:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
