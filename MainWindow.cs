using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace SGB2_Border_Injector
{
    public partial class MainWindow : Form
    {
        internal WriteLine WriteLineHandler;
        internal ThreadDone ThreadDoneHandler;

        private string border_file = "";
        private bool invert_icon = false;

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
            openFileDialog.Title = "Select Border Image";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBoxOutput.Text = "";
                    Bitmap img = Program.LoadImage(openFileDialog.FileName);
                    if (img != null)
                    {
                        pictureBox.Image = img;
                        border_file = openFileDialog.FileName;
                    }
                    else
                    {
                        pictureBox.Image = null;
                        textBoxOutput.Text = "Error loading image.";
                        border_file = "";
                    }
                }
                catch { }
            }
        }

        private void buttonLoadIcon_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select Icon Image";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBoxOutput.Text = "";
                    string file_name = openFileDialog.FileName;
                    bool success = Program.LoadIcon(file_name);
                    if (success)
                    {
                        comboBoxIcon.Items.Add("Icon: " + file_name.Substring(file_name.LastIndexOf('\\') + 1));
                        comboBoxIcon.SelectedIndex = comboBoxIcon.Items.Count - 1;
                    }
                    else
                    {
                        textBoxOutput.Text = "Could not load the selected icon.\r\n\r\n" +
                            "Please make sure that your icon is in png or bmp format, has a resolution of 16 × 16 pixels or 10 × 14 pixels and only uses the following colors:\r\n" +
                            "#000000, #ffffff, #efcebd, #ff8c31, #cecece, #a58c5a, #ffff6b, #6bff6b, #ff9494, #b5b5ff, #735a21, #bd8442, #cea573, #c6945a, #e7d6c6";
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
            
            // launch injection process in separate thread to keep window responsive
            string sgb2_rom = textBoxFilename.Text;
            int border = comboBoxSlot.SelectedIndex + 3;
            int icon = comboBoxIcon.SelectedIndex - 1;
            bool external_palettes = checkBoxExternalPalettes.Checked;
            bool backup = checkBoxBackup.Checked;
            new Thread(delegate () {
                bool success = Program.InjectCustomBorder(sgb2_rom, border_file, border, icon, external_palettes, backup);
                Invoke(WriteLineHandler, $"\r\nInjecting border status: {(success ? "Success" : "Error")}");
                Invoke(ThreadDoneHandler);
            }).Start();

        }

        private void comboBoxIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxIcon.Visible = comboBoxIcon.SelectedIndex > 0;
            pictureBoxIcon.Refresh();
        }

        private void pictureBoxIcon_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap icon = Program.icons[comboBoxIcon.SelectedIndex - 1];

            // colorMap for switching between active and inactive palette
            ColorMap[] colorMap = new ColorMap[] {
                new ColorMap {
                    OldColor = Color.FromArgb(0, 0, 0),
                    NewColor = Color.FromArgb(150, 90, 33)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(255, 255, 255),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(239, 206, 189),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(255, 140, 49),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(206, 206, 206),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(165, 140, 90),
                    NewColor = Color.FromArgb(132, 107, 49)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(255, 255, 107),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(107, 255, 107),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(255, 148, 148),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(181, 181, 255),
                    NewColor = Color.FromArgb(165, 140, 90)
                },
                new ColorMap {
                    OldColor = Color.FromArgb(115, 90, 33),
                    NewColor = Color.FromArgb(150, 90, 33)
                }
            };
            ImageAttributes attr = new ImageAttributes();
            if (invert_icon)
                attr.SetRemapTable(colorMap);

            //g.DrawImageUnscaled(icon.Clone(new Rectangle(1, 0, 13, 16), icon.PixelFormat), new Point(0, 0));
            Rectangle rect = new Rectangle(0, 0, 12, 16);
            g.DrawImage(icon.Clone(new Rectangle(1, 0, 13, 16), icon.PixelFormat), rect, 0, 0, 12, 16, GraphicsUnit.Pixel, attr);
        }

        private void pictureBoxIcon_Click(object sender, EventArgs e)
        {
            invert_icon = !invert_icon;
            pictureBoxIcon.Refresh();
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
