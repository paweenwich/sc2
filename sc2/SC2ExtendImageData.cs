﻿using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public static class SC2ExtendImageData
    {
        public static Bitmap ByteArrayToBitmap(byte[] data, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public static Bitmap ToBitmap(this ImageData imgData)
        {
            return ByteArrayToBitmap(imgData.Data.ToByteArray(), imgData.Size.X, imgData.Size.Y);
        }

        public static Bitmap ToDebugBitmap(this ImageData imgData, float scale = 50.0f, bool flgColor = false)
        {
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Yellow);
            SolidBrush drawBrush2 = new SolidBrush(System.Drawing.Color.White);
            Pen pen = new Pen(System.Drawing.Color.Yellow, 1);
            Bitmap bv = new Bitmap((int)(imgData.Size.X * scale), (int)(imgData.Size.Y * scale), PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bv);
            g.Clear(System.Drawing.Color.Black);
            for (int x = 0; x < imgData.Size.X; x++)
            {
                for (int y = 0; y < imgData.Size.Y; y++)
                {
                    byte value = imgData.GetValue(x, y);
                    if (flgColor)
                    {
                        System.Drawing.Color color = System.Drawing.Color.FromArgb(value, value, value);
                        switch (value)
                        {
                            case 127: color = System.Drawing.Color.Black; break;
                            case 134: color = System.Drawing.Color.Orange; break;
                            case 135: color = System.Drawing.Color.OrangeRed; break;
                            case 143: color = System.Drawing.Color.Brown; break;
                            case 142: color = System.Drawing.Color.Green; break;
                            case 141: color = System.Drawing.Color.Lime; break;
                            case 140: color = System.Drawing.Color.Cyan; break;
                        }
                        g.FillRectangle(new SolidBrush(color), new Rectangle((int)(x * scale), (int)(y * scale), (int)scale, (int)scale));
                    }
                    g.DrawString("" + value, drawFont, drawBrush2, new PointF((float)(x) * scale, (float)(y+0.5) * scale));
                    g.DrawString(String.Format("{0},{1}",x, imgData.Size.Y - y), drawFont, drawBrush, new PointF((float)(x) * scale, (float)(y) * scale));

                }
            }
            for (int x = 0; x < imgData.Size.X; x++)
            {
                int px = (int)(x * scale);
                g.DrawLine(pen, px, 0, px, bv.Height);
            }
            for (int y = 0; y < imgData.Size.Y; y++)
            {
                int py = (int)(y * scale);
                g.DrawLine(pen, 0, py, bv.Width, py);
            }

            g.Save();
            g.Dispose();
            return bv;
        }

        public static void Save(this ImageData imgData, String filePath)
        {
            byte[] ret = new byte[imgData.CalculateSize()];
            Google.Protobuf.CodedOutputStream os = new Google.Protobuf.CodedOutputStream(ret);
            imgData.WriteTo(os);
            File.WriteAllBytes(filePath, ret);
        }
        public static void Load(this ImageData imgData, String filePath)
        {
            byte[] ret = File.ReadAllBytes(filePath);
            Google.Protobuf.CodedInputStream ins = new Google.Protobuf.CodedInputStream(ret);
            imgData.MergeFrom(ins);
        }
        //x,y = data,bitmap cordinate
        public static byte GetValue(this ImageData imgData, int x, int y)
        {
            int addr = (y * imgData.Size.X) + x;
            return imgData.Data.ElementAt(addr);
        }
        //x,y = world cordinate
        public static bool IsPlaceable(this ImageData imgData, int x, int y,int size,int value = 255)
        {
            y = imgData.Size.Y - y;
            for(int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int px = x + i;
                    int py = y + j;
                    if (px >= imgData.Size.X) return false;
                    if (py >= imgData.Size.Y) return false;
                    if (imgData.GetValue(px, py) != value) return false;
                }
            }
            return true;
        }
        // return list of data cordinate
        public static List<Point2D> FindPattern(this ImageData imgdata,byte[][] pattern)
        {
            List<Point2D> ret = new List<Point2D>();
            for (int y = 0; y < imgdata.Size.Y - pattern.Length; y++)
            {
                for (int x = 0; x < imgdata.Size.X - pattern[0].Length; x++)
                {
                    bool found = true;
                    for(int j =0; j< pattern.Length; j++)
                    {
                        for (int i = 0; i < pattern[0].Length; i++)
                        {
                            int px = x + i;
                            int py = y + j;
                            if(imgdata.GetValue(px,py) != pattern[j][i])
                            {
                                found = false;
                                break;
                            }
                        }
                        if(found == false)
                        {
                            break;
                        }
                    }
                    if(found == true)
                    {
                        Point2D p = new Point2D();
                        p.X = x;
                        p.Y = y;
                        ret.Add(p);
                    }
                }
            }
            return ret;
        }
    }
}
