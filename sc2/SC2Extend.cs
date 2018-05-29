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


        public static bool OverlapWith(this Unit self,Unit other)
        {
            float d = self.Pos.Dist(other.Pos);
            return self.OverlapWith(other.Pos.X, other.Pos.Y, other.Radius);
        }

        public static bool OverlapWith(this Unit self, float x,float y,float range)
        {
            float d = self.Pos.Dist(x,y);
            return (d < (self.Radius + range + 1));
        }


    }

}
