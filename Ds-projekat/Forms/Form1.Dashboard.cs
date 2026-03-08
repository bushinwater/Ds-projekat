using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildDashboardPage()
        {
            Panel page = CreateBasePage();

            Panel card1 = CreateStatCard("Users", out lblUsersCount, 20, 20);
            Panel card2 = CreateStatCard("Locations", out lblLocationsCount, 290, 20);
            Panel card3 = CreateStatCard("Resources", out lblResourcesCount, 560, 20);
            Panel card4 = CreateStatCard("Reservations", out lblReservationsCount, 830, 20);

            GroupBox gbRecent = CreateGroupBox("Recent Reservations", 20, 170, 1080, 250);

            dgvRecentReservations = CreateGrid(
                "dgvRecentReservations",
                15,
                30,
                1050,
                200
            );

            gbRecent.Controls.Add(dgvRecentReservations);

            GroupBox gbInfo = CreateGroupBox("System Notes", 20, 440, 1080, 180);

            Label lblInfo = new Label();
            lblInfo.Text = "Ovde kasnije možeš prikazivati statistiku, obaveštenja, aktivne članarine i slično.";
            lblInfo.Left = 20;
            lblInfo.Top = 40;
            lblInfo.Width = 900;

            gbInfo.Controls.Add(lblInfo);

            page.Controls.Add(card1);
            page.Controls.Add(card2);
            page.Controls.Add(card3);
            page.Controls.Add(card4);

            page.Controls.Add(gbRecent);
            page.Controls.Add(gbInfo);

            return page;
        }
    }
}