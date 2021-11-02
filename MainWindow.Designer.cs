
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
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.buttonFileSelect = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxImage = new System.Windows.Forms.GroupBox();
            this.groupBoxFile = new System.Windows.Forms.GroupBox();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.groupBoxInject = new System.Windows.Forms.GroupBox();
            this.buttonInject = new System.Windows.Forms.Button();
            this.comboBoxSlot = new System.Windows.Forms.ComboBox();
            this.groupBoxSlot = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBoxImage.SuspendLayout();
            this.groupBoxFile.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxInject.SuspendLayout();
            this.groupBoxSlot.SuspendLayout();
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
            this.textBoxOutput.Size = new System.Drawing.Size(527, 255);
            this.textBoxOutput.TabIndex = 0;
            this.textBoxOutput.Text = "Image requirements: 256 x 224 px, with a maximum of 3 15 color palettes.\r\n\r\n     " +
    "Version 0.1\r\n       - by blizzz";
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Location = new System.Drawing.Point(16, 24);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(258, 23);
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
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Location = new System.Drawing.Point(313, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 226);
            this.panel1.TabIndex = 5;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "Super Game Boy 2 (Japan).sfc";
            this.saveFileDialog.Filter = "SFC ROM Files|*.sfc; *.bin|All files|*.*";
            this.saveFileDialog.Title = "Select Super Game Boy 2 (Japan) ROM file";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Lossless Image files|*.png; *.bmp|All files|*.*";
            this.openFileDialog.Title = "Select Border Image";
            // 
            // groupBoxImage
            // 
            this.groupBoxImage.Controls.Add(this.buttonLoadImage);
            this.groupBoxImage.Location = new System.Drawing.Point(12, 76);
            this.groupBoxImage.Name = "groupBoxImage";
            this.groupBoxImage.Size = new System.Drawing.Size(290, 63);
            this.groupBoxImage.TabIndex = 1;
            this.groupBoxImage.TabStop = false;
            this.groupBoxImage.Text = "Select Border Image";
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
            this.groupBoxFile.Text = "Select Super Game Boy 2 ROM File";
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Controls.Add(this.textBoxOutput);
            this.groupBoxOutput.Location = new System.Drawing.Point(11, 283);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(560, 300);
            this.groupBoxOutput.TabIndex = 4;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // groupBoxInject
            // 
            this.groupBoxInject.Controls.Add(this.buttonInject);
            this.groupBoxInject.Location = new System.Drawing.Point(12, 214);
            this.groupBoxInject.Name = "groupBoxInject";
            this.groupBoxInject.Size = new System.Drawing.Size(290, 63);
            this.groupBoxInject.TabIndex = 3;
            this.groupBoxInject.TabStop = false;
            this.groupBoxInject.Text = "Inject Border";
            // 
            // buttonInject
            // 
            this.buttonInject.Location = new System.Drawing.Point(16, 22);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(258, 23);
            this.buttonInject.TabIndex = 0;
            this.buttonInject.Text = "Inject";
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
            "9 (Chess Arena)"});
            this.comboBoxSlot.Location = new System.Drawing.Point(16, 24);
            this.comboBoxSlot.Name = "comboBoxSlot";
            this.comboBoxSlot.Size = new System.Drawing.Size(258, 21);
            this.comboBoxSlot.TabIndex = 0;
            this.comboBoxSlot.Text = "9 (Chess Arena)";
            // 
            // groupBoxSlot
            // 
            this.groupBoxSlot.Controls.Add(this.comboBoxSlot);
            this.groupBoxSlot.Location = new System.Drawing.Point(12, 145);
            this.groupBoxSlot.Name = "groupBoxSlot";
            this.groupBoxSlot.Size = new System.Drawing.Size(290, 63);
            this.groupBoxSlot.TabIndex = 2;
            this.groupBoxSlot.TabStop = false;
            this.groupBoxSlot.Text = "Select Border Slot";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 597);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBoxSlot);
            this.Controls.Add(this.groupBoxInject);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxFile);
            this.Controls.Add(this.groupBoxImage);
            this.Name = "MainWindow";
            this.Text = "Super Game Boy 2 Border Injector";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBoxImage.ResumeLayout(false);
            this.groupBoxFile.ResumeLayout(false);
            this.groupBoxFile.PerformLayout();
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxInject.ResumeLayout(false);
            this.groupBoxSlot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.Button buttonFileSelect;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBoxImage;
        private System.Windows.Forms.GroupBox groupBoxFile;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.GroupBox groupBoxInject;
        private System.Windows.Forms.Button buttonInject;
        private System.Windows.Forms.ComboBox comboBoxSlot;
        internal System.Windows.Forms.TextBox textBoxOutput;
        internal System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox groupBoxSlot;
    }
}