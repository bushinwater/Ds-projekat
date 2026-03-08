namespace Ds_projekat
{
    internal class LocationsForm : SectionFormBase
    {
        public LocationsForm()
        {
            BuildContent();
        }

        private void BuildContent()
        {
            var formGroup = CreateGroupBox("Location Details", 20, 20, 420, 610);

            formGroup.Controls.Add(CreateLabel("Location Name", 20, 40));
            formGroup.Controls.Add(CreateTextBox("txtLocationName", 180, 35, 200));

            formGroup.Controls.Add(CreateLabel("Address", 20, 85));
            formGroup.Controls.Add(CreateTextBox("txtLocationAddress", 180, 80, 200));

            formGroup.Controls.Add(CreateLabel("City", 20, 130));
            formGroup.Controls.Add(CreateTextBox("txtLocationCity", 180, 125, 200));

            formGroup.Controls.Add(CreateLabel("Working Hours", 20, 175));
            formGroup.Controls.Add(CreateTextBox("txtWorkingHours", 180, 170, 200));

            formGroup.Controls.Add(CreateLabel("Max Users", 20, 220));
            formGroup.Controls.Add(CreateTextBox("txtMaxUsers", 180, 215, 200));

            formGroup.Controls.Add(CreateActionButton("Add", 20, 295));
            formGroup.Controls.Add(CreateActionButton("Update", 140, 295));
            formGroup.Controls.Add(CreateActionButton("Delete", 260, 295));
            formGroup.Controls.Add(CreateActionButton("Clear", 20, 345));
            formGroup.Controls.Add(CreateActionButton("Refresh", 140, 345));

            var listGroup = CreateGroupBox("Locations List", 460, 20, 660, 610);
            listGroup.Controls.Add(CreateGrid("dgvLocations", 15, 30, 630, 560));

            Controls.Add(formGroup);
            Controls.Add(listGroup);
        }
    }
}
