using System.Windows.Forms;

namespace Ds_projekat
{
    internal class ResourcesForm : SectionFormBase
    {
        public ResourcesForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            GroupBox formGroup = CreateGroupBox("Resource Details", 20, 20, 460, 610);

            formGroup.Controls.Add(CreateLabel("Location", 20, 40));
            formGroup.Controls.Add(CreateComboBox("cbResourceLocation", 190, 35, 220));

            formGroup.Controls.Add(CreateLabel("Resource Name", 20, 85));
            formGroup.Controls.Add(CreateTextBox("txtResourceName", 190, 80, 220));

            formGroup.Controls.Add(CreateLabel("Resource Type", 20, 130));
            ComboBox typeCombo = CreateComboBox("cbResourceType", 190, 125, 220);
            typeCombo.Items.AddRange(new object[] { "Desk", "Room" });
            formGroup.Controls.Add(typeCombo);

            formGroup.Controls.Add(CreateLabel("Is Active", 20, 175));
            CheckBox activeCheck = new CheckBox
            {
                Name = "chkResourceIsActive",
                Left = 190,
                Top = 173
            };
            formGroup.Controls.Add(activeCheck);

            formGroup.Controls.Add(CreateLabel("Description", 20, 220));
            TextBox descriptionText = CreateTextBox("txtResourceDescription", 190, 215, 220);
            descriptionText.Multiline = true;
            descriptionText.Height = 60;
            formGroup.Controls.Add(descriptionText);

            formGroup.Controls.Add(CreateLabel("Desk Subtype", 20, 300));
            ComboBox deskSubtypeCombo = CreateComboBox("cbDeskSubtype", 190, 295, 220);
            deskSubtypeCombo.Items.AddRange(new object[] { "Hot", "Dedicated" });
            formGroup.Controls.Add(deskSubtypeCombo);

            formGroup.Controls.Add(CreateLabel("Room Capacity", 20, 345));
            formGroup.Controls.Add(CreateTextBox("txtRoomCapacity", 190, 340, 220));

            CheckBox projectorCheck = new CheckBox
            {
                Name = "chkProjector",
                Text = "Projector",
                Left = 190,
                Top = 385
            };
            formGroup.Controls.Add(projectorCheck);

            CheckBox tvCheck = new CheckBox
            {
                Name = "chkTV",
                Text = "TV",
                Left = 300,
                Top = 385
            };
            formGroup.Controls.Add(tvCheck);

            CheckBox boardCheck = new CheckBox
            {
                Name = "chkBoard",
                Text = "Board",
                Left = 190,
                Top = 415
            };
            formGroup.Controls.Add(boardCheck);

            CheckBox onlineCheck = new CheckBox
            {
                Name = "chkOnlineEquipment",
                Text = "Online Equipment",
                Left = 300,
                Top = 415
            };
            formGroup.Controls.Add(onlineCheck);

            formGroup.Controls.Add(CreateActionButton("Add", 20, 480));
            formGroup.Controls.Add(CreateActionButton("Update", 140, 480));
            formGroup.Controls.Add(CreateActionButton("Delete", 260, 480));

            GroupBox listGroup = CreateGroupBox("Resources List", 500, 20, 620, 610);
            listGroup.Controls.Add(CreateGrid("dgvResources", 15, 30, 590, 560));

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }
    }
}
