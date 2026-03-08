using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Panel BuildLocationsPage()
        {
            Panel page = CreateBasePage();

            GroupBox gbForm = CreateGroupBox("Location Details", 20, 20, 420, 610);

            gbForm.Controls.Add(CreateLabel("Location ID", 20, 40));
            txtLocationId = CreateTextBox("txtLocationId", 180, 35, 200);
            txtLocationId.ReadOnly = true;
            gbForm.Controls.Add(txtLocationId);

            gbForm.Controls.Add(CreateLabel("Location Name", 20, 85));
            txtLocationName = CreateTextBox("txtLocationName", 180, 80, 200);
            gbForm.Controls.Add(txtLocationName);

            gbForm.Controls.Add(CreateLabel("Address", 20, 130));
            txtLocationAddress = CreateTextBox("txtLocationAddress", 180, 125, 200);
            gbForm.Controls.Add(txtLocationAddress);

            gbForm.Controls.Add(CreateLabel("City", 20, 175));
            txtLocationCity = CreateTextBox("txtLocationCity", 180, 170, 200);
            gbForm.Controls.Add(txtLocationCity);

            gbForm.Controls.Add(CreateLabel("Working Hours", 20, 220));
            txtWorkingHours = CreateTextBox("txtWorkingHours", 180, 215, 200);
            gbForm.Controls.Add(txtWorkingHours);

            gbForm.Controls.Add(CreateLabel("Max Users", 20, 265));
            txtMaxUsers = CreateTextBox("txtMaxUsers", 180, 260, 200);
            gbForm.Controls.Add(txtMaxUsers);

            btnLocationAdd = CreateActionButton("Add", 20, 340);
            btnLocationUpdate = CreateActionButton("Update", 140, 340);
            btnLocationDelete = CreateActionButton("Delete", 260, 340);

            btnLocationClear = CreateActionButton("Clear", 20, 390);
            btnLocationRefresh = CreateActionButton("Refresh", 140, 390);

            gbForm.Controls.Add(btnLocationAdd);
            gbForm.Controls.Add(btnLocationUpdate);
            gbForm.Controls.Add(btnLocationDelete);
            gbForm.Controls.Add(btnLocationClear);
            gbForm.Controls.Add(btnLocationRefresh);

            GroupBox gbList = CreateGroupBox("Locations List", 460, 20, 660, 610);
            dgvLocations = CreateGrid("dgvLocations", 15, 30, 630, 560);
            gbList.Controls.Add(dgvLocations);

            page.Controls.Add(gbForm);
            page.Controls.Add(gbList);

            return page;
        }
    }
}