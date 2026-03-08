using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal sealed class ReservationDialogForm : Form
    {
        private readonly ReservationFacade _reservationFacade;
        private readonly UserFacade _userFacade;
        private readonly ResourceFacade _resourceFacade;

        private readonly int? _reservationId;

        private ComboBox _userComboBox;
        private ComboBox _resourceComboBox;
        private NumericUpDown _usersCountInput;
        private DateTimePicker _startPicker;
        private DateTimePicker _endPicker;
        private ComboBox _statusComboBox;
        private Label _statusLabel;
        private Button _saveButton;
        private Button _cancelReservationButton;
        private Button _finishButton;

        private List<User> _users;
        private List<Resource> _resources;

        public ReservationDialogForm(int? reservationId = null)
        {
            _reservationFacade = new ReservationFacade();
            _userFacade = new UserFacade();
            _resourceFacade = new ResourceFacade();
            _reservationId = reservationId;
            _users = new List<User>();
            _resources = new List<Resource>();

            BuildDialog();
            LoadLookupData();
            LoadReservationIfNeeded();
        }

        private void BuildDialog()
        {
            Text = _reservationId.HasValue ? "Izmena rezervacije" : "Nova rezervacija";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(500, 470);
            BackColor = AppTheme.AppBackgroundColor;
            ForeColor = AppTheme.TextColor;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            Label titleLabel = new Label
            {
                Left = 20,
                Top = 18,
                Width = 300,
                Height = 28,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = AppTheme.PrimaryColor,
                Text = _reservationId.HasValue ? "Dijalog za rezervaciju" : "Kreiranje rezervacije"
            };
            Controls.Add(titleLabel);

            Panel accentLine = new Panel
            {
                Left = 20,
                Top = 52,
                Width = 460,
                Height = 3,
                BackColor = AppTheme.AccentColor
            };
            Controls.Add(accentLine);

            Controls.Add(CreateLabel("Korisnik", 30, 85));
            _userComboBox = CreateComboBox("cbDialogUser", 180, 80, 270);
            Controls.Add(_userComboBox);

            Controls.Add(CreateLabel("Resurs", 30, 130));
            _resourceComboBox = CreateComboBox("cbDialogResource", 180, 125, 270);
            Controls.Add(_resourceComboBox);

            Controls.Add(CreateLabel("Broj korisnika", 30, 175));
            _usersCountInput = CreateNumericUpDown("numDialogUsersCount", 180, 170, 270, 0, 500);
            Controls.Add(_usersCountInput);

            Controls.Add(CreateLabel("Pocetak", 30, 220));
            _startPicker = CreateDateTimePicker("dtpDialogStart", 180, 215, 270);
            Controls.Add(_startPicker);

            Controls.Add(CreateLabel("Kraj", 30, 265));
            _endPicker = CreateDateTimePicker("dtpDialogEnd", 180, 260, 270);
            Controls.Add(_endPicker);

            Controls.Add(CreateLabel("Status", 30, 310));
            _statusComboBox = CreateComboBox("cbDialogStatus", 180, 305, 270);
            _statusComboBox.Items.AddRange(new object[] { "Active", "Finished", "Canceled" });
            Controls.Add(_statusComboBox);

            // Glavne akcije rezervacije su ovde, kroz modalni dijalog.
            _saveButton = CreateActionButton(_reservationId.HasValue ? "Sacuvaj izmene" : "Kreiraj", 30, 360, 130);
            _saveButton.Click += SaveButton_Click;
            Controls.Add(_saveButton);

            Button checkButton = CreateActionButton("Proveri", 175, 360, 90);
            checkButton.Click += CheckButton_Click;
            Controls.Add(checkButton);

            _cancelReservationButton = CreateActionButton("Otkazi rezervaciju", 280, 360, 170);
            _cancelReservationButton.Click += CancelReservationButton_Click;
            Controls.Add(_cancelReservationButton);

            _finishButton = CreateActionButton("Zavrsi", 30, 405, 130);
            _finishButton.Click += FinishButton_Click;
            Controls.Add(_finishButton);

            Button closeButton = CreateActionButton("Zatvori", 175, 405, 90);
            closeButton.Click += (s, e) => Close();
            Controls.Add(closeButton);

            _statusLabel = new Label
            {
                Left = 280,
                Top = 400,
                Width = 180,
                Height = 45,
                ForeColor = AppTheme.MutedTextColor,
                BackColor = Color.Transparent,
                Text = "Rezervacija se proverava pre cuvanja."
            };
            Controls.Add(_statusLabel);

            if (!_reservationId.HasValue)
            {
                _cancelReservationButton.Enabled = false;
                _finishButton.Enabled = false;
                _statusComboBox.Text = "Active";
            }
        }

        private void LoadLookupData()
        {
            _users = _userFacade.GetAll();
            _resources = _resourceFacade.GetAllResources();

            IEnumerable<Resource> resourcesForPicker = _resources;
            if (ActiveLocationContext.Instance.ActiveLocationId > 0 && !_reservationId.HasValue)
                resourcesForPicker = resourcesForPicker.Where(r => r.LocationID == ActiveLocationContext.Instance.ActiveLocationId);

            _userComboBox.DataSource = _users
                .Select(u => new UserOption
                {
                    UserID = u.UserID,
                    DisplayText = u.FirstName + " " + u.LastName + " (" + u.Email + ")"
                })
                .ToList();
            _userComboBox.DisplayMember = "DisplayText";
            _userComboBox.ValueMember = "UserID";
            _userComboBox.SelectedIndex = -1;

            _resourceComboBox.DataSource = resourcesForPicker
                .Select(r => new ResourceOption
                {
                    ResourceID = r.ResourceID,
                    DisplayText = r.ResourceName + " [" + r.ResourceType + "]"
                })
                .ToList();
            _resourceComboBox.DisplayMember = "DisplayText";
            _resourceComboBox.ValueMember = "ResourceID";
            _resourceComboBox.SelectedIndex = -1;

            _startPicker.Value = DateTime.Now;
            _endPicker.Value = DateTime.Now.AddHours(1);
            _statusComboBox.SelectedIndex = _statusComboBox.Items.Count > 0 ? 0 : -1;
        }

        private void LoadReservationIfNeeded()
        {
            if (!_reservationId.HasValue)
                return;

            Reservation reservation = _reservationFacade.GetReservationById(_reservationId.Value);
            if (reservation == null)
            {
                SetStatus("Rezervacija nije pronadjena.", false);
                return;
            }

            if (_resources.All(r => r.ResourceID != reservation.ResourceID))
            {
                Resource selectedResource = _resourceFacade.GetResource(reservation.ResourceID);
                if (selectedResource != null)
                    _resources.Add(selectedResource);
            }

            _resourceComboBox.DataSource = _resources
                .Select(r => new ResourceOption
                {
                    ResourceID = r.ResourceID,
                    DisplayText = r.ResourceName + " [" + r.ResourceType + "]"
                })
                .ToList();
            _resourceComboBox.DisplayMember = "DisplayText";
            _resourceComboBox.ValueMember = "ResourceID";

            _userComboBox.SelectedValue = reservation.UserID;
            _resourceComboBox.SelectedValue = reservation.ResourceID;
            _usersCountInput.Value = reservation.UsersCount.HasValue && reservation.UsersCount.Value > 0
                ? reservation.UsersCount.Value
                : 0;
            _startPicker.Value = reservation.StartDateTime;
            _endPicker.Value = reservation.EndDateTime;
            _statusComboBox.Text = reservation.ReservationStatus;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ServiceResult result = _reservationId.HasValue
                ? _reservationFacade.UpdateReservation(
                    _reservationId.Value,
                    GetSelectedUserId(),
                    GetSelectedResourceId(),
                    _startPicker.Value,
                    _endPicker.Value,
                    GetUsersCount(),
                    _statusComboBox.Text)
                : _reservationFacade.CreateReservation(
                    GetSelectedUserId(),
                    GetSelectedResourceId(),
                    _startPicker.Value,
                    _endPicker.Value,
                    GetUsersCount());

            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            ServiceResult result = _reservationFacade.CanReserve(
                GetSelectedUserId(),
                GetSelectedResourceId(),
                _startPicker.Value,
                _endPicker.Value,
                GetUsersCount());

            SetStatus(result.Message, result.Success);
        }

        private void CancelReservationButton_Click(object sender, EventArgs e)
        {
            if (!_reservationId.HasValue)
                return;

            ServiceResult result = _reservationFacade.CancelReservation(_reservationId.Value);
            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            if (!_reservationId.HasValue)
                return;

            ServiceResult result = _reservationFacade.FinishReservation(_reservationId.Value);
            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private int GetSelectedUserId()
        {
            return _userComboBox.SelectedValue == null ? 0 : Convert.ToInt32(_userComboBox.SelectedValue);
        }

        private int GetSelectedResourceId()
        {
            return _resourceComboBox.SelectedValue == null ? 0 : Convert.ToInt32(_resourceComboBox.SelectedValue);
        }

        private int? GetUsersCount()
        {
            return _usersCountInput.Value > 0 ? Decimal.ToInt32(_usersCountInput.Value) : (int?)null;
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private static Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Left = x,
                Top = y,
                AutoSize = true,
                ForeColor = AppTheme.TextColor,
                BackColor = Color.Transparent
            };
        }

        private static TextBox CreateTextBox(string name, int x, int y, int width)
        {
            return new TextBox
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = AppTheme.SurfaceColor,
                ForeColor = AppTheme.TextColor
            };
        }

        private static ComboBox CreateComboBox(string name, int x, int y, int width)
        {
            return new ComboBox
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppTheme.SurfaceColor,
                ForeColor = AppTheme.TextColor
            };
        }

        private static DateTimePicker CreateDateTimePicker(string name, int x, int y, int width)
        {
            return new DateTimePicker
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm"
            };
        }

        private static NumericUpDown CreateNumericUpDown(string name, int x, int y, int width, int minimum, int maximum)
        {
            return new NumericUpDown
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                Minimum = minimum,
                Maximum = maximum,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = AppTheme.SurfaceColor,
                ForeColor = AppTheme.TextColor
            };
        }

        private static Button CreateActionButton(string text, int x, int y, int width)
        {
            Button button = new Button
            {
                Text = text,
                Left = x,
                Top = y,
                Width = width,
                Height = 38,
                BackColor = AppTheme.PrimaryColor,
                ForeColor = AppTheme.SurfaceColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private sealed class UserOption
        {
            public int UserID { get; set; }
            public string DisplayText { get; set; } = "";
        }

        private sealed class ResourceOption
        {
            public int ResourceID { get; set; }
            public string DisplayText { get; set; } = "";
        }
    }
}
