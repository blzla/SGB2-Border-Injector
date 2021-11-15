using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SGB2_Border_Injector
{
    class Program
    {
        private static readonly (string name, int tilemap, (int addr, int chunk_length)[] tileset_chunks, int tileset_maxsize, int palettes, (int addr, byte value)[] patches, int icon)[] border_data = new (string, int, (int, int)[], int, int, (int, byte)[], int)[] {
            ("GB (SGB2)", 0x051800, new (int, int)[] { (0x05C2C0, 0x0) }, 0x0, 0x56E00, new (int, byte)[] { }, 0x061D80),
            ("Black (SGB2)", 0x0, new (int, int)[] { (0x0, 0x0) }, 0x0, 0x0, new (int, byte)[] { }, 0x061DC0),
            ("Printed Circuit Board", 0x052000, new (int, int)[] { (0x059100, 0x1D00) }, 0x1D00, 0x57080, new (int, byte)[] { }, 0x062000),
            ("Palm Trees", 0x067000, new (int, int)[] { (0x063000, 0x4000) }, 0x4000, 0x67880, new (int, byte)[] { (0x449B7, 0x10), (0x449D5, 0x18) }, 0x062040),
            ("Stone Mosaic", 0x052800, new (int, int)[] { (0x04E000, 0x16E0), (0x04C000, 0x2000) }, 0x36E0, 0x57480, new (int, byte)[] { (0x40FC6, 0x70), (0x40FC7, 0x0B), (0x40FE4, 0x70), (0x40FE5, 0x1B), (0x4101D, 0xE0), (0x4101E, 0x06) }, 0x062080),
            ("Gears", 0x06D000, new (int, int)[] { (0x068000, 0x5000) }, 0x5000, 0x67A80, new (int, byte)[] { (0x446DC, 0x18), (0x446FA, 0x20) }, 0x0620C0),
            ("Swamp", 0x053000, new (int, int)[] { (0x048000, 0x4000) }, 0x4000, 0x57A80, new (int, byte)[] { (0x4153E, 0x10), (0x41541, 0x10), (0x41544, 0xA0), (0x4154B, 0x09), (0x4155F, 0x18), (0x41562, 0xB0), (0x4157A, 0x08), (0x4157D, 0x3C), (0x41587, 0x0A), (0x041C6B, 0x60) }, 0x062100),
            ("Dolphins", 0x053800, new (int, int)[] { (0x05CCC0, 0x30C0) }, 0x30C0, 0x57C80, new (int, byte)[] { (0x4265B, 0x10), (0x42679, 0x10), (0x4267C, 0x10), (0x4267E, 0xC0), (0x4267F, 0xEC), (0x42686, 0x0B), (0x42696, 0xC0), (0x42697, 0x00), (0x4269A, 0x18), (0x4269E, 0xC0), (0x4269D, 0xFC), (0x426B5, 0x08), (0x426B8, 0x3C), (0x426BB, 0xB8), (0x426C2, 0x0A) }, 0x062140),
            ("Chess Arena", 0x054800, new (int, int)[] { (0x050000, 0x1000), (0x055800, 0x2000) }, 0x3000, 0x57880, new (int, byte)[] { (0x43333, 0x50), (0x4334E, 0x10), (0x43351, 0x08), (0x43354, 0xD8), (0x4336F, 0x10), (0x43372, 0xE8), (0x4338A, 0x08), (0x4338D, 0x3C), (0x43390, 0xC8) }, 0x061D40)
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
        internal static bool InjectCustomBorder(string sgb2_rom, string border_file, int border, int icon, bool external_palettes, bool backup)
        {
            if (string.IsNullOrEmpty(sgb2_rom) || string.IsNullOrEmpty(border_file) || !(border >= 3 && border <= 9))
            {
                WriteLine($"[ERROR] Please set all inputs.");
                return false;
            }

            // read image
            Bitmap border_bitmap = LoadImage(border_file);
            if (border_bitmap == null)
            {
                WriteLine($"[ERROR] Invalid image.");
                return false;
            }

            // build tile data
            var (tiles_data, tiles_flipped_data, tile_colors, too_many_colors) = BuildTileData(border_bitmap);
            if (too_many_colors)
            {
                WriteLine($"[ERROR] Image contains too many colors.");
                return false;
            }

            // build palettes
            var (found_working_palettes, palette_sets) = BuildPalettes(tile_colors, external_palettes);
            if (!found_working_palettes)
                return false;

            // build tileset
            byte[] tileset = BuildTileset(tiles_data, tiles_flipped_data, palette_sets);
            if (tileset.Length > border_data[border - 1].tileset_maxsize)
            {
                WriteLine($"[ERROR] Tileset is too big for slot {border}. Please select a different slot.");
                WriteLine($"Fits in slots: {FitsInSlots(tileset.Length)}");
                return false;
            }

            // build tilemap
            byte[] tilemap = BuildTilemap(tiles_data, tiles_flipped_data, palette_sets);

#if DEBUG
            File.WriteAllBytes("tilemap.bin", tilemap);
            File.WriteAllBytes("tileset.bin", tileset);
            File.WriteAllBytes("palettes.bin", ConvertPalettesToBytes(palette_sets));
#endif

            // start modifying rom file
            WriteLine();
            var (success, msg) = ValidateRomFile(sgb2_rom);
            if (!success)
                WriteLine($"[ERROR] SGB2 rom error: {msg}");

            if (backup && success)
                if (success = BackupFile(sgb2_rom))
                    WriteLine($"Created backup file: {(sgb2_rom.Substring(sgb2_rom.LastIndexOf('\\') + 1) + ".bak")}");
                else
                    WriteLine("[ERROR] Failed to create backup file.");

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
                    success &= PatchOutScreensaver(fs);
                    // automatically switch to new border on startup
                    success &= SetStartupBorder(fs, border);
                    WriteLine($"Applied {border_data[border - 1].patches.Length} DMA patches, disabled screensaver, set startup border: {(success ? "Done" : "Error")}");

                    success &= UpdateChecksum(fs);
                    WriteLine($"Updated checksum.");
                }
            }

            return success;
        }

        // load image from file
        internal static Bitmap LoadImage(string file_name)
        {
            try
            {
                Bitmap border_bitmap = new Bitmap(256, 224);
                Graphics g = Graphics.FromImage(border_bitmap);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                Bitmap file = new Bitmap(file_name);
                if (file.Width > 1024 || file.Height > 1024)
                {
                    WriteLine($"[ERROR] Image too big. Expected 256 x 224 px image, read {file.Width} x {file.Height} pixels.");
                    return null;
                }
                else if (file.Width != 256 || file.Height != 224)
                    WriteLine($"[WARN] Expected 256 x 224 px image, read {file.Width} x {file.Height} pixels.");
                g.DrawImageUnscaledAndClipped(file, new Rectangle(0, 0, file.Width, file.Height));
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 239, 206)), new Rectangle(48, 40, 160, 144));
                return border_bitmap;
            }
            catch { }
            return null;
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

            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 28; y++)
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
                        total_colors.Add(snes_col);
                    }
                    if (colors.Count > 15)
                    {
                        too_many_colors = true;
                        WriteLine($"ERROR. Too many colors in tile {x},{y} ({colors.Count}).");
                        break;
                    }

                    AddTile(tiles_data, tiles_flipped_data, tile, colors, x, y);

                    bool known_colors = false;
                    foreach (HashSet<int> c in tile_colors)
                    {
                        if (c.IsSupersetOf(colors))
                        {
                            known_colors = true;
                            break;
                        }
                        else if (c.IsSubsetOf(colors))
                        {
                            known_colors = true;
                            tile_colors.Remove(c);
                            tile_colors.Add(colors);
                            break;
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

            WriteLine($"Found {tiles_data.Count} unique tiles and {tiles_flipped_data.Count} flipped versions.");
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
            int r = 0, maxr = 100000;
            HashSet<int>[] palette_sets = new HashSet<int>[] { new HashSet<int>(), new HashSet<int>(), new HashSet<int>() };

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
                    WriteLine($"[ERROR] Couldn't find working palettes after {maxr} tries.");
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
                    WriteLine("[ERROR] External palettes don't work for current picture.");
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
        private static string FitsInSlots(int tileset_length)
        {
            List<int> slots = new List<int>();
            for (int i = 2; i < border_data.Length; i++)
            {
                if (border_data[i].tileset_maxsize >= tileset_length)
                    slots.Add(i + 1);
            }

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
                fs.Seek(border_data[border - 1].tilemap, SeekOrigin.Begin);
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
                fs.Seek(border_data[border - 1].palettes, SeekOrigin.Begin);
                fs.Write(palettes, 0, palettes.Length);
                return true;
            }
            catch { }
            return false;
        }

        // inject tileset at the correct offset in the rom file, may require write to multiple chunks of data
        private static bool SaveTileset(FileStream fs, byte[] tileset, int border)
        {
            if (tileset.Length > border_data[border - 1].tileset_maxsize)
                return false;
            try
            {
                int bytes_written = 0;
                var tileset_chunks = border_data[border - 1].tileset_chunks;
                foreach (var (addr, chunk_length) in tileset_chunks)
                {
                    fs.Seek(addr, SeekOrigin.Begin);
                    int chunk_bytes = (tileset.Length - bytes_written) > chunk_length ? chunk_length : (tileset.Length - bytes_written);
                    fs.Write(tileset, bytes_written, chunk_bytes);
                    bytes_written += chunk_bytes;
                    if (bytes_written >= tileset.Length)
                        break;
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
                fs.Seek(border_data[border - 1].icon, SeekOrigin.Begin);
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
                var patches = border_data[border - 1].patches;
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

        // automatically switch to the new border on startup
        // uses the same offsets as sgb settings editor app
        private static bool SetStartupBorder(FileStream fs, int border)
        {
            try
            {
                // check if there's already a jump to 87:F270, if not, add it
                fs.Seek(0x50B7, SeekOrigin.Begin);
                if (fs.ReadByte() != 0x5C)
                {
                    // move button initialization ahead
                    fs.Seek(0x50B1, SeekOrigin.Begin);
                    byte[] buttonConfig = new byte[12];
                    fs.Read(buttonConfig, 0, 12);
                    fs.Seek(0x50AB, SeekOrigin.Begin);
                    fs.Write(buttonConfig, 0, 12);
                    // then jump to custom code at 87:F270
                    fs.Write(new byte[] { 0x5C, 0x70, 0xF2, 0x87, 0xEA, 0xEA }, 0, 6); // jml $87f270
                }

                fs.Seek(0x03F270, SeekOrigin.Begin);
                // abort border change if game is SGB enhanced (do all enhanced games have a special border?)
                fs.Write(new byte[] { 0xA9, 0x03, 0xCD, 0x4C, 0x06, 0xD0, 0x04, 0x5C, 0xBD, 0xD0, 0x00 }, 0, 11); // lda #$03, cmp $064C, bne $f287, jml $00d0bd
                // enable BG3 for game display
                fs.Write(new byte[] { 0xA9, 0x16, 0x8D, 0x2C, 0x21 }, 0, 5); // lda #$16, sta $212c
                // store border # to 7e0c03 and 0 to 7e0341 to disable the sfx
                fs.Write(new byte[] { 0xA9, (byte)border, 0x8D, 0x03, 0x0C, 0x9C, 0x41, 0x03 }, 0, 8);
                // jump into border change routine, after the border and audio selection
                fs.Write(new byte[] { 0x5C, 0x30, 0xDE, 0x00 }, 0, 4);
                // keep BG1 (bottom menu) disabled during border transition (lda #$16, sta $212c)
                fs.Seek(0x441F8, SeekOrigin.Begin);
                fs.WriteByte(0x16);
                return true;
            }
            catch { }
            return false;
        }

        // remove screensaver functionality for all borders (timer, button combo and sound effect)
        // it might be possible to keep screensaver for other borders and only remove screensaver code for the modified border slot (not implemented)
        private static bool PatchOutScreensaver(FileStream fs)
        {
            try
            {
                fs.Seek(0x3D36, SeekOrigin.Begin);
                fs.WriteByte(0x80);
                fs.Seek(0xDDD6, SeekOrigin.Begin);
                fs.WriteByte(0x00);
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
                    g.DrawImageUnscaledAndClipped(icons[0], new Rectangle(2, 1, 10, 14));
                    g.DrawImageUnscaledAndClipped(icon, new Rectangle());
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
        private static void WriteLine(string line = "")
        {
            window.Invoke(window.WriteLineHandler, line);
        }
    }
}
