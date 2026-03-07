using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ds_projekat.UI.Pages
{
    public partial class ResourcesPage : UserControl
    {
        public ResourcesPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void ResourcesPage_Load(object sender, EventArgs e)
        {
            dgvResources.Rows.Clear();
            dgvResources.Rows.Add(1, "Desk 1", "Desk", "Dostupno", "Izmena", "Obriši");
            dgvResources.Rows.Add(2, "Desk 2", "Desk", "Zauzeto", "Izmena", "Obriši");
            dgvResources.Rows.Add(3, "Room A", "Room", "Zauzeto", "Izmena", "Obriši");
            dgvResources.Rows.Add(4, "Room B", "Room", "Dostupno", "Izmena", "Obriši");

            cbLocation.Items.Clear();
            cbLocation.Items.Add("Novi Sad - Centar");
            cbLocation.Items.Add("Beograd - Downtown");
            cbLocation.SelectedIndex = 0;
        }
        private void dgvResources_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvResources.Columns[e.ColumnIndex].Name == "colStatus")
            {
                var val = e.Value?.ToString()?.ToLower();
                if (val == null) return;

                var cell = dgvResources.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                if (val.Contains("dostup"))
                {
                    cell.Style.BackColor = Color.LightGreen;
                    cell.Style.ForeColor = Color.Black;
                }
                else if (val.Contains("zauzet"))
                {
                    cell.Style.BackColor = Color.OrangeRed;
                    cell.Style.ForeColor = Color.White;
                }
            }
            var colName = dgvResources.Columns[e.ColumnIndex].Name;
            if (colName == "colEditBtn")
            {
                var cell = dgvResources.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.BackColor = Color.RoyalBlue;
                cell.Style.ForeColor = Color.White;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
            else if (colName == "colDeleteBtn")
            {
                var cell = dgvResources.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.BackColor = Color.IndianRed;
                cell.Style.ForeColor = Color.White;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        private void dgvResources_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var col = dgvResources.Columns[e.ColumnIndex].Name;

            if (col == "colEditBtn")
            {
                // TODO: Otvori formu za izmenu resursa
                // var id = (int)dgvResources.Rows[e.RowIndex].Cells["colId"].Value;
            }
            else if (col == "colDeleteBtn")
            {
                // TODO: potvrda + brisanje
            }
        }
    }
}
