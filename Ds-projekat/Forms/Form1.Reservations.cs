using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildReservationsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Reservation Details", 20, 20, 430, 610);

            gbForm.Controls.Add(CreateLabel("Reservation ID", 20, 40));
            txtReservationId = CreateTextBox("txtReservationId", 180, 35, 210);
            txtReservationId.ReadOnly = true;
            gbForm.Controls.Add(txtReservationId);

            gbForm.Controls.Add(CreateLabel("User", 20, 85));
            cbReservationUser = CreateComboBox("cbReservationUser", 180, 80, 210);
            gbForm.Controls.Add(cbReservationUser);

            gbForm.Controls.Add(CreateLabel("Resource", 20, 130));
            cbReservationResource = CreateComboBox("cbReservationResource", 180, 125, 210);
            gbForm.Controls.Add(cbReservationResource);

            gbForm.Controls.Add(CreateLabel("Users Count", 20, 175));
            txtReservationUsersCount = CreateTextBox("txtReservationUsersCount", 180, 170, 210);
            gbForm.Controls.Add(txtReservationUsersCount);

            gbForm.Controls.Add(CreateLabel("Start DateTime", 20, 220));
            dtpReservationStart = new DateTimePicker();
            dtpReservationStart.Name = "dtpReservationStart";
            dtpReservationStart.Left = 180;
            dtpReservationStart.Top = 215;
            dtpReservationStart.Width = 210;
            dtpReservationStart.Format = DateTimePickerFormat.Custom;
            dtpReservationStart.CustomFormat = "dd.MM.yyyy HH:mm";
            gbForm.Controls.Add(dtpReservationStart);

            gbForm.Controls.Add(CreateLabel("End DateTime", 20, 265));
            dtpReservationEnd = new DateTimePicker();
            dtpReservationEnd.Name = "dtpReservationEnd";
            dtpReservationEnd.Left = 180;
            dtpReservationEnd.Top = 260;
            dtpReservationEnd.Width = 210;
            dtpReservationEnd.Format = DateTimePickerFormat.Custom;
            dtpReservationEnd.CustomFormat = "dd.MM.yyyy HH:mm";
            gbForm.Controls.Add(dtpReservationEnd);

            gbForm.Controls.Add(CreateLabel("Status", 20, 310));
            cbReservationStatus = CreateComboBox("cbReservationStatus", 180, 305, 210);
            cbReservationStatus.Items.AddRange(new object[] { "Active", "Finished", "Canceled" });
            gbForm.Controls.Add(cbReservationStatus);

            btnReservationCreate = CreateActionButton("Create", 20, 390);
            btnReservationCancel = CreateActionButton("Cancel", 140, 390);
            btnReservationFinish = CreateActionButton("Finish", 260, 390);

            btnReservationCheck = CreateActionButton("Check", 20, 440);
            btnReservationClear = CreateActionButton("Clear", 140, 440);
            btnReservationRefresh = CreateActionButton("Refresh", 260, 440);

            gbForm.Controls.Add(btnReservationCreate);
            gbForm.Controls.Add(btnReservationCancel);
            gbForm.Controls.Add(btnReservationFinish);
            gbForm.Controls.Add(btnReservationCheck);
            gbForm.Controls.Add(btnReservationClear);
            gbForm.Controls.Add(btnReservationRefresh);

            GroupBox gbList = CreateGroupBox("Reservations List", 470, 20, 650, 610);
            dgvReservations = CreateGrid("dgvReservations", 15, 30, 620, 560);
            gbList.Controls.Add(dgvReservations);

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }
    }
}