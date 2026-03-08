using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.Text = string.IsNullOrWhiteSpace(AppConfig.Instance.BrandName)
                ? "Coworking Space Management"
                : AppConfig.Instance.BrandName + " - Coworking Space Management";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 750);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            sidebarPanel = new Panel();
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Width = 240;
            sidebarPanel.BackColor = Color.FromArgb(32, 42, 68);

            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = Color.White;
            headerPanel.Padding = new Padding(20, 10, 20, 10);

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(245, 247, 250);
            contentPanel.Padding = new Padding(15);

            lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(35, 35, 35);
            lblTitle.Location = new Point(20, 12);

            lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(22, 48);

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblSubtitle);

            lblBrandName = new Label();
            lblBrandName.Text = string.IsNullOrWhiteSpace(AppConfig.Instance.BrandName) ? "DS PROJEKAT" : AppConfig.Instance.BrandName;
            lblBrandName.ForeColor = Color.White;
            lblBrandName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblBrandName.AutoSize = false;
            lblBrandName.TextAlign = ContentAlignment.MiddleCenter;
            lblBrandName.Dock = DockStyle.Top;
            lblBrandName.Height = 80;

            btnDashboard = CreateSidebarButton("Dashboard");
            btnUsers = CreateSidebarButton("Users");
            btnMemberships = CreateSidebarButton("Membership Types");
            btnLocations = CreateSidebarButton("Locations");
            btnResources = CreateSidebarButton("Resources");
            btnReservations = CreateSidebarButton("Reservations");
            btnAdmins = CreateSidebarButton("Admins");
            btnReports = CreateSidebarButton("Reports");
            btnExit = CreateSidebarButton("Exit");

            btnDashboard.Click += (s, e) =>
            {
                LoadDashboardData();
                ShowPage(pageDashboard, "Dashboard", "Pregled sistema");
            };
            btnUsers.Click += (s, e) => ShowPage(pageUsers, "Users", "Upravljanje korisnicima");
            btnMemberships.Click += (s, e) => ShowPage(pageMemberships, "Membership Types", "Upravljanje paketima članarina");
            btnLocations.Click += (s, e) => ShowPage(pageLocations, "Locations", "Upravljanje lokacijama");
            btnResources.Click += (s, e) => ShowPage(pageResources, "Resources", "Upravljanje resursima");
            btnReservations.Click += (s, e) => ShowPage(pageReservations, "Reservations", "Upravljanje rezervacijama");
            btnAdmins.Click += (s, e) => ShowPage(pageAdmins, "Admins", "Administracija naloga");
            btnReports.Click += (s, e) => ShowPage(pageReports, "Reports", "Izveštaji i CSV export");
            btnExit.Click += (s, e) => this.Close();

            FlowLayoutPanel menuPanel = new FlowLayoutPanel();
            menuPanel.Dock = DockStyle.Fill;
            menuPanel.FlowDirection = FlowDirection.TopDown;
            menuPanel.WrapContents = false;
            menuPanel.AutoScroll = true;
            menuPanel.Padding = new Padding(10, 10, 10, 10);

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
            sidebarPanel.Controls.Add(lblBrandName);

            pageDashboard = BuildDashboardPage();
            pageUsers = BuildUsersPage();
            pageMemberships = BuildMembershipsPage();
            pageLocations = BuildLocationsPage();
            pageResources = BuildResourcesPage();
            pageReservations = BuildReservationsPage();
            pageAdmins = BuildAdminsPage();
            pageReports = BuildReportsPage();

            contentPanel.Controls.Add(pageDashboard);
            contentPanel.Controls.Add(pageUsers);
            contentPanel.Controls.Add(pageMemberships);
            contentPanel.Controls.Add(pageLocations);
            contentPanel.Controls.Add(pageResources);
            contentPanel.Controls.Add(pageReservations);
            contentPanel.Controls.Add(pageAdmins);
            contentPanel.Controls.Add(pageReports);

            this.Controls.Add(contentPanel);
            this.Controls.Add(headerPanel);
            this.Controls.Add(sidebarPanel);

            this.ResumeLayout(false);
        }

        private void ShowPage(Panel page, string title, string subtitle)
        {
            foreach (Control ctrl in contentPanel.Controls)
                ctrl.Visible = false;

            page.Visible = true;
            page.BringToFront();

            lblTitle.Text = title;
            lblSubtitle.Text = subtitle;
        }
    }
}