using Google.Protobuf;
using SC2APIProtocol;
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
    public static class SC2Extend
    {
        public static void DrawCircle(this Graphics g, Pen pen,
                                 float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        public static void FillCircle(this Graphics g, Brush brush,
                                      float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        public static float Dist(this SC2APIProtocol.Point p1, SC2APIProtocol.Point p2)
        {
            return p1.Dist(p2.X, p2.Y);
        }

        public static float Dist(this SC2APIProtocol.Point p1, float x, float y)
        {
            float dx = p1.X - x;
            float dy = p1.Y - y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        public static System.Drawing.Point ToPoint(this SC2APIProtocol.Point self,float scale=1.0f)
        {
            return new System.Drawing.Point((int) (self.X*scale),(int) (self.Y*scale));
        }
        public static System.Drawing.Point ToTopLeftPoint(this SC2APIProtocol.Point self, int worldHeight, float scale = 1.0f)
        {
            return new System.Drawing.Point((int)(self.X * scale), (int)((worldHeight - self.Y) * scale));
        }
        public static SC2APIProtocol.Point ToButtomLeftPoint(this System.Drawing.Point self, int worldHeight, float scale = 1.0f)
        {
            SC2APIProtocol.Point p = new SC2APIProtocol.Point();
            p.X = (self.X * scale);
            p.Y = ((worldHeight - self.Y) * scale);
            return p;
        }

        public static float Dist(this Point2D p1, Point2D p2)
        {
            return p1.Dist(p2.X, p2.Y);
        }

        public static float Dist(this Point2D p1, float x, float y)
        {
            float dx = p1.X - x;
            float dy = p1.Y - y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static float Dist(this System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return p1.Dist(p2.X, p2.Y);
        }

        public static float Dist(this System.Drawing.Point p1, float x, float y)
        {
            float dx = p1.X - x;
            float dy = p1.Y - y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static bool HasCommand(this SC2APIProtocol.Action self)
        {
            return self.ActionRaw.UnitCommand != null;
        }
        public static String ToStringEx(this SC2APIProtocol.Action self)
        {
            if (self.HasCommand() && (self.ActionRaw.UnitCommand.AbilityId!=0))
            {
                return Enum.GetName(typeof(ABILITY_ID), self.ActionRaw.UnitCommand.AbilityId) + " " + self.ToString();
            }else
            {
                return self.ToString();
            }
        }

        public static void Save(this IMessage self, BinaryWriter s)
        {
            byte[] ret = self.ToByteArray();
            s.Write(ret.Length);
            s.Write(ret);
        }

        public static void Save(this IMessage self, String filePath)
        {
            byte[] ret = self.ToByteArray();
            File.WriteAllBytes(filePath, ret);
        }
        public static byte[] ToByteArray(this IMessage self, String filePath)
        {
            byte[] ret = new byte[self.CalculateSize()];
            Google.Protobuf.CodedOutputStream os = new Google.Protobuf.CodedOutputStream(ret);
            self.WriteTo(os);
            return ret;
        }

        public static void Load(this IMessage self, BinaryReader s)
        {
            int num = s.ReadInt32();
            byte[] data = s.ReadBytes(num);
            self.FromByteArray(data);
        }


        public static void Load(this IMessage self, String filePath)
        {
            byte[] ret = File.ReadAllBytes(filePath);
            self.FromByteArray(ret);
        }

        public static void FromByteArray(this IMessage self,byte[] ret)
        {
            Google.Protobuf.CodedInputStream ins = new Google.Protobuf.CodedInputStream(ret);
            self.MergeFrom(ins);
        }


        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        
    }

}
