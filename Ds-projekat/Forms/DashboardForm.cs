using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class DashboardForm : SectionFormBase
    {
        public DashboardForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            Panel card1 = CreateStatCard("Users", "0", 20, 20);
            Panel card2 = CreateStatCard("Locations", "0", 290, 20);
            Panel card3 = CreateStatCard("Resources", "0", 560, 20);
            Panel card4 = CreateStatCard("Reservations", "0", 830, 20);

            GroupBox recentGroup = CreateGroupBox("Recent Reservations", 20, 170, 1080, 250);
            recentGroup.Controls.Add(CreateGrid("dgvRecentReservations", 15, 30, 1050, 200));

            GroupBox notesGroup = CreateGroupBox("System Notes", 20, 440, 1080, 180);
            Label infoLabel = new Label
            {
                Text = "Ovde kasnije mozes prikazivati statistiku, obavestenja, aktivne clanarine i slicno.",
                Left = 20,
                Top = 40,
                Width = 900
            };
            notesGroup.Controls.Add(infoLabel);

            Controls.Add(card1);
            Controls.Add(card2);
            Controls.Add(card3);
            Controls.Add(card4);
            Controls.Add(recentGroup);
            Controls.Add(notesGroup);
        }
    }
}
