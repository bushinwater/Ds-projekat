using Ds_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeResourcesModule()
        {
            if (dgvResources == null)
                return;

            dgvResources.AutoGenerateColumns = true;
            dgvResources.SelectionChanged += dgvResources_SelectionChanged;

            btnResourceAdd.Click += btnResourceAdd_Click;
            btnResourceUpdate.Click += btnResourceUpdate_Click;
            btnResourceDelete.Click += btnResourceDelete_Click;
            btnResourceClear.Click += btnResourceClear_Click;
            btnResourceRefresh.Click += btnResourceRefresh_Click;

            cbResourceType.SelectedIndexChanged += cbResourceType_SelectedIndexChanged;
            cbResourceListLocation.SelectedIndexChanged += (s, e) => ApplyResourceFilters();
            cbResourceListType.SelectedIndexChanged += (s, e) => ApplyResourceFilters();

            LoadResourceLocations();
            LoadResources();
            ClearResourceForm();
            UpdateResourceTypeUI();
        }

        private void LoadResourceLocations()
        {
            try
            {
                _allResourceLocations = _locationFacade.GetAll();

                cbResourceLocation.DataSource = null;
                cbResourceLocation.DisplayMember = "LocationName";
                cbResourceLocation.ValueMember = "LocationID";
                cbResourceLocation.DataSource = _allResourceLocations;
                cbResourceLocation.SelectedIndex = -1;

                cbResourceListLocation.DataSource = null;
                var locationFilterItems = new List<Location> { new Location { LocationID = 0, LocationName = "All" } };
                locationFilterItems.AddRange(_allResourceLocations);
                cbResourceListLocation.DisplayMember = "LocationName";
                cbResourceListLocation.ValueMember = "LocationID";
                cbResourceListLocation.DataSource = locationFilterItems;
                cbResourceListLocation.SelectedValue = 0;

                if (cbResourceListType.SelectedItem == null)
                    cbResourceListType.SelectedItem = "All";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju lokacija za resurse:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadResources()
        {
            try
            {
                _allResources = _resourceFacade.GetAllResources();
                _allResourceLocations = _locationFacade.GetAll();
                ApplyResourceFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju resursa:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyResourceFilters()
        {
            IEnumerable<Resource> query = _allResources;

            if (cbResourceListLocation.SelectedValue != null && Convert.ToInt32(cbResourceListLocation.SelectedValue) > 0)
            {
                int locationId = Convert.ToInt32(cbResourceListLocation.SelectedValue);
                query = query.Where(r => r.LocationID == locationId);
            }

            string selectedType = cbResourceListType.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(selectedType) && selectedType != "All")
                query = query.Where(r => string.Equals(r.ResourceType, selectedType, StringComparison.OrdinalIgnoreCase));

            var rows = query
                .OrderBy(r => r.ResourceType)
                .ThenBy(r => r.ResourceName)
                .Select(r =>
                {
                    var location = _allResourceLocations.FirstOrDefault(l => l.LocationID == r.LocationID);
                    return new ResourceGridRow
                    {
                        ResourceID = r.ResourceID,
                        ResourceTypeGroup = string.Equals(r.ResourceType, "Desk", StringComparison.OrdinalIgnoreCase) ? "Desk / Workstation" : r.ResourceType,
                        Resource = r.ResourceName + " (" + r.ResourceID + ")",
                        Location = location != null ? location.LocationName + " (" + location.LocationID + ")" : "N/A",
                        ResourceType = r.ResourceType,
                        IsActive = r.IsActive,
                        Description = r.Description
                    };
                }).ToList();

            dgvResources.DataSource = null;
            dgvResources.DataSource = rows;
            FormatResourcesGrid();
        }

        private void FormatResourcesGrid()
        {
            if (dgvResources.Columns.Count == 0) return;
            if (dgvResources.Columns.Contains("ResourceID")) dgvResources.Columns["ResourceID"].Visible = false;
            if (dgvResources.Columns.Contains("ResourceTypeGroup")) dgvResources.Columns["ResourceTypeGroup"].HeaderText = "Group";
            if (dgvResources.Columns.Contains("Resource")) dgvResources.Columns["Resource"].HeaderText = "Resource";
            if (dgvResources.Columns.Contains("Location")) dgvResources.Columns["Location"].HeaderText = "Location";
            if (dgvResources.Columns.Contains("ResourceType")) dgvResources.Columns["ResourceType"].HeaderText = "Type";
            if (dgvResources.Columns.Contains("IsActive")) dgvResources.Columns["IsActive"].HeaderText = "Active";
            if (dgvResources.Columns.Contains("Description")) dgvResources.Columns["Description"].HeaderText = "Description";
        }

        private void dgvResources_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResources.CurrentRow == null) return;
            if (dgvResources.CurrentRow.DataBoundItem is not ResourceGridRow row) return;
            Resource resource = _allResources.FirstOrDefault(x => x.ResourceID == row.ResourceID);
            if (resource != null) FillResourceForm(resource);
        }

        private void FillResourceForm(Resource resource)
        {
            txtResourceId.Text = resource.ResourceID.ToString();
            cbResourceLocation.SelectedValue = resource.LocationID;
            txtResourceName.Text = resource.ResourceName;
            cbResourceType.SelectedItem = resource.ResourceType;
            chkResourceIsActive.Checked = resource.IsActive;
            txtResourceDescription.Text = resource.Description ?? "";
            cbDeskSubtype.SelectedIndex = -1;
            txtRoomCapacity.Clear();
            chkProjector.Checked = chkTV.Checked = chkBoard.Checked = chkOnlineEquipment.Checked = false;

            if (string.Equals(resource.ResourceType, "Desk", StringComparison.OrdinalIgnoreCase))
            {
                var desk = _resourceFacade.GetDeskDetails(resource.ResourceID);
                if (desk != null) cbDeskSubtype.SelectedItem = desk.DeskSubType;
            }
            else if (string.Equals(resource.ResourceType, "Room", StringComparison.OrdinalIgnoreCase))
            {
                var room = _resourceFacade.GetRoomDetails(resource.ResourceID);
                if (room != null)
                {
                    txtRoomCapacity.Text = room.Capacity.ToString();
                    chkProjector.Checked = room.HasProjector;
                    chkTV.Checked = room.HasTV;
                    chkBoard.Checked = room.HasBoard;
                    chkOnlineEquipment.Checked = room.HasOnlineEquipment;
                }
            }

            UpdateResourceTypeUI();
        }

        private Resource ReadResourceBaseFromForm()
        {
            if (cbResourceLocation.SelectedValue == null) throw new Exception("Location je obavezna.");
            if (string.IsNullOrWhiteSpace(txtResourceName.Text)) throw new Exception("Resource Name je obavezan.");
            if (cbResourceType.SelectedItem == null) throw new Exception("Resource Type je obavezan.");

            Resource resource = new Resource
            {
                LocationID = Convert.ToInt32(cbResourceLocation.SelectedValue),
                ResourceName = txtResourceName.Text.Trim(),
                ResourceType = cbResourceType.SelectedItem.ToString(),
                IsActive = chkResourceIsActive.Checked,
                Description = string.IsNullOrWhiteSpace(txtResourceDescription.Text) ? null : txtResourceDescription.Text.Trim()
            };

            if (int.TryParse(txtResourceId.Text, out int resourceId)) resource.ResourceID = resourceId;
            return resource;
        }

        private DeskDetails ReadDeskDetailsFromForm(int resourceId = 0)
        {
            if (cbDeskSubtype.SelectedItem == null) throw new Exception("Desk Subtype je obavezan za Desk resurs.");
            return new DeskDetails { ResourceID = resourceId, DeskSubType = cbDeskSubtype.SelectedItem.ToString() };
        }

        private RoomDetails ReadRoomDetailsFromForm(int resourceId = 0)
        {
            if (!int.TryParse(txtRoomCapacity.Text.Trim(), out int capacity)) throw new Exception("Room Capacity mora biti ceo broj.");
            return new RoomDetails
            {
                ResourceID = resourceId,
                Capacity = capacity,
                HasProjector = chkProjector.Checked,
                HasTV = chkTV.Checked,
                HasBoard = chkBoard.Checked,
                HasOnlineEquipment = chkOnlineEquipment.Checked
            };
        }

        private void ClearResourceForm()
        {
            txtResourceId.Clear();
            cbResourceLocation.SelectedIndex = -1;
            txtResourceName.Clear();
            cbResourceType.SelectedIndex = -1;
            chkResourceIsActive.Checked = false;
            txtResourceDescription.Clear();
            cbDeskSubtype.SelectedIndex = -1;
            txtRoomCapacity.Clear();
            chkProjector.Checked = chkTV.Checked = chkBoard.Checked = chkOnlineEquipment.Checked = false;
            UpdateResourceTypeUI();
            dgvResources.ClearSelection();
        }

        private void UpdateResourceTypeUI()
        {
            bool isDesk = cbResourceType.SelectedItem?.ToString() == "Desk";
            bool isRoom = cbResourceType.SelectedItem?.ToString() == "Room";
            cbDeskSubtype.Enabled = isDesk;
            txtRoomCapacity.Enabled = chkProjector.Enabled = chkTV.Enabled = chkBoard.Enabled = chkOnlineEquipment.Enabled = isRoom;
            if (!isDesk) cbDeskSubtype.SelectedIndex = -1;
            if (!isRoom)
            {
                txtRoomCapacity.Clear();
                chkProjector.Checked = chkTV.Checked = chkBoard.Checked = chkOnlineEquipment.Checked = false;
            }
        }

        private void cbResourceType_SelectedIndexChanged(object sender, EventArgs e) => UpdateResourceTypeUI();

        private void btnResourceAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Resource resource = ReadResourceBaseFromForm();
                resource.ResourceID = 0;
                if (resource.ResourceType == "Desk") _resourceFacade.AddDesk(resource, ReadDeskDetailsFromForm());
                else if (resource.ResourceType == "Room") _resourceFacade.AddRoom(resource, ReadRoomDetailsFromForm());
                else throw new Exception("Nepoznat tip resursa.");

                MessageBox.Show("Resurs je uspešno dodat.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadResources();
                ClearResourceForm();
                NotifyObservers("Resources", "Add", "Dodat je novi resurs: " + resource.ResourceName + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri dodavanju resursa:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResourceUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtResourceId.Text, out int resourceId)) throw new Exception("Odaberite resurs za izmenu.");
                Resource resource = ReadResourceBaseFromForm();
                if (resource.ResourceType == "Desk") _resourceFacade.UpdateDesk(resource, ReadDeskDetailsFromForm(resourceId));
                else if (resource.ResourceType == "Room") _resourceFacade.UpdateRoom(resource, ReadRoomDetailsFromForm(resourceId));
                else throw new Exception("Nepoznat tip resursa.");

                MessageBox.Show("Resurs je uspešno izmenjen.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadResources();
                ClearResourceForm();
                NotifyObservers("Resources", "Update", "Izmenjen je resurs ID=" + resource.ResourceID + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri izmeni resursa:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResourceDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtResourceId.Text, out int resourceId)) throw new Exception("Odaberite resurs za brisanje.");
                if (MessageBox.Show("Da li sigurno želite da obrišete resurs?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                bool hasReservations = _resourceFacade.ResourceHasReservations(resourceId);
                if (hasReservations)
                {
                    DialogResult result = MessageBox.Show("Ovaj resurs ima rezervacije. Da li želiš da obrišeš i te rezervacije?", "Potvrda brisanja", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel) return;
                    if (result == DialogResult.No) { MessageBox.Show("Resurs nije obrisan."); return; }
                    ServiceResult srWithReservations = _resourceFacade.DeleteResourceWithCheck(resourceId, true);
                    MessageBox.Show(srWithReservations.Message);
                    if (srWithReservations.Success)
                    {
                        LoadResources();
                        ClearResourceForm();
                        NotifyObservers("Resources", "Delete", "Obrisan je resurs ID=" + resourceId + " zajedno sa rezervacijama.");
                    }
                    return;
                }

                ServiceResult sr = _resourceFacade.DeleteResourceWithCheck(resourceId, false);
                MessageBox.Show(sr.Message);
                if (sr.Success)
                {
                    LoadResources();
                    ClearResourceForm();
                    NotifyObservers("Resources", "Delete", "Obrisan je resurs ID=" + resourceId + ".");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri brisanju resursa:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResourceClear_Click(object sender, EventArgs e) => ClearResourceForm();

        private void btnResourceRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadResourceLocations();
                LoadResources();
                ClearResourceForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri osvežavanju resursa:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
