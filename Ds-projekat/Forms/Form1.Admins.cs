using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildAdminsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbAdmin = CreateGroupBox("Admin Details", 20, 20, 420, 610);

            gbAdmin.Controls.Add(CreateLabel("User ID", 20, 40));
            txtAdminUserId = CreateTextBox("txtAdminUserId", 180, 35, 200);
            gbAdmin.Controls.Add(txtAdminUserId);

            gbAdmin.Controls.Add(CreateLabel("Role Name", 20, 85));
            txtAdminRoleName = CreateTextBox("txtAdminRoleName", 180, 80, 200);
            gbAdmin.Controls.Add(txtAdminRoleName);

            gbAdmin.Controls.Add(CreateLabel("Username", 20, 130));
            txtAdminUsername = CreateTextBox("txtAdminUsername", 180, 125, 200);
            gbAdmin.Controls.Add(txtAdminUsername);

            gbAdmin.Controls.Add(CreateLabel("Password", 20, 175));
            txtAdminPassword = CreateTextBox("txtAdminPassword", 180, 170, 200);
            txtAdminPassword.PasswordChar = '*';
            gbAdmin.Controls.Add(txtAdminPassword);

            btnAdminRegister = CreateActionButton("Register", 20, 250);
            btnAdminUpdate = CreateActionButton("Update", 140, 250);
            btnAdminDelete = CreateActionButton("Delete", 260, 250);

            btnAdminClear = CreateActionButton("Clear", 20, 300);
            btnAdminRefresh = CreateActionButton("Refresh", 140, 300);

            gbAdmin.Controls.Add(btnAdminRegister);
            gbAdmin.Controls.Add(btnAdminUpdate);
            gbAdmin.Controls.Add(btnAdminDelete);
            gbAdmin.Controls.Add(btnAdminClear);
            gbAdmin.Controls.Add(btnAdminRefresh);

            GroupBox gbList = CreateGroupBox("Admins List", 460, 20, 660, 610);
            dgvAdmins = CreateGrid("dgvAdmins", 15, 30, 630, 560);
            gbList.Controls.Add(dgvAdmins);

            page.Controls.Add(gbAdmin);
            page.Controls.Add(gbList);

            return page;
        }
    }
}