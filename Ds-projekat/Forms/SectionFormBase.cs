using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal abstract class SectionFormBase : Form
    {
        protected SectionFormBase()
        {
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ShowInTaskbar = false;
        }

        protected GroupBox CreateGroupBox(string text, int x, int y, int width, int height)
        {
            return new GroupBox
            {
                Text = text,
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.White
            };
        }

        protected Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Left = x,
                Top = y,
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };
        }

        protected TextBox CreateTextBox(string name, int x, int y, int width)
        {
            return new TextBox
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width
            };
        }

        protected ComboBox CreateComboBox(string name, int x, int y, int width)
        {
            return new ComboBox
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
        }

        protected Button CreateActionButton(string text, int x, int y, int width = 110)
        {
            Button button = new Button
            {
                Text = text,
                Left = x,
                Top = y,
                Width = width,
                Height = 36,
                BackColor = Color.FromArgb(52, 120, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        protected DataGridView CreateGrid(string name, int x, int y, int width, int height)
        {
            return new DataGridView
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
        }

        protected Panel CreateStatCard(string title, string value, int x, int y)
        {
            Panel card = new Panel
            {
                Left = x,
                Top = y,
                Width = 240,
                Height = 120,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label titleLabel = new Label
            {
                Text = title,
                Left = 20,
                Top = 20,
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            Label valueLabel = new Label
            {
                Text = value,
                Left = 20,
                Top = 55,
                AutoSize = true,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 120, 246)
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);

            return card;
        }

        protected DateTimePicker CreateDatePicker(string name, int x, int y, int width)
        {
            return new DateTimePicker
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width
            };
        }

        protected DateTimePicker CreateDateTimePicker(string name, int x, int y, int width)
        {
            return new DateTimePicker
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm"
            };
        }
    }
}
