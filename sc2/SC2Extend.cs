using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public static class SC2Extend
    {
        public static Bitmap ToBitmap(this ImageData imgData)
        {
            return ByteArrayToBitmap(imgData.Data.ToByteArray(), imgData.Size.X, imgData.Size.Y);
        }

        public static Bitmap ByteArrayToBitmap(byte[] data, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

    }

}
