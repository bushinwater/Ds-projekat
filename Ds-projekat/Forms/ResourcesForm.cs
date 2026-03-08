using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class ResourcesForm : SectionFormBase, IReloadableSection
    {
        private readonly ResourceFacade _resourceFacade;
        private readonly LocationFacade _locationFacade;

        private ComboBox _locationComboBox;
        private TextBox _resourceNameTextBox;
        private ComboBox _resourceTypeComboBox;
        private CheckBox _activeCheckBox;
        private TextBox _descriptionTextBox;
        private ComboBox _deskSubtypeComboBox;
        private NumericUpDown _roomCapacityInput;
        private CheckBox _projectorCheckBox;
        private CheckBox _tvCheckBox;
        private CheckBox _boardCheckBox;
        private CheckBox _onlineCheckBox;
        private ComboBox _listLocationFilterComboBox;
        private ComboBox _listTypeFilterComboBox;
        private DataGridView _resourcesGrid;
        private Label _statusLabel;

        private List<Location> _locations;
        private List<Resource> _resources;
        private int _selectedResourceId;

        public ResourcesForm()
        {
            _resourceFacade = new ResourceFacade();
            _locationFacade = new LocationFacade();
            _locations = new List<Location>();
            _resources = new List<Resource>();
            ActiveLocationContext.Instance.ActiveLocationChanged += ActiveLocationContext_ActiveLocationChanged;

            BuildContent();
            LoadData();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Detalji resursa", 20, 20, 460, 610);

            formGroup.Controls.Add(CreateLabel("Lokacija", 20, 40));
            _locationComboBox = CreateComboBox("cbResourceLocation", 190, 35, 220);
            formGroup.Controls.Add(_locationComboBox);

            formGroup.Controls.Add(CreateLabel("Naziv resursa", 20, 85));
            _resourceNameTextBox = CreateTextBox("txtResourceName", 190, 80, 220);
            formGroup.Controls.Add(_resourceNameTextBox);

            formGroup.Controls.Add(CreateLabel("Tip resursa", 20, 130));
            _resourceTypeComboBox = CreateComboBox("cbResourceType", 190, 125, 220);
            _resourceTypeComboBox.Items.AddRange(new object[] { "Desk", "Room", "Private Office" });
            _resourceTypeComboBox.SelectedIndexChanged += ResourceTypeComboBox_SelectedIndexChanged;
            formGroup.Controls.Add(_resourceTypeComboBox);

            formGroup.Controls.Add(CreateLabel("Aktivan", 20, 175));
            _activeCheckBox = CreateCheckBox("chkResourceIsActive", "", 190, 173);
            _activeCheckBox.Checked = true;
            formGroup.Controls.Add(_activeCheckBox);

            formGroup.Controls.Add(CreateLabel("Opis", 20, 220));
            _descriptionTextBox = CreateTextBox("txtResourceDescription", 190, 215, 220);
            _descriptionTextBox.Multiline = true;
            _descriptionTextBox.Height = 60;
            formGroup.Controls.Add(_descriptionTextBox);

            formGroup.Controls.Add(CreateLabel("Pod-tip stola", 20, 300));
            _deskSubtypeComboBox = CreateComboBox("cbDeskSubtype", 190, 295, 220);
            _deskSubtypeComboBox.Items.AddRange(new object[] { "Hot", "Dedicated" });
            formGroup.Controls.Add(_deskSubtypeComboBox);

            formGroup.Controls.Add(CreateLabel("Kapacitet sale", 20, 345));
            _roomCapacityInput = CreateNumericUpDown("numRoomCapacity", 190, 340, 220, 0, 1000);
            formGroup.Controls.Add(_roomCapacityInput);

            _projectorCheckBox = CreateCheckBox("chkProjector", "Projektor", 190, 385);
            formGroup.Controls.Add(_projectorCheckBox);

            _tvCheckBox = CreateCheckBox("chkTV", "TV", 300, 385);
            formGroup.Controls.Add(_tvCheckBox);

            _boardCheckBox = CreateCheckBox("chkBoard", "Tabla", 190, 415);
            formGroup.Controls.Add(_boardCheckBox);

            _onlineCheckBox = CreateCheckBox("chkOnlineEquipment", "Online oprema", 300, 415);
            formGroup.Controls.Add(_onlineCheckBox);

            Button addButton = CreateActionButton("Dodaj", 20, 480);
            addButton.Click += AddButton_Click;
            formGroup.Controls.Add(addButton);

            Button updateButton = CreateActionButton("Azuriraj", 140, 480);
            updateButton.Click += UpdateButton_Click;
            formGroup.Controls.Add(updateButton);

            Button deleteButton = CreateActionButton("Obrisi", 260, 480);
            deleteButton.Click += DeleteButton_Click;
            formGroup.Controls.Add(deleteButton);

            Button refreshButton = CreateActionButton("Osvezi", 20, 530);
            refreshButton.Click += RefreshButton_Click;
            formGroup.Controls.Add(refreshButton);

            Button clearButton = CreateActionButton("Ocisti", 140, 530);
            clearButton.Click += ClearButton_Click;
            formGroup.Controls.Add(clearButton);

            _statusLabel = new Label
            {
                Left = 20,
                Top = 570,
                Width = 410,
                Height = 32,
                ForeColor = MutedTextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Resursi se upisuju direktno u bazu."
            };
            formGroup.Controls.Add(_statusLabel);

            GroupBox listGroup = CreateGroupBox("Lista resursa", 500, 20, 620, 610);
            listGroup.Controls.Add(CreateLabel("Lokacija", 15, 40));
            _listLocationFilterComboBox = CreateComboBox("cbResourceListLocation", 80, 35, 180);
            _listLocationFilterComboBox.SelectedIndexChanged += ResourceListFilterChanged;
            listGroup.Controls.Add(_listLocationFilterComboBox);

            listGroup.Controls.Add(CreateLabel("Tip", 280, 40));
            _listTypeFilterComboBox = CreateComboBox("cbResourceListType", 330, 35, 140);
            _listTypeFilterComboBox.Items.AddRange(new object[] { "Desk", "Room", "Private Office" });
            _listTypeFilterComboBox.SelectedIndexChanged += ResourceListFilterChanged;
            listGroup.Controls.Add(_listTypeFilterComboBox);

            Button clearFiltersButton = CreateActionButton("Ocisti filtere", 485, 34, 110);
            clearFiltersButton.Click += ClearFiltersButton_Click;
            listGroup.Controls.Add(clearFiltersButton);

            _resourcesGrid = CreateGrid("dgvResources", 15, 90, 590, 500);
            _resourcesGrid.SelectionChanged += ResourcesGrid_SelectionChanged;
            listGroup.Controls.Add(_resourcesGrid);

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }

        public void LoadData()
        {
            try
            {
                int selectedResourceId = _selectedResourceId;
                _locations = _locationFacade.GetAll();
                _resources = _resourceFacade.GetAllResources();

                BindLocations();
                BindListLocationFilters();
                ApplyResourceFilters();
                UpdateResourceDetailsVisibility();

                if (selectedResourceId > 0)
                    SelectResource(selectedResourceId);
                else
                    ClearEditor();

                SetStatus("Resursi su ucitani iz baze.", true);
            }
            catch (Exception ex)
            {
                SetStatus("Greska pri ucitavanju resursa: " + ex.Message, false);
            }
        }

        private void BindLocations()
        {
            _locationComboBox.DataSource = null;
            _locationComboBox.DisplayMember = "LocationName";
            _locationComboBox.ValueMember = "LocationID";
            _locationComboBox.DataSource = _locations.ToList();
            _locationComboBox.SelectedIndex = -1;
        }

        private void BindResources()
        {
            Dictionary<int, string> locationNames = _locations.ToDictionary(l => l.LocationID, l => l.LocationName);

            List<ResourceGridRow> rows = _resources.Select(r =>
            {
                DeskDetails desk = string.Equals(r.ResourceType, "Desk", StringComparison.OrdinalIgnoreCase)
                    ? _resourceFacade.GetDeskDetails(r.ResourceID)
                    : null;

                RoomDetails room = string.Equals(r.ResourceType, "Room", StringComparison.OrdinalIgnoreCase)
                    ? _resourceFacade.GetRoomDetails(r.ResourceID)
                    : null;

                return new ResourceGridRow
                {
                    ResourceID = r.ResourceID,
                    LocationID = r.LocationID,
                    Location = locationNames.ContainsKey(r.LocationID) ? locationNames[r.LocationID] : "",
                    ResourceName = r.ResourceName,
                    ResourceType = r.ResourceType,
                    IsActive = r.IsActive,
                    Description = r.Description ?? "",
                    DeskSubtype = desk == null ? "" : desk.DeskSubType,
                    RoomCapacity = room == null ? 0 : room.Capacity,
                    HasProjector = room != null && room.HasProjector,
                    HasTV = room != null && room.HasTV,
                    HasBoard = room != null && room.HasBoard,
                    HasOnlineEquipment = room != null && room.HasOnlineEquipment
                };
            }).ToList();

            _resourcesGrid.DataSource = null;
            _resourcesGrid.DataSource = rows;
            SetGridHeader(_resourcesGrid, "ResourceID", "ID");
            SetGridHeader(_resourcesGrid, "Location", "Lokacija");
            SetGridHeader(_resourcesGrid, "ResourceName", "Naziv");
            SetGridHeader(_resourcesGrid, "ResourceType", "Tip");
            SetGridHeader(_resourcesGrid, "IsActive", "Aktivan");
            SetGridHeader(_resourcesGrid, "Description", "Opis");
            SetGridHeader(_resourcesGrid, "DeskSubtype", "Pod-tip stola");
            SetGridHeader(_resourcesGrid, "RoomCapacity", "Kapacitet");
            SetGridHeader(_resourcesGrid, "HasProjector", "Projektor");
            SetGridHeader(_resourcesGrid, "HasTV", "TV");
            SetGridHeader(_resourcesGrid, "HasBoard", "Tabla");
            SetGridHeader(_resourcesGrid, "HasOnlineEquipment", "Online oprema");
        }

        private void BindListLocationFilters()
        {
            _listLocationFilterComboBox.DataSource = null;
            _listLocationFilterComboBox.DisplayMember = "LocationName";
            _listLocationFilterComboBox.ValueMember = "LocationID";
            _listLocationFilterComboBox.DataSource = _locations.ToList();

            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
                _listLocationFilterComboBox.SelectedValue = ActiveLocationContext.Instance.ActiveLocationId;
            else
                _listLocationFilterComboBox.SelectedIndex = -1;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ServiceResult result = SaveResource(isUpdate: false);
            HandleResult(result, true);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (_selectedResourceId <= 0)
            {
                SetStatus("Izaberi resurs za azuriranje.", false);
                return;
            }

            ServiceResult result = SaveResource(isUpdate: true);
            HandleResult(result, false);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedResourceId <= 0)
            {
                SetStatus("Izaberi resurs za brisanje.", false);
                return;
            }

            DialogResult dialog = MessageBox.Show(
                "Da li zelis da obrises izabrani resurs?",
                "Delete resource",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog != DialogResult.Yes)
                return;

            ServiceResult result = _resourceFacade.DeleteResource(_selectedResourceId);
            HandleResult(result, true);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearEditor();
            SetStatus("Forma je ociscena.", true);
        }

        private void ResourceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResourceDetailsVisibility();
        }

        private void ResourceListFilterChanged(object sender, EventArgs e)
        {
            ApplyResourceFilters();
        }

        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            _listLocationFilterComboBox.SelectedIndex = -1;
            _listTypeFilterComboBox.SelectedIndex = -1;
            ApplyResourceFilters();
            SetStatus("Filteri resursa su ocisceni.", true);
        }

        private void ResourcesGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_resourcesGrid.CurrentRow == null)
                return;

            ResourceGridRow row = _resourcesGrid.CurrentRow.DataBoundItem as ResourceGridRow;
            if (row == null)
                return;

            _selectedResourceId = row.ResourceID;
            _resourceNameTextBox.Text = row.ResourceName;
            _resourceTypeComboBox.Text = row.ResourceType;
            _activeCheckBox.Checked = row.IsActive;
            _descriptionTextBox.Text = row.Description;
            _deskSubtypeComboBox.Text = row.DeskSubtype;
            _roomCapacityInput.Value = row.RoomCapacity > 0 ? row.RoomCapacity : 0;
            _projectorCheckBox.Checked = row.HasProjector;
            _tvCheckBox.Checked = row.HasTV;
            _boardCheckBox.Checked = row.HasBoard;
            _onlineCheckBox.Checked = row.HasOnlineEquipment;

            if (_locationComboBox.Items.Count > 0)
                _locationComboBox.SelectedValue = row.LocationID;

            UpdateResourceDetailsVisibility();
        }

        private ServiceResult SaveResource(bool isUpdate)
        {
            Resource resource = new Resource
            {
                ResourceID = _selectedResourceId,
                LocationID = _locationComboBox.SelectedValue == null ? 0 : Convert.ToInt32(_locationComboBox.SelectedValue),
                ResourceName = _resourceNameTextBox.Text.Trim(),
                ResourceType = _resourceTypeComboBox.Text,
                IsActive = _activeCheckBox.Checked,
                Description = _descriptionTextBox.Text.Trim()
            };

            if (string.Equals(_resourceTypeComboBox.Text, "Desk", StringComparison.OrdinalIgnoreCase))
            {
                // Desk koristi svoju detail tabelu.
                DeskDetails desk = new DeskDetails
                {
                    ResourceID = _selectedResourceId,
                    DeskSubType = _deskSubtypeComboBox.Text
                };

                return isUpdate
                    ? _resourceFacade.UpdateDesk(resource, desk)
                    : _resourceFacade.AddDesk(resource, desk);
            }

            if (string.Equals(_resourceTypeComboBox.Text, "Private Office", StringComparison.OrdinalIgnoreCase))
            {
                // Private office je ovde najjednostavniji poseban tip bez dodatnih detalja.
                return isUpdate
                    ? _resourceFacade.UpdatePrivateOffice(resource)
                    : _resourceFacade.AddPrivateOffice(resource);
            }

            RoomDetails room = new RoomDetails
            {
                ResourceID = _selectedResourceId,
                Capacity = Decimal.ToInt32(_roomCapacityInput.Value),
                HasProjector = _projectorCheckBox.Checked,
                HasTV = _tvCheckBox.Checked,
                HasBoard = _boardCheckBox.Checked,
                HasOnlineEquipment = _onlineCheckBox.Checked
            };

            return isUpdate
                ? _resourceFacade.UpdateRoom(resource, room)
                : _resourceFacade.AddRoom(resource, room);
        }

        private void UpdateResourceDetailsVisibility()
        {
            bool isDesk = string.Equals(_resourceTypeComboBox.Text, "Desk", StringComparison.OrdinalIgnoreCase);
            bool isRoom = string.Equals(_resourceTypeComboBox.Text, "Room", StringComparison.OrdinalIgnoreCase);

            _deskSubtypeComboBox.Enabled = isDesk;
            _roomCapacityInput.Enabled = isRoom;
            _projectorCheckBox.Enabled = isRoom;
            _tvCheckBox.Enabled = isRoom;
            _boardCheckBox.Enabled = isRoom;
            _onlineCheckBox.Enabled = isRoom;
        }

        private void HandleResult(ServiceResult result, bool clearAfterSuccess)
        {
            SetStatus(result.Message, result.Success);
            if (!result.Success)
                return;

            int selectedResourceId = result.NewId > 0 ? result.NewId : _selectedResourceId;
            LoadData();
            SetStatus(result.Message, result.Success);

            if (clearAfterSuccess)
                ClearEditor();
            else if (selectedResourceId > 0)
                SelectResource(selectedResourceId);
        }

        private void SelectResource(int resourceId)
        {
            foreach (DataGridViewRow row in _resourcesGrid.Rows)
            {
                ResourceGridRow data = row.DataBoundItem as ResourceGridRow;
                if (data == null || data.ResourceID != resourceId)
                    continue;

                row.Selected = true;
                _resourcesGrid.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void ClearEditor()
        {
            _selectedResourceId = 0;
            _locationComboBox.SelectedIndex = -1;
            _resourceNameTextBox.Clear();
            _resourceTypeComboBox.SelectedIndex = -1;
            _activeCheckBox.Checked = true;
            _descriptionTextBox.Clear();
            _deskSubtypeComboBox.SelectedIndex = -1;
            _roomCapacityInput.Value = 0;
            _projectorCheckBox.Checked = false;
            _tvCheckBox.Checked = false;
            _boardCheckBox.Checked = false;
            _onlineCheckBox.Checked = false;
            _resourcesGrid.ClearSelection();
            _resourcesGrid.CurrentCell = null;
            UpdateResourceDetailsVisibility();
        }

        private void ApplyResourceFilters()
        {
            BindResources();

            List<ResourceGridRow> rows = (_resourcesGrid.DataSource as List<ResourceGridRow>) ?? new List<ResourceGridRow>();

            if (_listLocationFilterComboBox.SelectedValue != null)
            {
                int locationId = Convert.ToInt32(_listLocationFilterComboBox.SelectedValue);
                rows = rows.Where(r => r.LocationID == locationId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(_listTypeFilterComboBox.Text))
            {
                rows = rows.Where(r => string.Equals(r.ResourceType, _listTypeFilterComboBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            _resourcesGrid.DataSource = null;
            _resourcesGrid.DataSource = rows
                .OrderBy(r => r.ResourceType)
                .ThenBy(r => r.ResourceName)
                .ToList();
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private void ActiveLocationContext_ActiveLocationChanged(object sender, EventArgs e)
        {
            if (_listLocationFilterComboBox.DataSource == null)
                return;

            if (ActiveLocationContext.Instance.ActiveLocationId > 0)
                _listLocationFilterComboBox.SelectedValue = ActiveLocationContext.Instance.ActiveLocationId;
            else
                _listLocationFilterComboBox.SelectedIndex = -1;

            ApplyResourceFilters();
        }

        private sealed class ResourceGridRow
        {
            public int ResourceID { get; set; }
            public int LocationID { get; set; }
            public string Location { get; set; } = "";
            public string ResourceName { get; set; } = "";
            public string ResourceType { get; set; } = "";
            public bool IsActive { get; set; }
            public string Description { get; set; } = "";
            public string DeskSubtype { get; set; } = "";
            public int RoomCapacity { get; set; }
            public bool HasProjector { get; set; }
            public bool HasTV { get; set; }
            public bool HasBoard { get; set; }
            public bool HasOnlineEquipment { get; set; }
        }
    }
}
