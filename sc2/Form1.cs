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
            ISC2Bot bot = (ISC2Bot) new TerranBot();
            Program.bot = bot;
            BuildMenu(bot);
            Thread newThread = new Thread(Program.RunSC2);
            newThread.Start();
        }

        private void BuildMenu(ISC2Bot bot)
        {
            mnuOption.DropDownItems.Clear();
            foreach (String name in bot.GetBoolProperty())
            {
                ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem()
                {
                    Name = "chk" + name,
                    Text = name,
                    Tag = name,
                    CheckOnClick = true,
                    Checked = bot.GetBoolProperty(name),

                };
                item.CheckedChanged += menuOptionCheckedChanged;
                mnuOption.DropDownItems.Add(item);
            }

        }


        private void sendCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*String[] input = txtInput.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
            }*/
            SC2Command cmd = new SC2Command();
            cmd.type = SC2CommandType.MORPH_ORBITAL;
            Program.bot.SendCommand(cmd);
        }



        private void menuOptionCheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            Console.WriteLine(String.Format("{0} {1}", menu.Tag, menu.Checked.ToString()));
            Program.bot.SetBoolProperty((String)menu.Tag, menu.Checked);
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageData data = new ImageData();
            data.Load(@"TerrainHeight.bin");
            //Console.WriteLine(data.ToString());
            Bitmap bmp = data.ToDebugBitmap(50f, true);
            Graphics g = Graphics.FromImage(bmp);
            Random r = new Random();
            g.Save();
            g.Dispose();
            bmp.Save(@"TerrainHeightDebug2.png", ImageFormat.Png);
        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageData data = new ImageData();
            data.Load(@"TerrainHeight.bin");
            foreach (TerranBuildPattern tbp in TerranData.rampPattens)
            {
                List<Point2D> ramp = data.FindPattern(tbp.pattern);
                foreach (Point2D p in ramp)
                {
                    Console.WriteLine(p.ToString());
                }
            }
        }
        private void test3ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }


}
