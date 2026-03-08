using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ds_projekat.Services;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeAdminsModule()
        {
            if (dgvAdmins == null)
                return;

            dgvAdmins.AutoGenerateColumns = true;
            dgvAdmins.SelectionChanged += dgvAdmins_SelectionChanged;

            btnAdminRegister.Click += btnAdminRegister_Click;
            btnAdminUpdate.Click += btnAdminUpdate_Click;
            btnAdminDelete.Click += btnAdminDelete_Click;
            btnAdminClear.Click += btnAdminClear_Click;
            btnAdminRefresh.Click += btnAdminRefresh_Click;

            LoadAdmins();
            ClearAdminForm();
        }

        private void LoadAdmins()
        {
            try
            {
                _allAdmins = GetAllAdminsFromRepository();

                dgvAdmins.DataSource = null;
                dgvAdmins.DataSource = _allAdmins;

                FormatAdminsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju admina:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private List<Admin> GetAllAdminsFromRepository()
        {
            List<Admin> list = new List<Admin>();

            foreach (DataGridViewRow row in dgvAdmins?.Rows ?? new DataGridViewRowCollection(null))
            {
                // ovo je samo fallback zaštita, realno se neće koristiti
            }

            // Pošto AdminFacade trenutno nema GetAll, čitamo preko repository-ja
            IAdminRepository repo = new AdminRepository();

            // Ako kasnije dodaš GetAll u interfejs i repository, zameni ovaj deo sa:
            // return _adminFacade.GetAll();

            if (repo is AdminRepository concreteRepoWithGetAll)
            {
                var method = concreteRepoWithGetAll.GetType().GetMethod("GetAll");
                if (method != null)
                {
                    object result = method.Invoke(concreteRepoWithGetAll, null);
                    if (result is List<Admin> adminList)
                        return adminList;
                }
            }

            return list;
        }

        private void FormatAdminsGrid()
        {
            if (dgvAdmins.Columns.Count == 0)
                return;

            if (dgvAdmins.Columns.Contains("UserID"))
                dgvAdmins.Columns["UserID"].HeaderText = "User ID";

            if (dgvAdmins.Columns.Contains("RoleName"))
                dgvAdmins.Columns["RoleName"].HeaderText = "Role Name";

            if (dgvAdmins.Columns.Contains("Username"))
                dgvAdmins.Columns["Username"].HeaderText = "Username";

            if (dgvAdmins.Columns.Contains("HashedPass"))
                dgvAdmins.Columns["HashedPass"].Visible = false;
        }

        private void dgvAdmins_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAdmins.CurrentRow == null)
                return;

            if (dgvAdmins.CurrentRow.DataBoundItem is not Admin admin)
                return;

            FillAdminForm(admin);
        }

        private void FillAdminForm(Admin admin)
        {
            txtAdminUserId.Text = admin.UserID.ToString();
            txtAdminRoleName.Text = admin.RoleName;
            txtAdminUsername.Text = admin.Username;
            txtAdminPassword.Clear();
        }

        private Admin ReadAdminFromForm(bool requirePassword)
        {
            if (!int.TryParse(txtAdminUserId.Text.Trim(), out int userId))
                throw new Exception("User ID mora biti ceo broj.");

            if (string.IsNullOrWhiteSpace(txtAdminRoleName.Text))
                throw new Exception("Role Name je obavezan.");

            if (string.IsNullOrWhiteSpace(txtAdminUsername.Text))
                throw new Exception("Username je obavezan.");

            if (requirePassword && string.IsNullOrWhiteSpace(txtAdminPassword.Text))
                throw new Exception("Password je obavezan.");

            Admin admin = new Admin
            {
                UserID = userId,
                RoleName = txtAdminRoleName.Text.Trim(),
                Username = txtAdminUsername.Text.Trim(),

                // OVDE IDE PLAIN PASSWORD; HASH RADI AdminFacade
                HashedPass = txtAdminPassword.Text
            };

            return admin;
        }

        private void ClearAdminForm()
        {
            txtAdminUserId.Clear();
            txtAdminRoleName.Clear();
            txtAdminUsername.Clear();
            txtAdminPassword.Clear();

            dgvAdmins.ClearSelection();
        }

        private void btnAdminRegister_Click(object sender, EventArgs e)
        {
            try
            {
                Admin admin = ReadAdminFromForm(true);

                ServiceResult result = _adminFacade.AddAdmin(admin);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadAdmins();
                ClearAdminForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri dodavanju admina:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAdminUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Admin admin = ReadAdminFromForm(false);

                ServiceResult result = _adminFacade.UpdateAdmin(admin);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadAdmins();
                ClearAdminForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri izmeni admina:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAdminDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtAdminUserId.Text, out int userId))
                    throw new Exception("Odaberite admina za brisanje.");

                DialogResult confirm = MessageBox.Show(
                    "Da li sigurno želite da obrišete admina?",
                    "Potvrda",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm != DialogResult.Yes)
                    return;

                ServiceResult result = _adminFacade.DeleteAdmin(userId);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadAdmins();
                ClearAdminForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri brisanju admina:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAdminClear_Click(object sender, EventArgs e)
        {
            ClearAdminForm();
        }

        private void btnAdminRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAdmins();
                ClearAdminForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri osvežavanju admina:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}