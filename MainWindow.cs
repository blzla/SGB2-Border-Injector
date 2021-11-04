using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SGB2_Border_Injector
{
    public partial class MainWindow : Form
    {
        internal WriteLine WriteLineHandler;
        internal ThreadDone ThreadDoneHandler;

        public MainWindow()
        {
            InitializeComponent();
            WriteLineHandler = new WriteLine(WriteLineMethod);
            ThreadDoneHandler = new ThreadDone(ThreadDoneMethod);
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
                        textBoxOutput.Text = "Super Game Boy 2 ROM detected.";
                        if (msg != string.Empty)
                            textBoxOutput.Text += $"\r\n\r\n{msg}";
                    }
                    else
                    {
                        textBoxFilename.Text = string.Empty;
                        textBoxOutput.Text = $"Invalid output file: {msg}\r\n\r\nPlease select the correct ROM:\r\nSuper Game Boy 2 (Japan).sfc\r\nFile size: 524,288 bytes\r\nCRC32: CB176E45";
                    }
                }
                catch { }
            }
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
                    {
                        pictureBox.Image = null;
                        textBoxOutput.Text = "Error loading image.";
                    }
                }
                catch { }
            }
        }

        private void buttonInject_Click(object sender, EventArgs e)
        {
            // disable button
            buttonInject.Enabled = false;
            Application.UseWaitCursor = true;
            textBoxOutput.Text = string.Empty;
            
            // launch injection process in separate thread to keep window reactive
            string sgb2_rom = textBoxFilename.Text;
            string border_file = openFileDialog.FileName;
            int border = comboBoxSlot.SelectedIndex + 3;
            bool external_palettes = checkBoxExternalPalettes.Checked;
            bool backup = checkBoxBackup.Checked;
            new Thread(delegate () {
                bool success = Program.InjectCustomBorder(sgb2_rom, border_file, border, external_palettes, backup);
                Invoke(WriteLineHandler, $"\r\nInjecting border status: {(success ? "Success" : "Error")}");
                Invoke(ThreadDoneHandler);
            }).Start();

        }

        internal delegate void WriteLine(string line = "");
        private void WriteLineMethod(string line = "")
        {
            textBoxOutput.AppendText(line + "\r\n");
        }

        internal delegate void ThreadDone();
        private void ThreadDoneMethod()
        {
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
            buttonInject.Enabled = true;
        }
    }
}
