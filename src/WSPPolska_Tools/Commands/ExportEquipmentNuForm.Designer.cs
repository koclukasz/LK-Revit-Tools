namespace WSPPolska_Tools
{
    partial class ExportEquipmentNuForm
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
            this.tabList = new System.Windows.Forms.ComboBox();
            this.ExportImpEquip = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.selectedParam = new System.Windows.Forms.ComboBox();
            this.customParameter = new System.Windows.Forms.TextBox();
            this.numberingScheme = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tabList
            // 
            this.tabList.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabList.FormattingEnabled = true;
            this.tabList.Location = new System.Drawing.Point(53, 58);
            this.tabList.Name = "tabList";
            this.tabList.Size = new System.Drawing.Size(121, 21);
            this.tabList.TabIndex = 0;
            // 
            // ExportImpEquip
            // 
            this.ExportImpEquip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportImpEquip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(66)))), ((int)(((byte)(58)))));
            this.ExportImpEquip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportImpEquip.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ExportImpEquip.Location = new System.Drawing.Point(413, 58);
            this.ExportImpEquip.Name = "ExportImpEquip";
            this.ExportImpEquip.Size = new System.Drawing.Size(90, 23);
            this.ExportImpEquip.TabIndex = 1;
            this.ExportImpEquip.Text = "Data Export";
            this.ExportImpEquip.UseVisualStyleBackColor = false;
            this.ExportImpEquip.Click += new System.EventHandler(this.ExportImpEquip_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonClose.Location = new System.Drawing.Point(531, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(23, 23);
            this.buttonClose.TabIndex = 11;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // selectedParam
            // 
            this.selectedParam.BackColor = System.Drawing.Color.WhiteSmoke;
            this.selectedParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectedParam.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.selectedParam.FormattingEnabled = true;
            this.selectedParam.Location = new System.Drawing.Point(214, 58);
            this.selectedParam.Name = "selectedParam";
            this.selectedParam.Size = new System.Drawing.Size(121, 22);
            this.selectedParam.TabIndex = 0;
            this.selectedParam.Text = "Parameter Name";
            // 
            // customParameter
            // 
            this.customParameter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.customParameter.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.customParameter.Location = new System.Drawing.Point(214, 86);
            this.customParameter.Name = "customParameter";
            this.customParameter.Size = new System.Drawing.Size(121, 22);
            this.customParameter.TabIndex = 12;
            this.customParameter.Text = "Custom Parameter";
            // 
            // numberingScheme
            // 
            this.numberingScheme.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numberingScheme.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numberingScheme.Location = new System.Drawing.Point(214, 123);
            this.numberingScheme.Name = "numberingScheme";
            this.numberingScheme.Size = new System.Drawing.Size(276, 22);
            this.numberingScheme.TabIndex = 13;
            this.numberingScheme.Text = "Insert Numbering Scheme";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(214, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 39);
            this.label1.TabIndex = 14;
            this.label1.Text = "Please provide expression for Equipment Number\nFor each letter use \"L\" for digit " +
    "use \"N\"\n LLL_LNN_NNN will be correct for MEC_L00_001";
            // 
            // ExportEquipmentNuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(566, 214);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numberingScheme);
            this.Controls.Add(this.customParameter);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.ExportImpEquip);
            this.Controls.Add(this.selectedParam);
            this.Controls.Add(this.tabList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExportEquipmentNuForm";
            this.Text = "Equipment No Verification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox tabList;
        private System.Windows.Forms.Button ExportImpEquip;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox selectedParam;
        private System.Windows.Forms.TextBox customParameter;
        private System.Windows.Forms.TextBox numberingScheme;
        private System.Windows.Forms.Label label1;
    }
}