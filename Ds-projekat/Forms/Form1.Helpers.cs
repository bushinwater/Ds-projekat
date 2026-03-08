using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private Button CreateSidebarButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = 200;
            btn.Height = 45;
            btn.Margin = new Padding(5);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(44, 58, 89);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private Panel CreateBasePage()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.FromArgb(245, 247, 250);
            return panel;
        }

        private GroupBox CreateGroupBox(string text, int x, int y, int w, int h)
        {
            GroupBox gb = new GroupBox();
            gb.Text = text;
            gb.Left = x;
            gb.Top = y;
            gb.Width = w;
            gb.Height = h;
            gb.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gb.BackColor = Color.White;
            return gb;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Left = x;
            lbl.Top = y;
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            return lbl;
        }

        private TextBox CreateTextBox(string name, int x, int y, int w)
        {
            TextBox tb = new TextBox();
            tb.Name = name;
            tb.Left = x;
            tb.Top = y;
            tb.Width = w;
            return tb;
        }

        private ComboBox CreateComboBox(string name, int x, int y, int w)
        {
            ComboBox cb = new ComboBox();
            cb.Name = name;
            cb.Left = x;
            cb.Top = y;
            cb.Width = w;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            return cb;
        }

        private Button CreateActionButton(string text, int x, int y, int w = 110)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Left = x;
            btn.Top = y;
            btn.Width = w;
            btn.Height = 36;
            btn.BackColor = Color.FromArgb(52, 120, 246);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private DataGridView CreateGrid(string name, int x, int y, int w, int h)
        {
            DataGridView dgv = new DataGridView();
            dgv.Name = name;
            dgv.Left = x;
            dgv.Top = y;
            dgv.Width = w;
            dgv.Height = h;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            return dgv;
        }

        private Panel CreateStatCard(string title, out Label valueLabel, int x, int y)
        {
            Panel card = new Panel();
            card.Left = x;
            card.Top = y;
            card.Width = 240;
            card.Height = 120;
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Left = 20;
            lblTitle.Top = 20;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            valueLabel = new Label();
            valueLabel.Text = "0";
            valueLabel.Left = 20;
            valueLabel.Top = 55;
            valueLabel.AutoSize = true;
            valueLabel.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            valueLabel.ForeColor = Color.FromArgb(52, 120, 246);

            card.Controls.Add(lblTitle);
            card.Controls.Add(valueLabel);

            return card;
        }
    }
}