using Ds_projekat.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal class LoginForm : Form
    {
        private readonly AdminFacade _adminFacade;
        private readonly string _brandName;

        private TextBox _usernameTextBox;
        private TextBox _passwordTextBox;
        private Button _loginButton;
        private Label _statusLabel;

        public LoginForm()
        {
            _adminFacade = new AdminFacade();
            try
            {
                AppConfig.Instance.Load("config.txt");
                _brandName = string.IsNullOrWhiteSpace(AppConfig.Instance.BrandName)
                    ? "Co-working Sistem"
                    : AppConfig.Instance.BrandName;
            }
            catch
            {
                _brandName = "Co-working Sistem";
            }
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            SuspendLayout();

            Text = "Prijava administratora";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(1100, 680);
            BackColor = AppTheme.AppBackgroundColor;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 520,
                BackColor = AppTheme.PrimaryColor
            };

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppTheme.SurfaceColor
            };

            BuildBrandPanel(leftPanel);
            BuildLoginPanel(rightPanel);

            Controls.Add(rightPanel);
            Controls.Add(leftPanel);

            ResumeLayout(false);
        }

        private void BuildBrandPanel(Panel parent)
        {
            Label brandBadge = new Label
            {
                Text = "ADMIN PANEL",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = AppTheme.PrimaryColor,
                BackColor = AppTheme.AccentColor,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                Size = new Size(130, 34),
                Location = new Point(54, 52)
            };

            Label title = new Label
            {
                Text = _brandName.Replace(" ", "\n"),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Georgia", 30F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(48, 128)
            };

            Label subtitle = new Label
            {
                Text = "Administratorski pristup korisnicima, lokacijama, rezervacijama i svim promenama koje se odmah upisuju u bazu.",
                MaximumSize = new Size(390, 0),
                AutoSize = true,
                ForeColor = Color.FromArgb(219, 234, 254),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(54, 250)
            };

            Panel divider = new Panel
            {
                BackColor = AppTheme.AccentColor,
                Size = new Size(96, 4),
                Location = new Point(56, 344)
            };

            parent.Controls.Add(brandBadge);
            parent.Controls.Add(title);
            parent.Controls.Add(subtitle);
            parent.Controls.Add(divider);
        }

        private void BuildLoginPanel(Panel parent)
        {
            Label smallBadge = new Label
            {
                Text = "PRIJAVA",
                AutoSize = false,
                Size = new Size(138, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = AppTheme.AccentSoftColor,
                ForeColor = AppTheme.PrimaryColor,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(85, 46)
            };

            Panel loginCard = new Panel
            {
                Size = new Size(420, 560),
                BackColor = AppTheme.SurfaceStrongColor,
                Location = new Point(85, 64)
            };

            Label welcomeLabel = new Label
            {
                Text = "Prijava",
                AutoSize = true,
                ForeColor = AppTheme.PrimaryColor,
                Font = new Font("Georgia", 28F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(44, 42)
            };

            Label infoLabel = new Label
            {
                Text = "Pristup je dozvoljen samo nalozima iz tabele Admins.",
                MaximumSize = new Size(320, 0),
                AutoSize = true,
                ForeColor = AppTheme.MutedTextColor,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(46, 98)
            };

            Panel topAccent = new Panel
            {
                BackColor = AppTheme.PrimaryColor,
                Location = new Point(46, 138),
                Size = new Size(72, 3)
            };

            Label userLabel = CreateFieldLabel("Korisnicko ime", new Point(46, 168));
            _usernameTextBox = CreateInputTextBox("txtLoginUsername", new Point(46, 198));

            Label passwordLabel = CreateFieldLabel("Lozinka", new Point(46, 270));
            _passwordTextBox = CreateInputTextBox("txtLoginPassword", new Point(46, 300));
            _passwordTextBox.PasswordChar = '*';

            _loginButton = new Button
            {
                Name = "btnLogin",
                Text = "Uloguj se",
                Size = new Size(326, 48),
                Location = new Point(46, 382),
                BackColor = AppTheme.PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point),
                Cursor = Cursors.Hand
            };
            _loginButton.FlatAppearance.BorderSize = 0;
            _loginButton.Click += LoginButton_Click;

            _statusLabel = new Label
            {
                AutoSize = false,
                Width = 326,
                Height = 58,
                Left = 46,
                Top = 444,
                ForeColor = AppTheme.MutedTextColor,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                Text = ""
            };

            loginCard.Controls.Add(welcomeLabel);
            loginCard.Controls.Add(infoLabel);
            loginCard.Controls.Add(topAccent);
            loginCard.Controls.Add(userLabel);
            loginCard.Controls.Add(_usernameTextBox);
            loginCard.Controls.Add(passwordLabel);
            loginCard.Controls.Add(_passwordTextBox);
            loginCard.Controls.Add(_loginButton);
            loginCard.Controls.Add(_statusLabel);

            parent.Controls.Add(smallBadge);
            parent.Controls.Add(loginCard);

            AcceptButton = _loginButton;
        }

        private static Label CreateFieldLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                ForeColor = AppTheme.TextColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point),
                Location = location
            };
        }

        private static TextBox CreateInputTextBox(string name, Point location)
        {
            return new TextBox
            {
                Name = name,
                Location = location,
                Size = new Size(326, 34),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = AppTheme.SurfaceColor,
                ForeColor = AppTheme.TextColor
            };
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            ServiceResult result = _adminFacade.AuthenticateAdmin(_usernameTextBox.Text.Trim(), _passwordTextBox.Text);
            SetStatus(result.Message, result.Success);

            if (!result.Success)
                return;

            OpenMainShell();
        }

        private void SetStatus(string message, bool success)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = success ? AppTheme.SuccessColor : AppTheme.DangerColor;
        }

        private void OpenMainShell()
        {
            if (!Visible)
                return;

            Hide();

            Form1 shell = new Form1();
            shell.FormClosed += MainShell_FormClosed;
            shell.Show();
        }

        private void MainShell_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
    }
}
