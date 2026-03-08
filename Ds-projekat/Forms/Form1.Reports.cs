using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildReportsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbExport = CreateGroupBox("CSV Export", 20, 20, 500, 430);

            gbExport.Controls.Add(CreateLabel("Users", 20, 50));
            btnExportUsers = CreateActionButton("Export Users", 180, 42, 180);
            gbExport.Controls.Add(btnExportUsers);

            gbExport.Controls.Add(CreateLabel("Resources", 20, 100));
            btnExportResources = CreateActionButton("Export Resources", 180, 92, 180);
            gbExport.Controls.Add(btnExportResources);

            gbExport.Controls.Add(CreateLabel("Locations", 20, 150));
            btnExportLocations = CreateActionButton("Export Locations", 180, 142, 180);
            gbExport.Controls.Add(btnExportLocations);

            gbExport.Controls.Add(CreateLabel("Membership Types", 20, 200));
            btnExportMemberships = CreateActionButton("Export Memberships", 180, 192, 180);
            gbExport.Controls.Add(btnExportMemberships);

            gbExport.Controls.Add(CreateLabel("Reservations by User", 20, 255));
            gbExport.Controls.Add(CreateLabel("User ID", 20, 295));
            txtReportReservationUserId = CreateTextBox("txtReportReservationUserId", 180, 290, 180);
            gbExport.Controls.Add(txtReportReservationUserId);

            btnExportReservations = CreateActionButton("Export Reservations", 180, 335, 180);
            gbExport.Controls.Add(btnExportReservations);

            GroupBox gbPreview = CreateGroupBox("Preview / Status", 550, 20, 570, 430);

            txtReportStatus = new TextBox();
            txtReportStatus.Name = "txtReportStatus";
            txtReportStatus.Multiline = true;
            txtReportStatus.ScrollBars = ScrollBars.Vertical;
            txtReportStatus.Left = 20;
            txtReportStatus.Top = 35;
            txtReportStatus.Width = 530;
            txtReportStatus.Height = 360;
            txtReportStatus.ReadOnly = true;
            gbPreview.Controls.Add(txtReportStatus);

            GroupBox gbNotes = CreateGroupBox("Napomena", 20, 480, 1100, 140);
            Label lbl = new Label();
            lbl.Text = "Export Reservations koristi User ID i pravi CSV samo za rezervacije tog korisnika.";
            lbl.Left = 20;
            lbl.Top = 40;
            lbl.Width = 900;
            gbNotes.Controls.Add(lbl);

            page.Controls.Add(gbExport);
            page.Controls.Add(gbPreview);
            page.Controls.Add(gbNotes);

            return page;
        }
    }
}