namespace Ds_projekat.UI.Pages
{
    partial class ResourcesPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnAddTop = new System.Windows.Forms.Button();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnAddSmall = new System.Windows.Forms.Button();
            this.btnEditSmall = new System.Windows.Forms.Button();
            this.btnDeleteSmall = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvResources = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEditBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlHeader.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.btnAddTop);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1175, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(20, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(134, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Resursi";
            // 
            // btnAddTop
            // 
            this.btnAddTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTop.BackColor = System.Drawing.Color.Blue;
            this.btnAddTop.FlatAppearance.BorderSize = 0;
            this.btnAddTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTop.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTop.ForeColor = System.Drawing.Color.White;
            this.btnAddTop.Location = new System.Drawing.Point(1002, 26);
            this.btnAddTop.Name = "btnAddTop";
            this.btnAddTop.Size = new System.Drawing.Size(170, 38);
            this.btnAddTop.TabIndex = 1;
            this.btnAddTop.Text = "+ Dodaj resurs";
            this.btnAddTop.UseVisualStyleBackColor = false;
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlFilter.Controls.Add(this.cbLocation);
            this.pnlFilter.Controls.Add(this.lblLocation);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 70);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1175, 55);
            this.pnlFilter.TabIndex = 1;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(20, 16);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(74, 23);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "Lokacija:";
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.FormattingEnabled = true;
            this.cbLocation.Location = new System.Drawing.Point(90, 15);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(200, 24);
            this.cbLocation.TabIndex = 1;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlToolbar.Controls.Add(this.btnDeleteSmall);
            this.pnlToolbar.Controls.Add(this.btnEditSmall);
            this.pnlToolbar.Controls.Add(this.btnAddSmall);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 125);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1175, 55);
            this.pnlToolbar.TabIndex = 2;
            // 
            // btnAddSmall
            // 
            this.btnAddSmall.BackColor = System.Drawing.Color.Blue;
            this.btnAddSmall.FlatAppearance.BorderSize = 0;
            this.btnAddSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSmall.ForeColor = System.Drawing.Color.White;
            this.btnAddSmall.Location = new System.Drawing.Point(20, 10);
            this.btnAddSmall.Name = "btnAddSmall";
            this.btnAddSmall.Size = new System.Drawing.Size(110, 36);
            this.btnAddSmall.TabIndex = 0;
            this.btnAddSmall.Text = "+ Dodaj";
            this.btnAddSmall.UseVisualStyleBackColor = false;
            // 
            // btnEditSmall
            // 
            this.btnEditSmall.BackColor = System.Drawing.Color.LightGray;
            this.btnEditSmall.FlatAppearance.BorderSize = 0;
            this.btnEditSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditSmall.ForeColor = System.Drawing.Color.Black;
            this.btnEditSmall.Location = new System.Drawing.Point(140, 10);
            this.btnEditSmall.Name = "btnEditSmall";
            this.btnEditSmall.Size = new System.Drawing.Size(110, 36);
            this.btnEditSmall.TabIndex = 1;
            this.btnEditSmall.Text = "Izmena";
            this.btnEditSmall.UseVisualStyleBackColor = false;
            // 
            // btnDeleteSmall
            // 
            this.btnDeleteSmall.BackColor = System.Drawing.Color.LightGray;
            this.btnDeleteSmall.FlatAppearance.BorderSize = 0;
            this.btnDeleteSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSmall.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteSmall.Location = new System.Drawing.Point(260, 10);
            this.btnDeleteSmall.Name = "btnDeleteSmall";
            this.btnDeleteSmall.Size = new System.Drawing.Size(110, 36);
            this.btnDeleteSmall.TabIndex = 2;
            this.btnDeleteSmall.Text = "Obriši";
            this.btnDeleteSmall.UseVisualStyleBackColor = false;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlFooter.Controls.Add(this.label1);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 631);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1175, 50);
            this.pnlFooter.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(524, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dodace se kasnije";
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.Controls.Add(this.dgvResources);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 180);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlGrid.Size = new System.Drawing.Size(1175, 451);
            this.pnlGrid.TabIndex = 4;
            // 
            // dgvResources
            // 
            this.dgvResources.AllowUserToAddRows = false;
            this.dgvResources.AllowUserToDeleteRows = false;
            this.dgvResources.AllowUserToResizeRows = false;
            this.dgvResources.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colType,
            this.colStatus,
            this.colEditBtn,
            this.colDeleteBtn});
            this.dgvResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResources.EnableHeadersVisualStyles = false;
            this.dgvResources.Location = new System.Drawing.Point(20, 10);
            this.dgvResources.MultiSelect = false;
            this.dgvResources.Name = "dgvResources";
            this.dgvResources.ReadOnly = true;
            this.dgvResources.RowHeadersVisible = false;
            this.dgvResources.RowHeadersWidth = 51;
            this.dgvResources.RowTemplate.Height = 24;
            this.dgvResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResources.Size = new System.Drawing.Size(1135, 431);
            this.dgvResources.TabIndex = 0;
            this.dgvResources.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResources_CellContentClick);
            this.dgvResources.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvResources_CellFormatting);
            // 
            // colId
            // 
            this.colId.HeaderText = "ID";
            this.colId.MinimumWidth = 6;
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.HeaderText = "Naziv";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "Tip resursa";
            this.colType.MinimumWidth = 6;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colEditBtn
            // 
            this.colEditBtn.HeaderText = "Izmena";
            this.colEditBtn.MinimumWidth = 6;
            this.colEditBtn.Name = "colEditBtn";
            this.colEditBtn.ReadOnly = true;
            // 
            // colDeleteBtn
            // 
            this.colDeleteBtn.HeaderText = "Obriši";
            this.colDeleteBtn.MinimumWidth = 6;
            this.colDeleteBtn.Name = "colDeleteBtn";
            this.colDeleteBtn.ReadOnly = true;
            // 
            // ResourcesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.Name = "ResourcesPage";
            this.Size = new System.Drawing.Size(1175, 681);
            this.Load += new System.EventHandler(this.ResourcesPage_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnAddTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnDeleteSmall;
        private System.Windows.Forms.Button btnEditSmall;
        private System.Windows.Forms.Button btnAddSmall;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvResources;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewButtonColumn colEditBtn;
        private System.Windows.Forms.DataGridViewButtonColumn colDeleteBtn;
    }
}
