using System.Windows.Forms;

namespace Ds_projekat
{
    internal class ReservationsForm : SectionFormBase
    {
        public ReservationsForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Reservation Details", 20, 20, 430, 610);

            formGroup.Controls.Add(CreateLabel("User", 20, 40));
            formGroup.Controls.Add(CreateComboBox("cbReservationUser", 180, 35, 210));

            formGroup.Controls.Add(CreateLabel("Resource", 20, 85));
            formGroup.Controls.Add(CreateComboBox("cbReservationResource", 180, 80, 210));

            formGroup.Controls.Add(CreateLabel("Users Count", 20, 130));
            formGroup.Controls.Add(CreateTextBox("txtReservationUsersCount", 180, 125, 210));

            formGroup.Controls.Add(CreateLabel("Start DateTime", 20, 175));
            formGroup.Controls.Add(CreateDateTimePicker("dtpReservationStart", 180, 170, 210));

            formGroup.Controls.Add(CreateLabel("End DateTime", 20, 220));
            formGroup.Controls.Add(CreateDateTimePicker("dtpReservationEnd", 180, 215, 210));

            formGroup.Controls.Add(CreateLabel("Status", 20, 265));
            ComboBox statusCombo = CreateComboBox("cbReservationStatus", 180, 260, 210);
            statusCombo.Items.AddRange(new object[] { "Active", "Finished", "Canceled" });
            formGroup.Controls.Add(statusCombo);

            formGroup.Controls.Add(CreateActionButton("Create", 20, 345));
            formGroup.Controls.Add(CreateActionButton("Cancel", 140, 345));
            formGroup.Controls.Add(CreateActionButton("Finish", 260, 345));
            formGroup.Controls.Add(CreateActionButton("Check", 20, 395));
            formGroup.Controls.Add(CreateActionButton("Clear", 140, 395));
            formGroup.Controls.Add(CreateActionButton("Refresh", 260, 395));

            GroupBox listGroup = CreateGroupBox("Reservations List", 470, 20, 650, 610);
            listGroup.Controls.Add(CreateGrid("dgvReservations", 15, 30, 620, 560));

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }
    }
}
