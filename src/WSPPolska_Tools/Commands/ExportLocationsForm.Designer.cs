namespace WSPPolska_Tools.Commands
{
    partial class ExportLocationsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportLocationsForm));
            this.viewSelection = new System.Windows.Forms.ComboBox();
            this.exportIFC = new System.Windows.Forms.CheckBox();
            this.exportNWC = new System.Windows.Forms.CheckBox();
            this.exportDWFx = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.selectDirectory = new System.Windows.Forms.Button();
            this.nameExample = new System.Windows.Forms.TextBox();
            this.namingSt = new System.Windows.Forms.ComboBox();
            this.exportFiles = new System.Windows.Forms.Button();
            this.ifcExportOpt = new System.Windows.Forms.ComboBox();
            this.exportLinks = new System.Windows.Forms.CheckBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // viewSelection
            // 
            this.viewSelection.BackColor = System.Drawing.Color.WhiteSmoke;
            this.viewSelection.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.viewSelection.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.viewSelection.FormattingEnabled = true;
            this.viewSelection.Location = new System.Drawing.Point(25, 62);
            this.viewSelection.Name = "viewSelection";
            this.viewSelection.Size = new System.Drawing.Size(121, 23);
            this.viewSelection.TabIndex = 0;
            this.viewSelection.Text = "viewSelection";
            this.viewSelection.SelectedIndexChanged += new System.EventHandler(this.viewSelection_SelectedIndexChanged);
            // 
            // exportIFC
            // 
            this.exportIFC.AutoSize = true;
            this.exportIFC.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exportIFC.Location = new System.Drawing.Point(202, 65);
            this.exportIFC.Name = "exportIFC";
            this.exportIFC.Size = new System.Drawing.Size(42, 19);
            this.exportIFC.TabIndex = 1;
            this.exportIFC.Text = "IFC";
            this.exportIFC.UseVisualStyleBackColor = true;
            // 
            // exportNWC
            // 
            this.exportNWC.AutoSize = true;
            this.exportNWC.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exportNWC.Location = new System.Drawing.Point(202, 88);
            this.exportNWC.Name = "exportNWC";
            this.exportNWC.Size = new System.Drawing.Size(54, 19);
            this.exportNWC.TabIndex = 2;
            this.exportNWC.Text = "NWC";
            this.exportNWC.UseVisualStyleBackColor = true;
            // 
            // exportDWFx
            // 
            this.exportDWFx.AutoSize = true;
            this.exportDWFx.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exportDWFx.Location = new System.Drawing.Point(202, 111);
            this.exportDWFx.Name = "exportDWFx";
            this.exportDWFx.Size = new System.Drawing.Size(58, 19);
            this.exportDWFx.TabIndex = 3;
            this.exportDWFx.Text = "DWFx";
            this.exportDWFx.UseVisualStyleBackColor = true;
            // 
            // selectDirectory
            // 
            this.selectDirectory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.selectDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.selectDirectory.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.selectDirectory.Location = new System.Drawing.Point(25, 89);
            this.selectDirectory.Name = "selectDirectory";
            this.selectDirectory.Size = new System.Drawing.Size(121, 23);
            this.selectDirectory.TabIndex = 4;
            this.selectDirectory.Text = "Select Folder";
            this.selectDirectory.UseVisualStyleBackColor = false;
            this.selectDirectory.Click += new System.EventHandler(this.selectDirectory_Click);
            // 
            // nameExample
            // 
            this.nameExample.BackColor = System.Drawing.Color.WhiteSmoke;
            this.nameExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameExample.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameExample.Location = new System.Drawing.Point(25, 157);
            this.nameExample.Name = "nameExample";
            this.nameExample.Size = new System.Drawing.Size(330, 16);
            this.nameExample.TabIndex = 5;
            this.nameExample.Text = "Name Example";
            // 
            // namingSt
            // 
            this.namingSt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.namingSt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.namingSt.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.namingSt.FormattingEnabled = true;
            this.namingSt.Items.AddRange(new object[] {
            "Prefix",
            "Suffix"});
            this.namingSt.Location = new System.Drawing.Point(400, 60);
            this.namingSt.Name = "namingSt";
            this.namingSt.Size = new System.Drawing.Size(121, 23);
            this.namingSt.TabIndex = 0;
            this.namingSt.Text = "Naming Standard";
            this.namingSt.SelectedIndexChanged += new System.EventHandler(this.namingSt_SelectedIndexChanged);
            // 
            // exportFiles
            // 
            this.exportFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.exportFiles.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.exportFiles.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exportFiles.Location = new System.Drawing.Point(550, 60);
            this.exportFiles.Name = "exportFiles";
            this.exportFiles.Size = new System.Drawing.Size(121, 23);
            this.exportFiles.TabIndex = 4;
            this.exportFiles.Text = "Export Files";
            this.exportFiles.UseVisualStyleBackColor = false;
            this.exportFiles.Click += new System.EventHandler(this.ExportLocations_Click);
            // 
            // ifcExportOpt
            // 
            this.ifcExportOpt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ifcExportOpt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ifcExportOpt.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ifcExportOpt.FormattingEnabled = true;
            this.ifcExportOpt.Location = new System.Drawing.Point(263, 61);
            this.ifcExportOpt.Name = "ifcExportOpt";
            this.ifcExportOpt.Size = new System.Drawing.Size(121, 23);
            this.ifcExportOpt.TabIndex = 0;
            this.ifcExportOpt.Text = "IFC Export Opt";
            this.ifcExportOpt.SelectedIndexChanged += new System.EventHandler(this.namingSt_SelectedIndexChanged);
            // 
            // exportLinks
            // 
            this.exportLinks.AutoSize = true;
            this.exportLinks.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exportLinks.Location = new System.Drawing.Point(202, 135);
            this.exportLinks.Name = "exportLinks";
            this.exportLinks.Size = new System.Drawing.Size(92, 19);
            this.exportLinks.TabIndex = 6;
            this.exportLinks.Text = "Export Links";
            this.exportLinks.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonClose.Location = new System.Drawing.Point(696, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(23, 23);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // ExportLocationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(731, 216);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.exportLinks);
            this.Controls.Add(this.nameExample);
            this.Controls.Add(this.exportFiles);
            this.Controls.Add(this.selectDirectory);
            this.Controls.Add(this.exportDWFx);
            this.Controls.Add(this.exportNWC);
            this.Controls.Add(this.exportIFC);
            this.Controls.Add(this.ifcExportOpt);
            this.Controls.Add(this.namingSt);
            this.Controls.Add(this.viewSelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExportLocationsForm";
            this.Text = "Export Locations";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox viewSelection;
        private System.Windows.Forms.CheckBox exportIFC;
        private System.Windows.Forms.CheckBox exportNWC;
        private System.Windows.Forms.CheckBox exportDWFx;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button selectDirectory;
        private System.Windows.Forms.TextBox nameExample;
        private System.Windows.Forms.ComboBox namingSt;
        private System.Windows.Forms.Button exportFiles;
        private System.Windows.Forms.ComboBox ifcExportOpt;
        private System.Windows.Forms.CheckBox exportLinks;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}