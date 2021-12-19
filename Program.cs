using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SGB2_Border_Injector
{
    class Program
    {
        private static readonly (string name, int tilemap, (int addr, int chunk_length)[] tileset_chunks, int tileset_maxsize, int palettes, (int addr, byte value)[] patches, int icon, int checksum)[] border_data = new (string, int, (int, int)[], int, int, (int, byte)[], int, int)[] {
            ("Printed Circuit Board", 0x52000, new (int, int)[] { (0x59100, 0x1D00), (0x73180, 0x1000), (0x74180, 0x1000), (0x75180, 0x620) }, 0x1D00, 0x57080, new (int, byte)[] { }, 0x62000, 0x202B),
            ("Palm Trees", 0x67000, new (int, int)[] { (0x63000, 0x4000), (0x757A0, 0x320) }, 0x4000, 0x67880, new (int, byte)[] { (0x449B7, 0x10), (0x449D5, 0x18) }, 0x62040, 0x2D9F),
            ("Stone Mosaic", 0x52800, new (int, int)[] { (0x4E000, 0x1800), (0x4C000, 0x2000), (0x75AC0, 0xB20) }, 0x3800, 0x57480, new (int, byte)[] { (0x40FC7, 0x0C), (0x40FE5, 0x14), (0x4101E, 0x08) }, 0x62080, 0x3754),
            ("Gears", 0x6D000, new (int, int)[] { (0x68000, 0x5000) }, 0x5000, 0x67A80, new (int, byte)[] { (0x446DC, 0x18), (0x446FA, 0x20) }, 0x620C0, 0x39F0),
            ("Swamp", 0x53000, new (int, int)[] { (0x48000, 0x4000), (0x765E0, 0x320) }, 0x4000, 0x57A80, new (int, byte)[] { (0x4153E, 0x10), (0x41541, 0x10), (0x41544, 0xA0), (0x4154B, 0x09), (0x4155F, 0x18), (0x41562, 0xB0), (0x4157A, 0x08), (0x4157D, 0x3C), (0x41587, 0x0A), (0x041C6B, 0x60) }, 0x62100, 0x2C70),
            ("Dolphins", 0x53800, new (int, int)[] { (0x5CCC0, 0x3140), (0x76900, 0xFE0), (0x77E00, 0x200) }, 0x3140, 0x57C80, new (int, byte)[] { (0x4265B, 0x10), (0x42679, 0x10), (0x4267C, 0x10), (0x4267E, 0xC0), (0x4267F, 0xEC), (0x42686, 0x0B), (0x42696, 0x40), (0x42697, 0x01), (0x4269A, 0x18), (0x4269C, 0xC0), (0x4269D, 0xFC), (0x426B5, 0x08), (0x426B8, 0x3C), (0x426BB, 0xB8), (0x426C2, 0x0A) }, 0x62140, 0x2FFF),
            ("Chess Arena", 0x54800, new (int, int)[] { (0x50000, 0x1000), (0x55800, 0x1400), (0x7F600, 0xA00), (0x778E0, 0x520), (0x78000, 0x1000) }, 0x2400, 0x57880, new (int, byte)[] { (0x43333, 0x50), (0x4334E, 0x10), (0x43351, 0x08), (0x43354, 0xD8), (0x4336C, 0x04), (0x4336F, 0x10), (0x43372, 0xE8), (0x4338A, 0x08), (0x4338D, 0x3C), (0x43390, 0xC8) }, 0x61D40, 0x2040),
            ("Windows (SGB1)", 0x084E0, new (int, int)[] { (0x18000, 0x3474) }, 0x3308, 0x1B4F4, new (int, byte)[] { (0x9C1F, 0x01), (0x9C25, 0xE0), (0x9C26, 0x84), (0x9CF3, 0x60), (0x9C5C, 0x60) }, 0x12690, -1),
            ("Cork Board (SGB1)", 0x27A20, new (int, int)[] { (0x23B20, 0x257B) }, 0x3308, 0x2611C, new (int, byte)[] { (0xB7EA, 0x20), (0xB7EB, 0xFA), (0xB7BA, 0x20), (0xB7BB, 0xBB), (0xB83E, 0x60) }, 0x126D0, -1),
            ("Log Cabin In The Countryside (SGB1)", 0x1C660, new (int, int)[] { (0x79000, 0x3300) }, 0x3308, 0x1D5E8, new (int, byte)[] { (0xA6B7, 0x0F), (0xA6BD, 0x00), (0xA6BE, 0x90), (0xA773, 0x80), (0xA774, 0x71), (0xA823, 0x80), (0xA824, 0x0D) }, 0x12710, -1),
            ("Movie Theater (SGB1)", 0x2C6C8, new (int, int)[] { (0x7C300, 0x3300) }, 0x3308, 0x2D8BC, new (int, byte)[] { (0xCF7B, 0xC8), (0xCF7C, 0xC6), (0xCF45, 0x0F), (0xCF4B, 0x00), (0xCF4C, 0xC3), (0xCFFB, 0x60), (0xCFE2, 0x60), (0xCF0C, 0x60), (0xCA4B, 0x60) }, 0x12750, -1),
            ("Cats (SGB1)", 0x1F716, new (int, int)[] { (0x20F73, 0x2BAC) }, 0x3308, 0x20DF3, new (int, byte)[] { (0xB90D, 0x03), (0xB917, 0x16), (0xB918, 0xF7), (0xB8DD, 0x04), (0xB8E3, 0x73), (0xB8E4, 0x8F), (0xB945, 0x80), (0xB946, 0x5B), (0xB9B5, 0x60) }, 0x12790, -1),
            ("Chequered Desk With Pencils (SGB1)", 0x58B80, new (int, int)[] { (0x45340, 0x2CC0) }, 0x3308, 0x29C21, new (int, byte)[] { (0xD3CE, 0x0B), (0xD3D8, 0x80), (0xD3D9, 0x8B), (0xD37B, 0x08), (0xD381, 0x40), (0xD382, 0xD3), (0xD406, 0x80), (0xD407, 0x5B) }, 0x127D0, -1),
            ("Escher (SGB1)", 0x20000, new (int, int)[] { (0x159C0, 0x2640) }, 0x3308, 0x1E78A, new (int, byte)[] { (0xC734, 0x04), (0xC73E, 0x00), (0xC73F, 0x80), (0xC704, 0x02), (0xC70A, 0xC0), (0xC70B, 0xD9), (0xC888, 0x60), (0xC83C, 0x60), (0xC794, 0x60) }, 0x123D0, -1)
        };

        private static readonly (int offset, int jump, (int bank, int address, int vram, int size)[] dma_chunks)[] extended_dma_chunks = new (int, int, (int, int, int, int)[])[]
        {
            (0x4070D, 0x8EAF30, new (int, int, int, int)[] { ( 0x8E, 0xB180, 0x1D00, 0x1000 ), ( 0x8E, 0xC180, 0x2D00, 0x1000 ), ( 0x8E, 0xD180, 0x3D00, 0x620 ) }),
            (0x449E8, 0x8EAF8F, new (int, int, int, int)[] { ( 0x8E, 0xD7A0, 0x4000, 0x320 ) }),
            (0x41034, 0x8EAFB2, new (int, int, int, int)[] { ( 0x8E, 0xDAC0, 0x3800, 0xB20) }),
            (0, 0, null),
            (0x41572, 0x8EAFD5, new (int, int, int, int)[] { ( 0x8E, 0xE5E0, 0x4000, 0x320 ) }),
            (0x426AD, 0x8EAFF8, new (int, int, int, int)[] { ( 0x8E, 0xE900, 0x3140, 0xFE0 ), ( 0x8E, 0xFE00, 0x4120, 0x200 ) }),
            (0x43382, 0x8EB039, new (int, int, int, int)[] { ( 0x8F, 0xF600, 0x2400, 0xA00 ), ( 0x8E, 0xF8E0, 0x2E00, 0x520 ), ( 0x8F, 0x8000, 0x3320, 0x1000 ) })
        };

        private static readonly (int tileset_dma_start, int tilemap_decompression)[] old_dma_locations = new (int, int)[]
        {
            (0x9B4B, 0x9C1E),
            (0xB7C4, 0xB7E3),
            (0xA6C7, 0xA743),
            (0xCF55, 0xCF74),
            (0xB8ED, 0xB90C),
            (0xD38B, 0xD3CD),
            (0xC714, 0xC733)
        };

        internal static readonly List<Bitmap> icons = new List<Bitmap>();
        private static readonly ushort[] icon_palette = new ushort[] { 0x0000, 0x7FFF, 0x5F3D, 0x1A3F, 0x6739, 0x2E34, 0x37FF, 0x37ED, 0x4A5F, 0x7ED6, 0x116E, 0x2217, 0x3A99, 0x2E58, 0x635C };

        private static MainWindow window = null;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            icons.Add(Properties.Resources.icon_goose);
            Application.Run(window = new MainWindow());
        }

        // Inject custom border main function
        internal static bool InjectCustomBorder(string sgb2_rom, string border_file, int border, int icon, bool set_startup, bool dither, bool external_palettes, bool backup)
        {
            if (string.IsNullOrEmpty(sgb2_rom) || string.IsNullOrEmpty(border_file) || !(border >= 3 && border <= 16))
            {
                WriteLine($"Please set all inputs.");
                return false;
            }

            // read image
            Bitmap border_bitmap = LoadImage(border_file, true);
            if (border_bitmap == null)
            {
                WriteLine($"Invalid image.");
                return false;
            }
            Bitmap original_bitmap = border_bitmap; // doing a proper copy with Clone makes the result worse. but why

            //if (CountColors(border_bitmap) > 45)
            //   border_bitmap = SimplePaletteQuantizer.SmartColorReducer(original_bitmap, 45, !dither, dither);
            

            for (int i = 0; i <= 30; i++)
            {
                // update border image preview
                if (border_bitmap != original_bitmap)
                    UpdateImage(border_bitmap);

                // build tile data
                var (tiles_data, tiles_flipped_data, tile_colors, too_many_colors) = BuildTileData(border_bitmap);
                if (too_many_colors)
                {
                    border_bitmap = SimplePaletteQuantizer.SmartColorReducer(original_bitmap, 45 - i, !dither, dither);
                    if (i < 30)
                        WriteLine($"Reducing colors to {45 - i}.\r\n");
                    else
                        WriteLine("Giving up.\r\n");
                    continue;
                }

                // build palettes
                var (found_working_palettes, palette_sets) = BuildPalettes(tile_colors, external_palettes);
                if (!found_working_palettes)
                {
                    if (external_palettes)
                        return false;

                    border_bitmap = SimplePaletteQuantizer.SmartColorReducer(original_bitmap, 45 - i, !dither, dither);
                    if (i < 30)
                        WriteLine($"Reducing colors to {45 - i}.\r\n");
                    else
                        WriteLine("Giving up.\r\n");
                    continue;
                }

                // build tileset
                byte[] tileset = BuildTileset(tiles_data, tiles_flipped_data, palette_sets);
                if (border > 9)
                {
                    WriteLine();
                    WriteLine("Compressing tileset...  ", false);
                    int uncompressed_size = tileset.Length;
                    if (uncompressed_size > 0x3308)
                    {
                        WriteLine();
                        WriteLine($"Tileset is too big for SGB1 slots. Input size: 0x{uncompressed_size:X4} bytes, max size: 0x3308 bytes.");
                        WriteLine("Please select an SGB2 slot.");
                        return false;
                    }
                    tileset = SGBCompression.Compress(tileset);
                    WriteLine($"Ratio: {string.Format("{0:0.00}", (float)tileset.Length / uncompressed_size)}, saved {uncompressed_size - tileset.Length} bytes.");

                    if (tileset.Length > border_data[border - 7].tileset_maxsize)
                    {
                        WriteLine($"Compressed tileset is too big for slot {border - 7}'. Input size: 0x{tileset.Length:X4}, max size of slot {border - 7}': 0x{border_data[border - 7].tileset_maxsize:X4} bytes.");
                        WriteLine($"Please select a different slot. Fits in all SGB2 slots and SGB1 slots: {FitsInCompressedSlots(tileset.Length)}");
                        return false;
                    }
                }
                else if (tileset.Length > 0x4320)
                {
                    // this shouldn't be possible
                    WriteLine($"Tileset exceeds maximum size");
                    return false;
                }

                // build tilemap
                byte[] tilemap = BuildTilemap(tiles_data, tiles_flipped_data, palette_sets);
                if (border > 9)
                {
                    WriteLine("Compressing tilemap...  ", false);
                    int uncompressed_size = tilemap.Length;
                    tilemap = SGBCompression.Compress(tilemap);
                    WriteLine($"Ratio: {string.Format("{0:0.00}", (float)tilemap.Length / uncompressed_size)}, saved {uncompressed_size - tilemap.Length} bytes.");

                    if (tilemap.Length > 0x546) // todo, increase tilemap size
                    {
                        WriteLine("Tilemap is too big for compressed slots.");
                        return false;
                    }
                }

#if DEBUG
                /*
            File.WriteAllBytes("tilemap.bin", tilemap);
            File.WriteAllBytes("tileset.bin", tileset);
            File.WriteAllBytes("palettes.bin", ConvertPalettesToBytes(palette_sets));

            File.WriteAllBytes("tilemap_compressed.bin", SGBCompression.Compress(tilemap));
            File.WriteAllBytes("tilemap_decompressed.bin", SGBCompression.Decompress(SGBCompression.Compress(tilemap)));
            Console.WriteLine(Enumerable.SequenceEqual(tilemap, SGBCompression.Decompress(SGBCompression.Compress(tilemap))));
            File.WriteAllBytes("tileset_compressed.bin", SGBCompression.Compress(tileset));
            File.WriteAllBytes("tileset_decompressed.bin", SGBCompression.Decompress(SGBCompression.Compress(tileset)));
            Console.WriteLine(Enumerable.SequenceEqual(tileset, SGBCompression.Decompress(SGBCompression.Compress(tileset))));
                */
#endif

                // start modifying rom file
                WriteLine();
                var (success, msg) = ValidateRomFile(sgb2_rom);
                if (!success)
                    WriteLine($"Not a valid SGB2 rom: {msg}");

                if (backup && success)
                    if (success = BackupFile(sgb2_rom))
                        WriteLine($"Created backup file: {Path.GetFileName(sgb2_rom)}.bak");
                    else
                        WriteLine("Error: Failed to create backup file. Aborting.");

                // file will only be modified if all previous steps were successful
                if (success)
                {
                    using (FileStream fs = new FileStream(sgb2_rom, FileMode.Open, FileAccess.ReadWrite))
                    {
                        // write data to file
                        WriteLine("Begin modifying file.");
                        success &= SaveTilemap(fs, tilemap, border);
                        success &= SaveTileset(fs, tileset, border);
                        success &= SavePalettes(fs, ConvertPalettesToBytes(palette_sets), border);
                        if (icon >= 0 && icon < icons.Count)
                            success = SaveBorderIcon(fs, ConvertIconToBytes(icons[icon]), border);
                        WriteLine($"Wrote tilemap, tileset, palettes{(icon >= 0 ? ", icon" : "")}: {(success ? "Done" : "Error")}");

                        // change memory transfers to allow for bigger tilesets
                        success &= ApplyDMAPatches(fs, border);
                        // remove screensaver, since it would glitch out
                        success &= DisableScreensaverSlot(fs, border);
                        // automatically switch to new border on startup
                        if (set_startup)
                            success &= SetStartupBorder(fs, border);
                        WriteLine($"Applied {border_data[border - 3].patches.Length} DMA patches, disabled screensaver{(set_startup ? ", set startup border" : "")}: {(success ? "Done" : "Error")}");

                        success &= UpdateChecksum(fs);
                        WriteLine($"Updated checksum.");
                    }
                }

                return success;
            }

            return false;

        }

        // load image from file
        internal static Bitmap LoadImage(string file_name, bool cached)
        {
            try
            {
                if (cached)
                    if (window.pictureBox.Image != null && window.pictureBox.Image.Width == 256 && window.pictureBox.Image.Height == 224)
                        return (Bitmap)window.pictureBox.Image;

                Bitmap border_bitmap = new Bitmap(256, 224, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(border_bitmap);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                Bitmap file = new Bitmap(file_name);
                if (file.Width > 1024 || file.Height > 1024)
                {
                    WriteLine($"Image too big. Expected 256 x 224 px image, read {file.Width} x {file.Height} pixels.");
                    return null;
                }
                else if (file.Width != 256 || file.Height != 224)
                    WriteLine($"Warning: Expected 256 x 224 px image, read {file.Width} x {file.Height} pixels. Image will be padded or cropped.");
                g.DrawImageUnscaledAndClipped(file, new Rectangle(0, 0, file.Width, file.Height));
                g.FillRectangle(new SolidBrush(Color.FromArgb(28, 28, 28)), new Rectangle(48, 40, 160, 144));

                return border_bitmap;
            }
            catch { }
            return null;
        }

        //
        internal static int CountColors(Bitmap image)
        {
            HashSet<Color> colors = new HashSet<Color>();
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    colors.Add(image.GetPixel(x, y));
            return colors.Count;
        }

        // check if rom file is a SGB2 rom file
        internal static (bool, string) ValidateRomFile(string file_name)
        {
            try
            {
                FileInfo f = new FileInfo(file_name);
                if (f.Length != 0x80000)
                    return (false, "Wrong filesize.");

                using (FileStream fs = new FileStream(file_name, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.Seek(0x7FC0, SeekOrigin.Begin);
                    byte[] internalTitle = new byte[14];
                    fs.Read(internalTitle, 0, 14);
                    if (!Enumerable.SequenceEqual(internalTitle, System.Text.Encoding.Default.GetBytes("Super GAMEBOY2")))
                        return (false, "Invalid game title.");

                    string msg = string.Empty;
                    fs.Seek(0x7FDE, SeekOrigin.Begin);
                    byte[] checksum = new byte[2];
                    fs.Read(checksum, 0, 2);
                    if (!Enumerable.SequenceEqual(checksum, new byte[] { 0xD5, 0x7D }))
                        msg = "File was already modified before. Files that were modified with this app or SGB Settings Editor are supported.";

                    return (true, msg);
                }
            }
            catch { }
            return (false, "File access error.");
        }

        // convert 24 bit RGB value from input image to 15 bit GBR value used on SNES
        private static int ConvertRGBtoSFC(int r, int g, int b)
        {
            int bgr15 = (b >> 3 << 10) + (g >> 3 << 5) + (r >> 3);
            return bgr15;
        }

        // creates a list of unique 8x8 pixel tiles and flipped / rotated versions, as well as the colors used
        private static (List<(int[] pixels, HashSet<int> colors, List<(int x, int y)> tile_locations)>, List<(int[] pixels, int duplicate_of, bool hflip, bool vflip, List<(int x, int y)> tile_locations)>, List<HashSet<int>>, bool) BuildTileData(Bitmap border_bitmap)
        {
            var tiles_data = new List<(int[], HashSet<int>, List<(int, int)>)>();
            var tiles_flipped_data = new List<(int[], int, bool, bool, List<(int x, int y)>)>();
            var total_colors = new HashSet<int>();
            var tile_colors = new List<HashSet<int>>();
            bool too_many_colors = false;

            // todo: swap x and y loops
            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    if (x >= 6 && x <= 25 && y >= 5 && y <= 22) // game area
                        continue;

                    int[] tile = new int[64];
                    HashSet<int> colors = new HashSet<int>();
                    for (int i = 0; i < 64; i++)
                    {
                        Color col = border_bitmap.GetPixel(x * 8 + i % 8, y * 8 + i / 8);
                        int snes_col = ConvertRGBtoSFC(col.R, col.G, col.B);
                        tile[i] = snes_col;
                        colors.Add(snes_col);
                    }
                    if (colors.Count > 15)
                    {
                        Bitmap colorful_tile = border_bitmap.Clone(new System.Drawing.Rectangle(x * 8, y * 8, 8, 8), PixelFormat.Format24bppRgb);
                        colorful_tile = SimplePaletteQuantizer.SmartColorReducer(colorful_tile, 15, true, false);
                        Graphics g = Graphics.FromImage(border_bitmap);
                        g.DrawImageUnscaledAndClipped(colorful_tile, new Rectangle(x * 8, y * 8, 8, 8));
                        //UpdateImage(border_bitmap);
                        //System.Threading.Thread.Sleep(1000);
                        // redo tile
                        x -= 1; // todo: swap x and y loop
                        continue;
                    }
                    total_colors.UnionWith(colors);

                    AddTile(tiles_data, tiles_flipped_data, tile, colors, x, y);

                    bool known_colors = false;
                    for (int i = tile_colors.Count - 1; i >= 0; i--)
                    {
                        HashSet<int> tile_color = tile_colors[i];
                        if (tile_color.IsSupersetOf(colors))
                        {
                            known_colors = true;
                            break;
                        }
                        else if (tile_color.IsSubsetOf(colors))
                        {
                            tile_colors.RemoveAt(i);
                        }
                    }
                    if (!known_colors)
                        tile_colors.Add(colors);
                }
                if (too_many_colors)
                    break;
            }

            if (total_colors.Count > 45)
                too_many_colors = true;

            WriteLine($"Found {tiles_data.Count} unique tiles, {tiles_flipped_data.Count} flipped versions and {536 - tiles_data.Count - tiles_flipped_data.Count} repeats.");
            WriteLine($"Found {tile_colors.Count} unique color tiles with a total of {total_colors.Count} colors.");

            return (tiles_data, tiles_flipped_data, tile_colors, too_many_colors);
        }

        // check if 8x8 pixel tile was already seen before or if it's a flipped / rotated version and add it to the correct list if new
        private static void AddTile(List<(int[] pixels, HashSet<int>, List<(int, int)> tile_locations)> tiles_data, List<(int[] pixels, int, bool, bool, List<(int, int)> tile_locations)> tiles_flipped_data, int[] tile, HashSet<int> colors, int x, int y)
        {
            for (int i = 0; i < tiles_data.Count; i++)
            {
                if (Enumerable.SequenceEqual(tiles_data[i].pixels, tile))
                {
                    tiles_data[i].tile_locations.Add((x, y));
                    return;
                }
            }

            for (int i = 0; i < tiles_flipped_data.Count; i++)
            {
                if (Enumerable.SequenceEqual(tiles_flipped_data[i].pixels, tile))
                {
                    tiles_flipped_data[i].tile_locations.Add((x, y));
                    return;
                }
            }

            bool h_flip = false, v_flip = false;
            int dup;
            if ((dup = IsFlipped(HVFlip, tiles_data, tile)) >= 0)
                h_flip = v_flip = true;
            else if ((dup = IsFlipped(HFlip, tiles_data, tile)) >= 0)
                h_flip = true;
            else if ((dup = IsFlipped(VFlip, tiles_data, tile)) >= 0)
                v_flip = true;

            if (!(h_flip || v_flip))
                tiles_data.Add((tile, colors, new List<(int, int)>() { (x, y) }));
            else
                tiles_flipped_data.Add((tile, dup, h_flip, v_flip, new List<(int, int)>() { (x, y) }));
        }

        private static int HVFlip(int x, int y)
        {
            return (7 - y) * 8 + (7 - x);
        }
        private static int HFlip(int x, int y)
        {
            return y * 8 + (7 - x);
        }
        private static int VFlip(int x, int y)
        {
            return (7 - y) * 8 + x;
        }

        private static int IsFlipped(Func<int, int, int> mapping, List<(int[] pixels, HashSet<int>, List<(int, int)>)> tiles_data, int[] tile)
        {
            int flip = -1;

            for (int i = 0; i < tiles_data.Count; i++)
            {
                bool match = true;
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                        if (tile[y * 8 + x] != tiles_data[i].pixels[mapping(x, y)])
                        {
                            match = false;
                            break;
                        }
                    if (!match)
                        break;
                }
                if (match)
                {
                    flip = i;
                    break;
                }
            }

            return flip;
        }

        // build palettes from colors sets. shuffles the order of which color sets are added to palettes around to find a working combination.
        // usually works in 1-2 tries for simple images or 100-150 tries for complicated pictures that use almost all available color slots.
        // give up if no working combination was found after 100000 tries.
        private static (bool, HashSet<int>[]) BuildPalettes(List<HashSet<int>> tile_colors, bool external_palettes)
        {
            bool found_working_palettes = false;
            int r = 0, maxr = 20000;
            HashSet<int>[] palette_sets = new HashSet<int>[] { new HashSet<int>(), new HashSet<int>(), new HashSet<int>() };

            // check if palettes are impossible first
            int temp_count = 0;
            foreach (var tile_color in tile_colors)
            {
                if (tile_color.Count == 15)
                    temp_count++;
            }
            if (temp_count >= 3 && tile_colors.Count > 3)
            {
                WriteLine($"Too many color combinations. Reducing colors...");
                return (false, palette_sets);
            }

            if (!external_palettes)
            {
                while (!found_working_palettes)
                {
                    foreach (var palette_set in palette_sets)
                        palette_set.Clear();

                    tile_colors = tile_colors.OrderBy(i => Guid.NewGuid()).ToList(); // shuffle order of color sets
                    foreach (HashSet<int> colorTile in tile_colors)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (FitsInPalette(palette_sets[i], colorTile))
                            {
                                palette_sets[i].UnionWith(colorTile);
                                break;
                            }
                        }
                    }
                    found_working_palettes = VerifyPalettes(palette_sets, tile_colors);

                    r++;
                    if (r == 1000)
                        WriteLine("Building palettes..."); // only display if it takes a while
                    else if (r >= maxr)
                        break;
                }
                if (found_working_palettes)
                    WriteLine($"Found working palettes after {r} {(r > 1 ? "tries" : "try")}.");
                else
                    WriteLine($"Couldn't find working palettes after {maxr} tries.");
            }
            else
            {
                try
                {
                    FileInfo f = new FileInfo("palettes.bin");
                    if (f.Length == 0x60)
                    {
                        using (FileStream fs = new FileStream("palettes.bin", FileMode.Open, FileAccess.Read))
                        {
                            byte[] buffer = new byte[0x60];
                            fs.Read(buffer, 0, 0x60);
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 1; j < 16; j++)
                                {
                                    int col = buffer[32 * i + 2 * j] + buffer[32 * i + 2 * j + 1] * 256;
                                    if (col > 0x7FFF)
                                        continue;
                                    palette_sets[i].Add(col);
                                }
                            }
                        }
                    }
                }
                catch { }
                found_working_palettes = VerifyPalettes(palette_sets, tile_colors);
                if (found_working_palettes)
                    WriteLine("Succesfully loaded external palettes.");
                else
                    WriteLine("Error: External palettes don't work for current picture.");
            }
            return (found_working_palettes, palette_sets);
        }

        // check if palette slot has enough unused slots for current tile colors
        private static bool FitsInPalette(HashSet<int> set, HashSet<int> colors)
        {
            int i = set.Count;
            foreach (int col in colors)
            {
                if (!set.Contains(col))
                    i++;
            }
            return i <= 15;
        }

        // check if palettes work for every tile in image
        private static bool VerifyPalettes(HashSet<int>[] palette_sets, List<HashSet<int>> tile_colors)
        {
            bool success = true;
            foreach (HashSet<int> colorTile in tile_colors)
            {
                if (!(colorTile.IsSubsetOf(palette_sets[0]) || colorTile.IsSubsetOf(palette_sets[1]) || colorTile.IsSubsetOf(palette_sets[2])))
                {
                    success = false;
                    break;
                }
            }
            return success;
        }

        // convert set to byte array for saving
        private static byte[] ConvertPalettesToBytes(HashSet<int>[] palette_sets)
        {
            byte[] palette_bytes = new byte[96];
            for (int i = 0; i < 3; i++)
            {
                int j = 0;
                foreach (int color in palette_sets[i])
                {
                    j++;
                    palette_bytes[32 * i + 2 * j] = BitConverter.GetBytes(color)[0];
                    palette_bytes[32 * i + 2 * j + 1] = BitConverter.GetBytes(color)[1];
                }
            }
            return palette_bytes;
        }

        // build snes 4 bpp tileset by replacing the colors with the offset of their color in the palette
        // and distribute the bits for every pixel over 4 bytes.
        // the tileset could be further reduced by detecting tiles that are identical, except for the palette they use (not implemented)
        private static byte[] BuildTileset(List<(int[] pixels, HashSet<int> colors, List<(int, int)>)> tiles_data, List<(int[], int, bool, bool, List<(int, int)>)> tiles_flipped_data, HashSet<int>[] palette_sets)
        {
            byte[] tileset = new byte[(tiles_data.Count + 1) * 32];
            for (int i = 0; i < tiles_data.Count; i++)
            {
                int[] palette = new int[16];
                byte[] tile_color_indices = new byte[64];
                for (int j = 0; j < 3; j++)
                {
                    if (tiles_data[i].colors.IsSubsetOf(palette_sets[j]))
                    {
                        palette = palette_sets[j].ToArray();
                        break;
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        byte color_index = (byte)(Array.IndexOf(palette, tiles_data[i].pixels[8 * y + (7 - x)]) + 1);
                        tile_color_indices[2 * y] = (byte)(tile_color_indices[2 * y] | (color_index & 0b0001) << x);
                        tile_color_indices[2 * y + 1] = (byte)(tile_color_indices[2 * y + 1] | ((color_index & 0b0010) >> 1) << x);
                        tile_color_indices[2 * y + 16] = (byte)(tile_color_indices[2 * y + 16] | ((color_index & 0b0100) >> 2) << x);
                        tile_color_indices[2 * y + 17] = (byte)(tile_color_indices[2 * y + 17] | ((color_index & 0b1000) >> 3) << x);
                    }
                }
                Array.Copy(tile_color_indices, 0, tileset, i * 32 + 32, 32);
            }
            return tileset;
        }

        // build tilemap from tileset and palettes
        private static byte[] BuildTilemap(List<(int[], HashSet<int> colors, List<(int, int)>)> tiles_data, List<(int[], int, bool, bool, List<(int, int)>)> tiles_flipped_data, HashSet<int>[] palette_sets)
        {
            byte[] tilemap = new byte[0x700];
            // process unique tiles first
            for (int i = 0; i < tiles_data.Count; i++)
            {
                var (_, colors, tile_locations) = tiles_data[i];
                foreach (var (x, y) in tile_locations)
                {
                    int offset = 2 * (x + y * 32);
                    int palette = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        if (colors.IsSubsetOf(palette_sets[j]))
                        {
                            palette = 4 + j;
                            break;
                        }
                    }
                    int data = 0;
                    data += i + 1; // add 1 because first tile is transparent
                    data |= palette << 10;
                    tilemap[offset] = BitConverter.GetBytes(data)[0];
                    tilemap[offset + 1] = BitConverter.GetBytes(data)[1];
                }
            }

            // process duplicates and flipped tiles
            for (int i = 0; i < tiles_flipped_data.Count; i++)
            {
                var (_, duplicate_of, hflip, vflip, tile_locations) = tiles_flipped_data[i];
                foreach (var (x, y) in tile_locations)
                {
                    int offset = 2 * (x + y * 32);
                    int palette = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        if (tiles_data[duplicate_of].colors.IsSubsetOf(palette_sets[j]))
                        {
                            palette = 4 + j; // sgb border palettes start at 4
                            break;
                        }
                    }
                    int data = 0;
                    data += duplicate_of + 1; // add 1 because first tile is transparent
                    data |= palette << 10;
                    if (hflip)
                        data |= 1 << 14;
                    if (vflip)
                        data |= 1 << 15;
                    tilemap[offset] = BitConverter.GetBytes(data)[0];
                    tilemap[offset + 1] = BitConverter.GetBytes(data)[1];
                }
            }
            return tilemap;
        }

        // check which border slots have enough space to fit the new tileset
        // max size for compressed slots is 3308 bytes
        private static string FitsInCompressedSlots(int tileset_length)
        {
            List<string> slots = new List<string>();
            for (int i = 7; i < border_data.Length; i++)
                if (border_data[i].tileset_maxsize >= tileset_length)
                    slots.Add($"{i - 4}'");

            return slots.Count > 0 ? string.Join(", ", slots) : "None";
        }

        // copy rom file as backup, overwrites exisisting .bak file
        private static bool BackupFile(string file_name)
        {
            try
            {
                File.Copy(file_name, file_name + ".bak", true);
                return true;
            }
            catch { }
            return false;
        }

        // inject tilemap at the correct offset in the rom file
        private static bool SaveTilemap(FileStream fs, byte[] tilemap, int border)
        {
            try
            {
                fs.Seek(border_data[border - 3].tilemap, SeekOrigin.Begin);
                fs.Write(tilemap, 0, tilemap.Length);
                return true;
            }
            catch { }
            return false;
        }

        // inject palettes at the correct offset in the rom file
        private static bool SavePalettes(FileStream fs, byte[] palettes, int border)
        {
            try
            {
                fs.Seek(border_data[border - 3].palettes, SeekOrigin.Begin);
                fs.Write(palettes, 0, palettes.Length);
                return true;
            }
            catch { }
            return false;
        }

        // inject tileset at the correct offset in the rom file, may require write to multiple chunks of data
        private static bool SaveTileset(FileStream fs, byte[] tileset, int border)
        {
            try
            {
                int bytes_written = 0;
                var tileset_chunks = border_data[border - 3].tileset_chunks;
                foreach (var (addr, chunk_length) in tileset_chunks)
                {
                    fs.Seek(addr, SeekOrigin.Begin);
                    int chunk_bytes = (tileset.Length - bytes_written) > chunk_length ? chunk_length : (tileset.Length - bytes_written);
                    fs.Write(tileset, bytes_written, chunk_bytes);
                    bytes_written += chunk_bytes;
                    if (bytes_written >= tileset.Length)
                        break;
                }
                if (border <= 9 && tileset.Length > border_data[border - 3].tileset_maxsize)
                {
                    CopyDMARoutine(fs);
                    PatchDMARoutines(fs, border);
                }
                else if (border >= 10) //todo check if tileset fits?
                {
                    CreateOldFullDMARoutine(fs);
                    PatchDMARoutines(fs, border);
                }
                return true;
            }
            catch { }
            return false;
        }

        // update icon in menu to custom icon
        private static bool SaveBorderIcon(FileStream fs, byte[] icon, int border)
        {
            try
            {
                fs.Seek(border_data[border - 3].icon, SeekOrigin.Begin);
                fs.Write(icon, 0, 64);
                fs.Seek(0x1C0, SeekOrigin.Current);
                fs.Write(icon, 64, 64);
                return true;
            }
            catch { }
            return false;
        }

        // apply patches to dma behaviour
        // this is necessary to write to the whole tileset in one chunk in vram
        // also makes sure that the tilemap gets updated only after all data is ready in vram
        private static bool ApplyDMAPatches(FileStream fs, int border)
        {
            try
            {
                var patches = border_data[border - 3].patches;
                foreach (var (offset, value) in patches)
                {
                    fs.Seek(offset, SeekOrigin.Begin);
                    fs.WriteByte(value);
                }
                return true;
            }
            catch { }
            return false;
        }

        //
        private static bool CopyDMARoutine(FileStream fs)
        {
            try
            {
                byte[] buf = new byte[0x30];
                fs.Seek(0x044D88, SeekOrigin.Begin);
                fs.Read(buf, 0, 0x30);
                fs.Seek(0x072F00, SeekOrigin.Begin);
                fs.Write(buf, 0, 0x30);
                return true;
            }
            catch { }
            return false;
        }

        private static bool CreateOldFullDMARoutine(FileStream fs)
        {
            try
            {
                fs.Seek(0x8456, SeekOrigin.Begin);
                WriteOldDMAChunkASM(fs, 0xCCF8, 0x0000, 0xD00);
                WriteOldDMAChunkASM(fs, 0xD9F8, 0x0D00, 0xD00);
                WriteOldDMAChunkASM(fs, 0xE6F8, 0x1A00, 0xD00);
                WriteOldDMAChunkASM(fs, 0xF3F8, 0x2700, 0xD00);
                fs.WriteByte(0x60);
            }
            catch { }
            return false;
        }

        private static bool PatchDMARoutines(FileStream fs, int border)
        {
            try
            {
                if (border < 10)
                {
                    // SGB2 borders
                    if (extended_dma_chunks[border - 3].offset > 0)
                    {
                        fs.Seek(extended_dma_chunks[border - 3].offset, SeekOrigin.Begin);
                        fs.WriteByte(0x22);
                        fs.Write(BitConverter.GetBytes(extended_dma_chunks[border - 3].jump), 0, 3);

                        int bank = extended_dma_chunks[border - 3].jump >> 16;
                        if (bank >= 0x80)
                            bank -= 0x80;

                        fs.Seek((bank * 0x8000) + (extended_dma_chunks[border - 3].jump % 0x10000) - 0x8000, SeekOrigin.Begin);
                        fs.Write(new byte[] { 0x22, 0x7B, 0xF8, 0x00 }, 0, 4);
                        foreach (var dma_chunk in extended_dma_chunks[border - 3].dma_chunks)
                        {
                            WriteDMAChunkASM(fs, dma_chunk);
                        }
                        fs.WriteByte(0x6B);
                    }
                }
                else
                {
                    // SGB1 borders
                    border -= 7;
                    // todo: jump to the correct location
                    fs.Seek(old_dma_locations[border - 3].tileset_dma_start, SeekOrigin.Begin);
                    fs.Write(new byte[] { 0x20, 0x56, 0x84 }, 0, 0x03);
                    fs.WriteByte(0x82);
                    byte skip_to_tilemap = (byte)(old_dma_locations[border - 3].tilemap_decompression - fs.Position - 2);
                    fs.WriteByte(skip_to_tilemap);
                    fs.WriteByte(0x00);
                }
            }
            catch { }
            return false;
        }

        private static bool WriteDMAChunkASM(FileStream fs, (int bank, int address, int vram, int size) dma_chunk)
        {
            try
            {
                byte[] b_size = BitConverter.GetBytes(dma_chunk.size);
                byte[] b_vram = BitConverter.GetBytes(dma_chunk.vram / 2);
                byte[] b_address = BitConverter.GetBytes(dma_chunk.address);
                fs.Write(new byte[] { 0xC2, 0x30, 0xA0, b_size[0], b_size[1], 0xA2, b_vram[0], b_vram[1], 0xA9, b_address[0], b_address[1], 0x8D, 0xD6, 0x03, 0xE2, 0x20, 0xA9, (byte)dma_chunk.bank, 0x8D, 0xD8, 0x03, 0x20, 0x00, 0xAF, 0xE2, 0x30, 0x22, 0x7B, 0xF8, 0x00 }, 0, 0x1E);
                return true;
            }
            catch { }
            return false;
        }

        private static bool WriteOldDMAChunkASM(FileStream fs, int address, int vram, int size)
        {
            try
            {
                byte[] b_size = BitConverter.GetBytes(size);
                byte[] b_vram = BitConverter.GetBytes(vram / 2);
                byte[] b_address = BitConverter.GetBytes(address);
                fs.Write(new byte[] { 0x22, 0x7B, 0xF8, 0x00, 0xC2, 0x20, 0xA9, b_size[0], b_size[1], 0x85, 0xCC, 0xA9, b_vram[0], b_vram[1], 0x85, 0xCE, 0xA9, b_address[0], b_address[1], 0x85, 0xD0, 0xE2, 0x20, 0xA9, 0x7E, 0x85, 0xD2, 0x22, 0x5C, 0xD6, 0x01 }, 0, 0x1F);
                return true;
            }
            catch { }
            return false;
        }

        // automatically switch to the new border on startup
        // uses the same offsets as sgb settings editor app
        private static bool SetStartupBorder(FileStream fs, int border)
        {
            bool old_border = border > 9;
            if (old_border)
                border -= 7;

            try
            {
                // check if there's already a jump to 87:F26C, if not, add it
                fs.Seek(0x50B7, SeekOrigin.Begin);
                int b = fs.ReadByte();
                if (b != 0x5C && b != 0x80)
                {
                    // move button initialization ahead
                    fs.Seek(0x50B1, SeekOrigin.Begin);
                    byte[] buttonConfig = new byte[12];
                    fs.Read(buttonConfig, 0, 12);
                    fs.Seek(0x50AB, SeekOrigin.Begin);
                    fs.Write(buttonConfig, 0, 12);
                }

                // skip code
                fs.Seek(0x50B7, SeekOrigin.Begin);
                fs.Write(new byte[] { 0x80, 0x04, 0xEA, 0xEA, 0xEA, 0xEA }, 0, 6);
                // jump to custom code at 87:F26C
                fs.Seek(0x50E9, SeekOrigin.Begin);
                fs.Write(new byte[] { 0x5C, 0x6C, 0xF2, 0x87 }, 0, 4); // jml $87f26C

                fs.Seek(0x03F26C, SeekOrigin.Begin);
                // overwritten code
                fs.Write(new byte[] { 0x22, 0x46, 0xD5, 0x01 }, 0, 4);
                // abort border change if game is SGB enhanced (do all enhanced games have a special border?)
                fs.Write(new byte[] { 0xA9, 0x03, 0xCD, 0x4C, 0x06, 0xD0, 0x04, 0x5C, 0xED, 0xD0, 0x00 }, 0, 11); // lda #$03, cmp $064C, bne $f287, jml $00d0bd
                // enable BG3 for game display
                fs.Write(new byte[] { 0xA9, 0x16, 0x8D, 0x2C, 0x21 }, 0, 5); // lda #$16, sta $212c
                // switch to old_border slots
                if (old_border)
                    fs.Write(new byte[] { 0xA9, 0x01, 0x8D, 0xFF, 0x07 }, 0, 5); // lda #$01; sta $7e07ff
                // store border # to 7e0c03 and 0 to 7e0341 to disable the sfx
                fs.Write(new byte[] { 0xA9, (byte)border, 0x8D, 0x03, 0x0C, 0x9C, 0x41, 0x03 }, 0, 8);
                // jump into border change routine, after the border and audio selection
                fs.Write(new byte[] { 0x5C, 0x30, 0xDE, 0x00 }, 0, 4);
                // overwrite potential old patch data with \0
                fs.Write(new byte[0x3F2A0 - (int)fs.Position], 0, 0x3F2A0 - (int)fs.Position);

                // keep BG1 (bottom menu) disabled during border transition (lda #$16, sta $212c)
                fs.Seek(0x441F8, SeekOrigin.Begin);
                fs.WriteByte(0x16);
                return true;
            }
            catch { }
            return false;
        }

        // disable screensaver for borders that were modified
        private static bool DisableScreensaverSlot(FileStream fs, int border)
        {
            try
            {
                if (border < 3 || border > 16)
                    return false;

                bool old_border = border > 9;
                if (old_border)
                    border -= 7;

                fs.Seek(0x3F2A0, SeekOrigin.Begin);
                int mod = fs.ReadByte();
                int mod_oldslots = fs.ReadByte();
                if (old_border)
                    mod_oldslots |= 0x01 << (border - 3);
                else
                    mod |= 0x01 << (border - 3);

                // try to detect if borders were changed with an older version of this tool
                for (int i = 3; i < 10; i++)
                {
                    var (_, _, tileset_chunks, _, palettes, _, _, checksum) = border_data[i - 3];
                    int b = 0;
                    fs.Seek(palettes, SeekOrigin.Begin);
                    for (int j = 0; j < 48; j++)
                        b += fs.ReadByte();

                    fs.Seek(tileset_chunks[0].addr + 0x20, SeekOrigin.Begin);
                    for (int j = 0; j < 64; j++)
                        b += fs.ReadByte();

                    if (b != checksum)
                        mod |= 0x01 << (i - 3);
                }

                // update byte to mark border(s) as modified
                fs.Seek(0x3F2A0, SeekOrigin.Begin);
                fs.WriteByte((byte)mod);
                fs.WriteByte((byte)mod_oldslots);

                // asm changes
                // jump to border modification check
                fs.Seek(0x5E3C, SeekOrigin.Begin);
                fs.Write(new byte[] { 0x22, 0xA2, 0xF2, 0x87 }, 0, 0x04);

                // jump to "ding" check
                fs.Seek(0xDDD3, SeekOrigin.Begin);
                fs.Write(new byte[] { 0x22, 0xCE, 0xF2, 0x87 }, 0, 0x04);

                fs.Seek(0x3F2A2, SeekOrigin.Begin);
                // check if border was modified
                fs.Write(new byte[] { 0x8F, 0x11, 0xC0, 0x7E, 0x9C, 0xFF, 0x02, 0xAD, 0x03, 0x0C, 0x38, 0xE9, 0x03, 0x30, 0x1C, 0x1A, 0xDA, 0xAA, 0xA9, 0x00, 0x2A, 0xCA, 0xD0, 0xFC, 0xAE, 0xFF, 0x07, 0xD0, 0x06, 0x2F, 0xA0, 0xF2, 0x87, 0x80, 0x04, 0x2F, 0xA1, 0xF2, 0x87, 0x8D, 0xFF, 0x02, 0xFA, 0x6B }, 0, 0x2C);
                // only "ding" on LLLLR input if the screensaver will be activated
                fs.Write(new byte[] { 0xAD, 0xFE, 0x02, 0xE2, 0x20, 0xD0, 0x03, 0xA9, 0x01, 0x6B, 0xA9, 0x00, 0x6B }, 0, 0x0D);

                return true;
            }
            catch { }
            return false;
        }

        // update the checksum and complement in the internal rom header
        private static bool UpdateChecksum(FileStream fs)
        {
            try
            {
                // write changes to file
                fs.Flush();

                // calculate checksum
                int checksum = 0, complement = 0, b = 0;
                fs.Seek(0, SeekOrigin.Begin);
                while ((b = fs.ReadByte()) != -1)
                    checksum += b; // checksum can never reach maxint32, so this is fine
                checksum &= 0x0000FFFF;
                complement = ~checksum & 0x0000FFFF;

                fs.Seek(0x7FDC, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(complement), 0, 2);
                fs.Write(BitConverter.GetBytes(checksum), 0, 2);
                return true;
            }
            catch { }
            return false;
        }

        // Load image from file and add it to icons
        internal static bool LoadIcon(string file_name)
        {
            try
            {
                Bitmap icon = new Bitmap(file_name);
                if (icon.Width == 10 && icon.Height == 14)
                {
                    Bitmap full_icon = new Bitmap(16, 16);
                    Graphics g = Graphics.FromImage(full_icon);
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    g.DrawImageUnscaledAndClipped(icons[0], new Rectangle(0, 0, 16, 16));
                    g.DrawImageUnscaledAndClipped(icon, new Rectangle(2, 1, 10, 14));
                    icon = full_icon;
                }
                else if(icon.Width != 16 || icon.Height != 16)
                {
                    return false;
                }
                
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        Color c = icon.GetPixel(x, y);
                        if (!icon_palette.Contains((ushort) ConvertRGBtoSFC(c.R, c.G, c.B)))
                            return false;
                    }
                }
                
                icons.Add(icon);
                return true;
            }
            catch { }

            return false;
        }

        // Convert icon bitmap to 4bpp tiles
        private static byte[] ConvertIconToBytes(Bitmap icon) {
            byte[] icon_bytes = new byte[128];

            if (icon.Width != 16 || icon.Height != 16)
                return icon_bytes;

            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        Color c = icon.GetPixel((7 - x) + 8 * (i % 2), y + 8 * (i / 2));
                        byte color_index = (byte)(Array.IndexOf(icon_palette, (ushort) ConvertRGBtoSFC(c.R, c.G, c.B)) + 1);
                        icon_bytes[32 * i + 2 * y] = (byte)(icon_bytes[32 * i + 2 * y] | (color_index & 0b0001) << x);
                        icon_bytes[32 * i + 2 * y + 1] = (byte)(icon_bytes[32 * i + 2 * y + 1] | ((color_index & 0b0010) >> 1) << x);
                        icon_bytes[32 * i + 2 * y + 16] = (byte)(icon_bytes[32 * i + 2 * y + 16] | ((color_index & 0b0100) >> 2) << x);
                        icon_bytes[32 * i + 2 * y + 17] = (byte)(icon_bytes[32 * i + 2 * y + 17] | ((color_index & 0b1000) >> 3) << x);
                    }
                }
            }

#if DEBUG
            File.WriteAllBytes("icon.bin", icon_bytes);
#endif
            return icon_bytes;
        }

        // write line to text box in the gui window
        internal static void WriteLine(string line = "", bool newLine = true)
        {
            window.Invoke(window.WriteLineHandler, line, newLine);
        }

        // change the preview picture
        internal static void UpdateImage(Bitmap image)
        {
            window.Invoke(window.UpdateImageHandler, new Bitmap(image));
        }
    }
}
