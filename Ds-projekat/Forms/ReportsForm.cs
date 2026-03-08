using System.Windows.Forms;

namespace Ds_projekat
{
    internal class ReportsForm : SectionFormBase
    {
        public ReportsForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            GroupBox exportGroup = CreateGroupBox("CSV Export", 20, 20, 500, 400);

            exportGroup.Controls.Add(CreateLabel("Users", 20, 50));
            exportGroup.Controls.Add(CreateActionButton("Export Users", 180, 42, 180));

            exportGroup.Controls.Add(CreateLabel("Resources", 20, 100));
            exportGroup.Controls.Add(CreateActionButton("Export Resources", 180, 92, 180));

            exportGroup.Controls.Add(CreateLabel("Locations", 20, 150));
            exportGroup.Controls.Add(CreateActionButton("Export Locations", 180, 142, 180));

            exportGroup.Controls.Add(CreateLabel("Membership Types", 20, 200));
            exportGroup.Controls.Add(CreateActionButton("Export Memberships", 180, 192, 180));

            exportGroup.Controls.Add(CreateLabel("Reservations", 20, 250));
            exportGroup.Controls.Add(CreateActionButton("Export Reservations", 180, 242, 180));

            GroupBox previewGroup = CreateGroupBox("Preview / Status", 550, 20, 570, 400);
            TextBox reportStatusText = new TextBox
            {
                Name = "txtReportStatus",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Left = 20,
                Top = 35,
                Width = 530,
                Height = 330
            };
            previewGroup.Controls.Add(reportStatusText);

            GroupBox notesGroup = CreateGroupBox("Napomena", 20, 450, 1100, 170);
            Label notesLabel = new Label
            {
                Text = "Ovde kasnije mozes povezati ReportFacade i Adapter za CSV export.",
                Left = 20,
                Top = 40,
                Width = 800
            };
            notesGroup.Controls.Add(notesLabel);

            Controls.Add(exportGroup);
            Controls.Add(previewGroup);
            Controls.Add(notesGroup);
        }
    }
}
