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
    public partial class ReservationsPage : UserControl
    {
        public ReservationsPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void ReservationsPage_Load(object sender, EventArgs e)
        {
            cbResource.Items.Clear();
            cbResource.Items.Add("[SVI RESURSI]");
            cbResource.Items.Add("Desk 1");
            cbResource.Items.Add("Desk 2");
            cbResource.Items.Add("Room A");
            cbResource.SelectedIndex = 0;

            dgvReservations.Rows.Clear();
            dgvReservations.Rows.Add(1, "Desk 1", "Ivana Mitrović (ivana@ex.com)", "29.11.2023, 11:00 - 19:00", 8, "Aktivan", "15.01.2024", "Izmena");
            dgvReservations.Rows.Add(2, "Desk 2", "Milan Jović (milan@ex.com)", "23.11.2023, 12:35 - 19:00", 4, "Aktivan", "20.02.2024", "Izmena");
            dgvReservations.Rows.Add(6, "Room A", "Milan Jović (milan@ex.com)", "25.11.2023, 17:40 - 19:30", 2, "Otkazana", "24.01.2024", "Izmena");
            dgvReservations.Rows.Add(7, "Room B", "Petar Radić (petar@ex.com)", "25.11.2023, 18:10 - 19:30", 1, "Završena", "22.02.2024", "Izmena");
        }

        private void dgvReservations_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var colName = dgvReservations.Columns[e.ColumnIndex].Name;

            if (colName == "colStatus")
            {
                var val = e.Value?.ToString()?.ToLower();
                if (val == null) return;

                var cell = dgvReservations.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                if (val.Contains("akt"))
                {
                    cell.Style.BackColor = Color.LightGreen;
                    cell.Style.ForeColor = Color.Black;
                }
                else if (val.Contains("otkaz"))
                {
                    cell.Style.BackColor = Color.IndianRed;
                    cell.Style.ForeColor = Color.White;
                }
                else if (val.Contains("zavr"))
                {
                    cell.Style.BackColor = Color.LightSlateGray;
                    cell.Style.ForeColor = Color.White;
                }
            }

            if (colName == "colEditBtn")
            {
                var cell = dgvReservations.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.BackColor = Color.RoyalBlue;
                cell.Style.ForeColor = Color.White;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        private void dgvReservations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvReservations.Columns[e.ColumnIndex].Name == "colEditBtn")
            {
                // TODO: Otvori formu za izmenu rezervacije
                // var id = (int)dgvReservations.Rows[e.RowIndex].Cells["colId"].Value;
            }
        }
    }
}
