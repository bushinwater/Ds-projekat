using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildResourcesPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Resource Details", 20, 20, 460, 610);

            gbForm.Controls.Add(CreateLabel("Resource ID", 20, 40));
            txtResourceId = CreateTextBox("txtResourceId", 190, 35, 220);
            txtResourceId.ReadOnly = true;
            gbForm.Controls.Add(txtResourceId);

            gbForm.Controls.Add(CreateLabel("Location", 20, 85));
            cbResourceLocation = CreateComboBox("cbResourceLocation", 190, 80, 220);
            gbForm.Controls.Add(cbResourceLocation);

            gbForm.Controls.Add(CreateLabel("Resource Name", 20, 130));
            txtResourceName = CreateTextBox("txtResourceName", 190, 125, 220);
            gbForm.Controls.Add(txtResourceName);

            gbForm.Controls.Add(CreateLabel("Resource Type", 20, 175));
            cbResourceType = CreateComboBox("cbResourceType", 190, 170, 220);
            cbResourceType.Items.AddRange(new object[] { "Desk", "Room" });
            gbForm.Controls.Add(cbResourceType);

            gbForm.Controls.Add(CreateLabel("Is Active", 20, 220));
            chkResourceIsActive = new CheckBox();
            chkResourceIsActive.Name = "chkResourceIsActive";
            chkResourceIsActive.Left = 190;
            chkResourceIsActive.Top = 218;
            gbForm.Controls.Add(chkResourceIsActive);

            gbForm.Controls.Add(CreateLabel("Description", 20, 265));
            txtResourceDescription = CreateTextBox("txtResourceDescription", 190, 260, 220);
            txtResourceDescription.Multiline = true;
            txtResourceDescription.Height = 60;
            gbForm.Controls.Add(txtResourceDescription);

            gbForm.Controls.Add(CreateLabel("Desk Subtype", 20, 340));
            cbDeskSubtype = CreateComboBox("cbDeskSubtype", 190, 335, 220);
            cbDeskSubtype.Items.AddRange(new object[] { "Hot", "Dedicated" });
            gbForm.Controls.Add(cbDeskSubtype);

            gbForm.Controls.Add(CreateLabel("Room Capacity", 20, 385));
            txtRoomCapacity = CreateTextBox("txtRoomCapacity", 190, 380, 220);
            gbForm.Controls.Add(txtRoomCapacity);

            chkProjector = new CheckBox();
            chkProjector.Name = "chkProjector";
            chkProjector.Text = "Projector";
            chkProjector.Left = 190;
            chkProjector.Top = 425;
            gbForm.Controls.Add(chkProjector);

            chkTV = new CheckBox();
            chkTV.Name = "chkTV";
            chkTV.Text = "TV";
            chkTV.Left = 300;
            chkTV.Top = 425;
            gbForm.Controls.Add(chkTV);

            chkBoard = new CheckBox();
            chkBoard.Name = "chkBoard";
            chkBoard.Text = "Board";
            chkBoard.Left = 190;
            chkBoard.Top = 455;
            gbForm.Controls.Add(chkBoard);

            chkOnlineEquipment = new CheckBox();
            chkOnlineEquipment.Name = "chkOnlineEquipment";
            chkOnlineEquipment.Text = "Online Equipment";
            chkOnlineEquipment.Left = 300;
            chkOnlineEquipment.Top = 455;
            gbForm.Controls.Add(chkOnlineEquipment);

            btnResourceAdd = CreateActionButton("Add", 20, 520);
            btnResourceUpdate = CreateActionButton("Update", 140, 520);
            btnResourceDelete = CreateActionButton("Delete", 260, 520);

            btnResourceClear = CreateActionButton("Clear", 20, 565);
            btnResourceRefresh = CreateActionButton("Refresh", 140, 565);

            gbForm.Controls.Add(btnResourceAdd);
            gbForm.Controls.Add(btnResourceUpdate);
            gbForm.Controls.Add(btnResourceDelete);
            gbForm.Controls.Add(btnResourceClear);
            gbForm.Controls.Add(btnResourceRefresh);

            GroupBox gbList = CreateGroupBox("Resources List", 500, 20, 620, 610);
            gbList.Controls.Add(CreateLabel("Location", 15, 32));
            cbResourceListLocation = CreateComboBox("cbResourceListLocation", 80, 27, 220);
            gbList.Controls.Add(cbResourceListLocation);

            gbList.Controls.Add(CreateLabel("Type", 320, 32));
            cbResourceListType = CreateComboBox("cbResourceListType", 365, 27, 150);
            cbResourceListType.Items.AddRange(new object[] { "All", "Desk", "Room" });
            gbList.Controls.Add(cbResourceListType);

            dgvResources = CreateGrid("dgvResources", 15, 75, 590, 515);
            gbList.Controls.Add(dgvResources);

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }
    }
}
