namespace WSPPolska_Tools
{
    partial class GeolocationForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExportData = new System.Windows.Forms.Button();
            this.PositionsDataGrid = new System.Windows.Forms.DataGridView();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.SetNaming = new System.Windows.Forms.Button();
            this.openCoordinatesExcel = new System.Windows.Forms.OpenFileDialog();
            this.ImportLocations = new System.Windows.Forms.Button();
            this.OpenForImport = new System.Windows.Forms.OpenFileDialog();
            this.ModelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Longitde = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PositionsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // ExportData
            // 
            this.ExportData.Location = new System.Drawing.Point(152, 27);
            this.ExportData.Name = "ExportData";
            this.ExportData.Size = new System.Drawing.Size(103, 23);
            this.ExportData.TabIndex = 0;
            this.ExportData.Text = "Export to Excel";
            this.ExportData.UseVisualStyleBackColor = true;
            this.ExportData.Click += new System.EventHandler(this.ExportData_Click);
            // 
            // PositionsDataGrid
            // 
            this.PositionsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PositionsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ModelName,
            this.LocationName,
            this.Latitude,
            this.Longitde,
            this.EL,
            this.Ang});
            this.PositionsDataGrid.Location = new System.Drawing.Point(152, 100);
            this.PositionsDataGrid.Name = "PositionsDataGrid";
            this.PositionsDataGrid.Size = new System.Drawing.Size(576, 199);
            this.PositionsDataGrid.TabIndex = 1;
            this.PositionsDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PositionsDataGrid_CellContentClick);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 100);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(122, 199);
            this.checkedListBox1.TabIndex = 2;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // SetNaming
            // 
            this.SetNaming.Location = new System.Drawing.Point(31, 27);
            this.SetNaming.Name = "SetNaming";
            this.SetNaming.Size = new System.Drawing.Size(103, 23);
            this.SetNaming.TabIndex = 3;
            this.SetNaming.Text = "Naming Standard";
            this.SetNaming.UseVisualStyleBackColor = true;
            this.SetNaming.Click += new System.EventHandler(this.SetNaming_Click);
            // 
            // openCoordinatesExcel
            // 
            this.openCoordinatesExcel.FileName = "openCoordinatesExcel";
            // 
            // ImportLocations
            // 
            this.ImportLocations.Location = new System.Drawing.Point(261, 27);
            this.ImportLocations.Name = "ImportLocations";
            this.ImportLocations.Size = new System.Drawing.Size(103, 23);
            this.ImportLocations.TabIndex = 4;
            this.ImportLocations.Text = "Import Locations";
            this.ImportLocations.UseVisualStyleBackColor = true;
            this.ImportLocations.Click += new System.EventHandler(this.ImportLocations_Click);
            // 
            // OpenForImport
            // 
            this.OpenForImport.FileName = "OpenForImport";
            // 
            // ModelName
            // 
            this.ModelName.HeaderText = "Model Name";
            this.ModelName.Name = "ModelName";
            // 
            // LocationName
            // 
            this.LocationName.HeaderText = "LocationName";
            this.LocationName.Name = "LocationName";
            // 
            // Latitude
            // 
            this.Latitude.HeaderText = "Latitude";
            this.Latitude.Name = "Latitude";
            // 
            // Longitde
            // 
            this.Longitde.HeaderText = "Longitde";
            this.Longitde.Name = "Longitde";
            // 
            // EL
            // 
            this.EL.HeaderText = "Elevation";
            this.EL.Name = "EL";
            // 
            // Ang
            // 
            this.Ang.HeaderText = "Angle ToNorth";
            this.Ang.Name = "Ang";
            // 
            // GeolocationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 320);
            this.Controls.Add(this.ImportLocations);
            this.Controls.Add(this.SetNaming);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.PositionsDataGrid);
            this.Controls.Add(this.ExportData);
            this.Name = "GeolocationForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.PositionsDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExportData;
        private System.Windows.Forms.DataGridView PositionsDataGrid;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button SetNaming;
        private System.Windows.Forms.OpenFileDialog openCoordinatesExcel;
        private System.Windows.Forms.Button ImportLocations;
        private System.Windows.Forms.OpenFileDialog OpenForImport;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Longitde;
        private System.Windows.Forms.DataGridViewTextBoxColumn EL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ang;
    }
}