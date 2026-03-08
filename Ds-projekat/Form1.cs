using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1 : Form
    {
        private readonly DashboardForm _dashboardForm = new DashboardForm();
        private readonly UsersForm _usersForm = new UsersForm();
        private readonly MembershipsForm _membershipsForm = new MembershipsForm();
        private readonly LocationsForm _locationsForm = new LocationsForm();
        private readonly ResourcesForm _resourcesForm = new ResourcesForm();
        private readonly ReservationsForm _reservationsForm = new ReservationsForm();
        private readonly AdminsForm _adminsForm = new AdminsForm();
        private readonly ReportsForm _reportsForm = new ReportsForm();

        private readonly List<Button> _menuButtons = new List<Button>();

        private Panel sidebarPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Label lblTitle;
        private Label lblSubtitle;

        private Button btnDashboard;
        private Button btnUsers;
        private Button btnMemberships;
        private Button btnLocations;
        private Button btnResources;
        private Button btnReservations;
        private Button btnAdmins;
        private Button btnReports;
        private Button btnExit;

        private readonly Color _normalColor = Color.FromArgb(44, 58, 89);
        private readonly Color _activeColor = Color.FromArgb(52, 120, 246);

        public Form1()
        {
            InitializeShell();
            ShowSection(_dashboardForm, btnDashboard, "Dashboard", "Pregled sistema");
        }

        private void InitializeShell()
        {
            SuspendLayout();

            Text = "Coworking Space Management";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1200, 750);
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = Color.FromArgb(32, 42, 68)
            };

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                Padding = new Padding(15)
            };

            lblTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 35),
                Location = new Point(20, 12)
            };

            lblSubtitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(22, 48)
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblSubtitle);

            Label logo = new Label
            {
                Text = "DS PROJEKAT",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };

            btnDashboard = CreateSidebarButton("Dashboard");
            btnUsers = CreateSidebarButton("Users");
            btnMemberships = CreateSidebarButton("Membership Types");
            btnLocations = CreateSidebarButton("Locations");
            btnResources = CreateSidebarButton("Resources");
            btnReservations = CreateSidebarButton("Reservations");
            btnAdmins = CreateSidebarButton("Admins");
            btnReports = CreateSidebarButton("Reports");
            btnExit = CreateSidebarButton("Exit");

            btnDashboard.Click += (s, e) => ShowSection(_dashboardForm, btnDashboard, "Dashboard", "Pregled sistema");
            btnUsers.Click += (s, e) => ShowSection(_usersForm, btnUsers, "Users", "Upravljanje korisnicima");
            btnMemberships.Click += (s, e) => ShowSection(_membershipsForm, btnMemberships, "Membership Types", "Upravljanje paketima clanarina");
            btnLocations.Click += (s, e) => ShowSection(_locationsForm, btnLocations, "Locations", "Upravljanje lokacijama");
            btnResources.Click += (s, e) => ShowSection(_resourcesForm, btnResources, "Resources", "Upravljanje resursima");
            btnReservations.Click += (s, e) => ShowSection(_reservationsForm, btnReservations, "Reservations", "Upravljanje rezervacijama");
            btnAdmins.Click += (s, e) => ShowSection(_adminsForm, btnAdmins, "Admins", "Administracija naloga");
            btnReports.Click += (s, e) => ShowSection(_reportsForm, btnReports, "Reports", "Izvestaji i CSV export");
            btnExit.Click += (s, e) => Close();

            _menuButtons.AddRange(new[]
            {
                btnDashboard, btnUsers, btnMemberships, btnLocations,
                btnResources, btnReservations, btnAdmins, btnReports, btnExit
            });

            FlowLayoutPanel menuPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(10, 10, 10, 10)
            };

            menuPanel.Controls.Add(btnDashboard);
            menuPanel.Controls.Add(btnUsers);
            menuPanel.Controls.Add(btnMemberships);
            menuPanel.Controls.Add(btnLocations);
            menuPanel.Controls.Add(btnResources);
            menuPanel.Controls.Add(btnReservations);
            menuPanel.Controls.Add(btnAdmins);
            menuPanel.Controls.Add(btnReports);
            menuPanel.Controls.Add(btnExit);

            sidebarPanel.Controls.Add(menuPanel);
            sidebarPanel.Controls.Add(logo);

            Controls.Add(contentPanel);
            Controls.Add(headerPanel);
            Controls.Add(sidebarPanel);

            ResumeLayout(false);
        }

        private Button CreateSidebarButton(string text)
        {
            Button button = new Button
            {
                Text = text,
                Width = 200,
                Height = 45,
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                BackColor = _normalColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void ShowSection(Form sectionForm, Button activeButton, string title, string subtitle)
        {
            if (!contentPanel.Controls.Contains(sectionForm))
            {
                contentPanel.Controls.Add(sectionForm);
                sectionForm.Show();
            }

            foreach (Control control in contentPanel.Controls)
            {
                control.Visible = false;
            }

            sectionForm.Visible = true;
            sectionForm.BringToFront();

            foreach (Button button in _menuButtons)
            {
                button.BackColor = _normalColor;
            }

            activeButton.BackColor = _activeColor;
            lblTitle.Text = title;
            lblSubtitle.Text = subtitle;
        }
    }
}
