using System.Windows.Forms;

namespace WSPPolska_Tools.Commands
{
    partial class StructureCostForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StructureCostForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.ExportData = new System.Windows.Forms.Button();
            this.CostInformationGrid = new System.Windows.Forms.DataGridView();
            this.ClearData = new System.Windows.Forms.Button();
            this.IncNaming = new System.Windows.Forms.Button();
            this.VolNotCal = new System.Windows.Forms.Button();
            this.IncNamingNo = new System.Windows.Forms.TextBox();
            this.NotCalcNo = new System.Windows.Forms.TextBox();
            this.RemainingSel = new System.Windows.Forms.Button();
            this.remainingCounter = new System.Windows.Forms.TextBox();
            this.SelectElementsFromRows = new System.Windows.Forms.Button();
            this.generalReinforcementRatios = new System.Windows.Forms.DataGridView();
            this.BeamsR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlabR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WallR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoundationR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ElementType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Material = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReinforcementRat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostInformationGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.generalReinforcementRatios)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonClose.Location = new System.Drawing.Point(859, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(23, 23);
            this.buttonClose.TabIndex = 13;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ExportData
            // 
            this.ExportData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.ExportData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExportData.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ExportData.Location = new System.Drawing.Point(113, 24);
            this.ExportData.Name = "ExportData";
            this.ExportData.Size = new System.Drawing.Size(103, 23);
            this.ExportData.TabIndex = 15;
            this.ExportData.Text = "Select Elements";
            this.ExportData.UseVisualStyleBackColor = false;
            this.ExportData.Click += new System.EventHandler(this.CostAnalysis_Click);
            // 
            // CostInformationGrid
            // 
            this.CostInformationGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CostInformationGrid.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.CostInformationGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CostInformationGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Category,
            this.ElementType,
            this.Material,
            this.Volume,
            this.ReinforcementRat,
            this.Length,
            this.UnitCost,
            this.TotalCost});
            this.CostInformationGrid.Location = new System.Drawing.Point(12, 187);
            this.CostInformationGrid.Name = "CostInformationGrid";
            this.CostInformationGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CostInformationGrid.Size = new System.Drawing.Size(870, 291);
            this.CostInformationGrid.TabIndex = 16;
            this.CostInformationGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CostInformationGrid_CellClick);
            // 
            // ClearData
            // 
            this.ClearData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.ClearData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ClearData.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ClearData.Location = new System.Drawing.Point(759, 24);
            this.ClearData.Name = "ClearData";
            this.ClearData.Size = new System.Drawing.Size(80, 23);
            this.ClearData.TabIndex = 15;
            this.ClearData.Text = "Clear Data";
            this.ClearData.UseVisualStyleBackColor = false;
            this.ClearData.Click += new System.EventHandler(this.ClearData_Click);
            // 
            // IncNaming
            // 
            this.IncNaming.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.IncNaming.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.IncNaming.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.IncNaming.Location = new System.Drawing.Point(238, 24);
            this.IncNaming.Name = "IncNaming";
            this.IncNaming.Size = new System.Drawing.Size(103, 23);
            this.IncNaming.TabIndex = 17;
            this.IncNaming.Text = "IncNaming";
            this.IncNaming.UseVisualStyleBackColor = false;
            this.IncNaming.Click += new System.EventHandler(this.IncNaming_Click);
            // 
            // VolNotCal
            // 
            this.VolNotCal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.VolNotCal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.VolNotCal.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.VolNotCal.Location = new System.Drawing.Point(363, 24);
            this.VolNotCal.Name = "VolNotCal";
            this.VolNotCal.Size = new System.Drawing.Size(135, 23);
            this.VolNotCal.TabIndex = 18;
            this.VolNotCal.Text = "Dims Not Calculated";
            this.VolNotCal.UseVisualStyleBackColor = false;
            this.VolNotCal.Click += new System.EventHandler(this.VolNotCal_Click);
            // 
            // IncNamingNo
            // 
            this.IncNamingNo.Location = new System.Drawing.Point(238, 66);
            this.IncNamingNo.Name = "IncNamingNo";
            this.IncNamingNo.Size = new System.Drawing.Size(100, 20);
            this.IncNamingNo.TabIndex = 19;
            // 
            // NotCalcNo
            // 
            this.NotCalcNo.Location = new System.Drawing.Point(363, 66);
            this.NotCalcNo.Name = "NotCalcNo";
            this.NotCalcNo.Size = new System.Drawing.Size(100, 20);
            this.NotCalcNo.TabIndex = 20;
            // 
            // RemainingSel
            // 
            this.RemainingSel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.RemainingSel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RemainingSel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.RemainingSel.Location = new System.Drawing.Point(518, 24);
            this.RemainingSel.Name = "RemainingSel";
            this.RemainingSel.Size = new System.Drawing.Size(135, 23);
            this.RemainingSel.TabIndex = 18;
            this.RemainingSel.Text = "RemainingSel";
            this.RemainingSel.UseVisualStyleBackColor = false;
            this.RemainingSel.Click += new System.EventHandler(this.RemainingSel_Click);
            // 
            // remainingCounter
            // 
            this.remainingCounter.Location = new System.Drawing.Point(518, 66);
            this.remainingCounter.Name = "remainingCounter";
            this.remainingCounter.Size = new System.Drawing.Size(100, 20);
            this.remainingCounter.TabIndex = 20;
            // 
            // SelectElementsFromRows
            // 
            this.SelectElementsFromRows.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.SelectElementsFromRows.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SelectElementsFromRows.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SelectElementsFromRows.Location = new System.Drawing.Point(41, 63);
            this.SelectElementsFromRows.Name = "SelectElementsFromRows";
            this.SelectElementsFromRows.Size = new System.Drawing.Size(175, 23);
            this.SelectElementsFromRows.TabIndex = 15;
            this.SelectElementsFromRows.Text = "Select Element from Row";
            this.SelectElementsFromRows.UseVisualStyleBackColor = false;
            this.SelectElementsFromRows.Click += new System.EventHandler(this.SelectElementsFromRows_Click);
            // 
            // generalReinforcementRatios
            // 
            this.generalReinforcementRatios.AllowUserToAddRows = false;
            this.generalReinforcementRatios.AllowUserToDeleteRows = false;
            this.generalReinforcementRatios.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generalReinforcementRatios.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.generalReinforcementRatios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.generalReinforcementRatios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BeamsR,
            this.ColumnR,
            this.SlabR,
            this.WallR,
            this.FoundationR});
            this.generalReinforcementRatios.Location = new System.Drawing.Point(12, 128);
            this.generalReinforcementRatios.Name = "generalReinforcementRatios";
            this.generalReinforcementRatios.Size = new System.Drawing.Size(870, 53);
            this.generalReinforcementRatios.TabIndex = 21;
            // 
            // BeamsR
            // 
            this.BeamsR.HeaderText = "Beams Reinforcement";
            this.BeamsR.Name = "BeamsR";
            this.BeamsR.Width = 160;
            // 
            // ColumnR
            // 
            this.ColumnR.HeaderText = "Column Reinforcement";
            this.ColumnR.Name = "ColumnR";
            this.ColumnR.Width = 160;
            // 
            // SlabR
            // 
            this.SlabR.HeaderText = "Slab Reinforcement";
            this.SlabR.Name = "SlabR";
            this.SlabR.Width = 160;
            // 
            // WallR
            // 
            this.WallR.HeaderText = "Wall Reinforcement";
            this.WallR.Name = "WallR";
            this.WallR.Width = 160;
            // 
            // FoundationR
            // 
            this.FoundationR.HeaderText = "Foundation Reinforcement";
            this.FoundationR.Name = "FoundationR";
            this.FoundationR.Width = 160;
            // 
            // pathTextBox
            // 
            this.pathTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pathTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.pathTextBox.Location = new System.Drawing.Point(13, 96);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(869, 23);
            this.pathTextBox.TabIndex = 22;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            // 
            // ElementType
            // 
            this.ElementType.HeaderText = "ElementType";
            this.ElementType.Name = "ElementType";
            // 
            // Material
            // 
            this.Material.HeaderText = "Material";
            this.Material.Name = "Material";
            // 
            // Volume
            // 
            this.Volume.HeaderText = "Volume";
            this.Volume.Name = "Volume";
            // 
            // ReinforcementRat
            // 
            this.ReinforcementRat.HeaderText = "Reinf Ratio / Mass per [m]";
            this.ReinforcementRat.Name = "ReinforcementRat";
            // 
            // Length
            // 
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            // 
            // UnitCost
            // 
            this.UnitCost.HeaderText = "UnitCost";
            this.UnitCost.Name = "UnitCost";
            // 
            // TotalCost
            // 
            this.TotalCost.HeaderText = "Total Cost";
            this.TotalCost.Name = "TotalCost";
            // 
            // StructureCostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(894, 490);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.generalReinforcementRatios);
            this.Controls.Add(this.remainingCounter);
            this.Controls.Add(this.NotCalcNo);
            this.Controls.Add(this.IncNamingNo);
            this.Controls.Add(this.RemainingSel);
            this.Controls.Add(this.VolNotCal);
            this.Controls.Add(this.IncNaming);
            this.Controls.Add(this.CostInformationGrid);
            this.Controls.Add(this.ClearData);
            this.Controls.Add(this.SelectElementsFromRows);
            this.Controls.Add(this.ExportData);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StructureCostForm";
            this.Text = "Cost Information";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostInformationGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.generalReinforcementRatios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button ExportData;
        private System.Windows.Forms.DataGridView CostInformationGrid;
        private System.Windows.Forms.Button ClearData;
        private System.Windows.Forms.Button IncNaming;
        private System.Windows.Forms.Button VolNotCal;
        private System.Windows.Forms.TextBox IncNamingNo;
        private System.Windows.Forms.TextBox NotCalcNo;
        private System.Windows.Forms.Button RemainingSel;
        private System.Windows.Forms.TextBox remainingCounter;
        private Button SelectElementsFromRows;
        private DataGridView generalReinforcementRatios;
        private DataGridViewTextBoxColumn BeamsR;
        private DataGridViewTextBoxColumn ColumnR;
        private DataGridViewTextBoxColumn SlabR;
        private DataGridViewTextBoxColumn WallR;
        private DataGridViewTextBoxColumn FoundationR;
        private TextBox pathTextBox;
        private DataGridViewTextBoxColumn Category;
        private DataGridViewTextBoxColumn ElementType;
        private DataGridViewTextBoxColumn Material;
        private DataGridViewTextBoxColumn Volume;
        private DataGridViewTextBoxColumn ReinforcementRat;
        private DataGridViewTextBoxColumn Length;
        private DataGridViewTextBoxColumn UnitCost;
        private DataGridViewTextBoxColumn TotalCost;
    }
}