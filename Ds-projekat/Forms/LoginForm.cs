using Ds_projekat.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class LoginForm : Form
    {
        private Panel leftPanel;
        private Panel rightPanel;

        private Label lblAppName;
        private Label lblAppSubtitle;
        private Label lblWelcome;
        private Label lblInfo;

        private Panel pnlLogin;

        private TextBox txtLoginUsername;
        private TextBox txtLoginPassword;
        private Button btnLogin;
        private Button btnExit;

        private readonly AdminFacade _adminFacade = new AdminFacade();

        public LoginForm()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.SuspendLayout();

            this.Text = "Login - Coworking Space Management";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 620);
            this.MinimumSize = new Size(900, 580);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 380;
            leftPanel.BackColor = Color.FromArgb(32, 42, 68);

            rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.BackColor = Color.FromArgb(245, 247, 250);

            lblAppName = new Label();
            lblAppName.Text = "DS PROJEKAT";
            lblAppName.ForeColor = Color.White;
            lblAppName.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblAppName.AutoSize = true;
            lblAppName.Location = new Point(45, 100);

            lblAppSubtitle = new Label();
            lblAppSubtitle.Text = "Coworking Space Management System";
            lblAppSubtitle.ForeColor = Color.FromArgb(220, 225, 235);
            lblAppSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Location = new Point(48, 150);

            lblWelcome = new Label();
            lblWelcome.Text = "Dobrodošli";
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(45, 245);

            lblInfo = new Label();
            lblInfo.Text =
                "Prijavite se administratorskim nalogom koji već postoji u bazi.\r\n" +
                "Samo postojeći admin može pristupiti sistemu.";
            lblInfo.ForeColor = Color.FromArgb(220, 225, 235);
            lblInfo.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            lblInfo.Size = new Size(300, 90);
            lblInfo.Location = new Point(48, 295);

            leftPanel.Controls.Add(lblAppName);
            leftPanel.Controls.Add(lblAppSubtitle);
            leftPanel.Controls.Add(lblWelcome);
            leftPanel.Controls.Add(lblInfo);

            BuildLoginPanel();

            rightPanel.Controls.Add(pnlLogin);

            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);

            this.ResumeLayout(false);
        }

        private void BuildLoginPanel()
        {
            pnlLogin = new Panel();
            pnlLogin.Size = new Size(430, 360);
            pnlLogin.BackColor = Color.White;
            pnlLogin.BorderStyle = BorderStyle.FixedSingle;
            pnlLogin.Left = 110;
            pnlLogin.Top = 110;

            Label lblTitle = new Label();
            lblTitle.Text = "Prijava";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(35, 30);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Unesite podatke administratora";
            lblSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(38, 72);

            Label lblUsername = CreateLabel("Username", 40, 130);
            txtLoginUsername = CreateTextBox("txtLoginUsername", 40, 155, 340);

            Label lblPassword = CreateLabel("Password", 40, 215);
            txtLoginPassword = CreateTextBox("txtLoginPassword", 40, 240, 340);
            txtLoginPassword.PasswordChar = '*';

            btnLogin = CreatePrimaryButton("Login", 40, 300, 160);
            btnLogin.Click += btnLogin_Click;

            btnExit = CreateSecondaryButton("Exit", 220, 300, 160);
            btnExit.Click += btnExit_Click;

            pnlLogin.Controls.Add(lblTitle);
            pnlLogin.Controls.Add(lblSubtitle);
            pnlLogin.Controls.Add(lblUsername);
            pnlLogin.Controls.Add(txtLoginUsername);
            pnlLogin.Controls.Add(lblPassword);
            pnlLogin.Controls.Add(txtLoginPassword);
            pnlLogin.Controls.Add(btnLogin);
            pnlLogin.Controls.Add(btnExit);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Left = x;
            lbl.Top = y;
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(40, 40, 40);
            return lbl;
        }

        private TextBox CreateTextBox(string name, int x, int y, int width)
        {
            TextBox tb = new TextBox();
            tb.Name = name;
            tb.Left = x;
            tb.Top = y;
            tb.Width = width;
            tb.Height = 32;
            tb.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            return tb;
        }

        private Button CreatePrimaryButton(string text, int x, int y, int width)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Left = x;
            btn.Top = y;
            btn.Width = width;
            btn.Height = 42;
            btn.BackColor = Color.FromArgb(52, 120, 246);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private Button CreateSecondaryButton(string text, int x, int y, int width)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Left = x;
            btn.Top = y;
            btn.Width = width;
            btn.Height = 42;
            btn.BackColor = Color.Gainsboro;
            btn.ForeColor = Color.FromArgb(40, 40, 40);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var admin = _adminFacade.Login(
                    txtLoginUsername.Text.Trim(),
                    txtLoginPassword.Text
                );

                this.Hide();

                using (Form1 mainForm = new Form1())
                {
                    mainForm.ShowDialog();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Login neuspešan:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}