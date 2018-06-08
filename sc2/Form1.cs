using Accord.MachineLearning;
using Newtonsoft.Json;
using SC2APIProtocol;
using Starcraft2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            ListGameStates();
            
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
            Bitmap bmp = placeMap.ToDebugBitmap(scale, null, new ToDebugBitmapOption {flgColor = true});
            Graphics g = Graphics.FromImage(bmp);
            foreach (Point2D p in points)
            {
                //Console.WriteLine(p.ToString());
                //g.DrawRectangle(penViolet, new Rectangle((int)(p.X * scale), bmp.Height - (int)(p.Y * scale), (int)(5 * scale), (int)(3 * scale)));
                g.DrawCircle(SC2ExtendImageData.penViolet, p.X * scale, bmp.Height - (p.Y * scale), 3 * scale);
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

        private void test3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SC2Bot bot = new TerranBot();
            bot.SetBoolProperty("Log", true);
            bot.Update(currentGameState);
            if (bot.enemyUnit.all.Count() > 0)
            {

            }


        }
        private void saveStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SC2Bot sc2bot = (SC2Bot) Program.bot;
            //sc2bot.newObservation.Save(@"NewObservation.bin");
            //var settings = new SharpSerializerBinarySettings(BinarySerializationMode.Burst);
            //var serializer = new SharpSerializer(false);
            //serializer.Serialize(sc2bot.gameState, "sc2bot.xml");
            TerranBot tb = new TerranBot();
            string output = JsonConvert.SerializeObject(tb);
            tb = JsonConvert.DeserializeObject<TerranBot>(output);
            //Console.WriteLine(output);
        }

        private void test4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void loadGameStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListGameStates();
        }

        private void tvGameState_Click(object sender, EventArgs e)
        {
        }

        private void tvGameState_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tv = (TreeView)(sender);
            LoadGameState(((FileInfo)tv.SelectedNode.Tag).FullName);

        }
        public void ListGameStates()
        {
            tvGameState.Nodes.Clear();
            String[] files = Directory.GetFiles(@".\gameState");
            foreach (String file in files)
            {
                FileInfo f = new FileInfo(file);
                Console.WriteLine(f.Name);
                TreeNode tn = new TreeNode(f.Name);
                tn.Tag = f;
                tvGameState.Nodes.Add(tn);
            }
        }
        public float picScreenScale = 10f;
        public SC2GameState currentGameState;
        public SC2Bot currentBot;
        public void LoadGameState(String fileName)
        {
            currentGameState = new SC2GameState(fileName);
            RefreshPicScreen();
        }

        public void DrawUnit(Bitmap bmp, List<Unit> units, float scale)
        {
            Graphics g = Graphics.FromImage(bmp);
            foreach (Unit u in units)
            {
                g.DrawLine(SC2ExtendImageData.penWhite,u.Pos.X * scale , bmp.Height - ((u.Pos.Y - u.Radius) * scale), u.Pos.X* scale,bmp.Height - ((u.Pos.Y + u.Radius) * scale));
                g.DrawLine(SC2ExtendImageData.penWhite,(u.Pos.X -u.Radius) * scale, bmp.Height - (u.Pos.Y  * scale), (u.Pos.X + u.Radius) *scale, bmp.Height - (u.Pos.Y * scale));
            }
            g.Save();
            g.Dispose();
        }

        public void RefreshPicScreen()
        {
            SC2GameState gs = currentGameState;
            Bitmap bmpHeight = gs.GameInfo.StartRaw.TerrainHeight.ToDebugBitmap(
                picScreenScale, gs.NewObservation.Observation.RawData.Units.ToList(), new ToDebugBitmapOption
                {
                    flgDrawGrid = chkDrawGrid.Checked,
                    flgDrawGridPos = chkDrawPosition.Checked,
                    flgDrawValue = chkDrawValue.Checked,
                    flgDrawTarget = chkDrawTarget.Checked,
                    flgColor = true
                }
            );
            currentBot = new TerranBot();
            //bot.SetBoolProperty("Log", true);
            currentBot.SetVariable(currentGameState);
            if (currentBot.enemyUnit.all.Count() > 0)
            {
                List<Unit> units = currentBot.enemyUnit.all.GetUnitInRange(currentBot.myUnit.armyUnit);
                if (units.Count > 0)
                {
                    Console.WriteLine(units.ToString());
                    DrawUnit(bmpHeight, units, picScreenScale);
                }    
            }

            picScreen.Image = bmpHeight;
            //bmpHeight.Save("test.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            picScreenScale *= 1.5f;
            if(picScreenScale> 100)
            {
                picScreenScale = 100;
            }
            RefreshPicScreen();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            picScreenScale /= 1.5f;
            if (picScreenScale < 10)
            {
                picScreenScale = 10;
            }
            RefreshPicScreen();
        }

        private void drawGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshPicScreen();
        }

        private void picScreen_MouseHover(object sender, EventArgs e)
        {
        }

        private System.Drawing.Point oldLocation = System.Drawing.Point.Empty;
        private void picScreen_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = picScreen.PointToClient(Cursor.Position);
            if (pos != oldLocation)
            {
                oldLocation = pos;
                // convert tol world position
                var p = pos.ToButtomLeftPoint(picScreen.Height, 1 / picScreenScale);
                if (currentBot != null)
                {
                    // Find Unit at this point 
                    Unit unitAtMouse = currentBot.allUnits.GetUnit(p);
                    if (unitAtMouse != null)
                    {
                        String strText = unitAtMouse.ToSimpleString() + "\n" +  unitAtMouse.ToJson();
                        //picScreenToolTip.SetToolTip(picScreen, strText);
                        //picScreenToolTip.ToolTipTitle = "Position";
                        picScreenToolTip.Show(strText, picScreen, 10000);

                        //currentBot.allUnits.Get
                        //Console.WriteLine("Mouse move " + strPos);
                        picScreenToolTip.Active = true;
                    }
                    else
                    {
                        picScreenToolTip.Active = false;
                    }
                }
                else
                {
                    picScreenToolTip.ToolTipTitle = "";
                }
            }
        }
    }


}
