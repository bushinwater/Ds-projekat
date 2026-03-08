using System.Windows.Forms;

namespace Ds_projekat
{
    internal class UsersForm : SectionFormBase
    {
        public UsersForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("User Details", 20, 20, 420, 610);

            formGroup.Controls.Add(CreateLabel("First Name", 20, 40));
            formGroup.Controls.Add(CreateTextBox("txtFirstName", 180, 35, 200));

            formGroup.Controls.Add(CreateLabel("Last Name", 20, 85));
            formGroup.Controls.Add(CreateTextBox("txtLastName", 180, 80, 200));

            formGroup.Controls.Add(CreateLabel("Email", 20, 130));
            formGroup.Controls.Add(CreateTextBox("txtUserEmail", 180, 125, 200));

            formGroup.Controls.Add(CreateLabel("Phone", 20, 175));
            formGroup.Controls.Add(CreateTextBox("txtUserPhone", 180, 170, 200));

            formGroup.Controls.Add(CreateLabel("Membership Type", 20, 220));
            formGroup.Controls.Add(CreateComboBox("cbUserMembershipType", 180, 215, 200));

            formGroup.Controls.Add(CreateLabel("Start Date", 20, 265));
            formGroup.Controls.Add(CreateDatePicker("dtpMembershipStart", 180, 260, 200));

            formGroup.Controls.Add(CreateLabel("End Date", 20, 310));
            formGroup.Controls.Add(CreateDatePicker("dtpMembershipEnd", 180, 305, 200));

            formGroup.Controls.Add(CreateLabel("Status", 20, 355));
            ComboBox statusCombo = CreateComboBox("cbUserStatus", 180, 350, 200);
            statusCombo.Items.AddRange(new object[] { "Active", "Paused", "Expired" });
            formGroup.Controls.Add(statusCombo);

            formGroup.Controls.Add(CreateActionButton("Add", 20, 430));
            formGroup.Controls.Add(CreateActionButton("Update", 140, 430));
            formGroup.Controls.Add(CreateActionButton("Delete", 260, 430));
            formGroup.Controls.Add(CreateActionButton("Clear", 20, 480));
            formGroup.Controls.Add(CreateActionButton("Search", 140, 480));
            formGroup.Controls.Add(CreateActionButton("Refresh", 260, 480));

            GroupBox listGroup = CreateGroupBox("Users List", 460, 20, 660, 610);
            listGroup.Controls.Add(CreateGrid("dgvUsers", 15, 30, 630, 560));

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }
    }
}
