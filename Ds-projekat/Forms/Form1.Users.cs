using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildUsersPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("User Details", 20, 20, 420, 610);

            gbForm.Controls.Add(CreateLabel("User ID", 20, 40));
            txtUserId = CreateTextBox("txtUserId", 180, 35, 200);
            txtUserId.ReadOnly = true;
            gbForm.Controls.Add(txtUserId);

            gbForm.Controls.Add(CreateLabel("First Name", 20, 85));
            txtFirstName = CreateTextBox("txtFirstName", 180, 80, 200);
            gbForm.Controls.Add(txtFirstName);

            gbForm.Controls.Add(CreateLabel("Last Name", 20, 130));
            txtLastName = CreateTextBox("txtLastName", 180, 125, 200);
            gbForm.Controls.Add(txtLastName);

            gbForm.Controls.Add(CreateLabel("Email", 20, 175));
            txtUserEmail = CreateTextBox("txtUserEmail", 180, 170, 200);
            gbForm.Controls.Add(txtUserEmail);

            gbForm.Controls.Add(CreateLabel("Phone", 20, 220));
            txtUserPhone = CreateTextBox("txtUserPhone", 180, 215, 200);
            gbForm.Controls.Add(txtUserPhone);

            gbForm.Controls.Add(CreateLabel("Membership Type", 20, 265));
            cbUserMembershipType = CreateComboBox("cbUserMembershipType", 180, 260, 200);
            gbForm.Controls.Add(cbUserMembershipType);

            gbForm.Controls.Add(CreateLabel("Start Date", 20, 310));
            dtpMembershipStart = new DateTimePicker();
            dtpMembershipStart.Name = "dtpMembershipStart";
            dtpMembershipStart.Left = 180;
            dtpMembershipStart.Top = 305;
            dtpMembershipStart.Width = 200;
            dtpMembershipStart.Format = DateTimePickerFormat.Short;
            gbForm.Controls.Add(dtpMembershipStart);

            gbForm.Controls.Add(CreateLabel("End Date", 20, 355));
            dtpMembershipEnd = new DateTimePicker();
            dtpMembershipEnd.Name = "dtpMembershipEnd";
            dtpMembershipEnd.Left = 180;
            dtpMembershipEnd.Top = 350;
            dtpMembershipEnd.Width = 200;
            dtpMembershipEnd.Format = DateTimePickerFormat.Short;
            gbForm.Controls.Add(dtpMembershipEnd);

            gbForm.Controls.Add(CreateLabel("Status", 20, 400));
            cbUserStatus = CreateComboBox("cbUserStatus", 180, 395, 200);
            cbUserStatus.Items.AddRange(new object[] { "Active", "Paused", "Expired" });
            gbForm.Controls.Add(cbUserStatus);

            btnUserAdd = CreateActionButton("Add", 20, 470);
            btnUserUpdate = CreateActionButton("Update", 140, 470);
            btnUserDelete = CreateActionButton("Delete", 260, 470);

            btnUserClear = CreateActionButton("Clear", 20, 520);
            btnUserSearch = CreateActionButton("Search", 140, 520);
            btnUserRefresh = CreateActionButton("Refresh", 260, 520);

            gbForm.Controls.Add(btnUserAdd);
            gbForm.Controls.Add(btnUserUpdate);
            gbForm.Controls.Add(btnUserDelete);
            gbForm.Controls.Add(btnUserClear);
            gbForm.Controls.Add(btnUserSearch);
            gbForm.Controls.Add(btnUserRefresh);

            GroupBox gbList = CreateGroupBox("Users List", 460, 20, 660, 610);

            gbList.Controls.Add(CreateLabel("Filter Membership", 15, 32));
            cbUserFilterMembershipType = CreateComboBox("cbUserFilterMembershipType", 140, 27, 190);
            gbList.Controls.Add(cbUserFilterMembershipType);

            gbList.Controls.Add(CreateLabel("Filter Status", 350, 32));
            cbUserFilterStatus = CreateComboBox("cbUserFilterStatus", 450, 27, 160);
            cbUserFilterStatus.Items.AddRange(new object[] { "All", "Active", "Paused", "Expired" });
            gbList.Controls.Add(cbUserFilterStatus);

            dgvUsers = CreateGrid("dgvUsers", 15, 75, 630, 515);
            gbList.Controls.Add(dgvUsers);

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }
    }
}
