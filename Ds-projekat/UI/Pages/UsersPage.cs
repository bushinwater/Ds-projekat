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
    public partial class UsersPage : UserControl
    {
        public UsersPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void UsersPage_Load(object sender, EventArgs e)
        {
            dgvUsers.Rows.Clear();
            dgvUsers.Rows.Add(1, "Milan Jović (milan@ex.com)", "Standard", "Aktivan", 98, "15.01.2024", "Izmena");
            dgvUsers.Rows.Add(2, "Ana Marković (ana@ex.com)", "Premium", "Aktivan", 54, "20.02.2024", "Izmena");
            dgvUsers.Rows.Add(3, "Petar Radić (petar@ex.com)", "Basic", "Neaktivan", 12, "27.02.2024", "Izmena");
        }
        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsers.Columns[e.ColumnIndex].Name == "Status")
            {
                var val = e.Value?.ToString()?.ToLower();
                if (val == null) return;

                if (val.Contains("aktivan"))
                {
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGreen;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (val.Contains("na čekanju") || val.Contains("cek"))
                {
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Khaki;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else
                {
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightCoral;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                }

                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }
    }
}
