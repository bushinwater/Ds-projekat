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
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju lokacija za resurse:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadResources()
        {
            try
            {
                _allResources = _resourceFacade.GetAllResources();

                dgvResources.DataSource = null;
                dgvResources.DataSource = _allResources;

                FormatResourcesGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju resursa:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void FormatResourcesGrid()
        {
            if (dgvResources.Columns.Count == 0) return;

            if (dgvResources.Columns.Contains("ResourceID"))
                dgvResources.Columns["ResourceID"].HeaderText = "ID";

            if (dgvResources.Columns.Contains("LocationID"))
                dgvResources.Columns["LocationID"].HeaderText = "Location ID";

            if (dgvResources.Columns.Contains("ResourceName"))
                dgvResources.Columns["ResourceName"].HeaderText = "Resource Name";

            if (dgvResources.Columns.Contains("ResourceType"))
                dgvResources.Columns["ResourceType"].HeaderText = "Type";

            if (dgvResources.Columns.Contains("IsActive"))
                dgvResources.Columns["IsActive"].HeaderText = "Active";

            if (dgvResources.Columns.Contains("Description"))
                dgvResources.Columns["Description"].HeaderText = "Description";
        }

        private void dgvResources_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResources.CurrentRow == null) return;
            if (dgvResources.CurrentRow.DataBoundItem is not Resource resource) return;

            FillResourceForm(resource);
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
            chkProjector.Checked = false;
            chkTV.Checked = false;
            chkBoard.Checked = false;
            chkOnlineEquipment.Checked = false;

            if (string.Equals(resource.ResourceType, "Desk", StringComparison.OrdinalIgnoreCase))
            {
                var desk = _resourceFacade.GetDeskDetails(resource.ResourceID);
                if (desk != null)
                    cbDeskSubtype.SelectedItem = desk.DeskSubType;
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
            if (cbResourceLocation.SelectedValue == null)
                throw new Exception("Location je obavezna.");

            if (string.IsNullOrWhiteSpace(txtResourceName.Text))
                throw new Exception("Resource Name je obavezan.");

            if (cbResourceType.SelectedItem == null)
                throw new Exception("Resource Type je obavezan.");

            Resource resource = new Resource
            {
                LocationID = Convert.ToInt32(cbResourceLocation.SelectedValue),
                ResourceName = txtResourceName.Text.Trim(),
                ResourceType = cbResourceType.SelectedItem.ToString(),
                IsActive = chkResourceIsActive.Checked,
                Description = string.IsNullOrWhiteSpace(txtResourceDescription.Text)
                    ? null
                    : txtResourceDescription.Text.Trim()
            };

            if (int.TryParse(txtResourceId.Text, out int resourceId))
                resource.ResourceID = resourceId;

            return resource;
        }

        private DeskDetails ReadDeskDetailsFromForm(int resourceId = 0)
        {
            if (cbDeskSubtype.SelectedItem == null)
                throw new Exception("Desk Subtype je obavezan za Desk resurs.");

            return new DeskDetails
            {
                ResourceID = resourceId,
                DeskSubType = cbDeskSubtype.SelectedItem.ToString()
            };
        }

        private RoomDetails ReadRoomDetailsFromForm(int resourceId = 0)
        {
            if (!int.TryParse(txtRoomCapacity.Text.Trim(), out int capacity))
                throw new Exception("Room Capacity mora biti ceo broj.");

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
            chkProjector.Checked = false;
            chkTV.Checked = false;
            chkBoard.Checked = false;
            chkOnlineEquipment.Checked = false;

            UpdateResourceTypeUI();
            dgvResources.ClearSelection();
        }

        private void UpdateResourceTypeUI()
        {
            bool isDesk = cbResourceType.SelectedItem?.ToString() == "Desk";
            bool isRoom = cbResourceType.SelectedItem?.ToString() == "Room";

            cbDeskSubtype.Enabled = isDesk;

            txtRoomCapacity.Enabled = isRoom;
            chkProjector.Enabled = isRoom;
            chkTV.Enabled = isRoom;
            chkBoard.Enabled = isRoom;
            chkOnlineEquipment.Enabled = isRoom;

            if (!isDesk)
                cbDeskSubtype.SelectedIndex = -1;

            if (!isRoom)
            {
                txtRoomCapacity.Clear();
                chkProjector.Checked = false;
                chkTV.Checked = false;
                chkBoard.Checked = false;
                chkOnlineEquipment.Checked = false;
            }
        }

        private void cbResourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResourceTypeUI();
        }

        private void btnResourceAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Resource resource = ReadResourceBaseFromForm();
                resource.ResourceID = 0;

                if (resource.ResourceType == "Desk")
                {
                    DeskDetails desk = ReadDeskDetailsFromForm();
                    _resourceFacade.AddDesk(resource, desk);
                }
                else if (resource.ResourceType == "Room")
                {
                    RoomDetails room = ReadRoomDetailsFromForm();
                    _resourceFacade.AddRoom(resource, room);
                }
                else
                {
                    throw new Exception("Nepoznat tip resursa.");
                }

                MessageBox.Show(
                    "Resurs je uspešno dodat.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadResources();
                ClearResourceForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri dodavanju resursa:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnResourceUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtResourceId.Text, out int resourceId))
                    throw new Exception("Odaberite resurs za izmenu.");

                Resource resource = ReadResourceBaseFromForm();

                if (resource.ResourceType == "Desk")
                {
                    DeskDetails desk = ReadDeskDetailsFromForm(resourceId);
                    _resourceFacade.UpdateDesk(resource, desk);
                }
                else if (resource.ResourceType == "Room")
                {
                    RoomDetails room = ReadRoomDetailsFromForm(resourceId);
                    ServiceResult result = _resourceFacade.UpdateRoom(resource, room);
                    if (!result.Success)
                        throw new Exception(result.Message);
                }
                else
                {
                    throw new Exception("Nepoznat tip resursa.");
                }

                MessageBox.Show(
                    "Resurs je uspešno izmenjen.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadResources();
                ClearResourceForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri izmeni resursa:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnResourceDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtResourceId.Text, out int resourceId))
                    throw new Exception("Odaberite resurs za brisanje.");

                DialogResult result = MessageBox.Show(
                    "Da li sigurno želite da obrišete resurs?",
                    "Potvrda brisanja",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;

                _resourceFacade.DeleteResource(resourceId);

                MessageBox.Show(
                    "Resurs je uspešno obrisan.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadResources();
                ClearResourceForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri brisanju resursa:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnResourceClear_Click(object sender, EventArgs e)
        {
            ClearResourceForm();
        }

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
                MessageBox.Show(
                    "Greška pri osvežavanju resursa:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}