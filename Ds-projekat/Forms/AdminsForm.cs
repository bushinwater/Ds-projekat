using System.Windows.Forms;

namespace Ds_projekat
{
    internal class AdminsForm : SectionFormBase
    {
        public AdminsForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            GroupBox adminGroup = CreateGroupBox("Admin Details", 20, 20, 420, 610);

            adminGroup.Controls.Add(CreateLabel("User", 20, 40));
            adminGroup.Controls.Add(CreateComboBox("cbAdminUser", 180, 35, 200));

            adminGroup.Controls.Add(CreateLabel("Role Name", 20, 85));
            adminGroup.Controls.Add(CreateTextBox("txtAdminRoleName", 180, 80, 200));

            adminGroup.Controls.Add(CreateLabel("Username", 20, 130));
            adminGroup.Controls.Add(CreateTextBox("txtAdminUsername", 180, 125, 200));

            adminGroup.Controls.Add(CreateLabel("Password", 20, 175));
            TextBox passwordText = CreateTextBox("txtAdminPassword", 180, 170, 200);
            passwordText.PasswordChar = '*';
            adminGroup.Controls.Add(passwordText);

            adminGroup.Controls.Add(CreateActionButton("Register", 20, 250));
            adminGroup.Controls.Add(CreateActionButton("Update", 140, 250));
            adminGroup.Controls.Add(CreateActionButton("Delete", 260, 250));
            adminGroup.Controls.Add(CreateActionButton("Clear", 20, 300));
            adminGroup.Controls.Add(CreateActionButton("Refresh", 140, 300));

            GroupBox loginGroup = CreateGroupBox("Admin Login", 20, 360, 420, 210);
            loginGroup.Controls.Add(CreateLabel("Username", 20, 40));
            loginGroup.Controls.Add(CreateTextBox("txtLoginUsername", 140, 35, 220));
            loginGroup.Controls.Add(CreateLabel("Password", 20, 85));
            TextBox loginPasswordText = CreateTextBox("txtLoginPassword", 140, 80, 220);
            loginPasswordText.PasswordChar = '*';
            loginGroup.Controls.Add(loginPasswordText);
            loginGroup.Controls.Add(CreateActionButton("Login", 140, 130, 120));

            GroupBox listGroup = CreateGroupBox("Admins List", 460, 20, 660, 610);
            listGroup.Controls.Add(CreateGrid("dgvAdmins", 15, 30, 630, 560));

            Controls.Add(adminGroup);
            Controls.Add(loginGroup);
            Controls.Add(listGroup);
        }
    }
}
