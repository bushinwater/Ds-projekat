using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class MembershipsForm : SectionFormBase, IReloadableSection
    {
        private readonly MembershipFacade _membershipFacade;

        private TextBox _packageNameTextBox;
        private NumericUpDown _priceInput;
        private NumericUpDown _durationDaysInput;
        private NumericUpDown _maxHoursInput;
        private CheckBox _meetingRoomCheckBox;
        private NumericUpDown _meetingRoomHoursInput;
        private DataGridView _membershipGrid;
        private Label _statusLabel;

        private List<MembershipType> _membershipTypes;
        private int _selectedMembershipId;

        public MembershipsForm()
        {
            _membershipFacade = new MembershipFacade();
            _membershipTypes = new List<MembershipType>();

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Detalji clanarine", 20, 20, 420, 610);

            formGroup.Controls.Add(CreateLabel("Naziv paketa", 20, 40));
            _packageNameTextBox = CreateTextBox("txtPackageName", 180, 35, 200);
            formGroup.Controls.Add(_packageNameTextBox);

            formGroup.Controls.Add(CreateLabel("Cena", 20, 85));
            _priceInput = CreateNumericUpDown("numPackagePrice", 180, 80, 200, 0, 1000000);
            _priceInput.DecimalPlaces = 2;
            _priceInput.Increment = 1;
            formGroup.Controls.Add(_priceInput);

            formGroup.Controls.Add(CreateLabel("Trajanje u danima", 20, 130));
            _durationDaysInput = CreateNumericUpDown("numDurationDays", 180, 125, 200, 0, 3650);
            formGroup.Controls.Add(_durationDaysInput);

            formGroup.Controls.Add(CreateLabel("Maks. sati/mesec", 20, 175));
            _maxHoursInput = CreateNumericUpDown("numMaxHoursMonth", 180, 170, 200, 0, 1000);
            formGroup.Controls.Add(_maxHoursInput);

            formGroup.Controls.Add(CreateLabel("Pristup salama", 20, 220));
            _meetingRoomCheckBox = CreateCheckBox("chkMeetingRoomAccess", "", 180, 218);
            _meetingRoomCheckBox.CheckedChanged += MeetingRoomCheckBox_CheckedChanged;
            formGroup.Controls.Add(_meetingRoomCheckBox);

            formGroup.Controls.Add(CreateLabel("Sati za sale", 20, 265));
            _meetingRoomHoursInput = CreateNumericUpDown("numMeetingRoomHours", 180, 260, 200, 0, 1000);
            formGroup.Controls.Add(_meetingRoomHoursInput);

            Button addButton = CreateActionButton("Dodaj", 20, 345);
            addButton.Click += AddButton_Click;
            formGroup.Controls.Add(addButton);

            Button updateButton = CreateActionButton("Azuriraj", 140, 345);
            updateButton.Click += UpdateButton_Click;
            formGroup.Controls.Add(updateButton);

            Button deleteButton = CreateActionButton("Obrisi", 260, 345);
            deleteButton.Click += DeleteButton_Click;
            formGroup.Controls.Add(deleteButton);

            Button clearButton = CreateActionButton("Ocisti", 20, 395);
            clearButton.Click += ClearButton_Click;
            formGroup.Controls.Add(clearButton);

            Button refreshButton = CreateActionButton("Osvezi", 140, 395);
            refreshButton.Click += RefreshButton_Click;
            formGroup.Controls.Add(refreshButton);

            _statusLabel = new Label
            {
                Left = 20,
                Top = 455,
                Width = 360,
                Height = 100,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Pregled i izmena tipova clanarina iz baze."
            };
            formGroup.Controls.Add(_statusLabel);

            GroupBox listGroup = CreateGroupBox("Tipovi clanarina", 460, 20, 660, 610);
            _membershipGrid = CreateGrid("dgvMembershipTypes", 15, 30, 630, 560);
            _membershipGrid.SelectionChanged += MembershipGrid_SelectionChanged;
            listGroup.Controls.Add(_membershipGrid);

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }

        public void LoadData()
        {
            try
            {
                int selectedMembershipId = _selectedMembershipId;
                _membershipTypes = _membershipFacade.GetAll();

                _membershipGrid.DataSource = null;
                _membershipGrid.DataSource = _membershipTypes
                    .Select(m => new MembershipGridRow
                    {
                        MembershipTypeID = m.MembershipTypeID,
                        PackageName = m.PackageName,
                        Price = m.Price,
                        DurationDays = m.DurationDays,
                        MaxReservationHoursPerMonth = m.MaxReservationHoursPerMonth,
                        MeetingRoomAccess = m.MeetingRoomAccess,
                        MeetingRoomHoursPerMonth = m.MeetingRoomHoursPerMonth
                    })
                    .ToList();
                SetGridHeader(_membershipGrid, "MembershipTypeID", "ID");
                SetGridHeader(_membershipGrid, "PackageName", "Naziv paketa");
                SetGridHeader(_membershipGrid, "Price", "Cena");
                SetGridHeader(_membershipGrid, "DurationDays", "Trajanje");
                SetGridHeader(_membershipGrid, "MaxReservationHoursPerMonth", "Maks. sati");
                SetGridHeader(_membershipGrid, "MeetingRoomAccess", "Pristup salama");
                SetGridHeader(_membershipGrid, "MeetingRoomHoursPerMonth", "Sati za sale");

                if (selectedMembershipId > 0)
                    SelectMembership(selectedMembershipId);
                else
                    ClearEditor();

                SetStatus("Tipovi clanarina su ucitani iz baze.", true);
            }
            catch (Exception ex)
            {
                SetStatus("Greska pri ucitavanju clanarina: " + ex.Message, false);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            MembershipType membershipType = BuildMembershipFromForm();
            ServiceResult result = _membershipFacade.AddMembershipType(membershipType);
            HandleResult(result, true);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (_selectedMembershipId <= 0)
            {
                SetStatus("Izaberi tip clanarine za azuriranje.", false);
                return;
            }

            MembershipType membershipType = BuildMembershipFromForm();
            membershipType.MembershipTypeID = _selectedMembershipId;

            ServiceResult result = _membershipFacade.UpdateMembershipType(membershipType);
            HandleResult(result, false);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedMembershipId <= 0)
            {
                SetStatus("Izaberi tip clanarine za brisanje.", false);
                return;
            }

            DialogResult dialog = MessageBox.Show(
                "Da li zelis da obrises izabrani tip clanarine?",
                "Delete membership type",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog != DialogResult.Yes)
                return;

            ServiceResult result = _membershipFacade.DeleteMembershipType(_selectedMembershipId);
            HandleResult(result, true);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearEditor();
            SetStatus("Forma je ociscena.", true);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void MembershipGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_membershipGrid.CurrentRow == null)
                return;

            MembershipGridRow row = _membershipGrid.CurrentRow.DataBoundItem as MembershipGridRow;
            if (row == null)
                return;

            _selectedMembershipId = row.MembershipTypeID;
            _packageNameTextBox.Text = row.PackageName;
            _priceInput.Value = row.Price;
            _durationDaysInput.Value = row.DurationDays;
            _maxHoursInput.Value = row.MaxReservationHoursPerMonth;
            _meetingRoomCheckBox.Checked = row.MeetingRoomAccess;
            _meetingRoomHoursInput.Value = row.MeetingRoomHoursPerMonth.HasValue
                ? row.MeetingRoomHoursPerMonth.Value
                : 0;

            UpdateMeetingRoomHoursState();
        }

        private void MeetingRoomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateMeetingRoomHoursState();
        }

        private MembershipType BuildMembershipFromForm()
        {
            return new MembershipType
            {
                PackageName = _packageNameTextBox.Text.Trim(),
                Price = _priceInput.Value,
                DurationDays = Decimal.ToInt32(_durationDaysInput.Value),
                MaxReservationHoursPerMonth = Decimal.ToInt32(_maxHoursInput.Value),
                MeetingRoomAccess = _meetingRoomCheckBox.Checked,
                MeetingRoomHoursPerMonth = _meetingRoomCheckBox.Checked
                    ? (int?)Decimal.ToInt32(_meetingRoomHoursInput.Value)
                    : null
            };
        }

        private void UpdateMeetingRoomHoursState()
        {
            _meetingRoomHoursInput.Enabled = _meetingRoomCheckBox.Checked;
            if (!_meetingRoomCheckBox.Checked)
                _meetingRoomHoursInput.Value = 0;
        }

        private void HandleResult(ServiceResult result, bool clearAfterSuccess)
        {
            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            int selectedMembershipId = result.NewId > 0 ? result.NewId : _selectedMembershipId;
            LoadData();
            SetStatus(result.Message, result.Success);

            if (clearAfterSuccess)
                ClearEditor();
            else if (selectedMembershipId > 0)
                SelectMembership(selectedMembershipId);
        }

        private void SelectMembership(int membershipTypeId)
        {
            foreach (DataGridViewRow row in _membershipGrid.Rows)
            {
                MembershipGridRow data = row.DataBoundItem as MembershipGridRow;
                if (data == null || data.MembershipTypeID != membershipTypeId)
                    continue;

                row.Selected = true;
                _membershipGrid.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void ClearEditor()
        {
            _selectedMembershipId = 0;
            _packageNameTextBox.Clear();
            _priceInput.Value = 0;
            _durationDaysInput.Value = 0;
            _maxHoursInput.Value = 0;
            _meetingRoomCheckBox.Checked = false;
            _meetingRoomHoursInput.Value = 0;
            _membershipGrid.ClearSelection();
            _membershipGrid.CurrentCell = null;
            UpdateMeetingRoomHoursState();
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private sealed class MembershipGridRow
        {
            public int MembershipTypeID { get; set; }
            public string PackageName { get; set; } = "";
            public decimal Price { get; set; }
            public int DurationDays { get; set; }
            public int MaxReservationHoursPerMonth { get; set; }
            public bool MeetingRoomAccess { get; set; }
            public int? MeetingRoomHoursPerMonth { get; set; }
        }
    }
}
