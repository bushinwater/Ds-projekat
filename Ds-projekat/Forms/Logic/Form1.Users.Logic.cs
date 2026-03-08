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

            LoadUserMembershipTypes();
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
                MessageBox.Show(
                    "Greška pri učitavanju tipova članstva:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadUsers()
        {
            try
            {
                _allUsers = _userFacade.GetAll();

                dgvUsers.DataSource = null;
                dgvUsers.DataSource = _allUsers;

                FormatUsersGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju korisnika:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void FormatUsersGrid()
        {
            if (dgvUsers.Columns.Count == 0) return;

            if (dgvUsers.Columns.Contains("UserID"))
                dgvUsers.Columns["UserID"].HeaderText = "ID";

            if (dgvUsers.Columns.Contains("FirstName"))
                dgvUsers.Columns["FirstName"].HeaderText = "First Name";

            if (dgvUsers.Columns.Contains("LastName"))
                dgvUsers.Columns["LastName"].HeaderText = "Last Name";

            if (dgvUsers.Columns.Contains("Email"))
                dgvUsers.Columns["Email"].HeaderText = "Email";

            if (dgvUsers.Columns.Contains("Phone"))
                dgvUsers.Columns["Phone"].HeaderText = "Phone";

            if (dgvUsers.Columns.Contains("MembershipTypeID"))
                dgvUsers.Columns["MembershipTypeID"].HeaderText = "Membership Type ID";

            if (dgvUsers.Columns.Contains("MembershipStartDate"))
                dgvUsers.Columns["MembershipStartDate"].HeaderText = "Start Date";

            if (dgvUsers.Columns.Contains("MembershipEndDate"))
                dgvUsers.Columns["MembershipEndDate"].HeaderText = "End Date";

            if (dgvUsers.Columns.Contains("AccountStatus"))
                dgvUsers.Columns["AccountStatus"].HeaderText = "Status";
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            if (dgvUsers.CurrentRow.DataBoundItem is not User user) return;

            FillUserForm(user);
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
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                throw new Exception("First Name je obavezno.");

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
                throw new Exception("Last Name je obavezno.");

            if (string.IsNullOrWhiteSpace(txtUserEmail.Text))
                throw new Exception("Email je obavezan.");

            if (string.IsNullOrWhiteSpace(txtUserPhone.Text))
                throw new Exception("Phone je obavezan.");

            if (cbUserMembershipType.SelectedValue == null)
                throw new Exception("Membership Type je obavezan.");

            if (cbUserStatus.SelectedItem == null)
                throw new Exception("Status je obavezan.");

            int membershipTypeId = Convert.ToInt32(cbUserMembershipType.SelectedValue);

            User user = new User
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtUserEmail.Text.Trim(),
                Phone = txtUserPhone.Text.Trim(),
                MembershipTypeID = membershipTypeId,
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

            if (int.TryParse(txtUserId.Text.Trim(), out int userId))
                query = query.Where(x => x.UserID == userId);

            if (!string.IsNullOrWhiteSpace(txtFirstName.Text))
                query = query.Where(x => x.FirstName != null &&
                                         x.FirstName.Contains(txtFirstName.Text.Trim(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(txtLastName.Text))
                query = query.Where(x => x.LastName != null &&
                                         x.LastName.Contains(txtLastName.Text.Trim(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(txtUserEmail.Text))
                query = query.Where(x => x.Email != null &&
                                         x.Email.Contains(txtUserEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(txtUserPhone.Text))
                query = query.Where(x => x.Phone != null &&
                                         x.Phone.Contains(txtUserPhone.Text.Trim(), StringComparison.OrdinalIgnoreCase));

            if (cbUserMembershipType.SelectedValue != null)
            {
                int membershipTypeId = Convert.ToInt32(cbUserMembershipType.SelectedValue);
                query = query.Where(x => x.MembershipTypeID == membershipTypeId);
            }

            if (cbUserStatus.SelectedItem != null)
            {
                string status = cbUserStatus.SelectedItem.ToString();
                query = query.Where(x => x.AccountStatus == status);
            }

            dgvUsers.DataSource = null;
            dgvUsers.DataSource = query.ToList();
            FormatUsersGrid();
        }

        private void UpdateEndDateFromMembership()
        {
            if (cbUserMembershipType.SelectedValue == null)
                return;

            int membershipTypeId = Convert.ToInt32(cbUserMembershipType.SelectedValue);

            var membership = _allMembershipTypes.FirstOrDefault(x => x.MembershipTypeID == membershipTypeId);
            if (membership == null)
                return;

            dtpMembershipEnd.Value = dtpMembershipStart.Value.Date.AddDays(membership.DurationDays);
        }

        private void cbUserMembershipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEndDateFromMembership();
        }

        private void dtpMembershipStart_ValueChanged(object sender, EventArgs e)
        {
            UpdateEndDateFromMembership();
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                User user = ReadUserFromForm();
                user.UserID = 0;

                _userFacade.AddUser(user);

                MessageBox.Show(
                    "Korisnik je uspešno dodat.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri dodavanju korisnika:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnUserUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtUserId.Text, out _))
                    throw new Exception("Odaberite korisnika za izmenu.");

                User user = ReadUserFromForm();
                _userFacade.UpdateUser(user);

                MessageBox.Show(
                    "Korisnik je uspešno izmenjen.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri izmeni korisnika:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnUserDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtUserId.Text, out int userId))
                    throw new Exception("Odaberite korisnika za brisanje.");

                DialogResult result = MessageBox.Show(
                    "Da li sigurno želite da obrišete korisnika?",
                    "Potvrda brisanja",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;

                _userFacade.DeleteUser(userId);

                MessageBox.Show(
                    "Korisnik je uspešno obrisan.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri brisanju korisnika:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnUserClear_Click(object sender, EventArgs e)
        {
            ClearUserForm();
        }

        private void btnUserSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyUserSearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri pretrazi korisnika:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnUserRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadUserMembershipTypes();
                LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri osvežavanju korisnika:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}