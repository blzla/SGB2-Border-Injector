using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGB2_Border_Injector
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBoxOutput.Text = "";
                    Bitmap img = Program.LoadImage(openFileDialog.FileName);
                    if (img != null)
                    {
                        pictureBox.Image = img;
                    }
                    else
                        textBoxOutput.Text = "Error loading image.";
                }
                catch { }
            }
        }

        private void buttonFileSelect_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var (valid, msg) = Program.ValidateRomFile(saveFileDialog.FileName);
                    if (valid)
                    {
                        textBoxFilename.Text = saveFileDialog.FileName;
                        textBoxOutput.Text = "Output file passed validation.";
                        if (msg != string.Empty)
                            textBoxOutput.Text += $"\r\n\r\n{msg}";
                    }
                    else
                    {
                        textBoxFilename.Text = string.Empty;
                        textBoxOutput.Text = $"Invalid output file: {msg}\r\n\r\nPlease select the correct rom:\r\nSuper Game Boy 2 (Japan).sfc\r\nFile size: 524,288 bytes\r\nCRC32: CB176E45";
                    }
                }
                catch { }
            }
        }

        private void buttonInject_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            textBoxOutput.Text = "Working... (can take up to 30 seconds)";
            Program.InjectCustomBorder(textBoxFilename.Text, openFileDialog.FileName, comboBoxSlot.SelectedIndex + 3, checkBoxExternalPalettes.Checked, checkBoxBackup.Checked);
            UseWaitCursor = false;
        }
    }
}
