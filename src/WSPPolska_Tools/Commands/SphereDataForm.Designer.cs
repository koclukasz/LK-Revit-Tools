namespace WSPPolska_Tools.Commands
{
    partial class SphereDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SphereDataForm));
            this.SphereId = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sphereIdBox = new System.Windows.Forms.TextBox();
            this.elemsIds1 = new System.Windows.Forms.Button();
            this.elemsIds1Box = new System.Windows.Forms.TextBox();
            this.elemsIds2Box = new System.Windows.Forms.TextBox();
            this.elemsIds2 = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SphereId
            // 
            this.SphereId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.SphereId.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SphereId.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SphereId.Location = new System.Drawing.Point(58, 68);
            this.SphereId.Name = "SphereId";
            this.SphereId.Size = new System.Drawing.Size(75, 23);
            this.SphereId.TabIndex = 0;
            this.SphereId.Text = "Sphere Id";
            this.SphereId.UseVisualStyleBackColor = true;
            this.SphereId.Click += new System.EventHandler(this.SphereId_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // sphereIdBox
            // 
            this.sphereIdBox.Location = new System.Drawing.Point(45, 106);
            this.sphereIdBox.Name = "sphereIdBox";
            this.sphereIdBox.Size = new System.Drawing.Size(100, 20);
            this.sphereIdBox.TabIndex = 14;
            // 
            // elemsIds1
            // 
            this.elemsIds1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.elemsIds1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.elemsIds1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.elemsIds1.Location = new System.Drawing.Point(187, 68);
            this.elemsIds1.Name = "elemsIds1";
            this.elemsIds1.Size = new System.Drawing.Size(87, 23);
            this.elemsIds1.TabIndex = 0;
            this.elemsIds1.Text = "Get Elems1 Ids";
            this.elemsIds1.UseVisualStyleBackColor = true;
            this.elemsIds1.Click += new System.EventHandler(this.ElemsIds1_Click);
            // 
            // elemsIds1Box
            // 
            this.elemsIds1Box.Location = new System.Drawing.Point(170, 106);
            this.elemsIds1Box.Name = "elemsIds1Box";
            this.elemsIds1Box.Size = new System.Drawing.Size(119, 20);
            this.elemsIds1Box.TabIndex = 14;
            // 
            // elemsIds2Box
            // 
            this.elemsIds2Box.Location = new System.Drawing.Point(308, 106);
            this.elemsIds2Box.Name = "elemsIds2Box";
            this.elemsIds2Box.Size = new System.Drawing.Size(119, 20);
            this.elemsIds2Box.TabIndex = 16;
            // 
            // elemsIds2
            // 
            this.elemsIds2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.elemsIds2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.elemsIds2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.elemsIds2.Location = new System.Drawing.Point(325, 68);
            this.elemsIds2.Name = "elemsIds2";
            this.elemsIds2.Size = new System.Drawing.Size(87, 23);
            this.elemsIds2.TabIndex = 15;
            this.elemsIds2.Text = "Get Elems2 Ids";
            this.elemsIds2.UseVisualStyleBackColor = true;
            this.elemsIds2.Click += new System.EventHandler(this.elemsIds2_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonClose.Location = new System.Drawing.Point(449, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(23, 23);
            this.buttonClose.TabIndex = 17;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // SphereDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(484, 169);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.elemsIds2Box);
            this.Controls.Add(this.elemsIds2);
            this.Controls.Add(this.elemsIds1Box);
            this.Controls.Add(this.sphereIdBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.elemsIds1);
            this.Controls.Add(this.SphereId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SphereDataForm";
            this.Text = "Speheres Data";
            this.Load += new System.EventHandler(this.SphereDataForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SphereId;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox sphereIdBox;
        private System.Windows.Forms.TextBox elemsIds1Box;
        private System.Windows.Forms.Button elemsIds1;
        private System.Windows.Forms.TextBox elemsIds2Box;
        private System.Windows.Forms.Button elemsIds2;
        private System.Windows.Forms.Button buttonClose;
    }
}