using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sc2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.bot = new TerranBot();
            Thread newThread = new Thread(Program.RunSC2);
            newThread.Start();
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageData data = new ImageData();
            data.Load(@"PlacementGrid.bin");
            //Console.WriteLine(data.ToString());
            Bitmap bmp = data.ToDebugBitmap();
            Graphics g = Graphics.FromImage(bmp);
            Random r = new Random();
            float scale = 50.0f;
            for(int i = 0; i < 10; i++)
            {
                while (true)
                {
                    int x = r.Next(data.Size.X);
                    int y = r.Next(data.Size.Y);
                    int size = 3;
                    float rad = (float)(size / 2.0);
                    if(data.IsPlaceable(x,y, 3))
                    {
                        g.DrawCircle(new Pen(System.Drawing.Color.Red, 2), (int) ((x+rad)*scale), (int) ((y+rad)*scale), rad*scale);
                        g.DrawRectangle(new Pen(System.Drawing.Color.Red, 2),(int) x * scale,(int) y * scale,(int) size * scale, (int) size * scale);
                        break;
                    }
                }
            }
            g.Save();
            g.Dispose();
            bmp.Save(@"PlacementGridDebug.png", ImageFormat.Png);
        }

        private void sendCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String[] input = txtInput.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if(input.Length == 2)
            {
                float v1 = (float)Double.Parse(input[0]);
                float v2 = (float)Double.Parse(input[1]);
                Console.WriteLine(String.Format("{0} {1}", v1, v2));
                SC2Command cmd = new SC2Command();
                cmd.type = SC2CommandType.BUILD_BARRAK;
                cmd.targetPos = new Point2D();
                cmd.targetPos.X = v1;
                cmd.targetPos.Y = v2;
                Program.bot.SendCommand(cmd);
                Console.WriteLine(cmd.ToString());
            }
        }
    }
}
