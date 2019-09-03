﻿namespace G1Tool
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureListBox = new System.Windows.Forms.ListBox();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.dimensionsGroupBox = new System.Windows.Forms.GroupBox();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxCompression = new System.Windows.Forms.ComboBox();
            this.labelCompression = new System.Windows.Forms.Label();
            this.checkBoxNormalMap = new System.Windows.Forms.CheckBox();
            this.numericUpDownMipMap = new System.Windows.Forms.NumericUpDown();
            this.mipmapCountLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxExHeader = new System.Windows.Forms.CheckBox();
            this.previewGroupBox = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.settingsGroupBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.dimensionsGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMipMap)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.previewGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(845, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // textureListBox
            // 
            this.textureListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.textureListBox.FormattingEnabled = true;
            this.textureListBox.Location = new System.Drawing.Point(0, 24);
            this.textureListBox.Name = "textureListBox";
            this.textureListBox.Size = new System.Drawing.Size(120, 538);
            this.textureListBox.TabIndex = 1;
            this.textureListBox.SelectedIndexChanged += new System.EventHandler(this.TextureListBox_SelectedIndexChanged);
            this.textureListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextureListBox_MouseDown);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.flowLayoutPanel1);
            this.settingsGroupBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingsGroupBox.Location = new System.Drawing.Point(645, 24);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(200, 538);
            this.settingsGroupBox.TabIndex = 2;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Settings";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.dimensionsGroupBox);
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(194, 519);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // dimensionsGroupBox
            // 
            this.dimensionsGroupBox.Controls.Add(this.heightLabel);
            this.dimensionsGroupBox.Controls.Add(this.widthLabel);
            this.dimensionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.dimensionsGroupBox.Name = "dimensionsGroupBox";
            this.dimensionsGroupBox.Size = new System.Drawing.Size(188, 65);
            this.dimensionsGroupBox.TabIndex = 0;
            this.dimensionsGroupBox.TabStop = false;
            this.dimensionsGroupBox.Text = "Dimensions";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(7, 42);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(41, 13);
            this.heightLabel.TabIndex = 1;
            this.heightLabel.Text = "Height:";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(7, 20);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(38, 13);
            this.widthLabel.TabIndex = 0;
            this.widthLabel.Text = "Width:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxCompression);
            this.groupBox2.Controls.Add(this.labelCompression);
            this.groupBox2.Controls.Add(this.checkBoxNormalMap);
            this.groupBox2.Controls.Add(this.numericUpDownMipMap);
            this.groupBox2.Controls.Add(this.mipmapCountLabel);
            this.groupBox2.Location = new System.Drawing.Point(3, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(188, 134);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Attributes";
            // 
            // comboBoxCompression
            // 
            this.comboBoxCompression.Enabled = false;
            this.comboBoxCompression.FormattingEnabled = true;
            this.comboBoxCompression.Location = new System.Drawing.Point(13, 80);
            this.comboBoxCompression.Name = "comboBoxCompression";
            this.comboBoxCompression.Size = new System.Drawing.Size(106, 21);
            this.comboBoxCompression.TabIndex = 4;
            this.comboBoxCompression.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCompression_SelectedIndexChanged);
            // 
            // labelCompression
            // 
            this.labelCompression.AutoSize = true;
            this.labelCompression.Location = new System.Drawing.Point(13, 63);
            this.labelCompression.Name = "labelCompression";
            this.labelCompression.Size = new System.Drawing.Size(102, 13);
            this.labelCompression.TabIndex = 3;
            this.labelCompression.Text = "Compression format:";
            // 
            // checkBoxNormalMap
            // 
            this.checkBoxNormalMap.AutoSize = true;
            this.checkBoxNormalMap.Enabled = false;
            this.checkBoxNormalMap.Location = new System.Drawing.Point(13, 107);
            this.checkBoxNormalMap.Name = "checkBoxNormalMap";
            this.checkBoxNormalMap.Size = new System.Drawing.Size(82, 17);
            this.checkBoxNormalMap.TabIndex = 2;
            this.checkBoxNormalMap.Text = "Normal map";
            this.checkBoxNormalMap.UseVisualStyleBackColor = true;
            this.checkBoxNormalMap.CheckedChanged += new System.EventHandler(this.CheckBoxNormalMap_CheckedChanged);
            // 
            // numericUpDownMipMap
            // 
            this.numericUpDownMipMap.Enabled = false;
            this.numericUpDownMipMap.Location = new System.Drawing.Point(13, 36);
            this.numericUpDownMipMap.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownMipMap.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMipMap.Name = "numericUpDownMipMap";
            this.numericUpDownMipMap.Size = new System.Drawing.Size(106, 20);
            this.numericUpDownMipMap.TabIndex = 1;
            this.numericUpDownMipMap.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMipMap.ValueChanged += new System.EventHandler(this.NumericUpDownMipMap_ValueChanged);
            // 
            // mipmapCountLabel
            // 
            this.mipmapCountLabel.AutoSize = true;
            this.mipmapCountLabel.Location = new System.Drawing.Point(10, 20);
            this.mipmapCountLabel.Name = "mipmapCountLabel";
            this.mipmapCountLabel.Size = new System.Drawing.Size(160, 13);
            this.mipmapCountLabel.TabIndex = 0;
            this.mipmapCountLabel.Text = "Mipmap count (includes texture):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxExHeader);
            this.groupBox1.Location = new System.Drawing.Point(3, 214);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 44);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unknowns";
            // 
            // checkBoxExHeader
            // 
            this.checkBoxExHeader.AutoSize = true;
            this.checkBoxExHeader.Enabled = false;
            this.checkBoxExHeader.Location = new System.Drawing.Point(13, 19);
            this.checkBoxExHeader.Name = "checkBoxExHeader";
            this.checkBoxExHeader.Size = new System.Drawing.Size(115, 17);
            this.checkBoxExHeader.TabIndex = 2;
            this.checkBoxExHeader.Text = "Uses Extra Header";
            this.checkBoxExHeader.UseVisualStyleBackColor = true;
            // 
            // previewGroupBox
            // 
            this.previewGroupBox.Controls.Add(this.pictureBox1);
            this.previewGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewGroupBox.Location = new System.Drawing.Point(120, 24);
            this.previewGroupBox.Name = "previewGroupBox";
            this.previewGroupBox.Size = new System.Drawing.Size(525, 538);
            this.previewGroupBox.TabIndex = 3;
            this.previewGroupBox.TabStop = false;
            this.previewGroupBox.Text = "Preview";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(519, 519);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 562);
            this.Controls.Add(this.previewGroupBox);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.textureListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Raytwo\'s Slighty Less Empty G1T Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.settingsGroupBox.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.dimensionsGroupBox.ResumeLayout(false);
            this.dimensionsGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMipMap)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.previewGroupBox.ResumeLayout(false);
            this.previewGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ListBox textureListBox;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.GroupBox previewGroupBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox dimensionsGroupBox;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownMipMap;
        private System.Windows.Forms.Label mipmapCountLabel;
        private System.Windows.Forms.CheckBox checkBoxExHeader;
        private System.Windows.Forms.CheckBox checkBoxNormalMap;
        private System.Windows.Forms.ComboBox comboBoxCompression;
        private System.Windows.Forms.Label labelCompression;
    }
}

