using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal abstract class SectionFormBase : Form
    {
        protected static readonly Color AppBackgroundColor = AppTheme.AppBackgroundColor;
        protected static readonly Color SurfaceColor = AppTheme.SurfaceColor;
        protected static readonly Color SurfaceStrongColor = AppTheme.SurfaceStrongColor;
        protected static readonly Color PrimaryColor = AppTheme.PrimaryColor;
        protected static readonly Color PrimaryMutedColor = AppTheme.PrimaryMutedColor;
        protected static readonly Color AccentColor = AppTheme.AccentColor;
        protected static readonly Color AccentSoftColor = AppTheme.AccentSoftColor;
        protected static readonly Color BorderColor = AppTheme.BorderColor;
        protected static readonly Color TextColor = AppTheme.TextColor;
        protected static readonly Color MutedTextColor = AppTheme.MutedTextColor;

        protected SectionFormBase()
        {
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = AppBackgroundColor;
            ForeColor = TextColor;
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
                BackColor = SurfaceStrongColor,
                ForeColor = PrimaryColor
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
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = TextColor,
                BackColor = Color.Transparent
            };
        }

        protected TextBox CreateTextBox(string name, int x, int y, int width)
        {
            return new TextBox
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SurfaceColor,
                ForeColor = TextColor
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
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = SurfaceColor,
                ForeColor = TextColor
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
                Height = 38,
                BackColor = PrimaryColor,
                ForeColor = SurfaceColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
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
                BackgroundColor = SurfaceStrongColor,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                GridColor = BorderColor,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                RowHeadersVisible = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = PrimaryColor,
                    ForeColor = SurfaceColor,
                    SelectionBackColor = PrimaryMutedColor,
                    SelectionForeColor = SurfaceColor,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = SurfaceStrongColor,
                    ForeColor = TextColor,
                    SelectionBackColor = AccentSoftColor,
                    SelectionForeColor = PrimaryColor
                }
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
                BackColor = SurfaceStrongColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel accentStrip = new Panel
            {
                Dock = DockStyle.Top,
                Height = 6,
                BackColor = AccentColor
            };

            Label titleLabel = new Label
            {
                Text = title,
                Left = 20,
                Top = 20,
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = MutedTextColor
            };

            Label valueLabel = new Label
            {
                Text = value,
                Left = 20,
                Top = 48,
                AutoSize = true,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = PrimaryColor
            };

            card.Controls.Add(accentStrip);
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
                Width = width,
                Format = DateTimePickerFormat.Short,
                CalendarMonthBackground = SurfaceStrongColor,
                CalendarForeColor = TextColor
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
                CustomFormat = "dd.MM.yyyy HH:mm",
                CalendarMonthBackground = SurfaceStrongColor,
                CalendarForeColor = TextColor
            };
        }

        protected NumericUpDown CreateNumericUpDown(string name, int x, int y, int width, int minimum, int maximum)
        {
            return new NumericUpDown
            {
                Name = name,
                Left = x,
                Top = y,
                Width = width,
                Minimum = minimum,
                Maximum = maximum,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SurfaceColor,
                ForeColor = TextColor,
                ThousandsSeparator = false
            };
        }

        protected CheckBox CreateCheckBox(string name, string text, int x, int y)
        {
            return new CheckBox
            {
                Name = name,
                Text = text,
                Left = x,
                Top = y,
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point)
            };
        }

        protected void SetGridHeader(DataGridView grid, string columnName, string headerText)
        {
            if (grid == null || !grid.Columns.Contains(columnName))
                return;

            grid.Columns[columnName].HeaderText = headerText;
        }
    }
}
