using Ds_projekat.Commands;
using Ds_projekat.Services;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class ReportsForm : SectionFormBase, IReloadableSection
    {
        private readonly ReportFacade _reportFacade;
        private readonly ReportCommandFactory _reportCommandFactory;
        private readonly UserFacade _userFacade;
        private readonly LocationFacade _locationFacade;
        private readonly ResourceFacade _resourceFacade;
        private readonly MembershipFacade _membershipFacade;
        private readonly ReservationFacade _reservationFacade;
        private readonly Timer _autoExportTimer;

        private TextBox _reportStatusTextBox;
        private TextBox _autoExportFolderTextBox;
        private ComboBox _autoExportIntervalComboBox;
        private DateTimePicker _monthlyReportPicker;
        private Label _autoExportInfoLabel;

        private bool _autoExportEnabled;
        private DateTime _nextAutoExportAt;

        public ReportsForm()
        {
            _reportFacade = new ReportFacade();
            _reportCommandFactory = new ReportCommandFactory(_reportFacade);
            _userFacade = new UserFacade();
            _locationFacade = new LocationFacade();
            _resourceFacade = new ResourceFacade();
            _membershipFacade = new MembershipFacade();
            _reservationFacade = new ReservationFacade();

            _autoExportTimer = new Timer();
            _autoExportTimer.Interval = 60000;
            _autoExportTimer.Tick += AutoExportTimer_Tick;

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            GroupBox exportGroup = CreateGroupBox("CSV izvoz", 20, 20, 500, 400);

            exportGroup.Controls.Add(CreateLabel("Korisnici", 20, 50));
            Button exportUsersButton = CreateActionButton("Izvezi korisnike", 180, 42, 180);
            exportUsersButton.Click += (s, e) => ExportToCsv("users");
            exportGroup.Controls.Add(exportUsersButton);

            exportGroup.Controls.Add(CreateLabel("Resursi", 20, 100));
            Button exportResourcesButton = CreateActionButton("Izvezi resurse", 180, 92, 180);
            exportResourcesButton.Click += (s, e) => ExportToCsv("resources");
            exportGroup.Controls.Add(exportResourcesButton);

            exportGroup.Controls.Add(CreateLabel("Lokacije", 20, 150));
            Button exportLocationsButton = CreateActionButton("Izvezi lokacije", 180, 142, 180);
            exportLocationsButton.Click += (s, e) => ExportToCsv("locations");
            exportGroup.Controls.Add(exportLocationsButton);

            exportGroup.Controls.Add(CreateLabel("Clanarine", 20, 200));
            Button exportMembershipsButton = CreateActionButton("Izvezi clanarine", 180, 192, 180);
            exportMembershipsButton.Click += (s, e) => ExportToCsv("memberships");
            exportGroup.Controls.Add(exportMembershipsButton);

            exportGroup.Controls.Add(CreateLabel("Rezervacije", 20, 250));
            Button exportReservationsButton = CreateActionButton("Izvezi rezervacije", 180, 242, 180);
            exportReservationsButton.Click += (s, e) => ExportToCsv("reservations");
            exportGroup.Controls.Add(exportReservationsButton);

            exportGroup.Controls.Add(CreateLabel("Mesecni pregled", 20, 300));
            _monthlyReportPicker = CreateDatePicker("dtpMonthlyReport", 180, 295, 120);
            _monthlyReportPicker.Format = DateTimePickerFormat.Custom;
            _monthlyReportPicker.CustomFormat = "MM.yyyy";
            _monthlyReportPicker.ShowUpDown = true;
            exportGroup.Controls.Add(_monthlyReportPicker);

            Button exportMonthlyButton = CreateActionButton("Izvezi mesecni", 320, 292, 140);
            exportMonthlyButton.Click += (s, e) => ExportToCsv("monthly");
            exportGroup.Controls.Add(exportMonthlyButton);

            GroupBox previewGroup = CreateGroupBox("Pregled / status", 550, 20, 570, 400);
            _reportStatusTextBox = new TextBox
            {
                Name = "txtReportStatus",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Left = 20,
                Top = 35,
                Width = 530,
                Height = 330,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SurfaceColor,
                ForeColor = TextColor
            };
            previewGroup.Controls.Add(_reportStatusTextBox);

            GroupBox autoExportGroup = CreateGroupBox("Automatski CSV izvoz", 20, 450, 500, 170);
            autoExportGroup.Controls.Add(CreateLabel("Folder", 20, 45));

            _autoExportFolderTextBox = CreateTextBox("txtAutoExportFolder", 90, 40, 250);
            _autoExportFolderTextBox.ReadOnly = true;
            autoExportGroup.Controls.Add(_autoExportFolderTextBox);

            Button browseFolderButton = CreateActionButton("Izaberi", 355, 39, 110);
            browseFolderButton.Click += BrowseFolderButton_Click;
            autoExportGroup.Controls.Add(browseFolderButton);

            autoExportGroup.Controls.Add(CreateLabel("Interval", 20, 90));
            _autoExportIntervalComboBox = CreateComboBox("cbAutoExportInterval", 90, 85, 140);
            _autoExportIntervalComboBox.Items.AddRange(new object[] { "1h", "5h", "1d", "1w", "1M" });
            _autoExportIntervalComboBox.SelectedIndex = 0;
            autoExportGroup.Controls.Add(_autoExportIntervalComboBox);

            Button startAutoExportButton = CreateActionButton("Pokreni", 250, 84, 100);
            startAutoExportButton.Click += StartAutoExportButton_Click;
            autoExportGroup.Controls.Add(startAutoExportButton);

            Button stopAutoExportButton = CreateActionButton("Zaustavi", 365, 84, 100);
            stopAutoExportButton.Click += StopAutoExportButton_Click;
            autoExportGroup.Controls.Add(stopAutoExportButton);

            _autoExportInfoLabel = new Label
            {
                Left = 20,
                Top = 128,
                Width = 450,
                Height = 26,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Text = ""
            };
            autoExportGroup.Controls.Add(_autoExportInfoLabel);

            Controls.Add(exportGroup);
            Controls.Add(previewGroup);
            Controls.Add(autoExportGroup);
        }

        public void LoadData()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Pregled podataka iz baze");
                builder.AppendLine();
                builder.AppendLine("Korisnici: " + _userFacade.GetAll().Count);
                builder.AppendLine("Lokacije: " + _locationFacade.GetAll().Count);
                builder.AppendLine("Resursi: " + _resourceFacade.GetAllResources().Count);
                builder.AppendLine("Tipovi clanarina: " + _membershipFacade.GetAll().Count);
                builder.AppendLine("Rezervacije: " + _reservationFacade.GetAllReservations().Count);
                builder.AppendLine();
                builder.AppendLine("Izabrani mesec za monthly report: " + _monthlyReportPicker.Value.ToString("MM.yyyy"));

                if (_autoExportEnabled)
                    builder.AppendLine("Auto export aktivan. Sledeci export: " + _nextAutoExportAt.ToString("dd.MM.yyyy HH:mm"));

                _reportStatusTextBox.Text = builder.ToString();
                _reportStatusTextBox.ForeColor = TextColor;
            }
            catch (Exception ex)
            {
                _reportStatusTextBox.Text = "Greska pri ucitavanju pregleda izvestaja: " + ex.Message;
                _reportStatusTextBox.ForeColor = AppTheme.DangerColor;
            }
        }

        private void ExportToCsv(string reportType)
        {
            using SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = reportType + "-" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".csv"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            ServiceResult result = CreateCommand(reportType).Execute(saveFileDialog.FileName);
            _reportStatusTextBox.Text = result.Message + Environment.NewLine + Environment.NewLine + saveFileDialog.FileName;
            _reportStatusTextBox.ForeColor = result.Success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private void BrowseFolderButton_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            _autoExportFolderTextBox.Text = dialog.SelectedPath;
        }

        private void StartAutoExportButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_autoExportFolderTextBox.Text) || !Directory.Exists(_autoExportFolderTextBox.Text))
            {
                _autoExportInfoLabel.Text = "Izaberi validan folder za auto export.";
                _autoExportInfoLabel.ForeColor = AppTheme.DangerColor;
                return;
            }

            _autoExportEnabled = true;
            _nextAutoExportAt = DateTime.Now.Add(GetSelectedInterval());
            _autoExportTimer.Start();

            _autoExportInfoLabel.Text = "Auto export je aktivan. Sledeci export: " + _nextAutoExportAt.ToString("dd.MM.yyyy HH:mm");
            _autoExportInfoLabel.ForeColor = AppTheme.SuccessColor;
            LoadData();
        }

        private void StopAutoExportButton_Click(object sender, EventArgs e)
        {
            _autoExportEnabled = false;
            _autoExportTimer.Stop();
            _autoExportInfoLabel.Text = "";
            _autoExportInfoLabel.ForeColor = MutedTextColor;
            LoadData();
        }

        private void AutoExportTimer_Tick(object sender, EventArgs e)
        {
            if (!_autoExportEnabled || DateTime.Now < _nextAutoExportAt)
                return;

            try
            {
                string folder = _autoExportFolderTextBox.Text.Trim();
                string stamp = DateTime.Now.ToString("yyyyMMdd-HHmm");
                string filePath = Path.Combine(folder, "auto-monthly-report-" + stamp + ".csv");
                ServiceResult result = CreateCommand("monthly").Execute(filePath);

                if (result.Success)
                {
                    _autoExportInfoLabel.Text = "Auto export uspesan. Sledeci export: " + DateTime.Now.Add(GetSelectedInterval()).ToString("dd.MM.yyyy HH:mm");
                    _autoExportInfoLabel.ForeColor = AppTheme.SuccessColor;
                }
                else
                {
                    _autoExportInfoLabel.Text = result.Message;
                    _autoExportInfoLabel.ForeColor = AppTheme.DangerColor;
                }
            }
            catch (Exception ex)
            {
                _autoExportInfoLabel.Text = "Greska auto exporta: " + ex.Message;
                _autoExportInfoLabel.ForeColor = AppTheme.DangerColor;
            }

            _nextAutoExportAt = DateTime.Now.Add(GetSelectedInterval());
            LoadData();
        }

        // Command factory bira koju export akciju forma pokrece.
        private IReportCommand CreateCommand(string reportType)
        {
            return _reportCommandFactory.Create(reportType, _monthlyReportPicker.Value);
        }

        private TimeSpan GetSelectedInterval()
        {
            switch (_autoExportIntervalComboBox.Text)
            {
                case "5h":
                    return TimeSpan.FromHours(5);
                case "1d":
                    return TimeSpan.FromDays(1);
                case "1w":
                    return TimeSpan.FromDays(7);
                case "1M":
                    return TimeSpan.FromDays(30);
                default:
                    return TimeSpan.FromHours(1);
            }
        }
    }
}
