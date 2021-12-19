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
        internal UpdateImage UpdateImageHandler;
        internal ThreadDone ThreadDoneHandler;

        private string border_file = "";
        private bool invert_icon = false;
        private Thread t = null;
        bool refresh_image = false;

        public MainWindow()
        {
            InitializeComponent();
            WriteLineHandler = new WriteLine(WriteLineMethod);
            UpdateImageHandler = new UpdateImage(UpdateImageMethod);
            ThreadDoneHandler = new ThreadDone(ThreadDoneMethod);
        }

        private void buttonFileSelect_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select Super Game Boy 2 (Japan) ROM file";
            openFileDialog.FileName = "Super Game Boy 2 (Japan).sfc";
            openFileDialog.Filter = "SFC ROM Files|*.sfc; *.bin|All files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var (valid, msg) = Program.ValidateRomFile(openFileDialog.FileName);
                    if (valid)
                    {
                        textBoxFilename.Text = openFileDialog.FileName;
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
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Lossless Image files|*.png; *.bmp|All files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBoxOutput.Text = "";
                    RefreshImage(openFileDialog.FileName);
                }
                catch { }
            }
        }

        private void RefreshImage(string filename)
        {
            buttonSaveDitheredImage.Visible = false;
            buttonLoadImage.Width = 257;

            if (filename == null || filename == "")
                return;

            Bitmap img = Program.LoadImage(filename, false);
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

        private void buttonLoadIcon_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select Icon Image";
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Lossless Image files|*.png; *.bmp|All files|*.*";
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
            if (buttonInject.Text == "Inject Border")
            {
                // disable button
                //buttonInject.Enabled = false;
                buttonInject.Text = "Cancel Action";
                Application.UseWaitCursor = true;
                textBoxOutput.Text = string.Empty;

                // launch injection process in separate thread to keep window responsive
                string sgb2_rom = textBoxFilename.Text;
                int border = comboBoxSlot.SelectedIndex + 3;
                int icon = comboBoxIcon.SelectedIndex - 1;
                bool set_startup = checkBoxSetStartup.Checked;
                bool dither = checkBoxDither.Checked;
                bool external_palettes = checkBoxExternalPalettes.Checked;
                bool backup = checkBoxBackup.Checked;
                t = new Thread(delegate ()
                {
                    try
                    {
                        bool success = Program.InjectCustomBorder(sgb2_rom, border_file, border, icon, set_startup, dither, external_palettes, backup);
                        Program.WriteLine($"\r\nInjecting border status: {(success ? "Success" : "Error")}");
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Console.WriteLine(ex);
#endif
                        Program.WriteLine();
                        if (ex is System.Threading.ThreadAbortException)
                            Program.WriteLine("Action aborted.");
                        else
                            Program.WriteLine("The program encountered an error. Aborting.");

                    }
                    finally
                    {
                        Invoke(ThreadDoneHandler);
                    }
                })
                {
                    IsBackground = true // abort thread if window is closed
                };
                t.Start();
            }
            else
            {
                try
                {
                    t.Abort();
                }
                catch { }
            }
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

        internal delegate void WriteLine(string line = "", bool newLine = true);
        private void WriteLineMethod(string line = "", bool newLine = true)
        {
            textBoxOutput.AppendText(line + (newLine ? "\r\n" : ""));
        }

        internal delegate void UpdateImage(Bitmap image);
        private void UpdateImageMethod(Bitmap image)
        {
            pictureBox.Image = image;
            buttonSaveDitheredImage.Visible = true;
            buttonLoadImage.Width = 179;
        }

        internal delegate void ThreadDone();
        private void ThreadDoneMethod()
        {
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
            //buttonInject.Enabled = true;
            buttonInject.Text = "Inject Border";
            if (refresh_image)
            {
                RefreshImage(border_file);
                refresh_image = false;
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (pictureBox.Image != null) 
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 239, 206)), new Rectangle(48, 40, 160, 144));
        }

        private void checkBoxDither_CheckedChanged(object sender, EventArgs e)
        {
            if (t != null && t.IsAlive)
                refresh_image = true;
            else
                RefreshImage(border_file);                
        }

        private void buttonSaveDitheredImage_Click(object sender, EventArgs e)
        {
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox.Image.Save(saveImageDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    textBoxOutput.AppendText("\r\nError: Could not save file.");
                    return;
                }
                textBoxOutput.AppendText($"\r\nImage saved as {saveImageDialog.FileName}.");
            }
        }
    }
}
