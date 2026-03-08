using Ds_projekat.Services;
using Ds_projekat.Observer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1 : Form
    {
        private Services.AutoExportManager _autoExportManager;
        private Timer _autoExportTimer;

        private Panel sidebarPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblBrandName;

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

        private readonly UserFacade _userFacade = new UserFacade();
        private readonly MembershipFacade _membershipFacade = new MembershipFacade();

        private List<User> _allUsers = new List<User>();
        private List<MembershipType> _allMembershipTypes = new List<MembershipType>();

        private List<MembershipType> _allMembershipTypesGrid = new List<MembershipType>();

        private TextBox txtMembershipTypeId;
        private TextBox txtPackageName;
        private TextBox txtPackagePrice;
        private TextBox txtDurationDays;
        private TextBox txtMaxHoursMonth;
        private TextBox txtMeetingRoomHours;

        private CheckBox chkMeetingRoomAccess;

        private Button btnMembershipAdd;
        private Button btnMembershipUpdate;
        private Button btnMembershipDelete;
        private Button btnMembershipClear;
        private Button btnMembershipRefresh;

        private DataGridView dgvMembershipTypes;

        private readonly LocationFacade _locationFacade = new LocationFacade();

        private List<Location> _allLocations = new List<Location>();

        private TextBox txtLocationId;
        private TextBox txtLocationName;
        private TextBox txtLocationAddress;
        private TextBox txtLocationCity;
        private TextBox txtWorkingHours;
        private TextBox txtMaxUsers;

        private Button btnLocationAdd;
        private Button btnLocationUpdate;
        private Button btnLocationDelete;
        private Button btnLocationClear;
        private Button btnLocationRefresh;

        private DataGridView dgvLocations;

        private readonly ResourceFacade _resourceFacade = new ResourceFacade();

        private List<Resource> _allResources = new List<Resource>();
        private List<Location> _allResourceLocations = new List<Location>();

        private TextBox txtResourceId;
        private ComboBox cbResourceLocation;
        private TextBox txtResourceName;
        private ComboBox cbResourceType;
        private CheckBox chkResourceIsActive;
        private TextBox txtResourceDescription;
        private ComboBox cbDeskSubtype;
        private TextBox txtRoomCapacity;
        private CheckBox chkProjector;
        private CheckBox chkTV;
        private CheckBox chkBoard;
        private CheckBox chkOnlineEquipment;

        private Button btnResourceAdd;
        private Button btnResourceUpdate;
        private Button btnResourceDelete;
        private Button btnResourceClear;
        private Button btnResourceRefresh;

        private DataGridView dgvResources;
        private ComboBox cbResourceListLocation;
        private ComboBox cbResourceListType;

        private readonly ReservationFacade _reservationFacade = new ReservationFacade();

        private List<Reservation> _allReservations = new List<Reservation>();
        private List<User> _allReservationUsers = new List<User>();
        private List<Resource> _allReservationResources = new List<Resource>();

        private TextBox txtReservationId;
        private ComboBox cbReservationUser;
        private ComboBox cbReservationResource;
        private TextBox txtReservationUsersCount;
        private DateTimePicker dtpReservationStart;
        private DateTimePicker dtpReservationEnd;
        private ComboBox cbReservationStatus;

        private Button btnReservationCreate;
        private Button btnReservationCancel;
        private Button btnReservationFinish;
        private Button btnReservationCheck;
        private Button btnReservationClear;
        private Button btnReservationRefresh;

        private DataGridView dgvReservations;
        private ComboBox cbReservationFilterUser;
        private ComboBox cbReservationFilterLocation;
        private DateTimePicker dtpReservationFilterDate;
        private Label lblReservationOccupancy;

        private bool _isBindingReservations;
        private bool _isFillingReservationForm;

        private readonly AdminFacade _adminFacade = new AdminFacade();

        private List<Admin> _allAdmins = new List<Admin>();

        private TextBox txtAdminUserId;
        private TextBox txtAdminRoleName;
        private TextBox txtAdminUsername;
        private TextBox txtAdminPassword;

        private Button btnAdminRegister;
        private Button btnAdminUpdate;
        private Button btnAdminDelete;
        private Button btnAdminClear;
        private Button btnAdminRefresh;

        private DataGridView dgvAdmins;

        private readonly ReportFacade _reportFacade = new ReportFacade();

        private Button btnExportUsers;
        private Button btnExportResources;
        private Button btnExportLocations;
        private Button btnExportMemberships;
        private Button btnExportReservations;

        private TextBox txtReportStatus;
        private TextBox txtReportReservationUserId;

        private Label lblUsersCount;
        private Label lblLocationsCount;
        private Label lblResourcesCount;
        private Label lblReservationsCount;

        private DataGridView dgvRecentReservations;
        //za main
        private TextBox txtUserId;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtUserEmail;
        private TextBox txtUserPhone;

        private ComboBox cbUserMembershipType;
        private ComboBox cbUserStatus;
        private ComboBox cbUserFilterMembershipType;
        private ComboBox cbUserFilterStatus;

        private DateTimePicker dtpMembershipStart;
        private DateTimePicker dtpMembershipEnd;

        private Button btnUserAdd;
        private Button btnUserUpdate;
        private Button btnUserDelete;
        private Button btnUserClear;
        private Button btnUserSearch;
        private Button btnUserRefresh;

        private DataGridView dgvUsers;
        public Form1()
        {
            InitializeComponentCustom();

            _autoExportManager = new Services.AutoExportManager();

            _autoExportTimer = new Timer();
            _autoExportTimer.Interval = 60000; // 1 minut
            _autoExportTimer.Tick += AutoExportTimer_Tick;
            _autoExportTimer.Start();

            InitializeUsersModule();
            InitializeMembershipsModule();
            InitializeLocationsModule();
            InitializeResourcesModule();
            InitializeReservationsModule();
            InitializeAdminsModule();
            InitializeReportsModule();
            InitializeObserverPattern();

            InitializeDashboard();

            ShowPage(pageDashboard, "Dashboard", "Pregled sistema");

            LoadAutoExportSettingsToGui();
            CheckAutoExportNow();

            Button btnSave = FindControlRecursive<Button>(this, "btnSaveAutoExport");
            if (btnSave != null)
                btnSave.Click += BtnSaveAutoExport_Click;
        }

        private void AutoExportTimer_Tick(object sender, EventArgs e)
        {
            CheckAutoExportNow();
        }

        private void CheckAutoExportNow()
        {
            var result = _autoExportManager.RunAutoExportIfNeeded();
            UpdateAutoExportLabel();

            if (result != null && result.Success)
                NotifyObservers("Reports", "AutoExport", result.Message);
        }

        private void LoadAutoExportSettingsToGui()
        {
            var settings = _autoExportManager.LoadSettings();

            TextBox txtDays = FindControlRecursive<TextBox>(this, "txtAutoExportDays");
            TextBox txtFolder = FindControlRecursive<TextBox>(this, "txtAutoExportFolder");

            if (txtDays != null)
                txtDays.Text = settings.IntervalDays.ToString();

            if (txtFolder != null)
                txtFolder.Text = settings.ExportFolderPath;

            UpdateAutoExportLabel();
        }

        private void UpdateAutoExportLabel()
        {
            var settings = _autoExportManager.LoadSettings();
            Label lbl = FindControlRecursive<Label>(this, "lblNextExportDate");

            if (lbl == null)
                return;

            if (!settings.Enabled)
            {
                lbl.Text = "Next export: disabled";
                return;
            }

            DateTime nextDate = _autoExportManager.GetNextExportDate(settings);
            lbl.Text = "Next export: " + nextDate.ToString("dd.MM.yyyy");
        }

        private T FindControlRecursive<T>(Control parent, string name) where T : Control
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Name == name && ctrl is T)
                    return (T)ctrl;

                T found = FindControlRecursive<T>(ctrl, name);
                if (found != null)
                    return found;
            }

            return null;
        }

        private void BtnSaveAutoExport_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox txtDays = FindControlRecursive<TextBox>(this, "txtAutoExportDays");
                TextBox txtFolder = FindControlRecursive<TextBox>(this, "txtAutoExportFolder");

                if (txtDays == null || txtFolder == null)
                {
                    MessageBox.Show("Auto export kontrole nisu pronađene.");
                    return;
                }

                int intervalDays;
                if (!int.TryParse(txtDays.Text.Trim(), out intervalDays) || intervalDays <= 0)
                {
                    MessageBox.Show("Unesi ispravan broj dana.");
                    return;
                }

                string folderPath = txtFolder.Text.Trim();
                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    MessageBox.Show("Unesi folder za export.");
                    return;
                }

                var settings = _autoExportManager.LoadSettings();
                settings.Enabled = true;
                settings.IntervalDays = intervalDays;
                settings.ExportFolderPath = folderPath;

                if (settings.LastExportDate == DateTime.MinValue)
                    settings.LastExportDate = DateTime.Now;

                _autoExportManager.SaveSettings(settings);
                UpdateAutoExportLabel();
                NotifyObservers("Reports", "AutoExportSettings", "Podešen je automatski CSV export na svakih " + intervalDays + " dana.");

                MessageBox.Show("Automatski export je podešen.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška: " + ex.Message);
            }
        }
    }
}