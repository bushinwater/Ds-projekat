using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1 : Form
    {
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

        private Panel pageDashboard;
        private Panel pageUsers;
        private Panel pageMemberships;
        private Panel pageLocations;
        private Panel pageResources;
        private Panel pageReservations;
        private Panel pageAdmins;
        private Panel pageReports;

        public Form1()
        {
            InitializeComponentCustom();
            ShowPage(pageDashboard, "Dashboard", "Pregled sistema");
        }

        private void InitializeComponentCustom()
        {
            this.SuspendLayout();

            this.Text = "Coworking Space Management";
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

            Label logo = new Label();
            logo.Text = "DS PROJEKAT";
            logo.ForeColor = Color.White;
            logo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            logo.AutoSize = false;
            logo.TextAlign = ContentAlignment.MiddleCenter;
            logo.Dock = DockStyle.Top;
            logo.Height = 80;

            btnDashboard = CreateSidebarButton("Dashboard");
            btnUsers = CreateSidebarButton("Users");
            btnMemberships = CreateSidebarButton("Membership Types");
            btnLocations = CreateSidebarButton("Locations");
            btnResources = CreateSidebarButton("Resources");
            btnReservations = CreateSidebarButton("Reservations");
            btnAdmins = CreateSidebarButton("Admins");
            btnReports = CreateSidebarButton("Reports");
            btnExit = CreateSidebarButton("Exit");

            btnDashboard.Click += (s, e) => ShowPage(pageDashboard, "Dashboard", "Pregled sistema");
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
            sidebarPanel.Controls.Add(logo);

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

        private Button CreateSidebarButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = 200;
            btn.Height = 45;
            btn.Margin = new Padding(5);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(44, 58, 89);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
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

        private Panel CreateBasePage()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.FromArgb(245, 247, 250);
            return panel;
        }

        private GroupBox CreateGroupBox(string text, int x, int y, int w, int h)
        {
            GroupBox gb = new GroupBox();
            gb.Text = text;
            gb.Left = x;
            gb.Top = y;
            gb.Width = w;
            gb.Height = h;
            gb.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gb.BackColor = Color.White;
            return gb;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Left = x;
            lbl.Top = y;
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            return lbl;
        }

        private TextBox CreateTextBox(string name, int x, int y, int w)
        {
            TextBox tb = new TextBox();
            tb.Name = name;
            tb.Left = x;
            tb.Top = y;
            tb.Width = w;
            return tb;
        }

        private ComboBox CreateComboBox(string name, int x, int y, int w)
        {
            ComboBox cb = new ComboBox();
            cb.Name = name;
            cb.Left = x;
            cb.Top = y;
            cb.Width = w;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            return cb;
        }

        private Button CreateActionButton(string text, int x, int y, int w = 110)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Left = x;
            btn.Top = y;
            btn.Width = w;
            btn.Height = 36;
            btn.BackColor = Color.FromArgb(52, 120, 246);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private DataGridView CreateGrid(string name, int x, int y, int w, int h)
        {
            DataGridView dgv = new DataGridView();
            dgv.Name = name;
            dgv.Left = x;
            dgv.Top = y;
            dgv.Width = w;
            dgv.Height = h;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            return dgv;
        }

        private Panel BuildDashboardPage()
        {
            Panel page = CreateBasePage();

            Panel card1 = CreateStatCard("Users", "0", 20, 20);
            Panel card2 = CreateStatCard("Locations", "0", 290, 20);
            Panel card3 = CreateStatCard("Resources", "0", 560, 20);
            Panel card4 = CreateStatCard("Reservations", "0", 830, 20);

            GroupBox gbRecent = CreateGroupBox("Recent Reservations", 20, 170, 1080, 250);
            DataGridView dgvRecent = CreateGrid("dgvRecentReservations", 15, 30, 1050, 200);
            gbRecent.Controls.Add(dgvRecent);

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

        private Panel CreateStatCard(string title, string value, int x, int y)
        {
            Panel card = new Panel();
            card.Left = x;
            card.Top = y;
            card.Width = 240;
            card.Height = 120;
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lbl1 = new Label();
            lbl1.Text = title;
            lbl1.Left = 20;
            lbl1.Top = 20;
            lbl1.AutoSize = true;
            lbl1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            Label lbl2 = new Label();
            lbl2.Text = value;
            lbl2.Left = 20;
            lbl2.Top = 55;
            lbl2.AutoSize = true;
            lbl2.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lbl2.ForeColor = Color.FromArgb(52, 120, 246);

            card.Controls.Add(lbl1);
            card.Controls.Add(lbl2);

            return card;
        }

        private Panel BuildUsersPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("User Details", 20, 20, 420, 610);

            gbForm.Controls.Add(CreateLabel("User ID", 20, 40));
            gbForm.Controls.Add(CreateTextBox("txtUserId", 180, 35, 200));

            gbForm.Controls.Add(CreateLabel("First Name", 20, 85));
            gbForm.Controls.Add(CreateTextBox("txtFirstName", 180, 80, 200));

            gbForm.Controls.Add(CreateLabel("Last Name", 20, 130));
            gbForm.Controls.Add(CreateTextBox("txtLastName", 180, 125, 200));

            gbForm.Controls.Add(CreateLabel("Email", 20, 175));
            gbForm.Controls.Add(CreateTextBox("txtUserEmail", 180, 170, 200));

            gbForm.Controls.Add(CreateLabel("Phone", 20, 220));
            gbForm.Controls.Add(CreateTextBox("txtUserPhone", 180, 215, 200));

            gbForm.Controls.Add(CreateLabel("Membership Type", 20, 265));
            gbForm.Controls.Add(CreateComboBox("cbUserMembershipType", 180, 260, 200));

            gbForm.Controls.Add(CreateLabel("Start Date", 20, 310));
            DateTimePicker dtpStart = new DateTimePicker();
            dtpStart.Name = "dtpMembershipStart";
            dtpStart.Left = 180;
            dtpStart.Top = 305;
            dtpStart.Width = 200;
            gbForm.Controls.Add(dtpStart);

            gbForm.Controls.Add(CreateLabel("End Date", 20, 355));
            DateTimePicker dtpEnd = new DateTimePicker();
            dtpEnd.Name = "dtpMembershipEnd";
            dtpEnd.Left = 180;
            dtpEnd.Top = 350;
            dtpEnd.Width = 200;
            gbForm.Controls.Add(dtpEnd);

            gbForm.Controls.Add(CreateLabel("Status", 20, 400));
            ComboBox cbStatus = CreateComboBox("cbUserStatus", 180, 395, 200);
            cbStatus.Items.AddRange(new object[] { "Active", "Paused", "Expired" });
            gbForm.Controls.Add(cbStatus);

            gbForm.Controls.Add(CreateActionButton("Add", 20, 470));
            gbForm.Controls.Add(CreateActionButton("Update", 140, 470));
            gbForm.Controls.Add(CreateActionButton("Delete", 260, 470));

            gbForm.Controls.Add(CreateActionButton("Clear", 20, 520));
            gbForm.Controls.Add(CreateActionButton("Search", 140, 520));
            gbForm.Controls.Add(CreateActionButton("Refresh", 260, 520));

            GroupBox gbList = CreateGroupBox("Users List", 460, 20, 660, 610);
            gbList.Controls.Add(CreateGrid("dgvUsers", 15, 30, 630, 560));

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }

        private Panel BuildMembershipsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Membership Type Details", 20, 20, 420, 610);

            gbForm.Controls.Add(CreateLabel("ID", 20, 40));
            gbForm.Controls.Add(CreateTextBox("txtMembershipTypeId", 180, 35, 200));

            gbForm.Controls.Add(CreateLabel("Package Name", 20, 85));
            gbForm.Controls.Add(CreateTextBox("txtPackageName", 180, 80, 200));

            gbForm.Controls.Add(CreateLabel("Price", 20, 130));
            gbForm.Controls.Add(CreateTextBox("txtPackagePrice", 180, 125, 200));

            gbForm.Controls.Add(CreateLabel("Duration Days", 20, 175));
            gbForm.Controls.Add(CreateTextBox("txtDurationDays", 180, 170, 200));

            gbForm.Controls.Add(CreateLabel("Max Hours/Month", 20, 220));
            gbForm.Controls.Add(CreateTextBox("txtMaxHoursMonth", 180, 215, 200));

            gbForm.Controls.Add(CreateLabel("Meeting Room Access", 20, 265));
            CheckBox chkMeetingRoom = new CheckBox();
            chkMeetingRoom.Name = "chkMeetingRoomAccess";
            chkMeetingRoom.Left = 180;
            chkMeetingRoom.Top = 263;
            gbForm.Controls.Add(chkMeetingRoom);

            gbForm.Controls.Add(CreateLabel("Meeting Room Hours", 20, 310));
            gbForm.Controls.Add(CreateTextBox("txtMeetingRoomHours", 180, 305, 200));

            gbForm.Controls.Add(CreateActionButton("Add", 20, 390));
            gbForm.Controls.Add(CreateActionButton("Update", 140, 390));
            gbForm.Controls.Add(CreateActionButton("Delete", 260, 390));

            gbForm.Controls.Add(CreateActionButton("Clear", 20, 440));
            gbForm.Controls.Add(CreateActionButton("Refresh", 140, 440));

            GroupBox gbList = CreateGroupBox("Membership Types", 460, 20, 660, 610);
            gbList.Controls.Add(CreateGrid("dgvMembershipTypes", 15, 30, 630, 560));

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }

        private Panel BuildLocationsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Location Details", 20, 20, 420, 610);

            gbForm.Controls.Add(CreateLabel("Location ID", 20, 40));
            gbForm.Controls.Add(CreateTextBox("txtLocationId", 180, 35, 200));

            gbForm.Controls.Add(CreateLabel("Location Name", 20, 85));
            gbForm.Controls.Add(CreateTextBox("txtLocationName", 180, 80, 200));

            gbForm.Controls.Add(CreateLabel("Address", 20, 130));
            gbForm.Controls.Add(CreateTextBox("txtLocationAddress", 180, 125, 200));

            gbForm.Controls.Add(CreateLabel("City", 20, 175));
            gbForm.Controls.Add(CreateTextBox("txtLocationCity", 180, 170, 200));

            gbForm.Controls.Add(CreateLabel("Working Hours", 20, 220));
            gbForm.Controls.Add(CreateTextBox("txtWorkingHours", 180, 215, 200));

            gbForm.Controls.Add(CreateLabel("Max Users", 20, 265));
            gbForm.Controls.Add(CreateTextBox("txtMaxUsers", 180, 260, 200));

            gbForm.Controls.Add(CreateActionButton("Add", 20, 340));
            gbForm.Controls.Add(CreateActionButton("Update", 140, 340));
            gbForm.Controls.Add(CreateActionButton("Delete", 260, 340));

            gbForm.Controls.Add(CreateActionButton("Clear", 20, 390));
            gbForm.Controls.Add(CreateActionButton("Refresh", 140, 390));

            GroupBox gbList = CreateGroupBox("Locations List", 460, 20, 660, 610);
            gbList.Controls.Add(CreateGrid("dgvLocations", 15, 30, 630, 560));

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }

        private Panel BuildResourcesPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Resource Details", 20, 20, 460, 610);

            gbForm.Controls.Add(CreateLabel("Resource ID", 20, 40));
            gbForm.Controls.Add(CreateTextBox("txtResourceId", 190, 35, 220));

            gbForm.Controls.Add(CreateLabel("Location", 20, 85));
            gbForm.Controls.Add(CreateComboBox("cbResourceLocation", 190, 80, 220));

            gbForm.Controls.Add(CreateLabel("Resource Name", 20, 130));
            gbForm.Controls.Add(CreateTextBox("txtResourceName", 190, 125, 220));

            gbForm.Controls.Add(CreateLabel("Resource Type", 20, 175));
            ComboBox cbType = CreateComboBox("cbResourceType", 190, 170, 220);
            cbType.Items.AddRange(new object[] { "Desk", "Room" });
            gbForm.Controls.Add(cbType);

            gbForm.Controls.Add(CreateLabel("Is Active", 20, 220));
            CheckBox chkActive = new CheckBox();
            chkActive.Name = "chkResourceIsActive";
            chkActive.Left = 190;
            chkActive.Top = 218;
            gbForm.Controls.Add(chkActive);

            gbForm.Controls.Add(CreateLabel("Description", 20, 265));
            TextBox txtDesc = CreateTextBox("txtResourceDescription", 190, 260, 220);
            txtDesc.Multiline = true;
            txtDesc.Height = 60;
            gbForm.Controls.Add(txtDesc);

            gbForm.Controls.Add(CreateLabel("Desk Subtype", 20, 340));
            ComboBox cbDeskSubtype = CreateComboBox("cbDeskSubtype", 190, 335, 220);
            cbDeskSubtype.Items.AddRange(new object[] { "Hot", "Dedicated" });
            gbForm.Controls.Add(cbDeskSubtype);

            gbForm.Controls.Add(CreateLabel("Room Capacity", 20, 385));
            gbForm.Controls.Add(CreateTextBox("txtRoomCapacity", 190, 380, 220));

            CheckBox chkProjector = new CheckBox();
            chkProjector.Name = "chkProjector";
            chkProjector.Text = "Projector";
            chkProjector.Left = 190;
            chkProjector.Top = 425;
            gbForm.Controls.Add(chkProjector);

            CheckBox chkTV = new CheckBox();
            chkTV.Name = "chkTV";
            chkTV.Text = "TV";
            chkTV.Left = 300;
            chkTV.Top = 425;
            gbForm.Controls.Add(chkTV);

            CheckBox chkBoard = new CheckBox();
            chkBoard.Name = "chkBoard";
            chkBoard.Text = "Board";
            chkBoard.Left = 190;
            chkBoard.Top = 455;
            gbForm.Controls.Add(chkBoard);

            CheckBox chkOnlineEq = new CheckBox();
            chkOnlineEq.Name = "chkOnlineEquipment";
            chkOnlineEq.Text = "Online Equipment";
            chkOnlineEq.Left = 300;
            chkOnlineEq.Top = 455;
            gbForm.Controls.Add(chkOnlineEq);

            gbForm.Controls.Add(CreateActionButton("Add", 20, 520));
            gbForm.Controls.Add(CreateActionButton("Update", 140, 520));
            gbForm.Controls.Add(CreateActionButton("Delete", 260, 520));

            GroupBox gbList = CreateGroupBox("Resources List", 500, 20, 620, 610);
            gbList.Controls.Add(CreateGrid("dgvResources", 15, 30, 590, 560));

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }

        private Panel BuildReservationsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Reservation Details", 20, 20, 430, 610);

            gbForm.Controls.Add(CreateLabel("Reservation ID", 20, 40));
            gbForm.Controls.Add(CreateTextBox("txtReservationId", 180, 35, 210));

            gbForm.Controls.Add(CreateLabel("User", 20, 85));
            gbForm.Controls.Add(CreateComboBox("cbReservationUser", 180, 80, 210));

            gbForm.Controls.Add(CreateLabel("Resource", 20, 130));
            gbForm.Controls.Add(CreateComboBox("cbReservationResource", 180, 125, 210));

            gbForm.Controls.Add(CreateLabel("Users Count", 20, 175));
            gbForm.Controls.Add(CreateTextBox("txtReservationUsersCount", 180, 170, 210));

            gbForm.Controls.Add(CreateLabel("Start DateTime", 20, 220));
            DateTimePicker dtpStart = new DateTimePicker();
            dtpStart.Name = "dtpReservationStart";
            dtpStart.Left = 180;
            dtpStart.Top = 215;
            dtpStart.Width = 210;
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.CustomFormat = "dd.MM.yyyy HH:mm";
            gbForm.Controls.Add(dtpStart);

            gbForm.Controls.Add(CreateLabel("End DateTime", 20, 265));
            DateTimePicker dtpEnd = new DateTimePicker();
            dtpEnd.Name = "dtpReservationEnd";
            dtpEnd.Left = 180;
            dtpEnd.Top = 260;
            dtpEnd.Width = 210;
            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.CustomFormat = "dd.MM.yyyy HH:mm";
            gbForm.Controls.Add(dtpEnd);

            gbForm.Controls.Add(CreateLabel("Status", 20, 310));
            ComboBox cbStatus = CreateComboBox("cbReservationStatus", 180, 305, 210);
            cbStatus.Items.AddRange(new object[] { "Active", "Finished", "Canceled" });
            gbForm.Controls.Add(cbStatus);

            gbForm.Controls.Add(CreateActionButton("Create", 20, 390));
            gbForm.Controls.Add(CreateActionButton("Cancel", 140, 390));
            gbForm.Controls.Add(CreateActionButton("Finish", 260, 390));

            gbForm.Controls.Add(CreateActionButton("Check", 20, 440));
            gbForm.Controls.Add(CreateActionButton("Clear", 140, 440));
            gbForm.Controls.Add(CreateActionButton("Refresh", 260, 440));

            GroupBox gbList = CreateGroupBox("Reservations List", 470, 20, 650, 610);
            gbList.Controls.Add(CreateGrid("dgvReservations", 15, 30, 620, 560));

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }

        private Panel BuildAdminsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbAdmin = CreateGroupBox("Admin Details", 20, 20, 420, 610);

            gbAdmin.Controls.Add(CreateLabel("User ID", 20, 40));
            gbAdmin.Controls.Add(CreateTextBox("txtAdminUserId", 180, 35, 200));

            gbAdmin.Controls.Add(CreateLabel("Role Name", 20, 85));
            gbAdmin.Controls.Add(CreateTextBox("txtAdminRoleName", 180, 80, 200));

            gbAdmin.Controls.Add(CreateLabel("Username", 20, 130));
            gbAdmin.Controls.Add(CreateTextBox("txtAdminUsername", 180, 125, 200));

            gbAdmin.Controls.Add(CreateLabel("Password", 20, 175));
            TextBox txtPass = CreateTextBox("txtAdminPassword", 180, 170, 200);
            txtPass.PasswordChar = '*';
            gbAdmin.Controls.Add(txtPass);

            gbAdmin.Controls.Add(CreateActionButton("Register", 20, 250));
            gbAdmin.Controls.Add(CreateActionButton("Update", 140, 250));
            gbAdmin.Controls.Add(CreateActionButton("Delete", 260, 250));

            gbAdmin.Controls.Add(CreateActionButton("Clear", 20, 300));
            gbAdmin.Controls.Add(CreateActionButton("Refresh", 140, 300));

            GroupBox gbLogin = CreateGroupBox("Admin Login", 20, 360, 420, 210);
            gbLogin.Controls.Add(CreateLabel("Username", 20, 40));
            gbLogin.Controls.Add(CreateTextBox("txtLoginUsername", 140, 35, 220));
            gbLogin.Controls.Add(CreateLabel("Password", 20, 85));
            TextBox txtLoginPass = CreateTextBox("txtLoginPassword", 140, 80, 220);
            txtLoginPass.PasswordChar = '*';
            gbLogin.Controls.Add(txtLoginPass);
            gbLogin.Controls.Add(CreateActionButton("Login", 140, 130, 120));

            GroupBox gbList = CreateGroupBox("Admins List", 460, 20, 660, 610);
            gbList.Controls.Add(CreateGrid("dgvAdmins", 15, 30, 630, 560));

            page.Controls.Add(gbAdmin);
            page.Controls.Add(gbLogin);
            page.Controls.Add(gbList);

            return page;
        }

        private Panel BuildReportsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbExport = CreateGroupBox("CSV Export", 20, 20, 500, 400);

            gbExport.Controls.Add(CreateLabel("Users", 20, 50));
            gbExport.Controls.Add(CreateActionButton("Export Users", 180, 42, 180));

            gbExport.Controls.Add(CreateLabel("Resources", 20, 100));
            gbExport.Controls.Add(CreateActionButton("Export Resources", 180, 92, 180));

            gbExport.Controls.Add(CreateLabel("Locations", 20, 150));
            gbExport.Controls.Add(CreateActionButton("Export Locations", 180, 142, 180));

            gbExport.Controls.Add(CreateLabel("Membership Types", 20, 200));
            gbExport.Controls.Add(CreateActionButton("Export Memberships", 180, 192, 180));

            gbExport.Controls.Add(CreateLabel("Reservations", 20, 250));
            gbExport.Controls.Add(CreateActionButton("Export Reservations", 180, 242, 180));

            GroupBox gbPreview = CreateGroupBox("Preview / Status", 550, 20, 570, 400);

            TextBox txtReportStatus = new TextBox();
            txtReportStatus.Name = "txtReportStatus";
            txtReportStatus.Multiline = true;
            txtReportStatus.ScrollBars = ScrollBars.Vertical;
            txtReportStatus.Left = 20;
            txtReportStatus.Top = 35;
            txtReportStatus.Width = 530;
            txtReportStatus.Height = 330;
            gbPreview.Controls.Add(txtReportStatus);

            GroupBox gbNotes = CreateGroupBox("Napomena", 20, 450, 1100, 170);
            Label lbl = new Label();
            lbl.Text = "Ovde kasnije možeš povezati ReportFacade i Adapter za CSV export.";
            lbl.Left = 20;
            lbl.Top = 40;
            lbl.Width = 800;
            gbNotes.Controls.Add(lbl);

            page.Controls.Add(gbExport);
            page.Controls.Add(gbPreview);
            page.Controls.Add(gbNotes);

            return page;
        }
    }
}