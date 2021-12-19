using System.Drawing;
using SimplePaletteQuantizer.Ditherers;
using SimplePaletteQuantizer.Ditherers.Ordered;
using SimplePaletteQuantizer.Helpers;
using SimplePaletteQuantizer.Quantizers;
//using SimplePaletteQuantizer.Quantizers.DistinctSelection;
using SimplePaletteQuantizer.Quantizers.Octree;
using SimplePaletteQuantizer.Quantizers.XiaolinWu;

namespace SGB2_Border_Injector
{
    // Code from:
    // A Simple - Yet Quite Powerful - Palette Quantizer in C# (Version 5.0)
    // by Smart K8
    // https://www.codeproject.com/Articles/66341/A-Simple-Yet-Quite-Powerful-Palette-Quantizer-in-C
    //
    // WuColorQuantizer by Xiaolin Wu
    //
    // Licensed under The Code Project Open License (CPOL) 1.02
    // https://www.codeproject.com/info/cpol10.aspx

    class SimplePaletteQuantizer
    {
        public static Bitmap SmartColorReducer(Image sourceImage, int colorCount, bool wu, bool dither)
        {
            IColorQuantizer quantizer = wu ? (IColorQuantizer) new WuColorQuantizer() : (IColorQuantizer) new OctreeQuantizer();
            IColorDitherer ditherer = dither ? new BayerDitherer4() : null;
            int parallelTaskCount = quantizer.AllowParallel ? 4 : 1;
            Bitmap quantized = (Bitmap)ImageBuffer.QuantizeImage(sourceImage, quantizer, ditherer, colorCount, parallelTaskCount);
            return quantized.Clone(new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        /*public static Bitmap SimpleColorReducer(Image sourceImage, int colorCount)
        {
            IColorQuantizer quantizer = new DistinctSelectionQuantizer();
            int parallelTaskCount = quantizer.AllowParallel ? 4 : 1;

            return (Bitmap)ImageBuffer.QuantizeImage(sourceImage, quantizer, null, colorCount, parallelTaskCount);
        }*/

    }
}
