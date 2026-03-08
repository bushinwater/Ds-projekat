using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildMembershipsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Membership Type Details", 20, 20, 420, 610);

            gbForm.Controls.Add(CreateLabel("ID", 20, 40));
            txtMembershipTypeId = CreateTextBox("txtMembershipTypeId", 180, 35, 200);
            txtMembershipTypeId.ReadOnly = true;
            gbForm.Controls.Add(txtMembershipTypeId);

            gbForm.Controls.Add(CreateLabel("Package Name", 20, 85));
            txtPackageName = CreateTextBox("txtPackageName", 180, 80, 200);
            gbForm.Controls.Add(txtPackageName);

            gbForm.Controls.Add(CreateLabel("Price", 20, 130));
            txtPackagePrice = CreateTextBox("txtPackagePrice", 180, 125, 200);
            gbForm.Controls.Add(txtPackagePrice);

            gbForm.Controls.Add(CreateLabel("Duration Days", 20, 175));
            txtDurationDays = CreateTextBox("txtDurationDays", 180, 170, 200);
            gbForm.Controls.Add(txtDurationDays);

            gbForm.Controls.Add(CreateLabel("Max Hours/Month", 20, 220));
            txtMaxHoursMonth = CreateTextBox("txtMaxHoursMonth", 180, 215, 200);
            gbForm.Controls.Add(txtMaxHoursMonth);

            gbForm.Controls.Add(CreateLabel("Meeting Room Access", 20, 265));
            chkMeetingRoomAccess = new CheckBox();
            chkMeetingRoomAccess.Name = "chkMeetingRoomAccess";
            chkMeetingRoomAccess.Left = 180;
            chkMeetingRoomAccess.Top = 263;
            gbForm.Controls.Add(chkMeetingRoomAccess);

            gbForm.Controls.Add(CreateLabel("Meeting Room Hours", 20, 310));
            txtMeetingRoomHours = CreateTextBox("txtMeetingRoomHours", 180, 305, 200);
            gbForm.Controls.Add(txtMeetingRoomHours);

            btnMembershipAdd = CreateActionButton("Add", 20, 390);
            btnMembershipUpdate = CreateActionButton("Update", 140, 390);
            btnMembershipDelete = CreateActionButton("Delete", 260, 390);

            btnMembershipClear = CreateActionButton("Clear", 20, 440);
            btnMembershipRefresh = CreateActionButton("Refresh", 140, 440);

            gbForm.Controls.Add(btnMembershipAdd);
            gbForm.Controls.Add(btnMembershipUpdate);
            gbForm.Controls.Add(btnMembershipDelete);
            gbForm.Controls.Add(btnMembershipClear);
            gbForm.Controls.Add(btnMembershipRefresh);

            GroupBox gbList = CreateGroupBox("Membership Types", 460, 20, 660, 610);
            dgvMembershipTypes = CreateGrid("dgvMembershipTypes", 15, 30, 630, 560);
            gbList.Controls.Add(dgvMembershipTypes);

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }
    }
}