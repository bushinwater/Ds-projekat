using System.Windows.Forms;

namespace Ds_projekat
{
    internal class MembershipsForm : SectionFormBase
    {
        public MembershipsForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Membership Type Details", 20, 20, 420, 610);

            formGroup.Controls.Add(CreateLabel("Package Name", 20, 40));
            formGroup.Controls.Add(CreateTextBox("txtPackageName", 180, 35, 200));

            formGroup.Controls.Add(CreateLabel("Price", 20, 85));
            formGroup.Controls.Add(CreateTextBox("txtPackagePrice", 180, 80, 200));

            formGroup.Controls.Add(CreateLabel("Duration Days", 20, 130));
            formGroup.Controls.Add(CreateTextBox("txtDurationDays", 180, 125, 200));

            formGroup.Controls.Add(CreateLabel("Max Hours/Month", 20, 175));
            formGroup.Controls.Add(CreateTextBox("txtMaxHoursMonth", 180, 170, 200));

            formGroup.Controls.Add(CreateLabel("Meeting Room Access", 20, 220));
            CheckBox meetingRoomCheck = new CheckBox
            {
                Name = "chkMeetingRoomAccess",
                Left = 180,
                Top = 218
            };
            formGroup.Controls.Add(meetingRoomCheck);

            formGroup.Controls.Add(CreateLabel("Meeting Room Hours", 20, 265));
            formGroup.Controls.Add(CreateTextBox("txtMeetingRoomHours", 180, 260, 200));

            formGroup.Controls.Add(CreateActionButton("Add", 20, 345));
            formGroup.Controls.Add(CreateActionButton("Update", 140, 345));
            formGroup.Controls.Add(CreateActionButton("Delete", 260, 345));
            formGroup.Controls.Add(CreateActionButton("Clear", 20, 395));
            formGroup.Controls.Add(CreateActionButton("Refresh", 140, 395));

            GroupBox listGroup = CreateGroupBox("Membership Types", 460, 20, 660, 610);
            listGroup.Controls.Add(CreateGrid("dgvMembershipTypes", 15, 30, 630, 560));

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }
    }
}
