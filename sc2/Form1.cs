using Accord.MachineLearning;
using SC2APIProtocol;
using Starcraft2;
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
            TerranBot tb = (TerranBot)Program.bot;
            Console.WriteLine(tb.allUnits.Count.ToString());
            List<Point2D> points = tb.FindPlaceables((int)28.5, (int)60.5, 15, UNIT_TYPEID.TERRAN_BARRACKS, true);
            ImageData heightMap = new ImageData();
            heightMap.Load(@"TerrainHeight.bin");
            ImageData placeMap = new ImageData();
            placeMap.Load(@"PlacementGrid.bin");
            float scale = 50f;
            Bitmap bmp = placeMap.ToDebugBitmap(scale, true);
            Graphics g = Graphics.FromImage(bmp);
            foreach (Point2D p in points)
            {
                //Console.WriteLine(p.ToString());
                //g.DrawRectangle(penViolet, new Rectangle((int)(p.X * scale), bmp.Height - (int)(p.Y * scale), (int)(5 * scale), (int)(3 * scale)));
                g.DrawCircle(penViolet, p.X * scale, bmp.Height - (p.Y * scale), 3 * scale);
            }

            g.Save();
            g.Dispose();
            bmp.Save(@"TerrainHeightWithUnitDebug.png", ImageFormat.Png);

        }
        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResponseObservation obs = new ResponseObservation();
            obs.Load(@"NewObservation.bin");

            List<Unit> allUnits = obs.Observation.RawData.Units.ToList();
            ImageData heightMap = new ImageData();
            heightMap.Load(@"TerrainHeight.bin");
            ImageData placeMap = new ImageData();
            placeMap.Load(@"PlacementGrid.bin");
            placeMap = SC2Bot.CreatePlaceableImageData(placeMap, allUnits);
            // collect base expansion
            List<Unit> gas = allUnits.GetUnits(UNIT_TYPEID.NEUTRAL_VESPENEGEYSER);
            //List<Point2D> points = allUnits.FindBaseLocation();
            Console.WriteLine("" + gas.Count);
            double[][] data = new double[gas.Count][];
            for(int i = 0; i < gas.Count; i++)
            {
                data[i] = new double[] { gas[i].Pos.X, gas[i].Pos.Y };
            }
            KMeans kmeans = new KMeans(k: gas.Count/2);
            KMeansClusterCollection clusters  = kmeans.Learn(data);
            
        }
        public Pen penRed = new Pen(System.Drawing.Color.Red, 5);
        public Pen penGreen = new Pen(System.Drawing.Color.Green, 5);
        public Pen penBlue = new Pen(System.Drawing.Color.Blue, 5);
        public Pen penWhite = new Pen(System.Drawing.Color.White, 5);
        public Pen penYellow = new Pen(System.Drawing.Color.Yellow, 5);
        public Pen penBlack = new Pen(System.Drawing.Color.Black, 5);
        public Pen penOrange = new Pen(System.Drawing.Color.Orange, 5);
        public Pen penViolet = new Pen(System.Drawing.Color.Violet, 5);

/*        public byte[][] block2x2 = new byte[][]
        {
            new byte[] {1,1},
            new byte[] {1,1},
        };
        public byte[][] blockBarrak = new byte[][]
        {
            new byte[] {1,1,1,0,0},
            new byte[] {1,1,1,1,1},
            new byte[] {1,1,1,1,1},
        };
        */

        private void test3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResponseObservation obs = new ResponseObservation();
            obs.Load(@"NewObservation.bin");

            List<Unit> allUnits = obs.Observation.RawData.Units.ToList();
            ImageData heightMap = new ImageData();
            heightMap.Load(@"TerrainHeight.bin");
            ImageData placeMap = new ImageData();
            placeMap.Load(@"PlacementGrid.bin");
            placeMap = SC2Bot.CreatePlaceableImageData(placeMap, allUnits);
            float scale = 50f;
            Bitmap bmp = placeMap.ToDebugBitmap(scale, true);
            Graphics g = Graphics.FromImage(bmp);
            Unit cc = null;
            
            foreach (Unit u in allUnits)
            {
                //Console.WriteLine("Checking " + ((UNIT_TYPEID)u.UnitType).ToString());
                if(u.UnitType == (int)UNIT_TYPEID.TERRAN_COMMANDCENTER)
                {
                    cc = u;
                }
/*                Pen pen = penWhite;
                switch (u.Alliance)
                {
                    case Alliance.Enemy: pen = penRed; break;
                    case Alliance.Neutral: pen = penGreen; break;
                    case Alliance.Self: pen = penBlue; break;
                }
                if(isPlaceable(u, placeMap, allUnits)||(u.idealRedius()< 1)||(u.Alliance == Alliance.Neutral))
                {

                    g.DrawCircle(pen, u.Pos.X * scale, bmp.Height - (u.Pos.Y * scale), u.Radius * scale);
                }
                else
                {
                    Console.WriteLine("Overlap detect " + u.ToStringEx());
                    g.DrawCircle(penOrange, u.Pos.X * scale, bmp.Height - (u.Pos.Y * scale), u.Radius * scale);
                }
*/                
            }
            int r = 10;
            Unit testUnit = SC2Bot.FakeBuildingUnit(UNIT_TYPEID.TERRAN_BARRACKS);
            byte[][] pattern = testUnit.GetBlock();
            bool flgSameLevel = true;
            int ccHeight = heightMap.GetValue((int)(cc.Pos.X), heightMap.Size.Y - (int)(cc.Pos.Y));
            for (int y = (int)(cc.Pos.Y - r); y < (int)(cc.Pos.Y + r); y++)
            {
                if ((y < 0) || (y > placeMap.Size.Y)) continue;
                for (int x = (int)(cc.Pos.X - r); x < (int)(cc.Pos.X + r); x++)
                {
                    if ((x < 0) || (x > placeMap.Size.X)) continue;
                    if(placeMap.IsPlaceable(x, y, pattern))
                    {
                        if( (x== 27) && (y == 56))
                        {
                            //Console.WriteLine(String.Format("{0} {1}", x, y));
                        }
                        if (flgSameLevel)
                        {
                            if(!heightMap.IsPlaceable(x,y,pattern, ccHeight))
                            {
                                continue;
                            }
                        }
                        testUnit.SetPos(x + testUnit.idealRedius(), y - testUnit.idealRedius());
                        if(!isPlaceable(testUnit, placeMap, allUnits))
                        {
                            continue;
                        }
                        int ny = y;
                        g.DrawRectangle(penViolet, new Rectangle((int) (x*scale),bmp.Height - (int) (ny*scale), (int)(pattern[0].Length*scale),(int)(pattern.Length*scale)));
                    }
                }
            }
            g.Save();
            g.Dispose();
            bmp.Save(@"TerrainHeightWithUnit.png", ImageFormat.Png);

        }
        public bool isPlaceable(Unit u, ImageData placeMap, List<Unit> allUnits,byte[][] pattern = null)
        {
            if (pattern == null)
            {
                pattern = u.GetBlock();
            }
            float dx =(float) (pattern[0].Length / 2.0);
            float dy =(float) (pattern.Length / 2.0);
            float d = Math.Min(dx, dy);
            if (placeMap.IsPlaceable((int)(u.Pos.X - d), (int)(u.Pos.Y + d), pattern))
            {
                if (pattern.Length == pattern[0].Length)
                {
                    if (u.OverlapWith(allUnits))
                    {
                        return false;
                    }
                }
                else
                {
                    //Console.WriteLine(String.Format("Block {0}x{1} found", pattern[0].Length, pattern.Length));
                }
                return true;
            }
            return false;
        }
        private void saveStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SC2Bot sc2bot = (SC2Bot) Program.bot;
            sc2bot.gameState.NewObservation.Save(@"NewObservation.bin");
        }
    }


}
