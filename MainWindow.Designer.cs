
namespace SGB2_Border_Injector
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.buttonFileSelect = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxImage = new System.Windows.Forms.GroupBox();
            this.buttonSaveDitheredImage = new System.Windows.Forms.Button();
            this.groupBoxFile = new System.Windows.Forms.GroupBox();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.buttonInject = new System.Windows.Forms.Button();
            this.comboBoxSlot = new System.Windows.Forms.ComboBox();
            this.groupBoxSlot = new System.Windows.Forms.GroupBox();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.buttonLoadIcon = new System.Windows.Forms.Button();
            this.comboBoxIcon = new System.Windows.Forms.ComboBox();
            this.checkBoxExternalPalettes = new System.Windows.Forms.CheckBox();
            this.checkBoxBackup = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxDither = new System.Windows.Forms.CheckBox();
            this.checkBoxSetStartup = new System.Windows.Forms.CheckBox();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBoxImage.SuspendLayout();
            this.groupBoxFile.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxSlot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxFilename
            // 
            this.textBoxFilename.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxFilename.Location = new System.Drawing.Point(16, 24);
            this.textBoxFilename.Name = "textBoxFilename";
            this.textBoxFilename.ReadOnly = true;
            this.textBoxFilename.Size = new System.Drawing.Size(178, 20);
            this.textBoxFilename.TabIndex = 0;
            this.textBoxFilename.TabStop = false;
            // 
            // buttonFileSelect
            // 
            this.buttonFileSelect.Location = new System.Drawing.Point(200, 22);
            this.buttonFileSelect.Name = "buttonFileSelect";
            this.buttonFileSelect.Size = new System.Drawing.Size(74, 23);
            this.buttonFileSelect.TabIndex = 1;
            this.buttonFileSelect.Text = "Select File";
            this.buttonFileSelect.UseVisualStyleBackColor = true;
            this.buttonFileSelect.Click += new System.EventHandler(this.buttonFileSelect_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxOutput.Location = new System.Drawing.Point(16, 24);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(527, 152);
            this.textBoxOutput.TabIndex = 0;
            this.textBoxOutput.Text = "Image requirements: 256 × 224 px, with a maximum of 3 × 15 color palettes.\r\n\r\n   " +
    "  Version 1.0 Beta1\r\n       - by blizzz";
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Location = new System.Drawing.Point(16, 24);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(257, 23);
            this.buttonLoadImage.TabIndex = 0;
            this.buttonLoadImage.Text = "Select Border Image";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox.Location = new System.Drawing.Point(1, 1);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(256, 224);
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Location = new System.Drawing.Point(313, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 226);
            this.panel1.TabIndex = 9;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Lossless Image files|*.png; *.bmp|All files|*.*";
            this.openFileDialog.Title = "Select Border Image";
            // 
            // groupBoxImage
            // 
            this.groupBoxImage.Controls.Add(this.buttonSaveDitheredImage);
            this.groupBoxImage.Controls.Add(this.buttonLoadImage);
            this.groupBoxImage.Location = new System.Drawing.Point(12, 76);
            this.groupBoxImage.Name = "groupBoxImage";
            this.groupBoxImage.Size = new System.Drawing.Size(290, 63);
            this.groupBoxImage.TabIndex = 1;
            this.groupBoxImage.TabStop = false;
            this.groupBoxImage.Text = "Border Image";
            // 
            // buttonSaveDitheredImage
            // 
            this.buttonSaveDitheredImage.Location = new System.Drawing.Point(200, 24);
            this.buttonSaveDitheredImage.Name = "buttonSaveDitheredImage";
            this.buttonSaveDitheredImage.Size = new System.Drawing.Size(74, 23);
            this.buttonSaveDitheredImage.TabIndex = 1;
            this.buttonSaveDitheredImage.Text = "Save Image";
            this.toolTip.SetToolTip(this.buttonSaveDitheredImage, "Save color reduced /dithered image.");
            this.buttonSaveDitheredImage.UseVisualStyleBackColor = true;
            this.buttonSaveDitheredImage.Visible = false;
            this.buttonSaveDitheredImage.Click += new System.EventHandler(this.buttonSaveDitheredImage_Click);
            // 
            // groupBoxFile
            // 
            this.groupBoxFile.Controls.Add(this.textBoxFilename);
            this.groupBoxFile.Controls.Add(this.buttonFileSelect);
            this.groupBoxFile.Location = new System.Drawing.Point(12, 7);
            this.groupBoxFile.Name = "groupBoxFile";
            this.groupBoxFile.Size = new System.Drawing.Size(290, 63);
            this.groupBoxFile.TabIndex = 0;
            this.groupBoxFile.TabStop = false;
            this.groupBoxFile.Text = "Super Game Boy 2 ROM File";
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Controls.Add(this.textBoxOutput);
            this.groupBoxOutput.Location = new System.Drawing.Point(11, 286);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(560, 192);
            this.groupBoxOutput.TabIndex = 8;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // buttonInject
            // 
            this.buttonInject.Location = new System.Drawing.Point(27, 251);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(258, 23);
            this.buttonInject.TabIndex = 7;
            this.buttonInject.Text = "Inject Border";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.buttonInject_Click);
            // 
            // comboBoxSlot
            // 
            this.comboBoxSlot.FormattingEnabled = true;
            this.comboBoxSlot.Items.AddRange(new object[] {
            "3 (Printed Circuit Board)",
            "4 (Palm Trees)",
            "5 (Stone Mosaic)",
            "6 (Gears)",
            "7 (Swamp)",
            "8 (Dolphins)",
            "9 (Chess Arena)",
            "3\' (SGB1 Windows, 12 KB)",
            "4\' (SGB1 Cork Board, 9 KB)",
            "5\' (SGB1 Log Cabin, 12 KB)",
            "6\' (SGB1 Movie Theater, 12 KB)",
            "7\' (SGB1 Cats, 10 KB)",
            "8\' (SGB1 Desk, 11 KB)",
            "9\' (SGB1 Escher, 9 KB)"});
            this.comboBoxSlot.Location = new System.Drawing.Point(16, 24);
            this.comboBoxSlot.Name = "comboBoxSlot";
            this.comboBoxSlot.Size = new System.Drawing.Size(258, 21);
            this.comboBoxSlot.TabIndex = 0;
            this.comboBoxSlot.Text = "3 (Printed Circuit Board)";
            // 
            // groupBoxSlot
            // 
            this.groupBoxSlot.Controls.Add(this.pictureBoxIcon);
            this.groupBoxSlot.Controls.Add(this.buttonLoadIcon);
            this.groupBoxSlot.Controls.Add(this.comboBoxIcon);
            this.groupBoxSlot.Controls.Add(this.comboBoxSlot);
            this.groupBoxSlot.Location = new System.Drawing.Point(12, 145);
            this.groupBoxSlot.Name = "groupBoxSlot";
            this.groupBoxSlot.Size = new System.Drawing.Size(290, 89);
            this.groupBoxSlot.TabIndex = 2;
            this.groupBoxSlot.TabStop = false;
            this.groupBoxSlot.Text = "Border Slot";
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Image = global::SGB2_Border_Injector.Properties.Resources.icon_goose;
            this.pictureBoxIcon.InitialImage = null;
            this.pictureBoxIcon.Location = new System.Drawing.Point(183, 55);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(12, 16);
            this.pictureBoxIcon.TabIndex = 8;
            this.pictureBoxIcon.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxIcon, "Click to toggle between active and inactive display.");
            this.pictureBoxIcon.Click += new System.EventHandler(this.pictureBoxIcon_Click);
            this.pictureBoxIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxIcon_Paint);
            // 
            // buttonLoadIcon
            // 
            this.buttonLoadIcon.Location = new System.Drawing.Point(199, 51);
            this.buttonLoadIcon.Name = "buttonLoadIcon";
            this.buttonLoadIcon.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadIcon.TabIndex = 2;
            this.buttonLoadIcon.Text = "Add Icon";
            this.toolTip.SetToolTip(this.buttonLoadIcon, "Add custom icon. See readme for more information.");
            this.buttonLoadIcon.UseVisualStyleBackColor = true;
            this.buttonLoadIcon.Click += new System.EventHandler(this.buttonLoadIcon_Click);
            // 
            // comboBoxIcon
            // 
            this.comboBoxIcon.FormattingEnabled = true;
            this.comboBoxIcon.Items.AddRange(new object[] {
            "Don\'t change icon",
            "Icon: Goose"});
            this.comboBoxIcon.Location = new System.Drawing.Point(16, 52);
            this.comboBoxIcon.Name = "comboBoxIcon";
            this.comboBoxIcon.Size = new System.Drawing.Size(162, 21);
            this.comboBoxIcon.TabIndex = 1;
            this.comboBoxIcon.Text = "Icon: Goose";
            this.comboBoxIcon.SelectedIndexChanged += new System.EventHandler(this.comboBoxIcon_SelectedIndexChanged);
            // 
            // checkBoxExternalPalettes
            // 
            this.checkBoxExternalPalettes.AutoSize = true;
            this.checkBoxExternalPalettes.Location = new System.Drawing.Point(427, 267);
            this.checkBoxExternalPalettes.Name = "checkBoxExternalPalettes";
            this.checkBoxExternalPalettes.Size = new System.Drawing.Size(132, 17);
            this.checkBoxExternalPalettes.TabIndex = 6;
            this.checkBoxExternalPalettes.Text = "Load External Palettes";
            this.toolTip.SetToolTip(this.checkBoxExternalPalettes, "Load palettes from palettes.bin. See readme.txt.\r\n3 x 16 BGR15 colors = 96 bytes," +
        " first color in each palette is ignored.");
            this.checkBoxExternalPalettes.UseVisualStyleBackColor = true;
            // 
            // checkBoxBackup
            // 
            this.checkBoxBackup.AutoSize = true;
            this.checkBoxBackup.Location = new System.Drawing.Point(333, 246);
            this.checkBoxBackup.Name = "checkBoxBackup";
            this.checkBoxBackup.Size = new System.Drawing.Size(82, 17);
            this.checkBoxBackup.TabIndex = 3;
            this.checkBoxBackup.Text = "Backup File";
            this.toolTip.SetToolTip(this.checkBoxBackup, "Create copy of rom file before writing changes.");
            this.checkBoxBackup.UseVisualStyleBackColor = true;
            // 
            // checkBoxDither
            // 
            this.checkBoxDither.AutoSize = true;
            this.checkBoxDither.Location = new System.Drawing.Point(333, 267);
            this.checkBoxDither.Name = "checkBoxDither";
            this.checkBoxDither.Size = new System.Drawing.Size(54, 17);
            this.checkBoxDither.TabIndex = 5;
            this.checkBoxDither.Text = "Dither";
            this.toolTip.SetToolTip(this.checkBoxDither, "Use dithering if colors have to be reduced.\r\nNot recommended for pixel art.");
            this.checkBoxDither.UseVisualStyleBackColor = true;
            this.checkBoxDither.CheckedChanged += new System.EventHandler(this.checkBoxDither_CheckedChanged);
            // 
            // checkBoxSetStartup
            // 
            this.checkBoxSetStartup.AutoSize = true;
            this.checkBoxSetStartup.Location = new System.Drawing.Point(427, 246);
            this.checkBoxSetStartup.Name = "checkBoxSetStartup";
            this.checkBoxSetStartup.Size = new System.Drawing.Size(127, 17);
            this.checkBoxSetStartup.TabIndex = 4;
            this.checkBoxSetStartup.Text = "Set as Startup Border";
            this.toolTip.SetToolTip(this.checkBoxSetStartup, "Switch to custom border on startup.");
            this.checkBoxSetStartup.UseVisualStyleBackColor = true;
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.Filter = "PNG Image|*.png";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 491);
            this.Controls.Add(this.checkBoxSetStartup);
            this.Controls.Add(this.checkBoxDither);
            this.Controls.Add(this.buttonInject);
            this.Controls.Add(this.checkBoxBackup);
            this.Controls.Add(this.checkBoxExternalPalettes);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBoxSlot);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxFile);
            this.Controls.Add(this.groupBoxImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(600, 530);
            this.MinimumSize = new System.Drawing.Size(600, 530);
            this.Name = "MainWindow";
            this.Text = "Super Game Boy 2 Border Injector";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBoxImage.ResumeLayout(false);
            this.groupBoxFile.ResumeLayout(false);
            this.groupBoxFile.PerformLayout();
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxSlot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.Button buttonFileSelect;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBoxImage;
        private System.Windows.Forms.GroupBox groupBoxFile;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.Button buttonInject;
        private System.Windows.Forms.ComboBox comboBoxSlot;
        internal System.Windows.Forms.TextBox textBoxOutput;
        internal System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox groupBoxSlot;
        private System.Windows.Forms.CheckBox checkBoxBackup;
        private System.Windows.Forms.CheckBox checkBoxExternalPalettes;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonLoadIcon;
        internal System.Windows.Forms.ComboBox comboBoxIcon;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.CheckBox checkBoxDither;
        internal System.Windows.Forms.Button buttonSaveDitheredImage;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.CheckBox checkBoxSetStartup;
    }
}