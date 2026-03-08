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

        private readonly Color _shellBackgroundColor = AppTheme.AppBackgroundColor;
        private readonly Color _surfaceColor = AppTheme.SurfaceStrongColor;
        private readonly Color _normalColor = AppTheme.PrimaryMutedColor;
        private readonly Color _activeColor = AppTheme.AccentColor;
        private readonly Color _sidebarColor = AppTheme.PrimaryColor;
        private readonly Color _textColor = AppTheme.TextColor;
        private readonly Color _mutedTextColor = AppTheme.MutedTextColor;
        private readonly string _brandName;

        public Form1()
        {
            try
            {
                AppConfig.Instance.Load("config.txt");
                _brandName = string.IsNullOrWhiteSpace(AppConfig.Instance.BrandName)
                    ? "Coworking"
                    : AppConfig.Instance.BrandName;
            }
            catch
            {
                _brandName = "Coworking";
            }

            InitializeShell();
            ShowSection(_dashboardForm, btnDashboard, "Kontrolna tabla", "Pregled sistema");
        }

        private void InitializeShell()
        {
            SuspendLayout();

            Text = _brandName + " - Administratorski panel";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1200, 750);
            BackColor = _shellBackgroundColor;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = _sidebarColor
            };

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = _surfaceColor,
                Padding = new Padding(20, 10, 20, 10)
            };

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _shellBackgroundColor,
                Padding = new Padding(15)
            };

            lblTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Georgia", 18F, FontStyle.Bold),
                ForeColor = _textColor,
                Location = new Point(20, 12)
            };

            lblSubtitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = _mutedTextColor,
                Location = new Point(22, 48)
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblSubtitle);

            Panel accentBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 3,
                BackColor = _activeColor
            };
            headerPanel.Controls.Add(accentBar);

            Label logo = new Label
            {
                Text = _brandName.ToUpperInvariant(),
                ForeColor = Color.White,
                Font = new Font("Georgia", 16F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };

            btnDashboard = CreateSidebarButton("Kontrolna tabla");
            btnUsers = CreateSidebarButton("Korisnici");
            btnMemberships = CreateSidebarButton("Clanarine");
            btnLocations = CreateSidebarButton("Lokacije");
            btnResources = CreateSidebarButton("Resursi");
            btnReservations = CreateSidebarButton("Rezervacije");
            btnAdmins = CreateSidebarButton("Administratori");
            btnReports = CreateSidebarButton("Izvestaji");
            btnExit = CreateSidebarButton("Izlaz");

            btnDashboard.Click += (s, e) => ShowSection(_dashboardForm, btnDashboard, "Kontrolna tabla", "Pregled sistema");
            btnUsers.Click += (s, e) => ShowSection(_usersForm, btnUsers, "Korisnici", "Upravljanje korisnicima");
            btnMemberships.Click += (s, e) => ShowSection(_membershipsForm, btnMemberships, "Clanarine", "Upravljanje paketima clanarina");
            btnLocations.Click += (s, e) => ShowSection(_locationsForm, btnLocations, "Lokacije", "Upravljanje lokacijama");
            btnResources.Click += (s, e) => ShowSection(_resourcesForm, btnResources, "Resursi", "Upravljanje resursima");
            btnReservations.Click += (s, e) => ShowSection(_reservationsForm, btnReservations, "Rezervacije", "Upravljanje rezervacijama");
            btnAdmins.Click += (s, e) => ShowSection(_adminsForm, btnAdmins, "Administratori", "Administracija naloga");
            btnReports.Click += (s, e) => ShowSection(_reportsForm, btnReports, "Izvestaji", "Izvestaji i CSV export");
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
                Padding = new Padding(10, 10, 10, 10),
                BackColor = _sidebarColor
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
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0)
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

            if (sectionForm is IReloadableSection reloadableSection)
            {
                reloadableSection.LoadData();
            }

            foreach (Button button in _menuButtons)
            {
                button.BackColor = _normalColor;
                button.ForeColor = Color.White;
            }

            activeButton.BackColor = _activeColor;
            activeButton.ForeColor = _sidebarColor;
            lblTitle.Text = title;
            lblSubtitle.Text = subtitle;
        }
    }
}
