using Ds_projekat.UI.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ds_projekat.UI.Forms
{
    public partial class MainShellForm : Form
    {
        private readonly UsersPage _usersPage = new UsersPage();
        private readonly ResourcesPage _resourcesPage = new ResourcesPage();
        private readonly ReservationsPage _reservationsPage = new ReservationsPage();

        public MainShellForm()
        {
            InitializeComponent();
            _menuButtons.AddRange(new[] { btnUsers, btnResources, btnReservations, btnLocations, btnMemberships, btnReports });
            btnUsers.Image = Properties.Resources.user;
            btnResources.Image = Properties.Resources.resursi;
            btnReservations.Image = Properties.Resources.rezervacije;
            btnLocations.Image = Properties.Resources.lokacija;
            btnMemberships.Image = Properties.Resources.clanstvo;
            btnReports.Image = Properties.Resources.izvestaj;
        }

        private void MainShellForm_Load(object sender, EventArgs e)
        {

        }
        private readonly List<Button> _menuButtons = new();

        private Color NormalColor = Color.FromArgb(35, 59, 93);
        private Color ActiveColor = Color.FromArgb(35, 120, 220);

        private void SetActive(Button active)
        {
            foreach (var b in _menuButtons)
                b.BackColor = NormalColor;

            active.BackColor = ActiveColor;
        }
        private void ShowPage(UserControl page)
        {
            pnlContent.Controls.Clear();
            page.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(page);
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            ShowPage(_usersPage);
        }

        private void btnResources_Click(object sender, EventArgs e)
        {
            ShowPage(_resourcesPage);
        }

        private void btnReservations_Click(object sender, EventArgs e)
        {
            ShowPage(_reservationsPage);
        }
    }
}
