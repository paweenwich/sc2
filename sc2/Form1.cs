using Accord.MachineLearning;
using Newtonsoft.Json;
using SC2APIProtocol;
using Starcraft2;
using System;
using System.Collections;
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
            ISC2Bot bot = (ISC2Bot) new TerranBot(new SC2BotOption {flgReadMode = false });
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
        /*    SC2Command cmd = new SC2Command();
            cmd.type = SC2CommandType.MORPH_ORBITAL;
            Program.bot.SendCommand(cmd);*/
        }



        private void menuOptionCheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            Console.WriteLine(String.Format("{0} {1}", menu.Tag, menu.Checked.ToString()));
            Program.bot.SetBoolProperty((String)menu.Tag, menu.Checked);
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TarranQLearning q = new TarranQLearning();
            for (int i = 0; i < 10; i++) {
                QStringState s = (QStringState)String.Format("{0:0000}",i);
                int action = q.GetActionFromIndex(q.GetActionIndex(s));
                TarranQLearningAction ta = (TarranQLearningAction)action;
                Console.WriteLine(ta.ToString());
            }
            String tmp = JsonConvert.SerializeObject(q);
            Console.WriteLine(tmp);
            BinaryWriter bw = new BinaryWriter(new FileStream("test.q", FileMode.Create));
            q.Save(bw);
            bw.Close();

            BinaryReader br = new BinaryReader(new FileStream("test.q", FileMode.Open));
            TarranQLearning q2 = new TarranQLearning();
            q2.Load(br);
            br.Close();
            Console.WriteLine(JsonConvert.SerializeObject(q2));

            //TarranQLearning q3 = JsonConvert.DeserializeObject<TarranQLearning>(tmp);
            //Console.WriteLine(JsonConvert.SerializeObject(q3));

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

        }
        private void saveStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            if(tv.SelectedNode.Tag.GetType() == typeof(FileInfo))
            {
                LoadGameState(((FileInfo)tv.SelectedNode.Tag).FullName);
                return;
            }
            if (tv.SelectedNode.Tag.GetType() == typeof(DirectoryInfo))
            {
                if (tv.SelectedNode.GetNodeCount(false) ==0)
                {
                    ListGameStatesFromDir(tv.SelectedNode,((DirectoryInfo)tv.SelectedNode.Tag).FullName);
                }
                return;
            }
        }
        public void ListGameStatesFromDir(TreeNode node,String dirName)
        {
            String[] files = Directory.GetFiles(dirName);
            foreach (String file in files)
            {
                FileInfo f = new FileInfo(file);
                //Console.WriteLine(f.Name);
                TreeNode tn = new TreeNode(f.Name);
                tn.ImageIndex = 2;
                tn.SelectedImageIndex = 2;
                tn.Tag = f;
                node.Nodes.Add(tn);
            }
        }
        public void ListGameStates()
        {
            tvGameState.Nodes.Clear();
            List<String> dirs = Directory.GetDirectories(@".").Where(u => u.StartsWith(@".\GS")).ToList();
            foreach(String dir in dirs)
            {
                DirectoryInfo d = new DirectoryInfo(dir);
                TreeNode tn = new TreeNode(d.Name);
                tn.Tag = d;
                tvGameState.Nodes.Add(tn);
            }
        }
        public float picScreenScale = 16f;
        public SC2GameState currentGameState;
        public SC2Bot currentBot;
        public void LoadGameState(String fileName)
        {
            currentGameState = new SC2GameState(fileName);
            RefreshPicScreen();
            Console.WriteLine("Score = " + currentGameState.NewObservation.Observation.Score.ToString());
            Console.WriteLine("AIActions = " + currentGameState.AIActions.ToStringEx());
        }

        public void DrawUnit(Bitmap bmp, List<Unit> units, float scale)
        {
            Graphics g = Graphics.FromImage(bmp);
            foreach (Unit u in units)
            {
                g.DrawLine(SC2ExtendImageData.penViolet2,u.Pos.X * scale , bmp.Height - ((u.Pos.Y - u.Radius) * scale), u.Pos.X* scale,bmp.Height - ((u.Pos.Y + u.Radius) * scale));
                g.DrawLine(SC2ExtendImageData.penViolet2, (u.Pos.X -u.Radius) * scale, bmp.Height - (u.Pos.Y  * scale), (u.Pos.X + u.Radius) *scale, bmp.Height - (u.Pos.Y * scale));
            }
            g.Save();
            g.Dispose();
        }

        public void DrawUnitAction(Bitmap bmp, Unit u, float scale, List<Unit> allUnit, ActionRawUnitCommand cmd, bool isAI = false)
        {
            Graphics g = Graphics.FromImage(bmp);
            if (isAI)
            {
                if (cmd != null)
                {
                    g.DrawString(Enum.GetName(typeof(ABILITY_ID), cmd.AbilityId), SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushWhite, u.Pos.X * scale + 1, bmp.Height - (u.Pos.Y * scale) - 5 + 1);
                    g.DrawString(Enum.GetName(typeof(ABILITY_ID), cmd.AbilityId), SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushBlack, u.Pos.X * scale, bmp.Height - (u.Pos.Y * scale) - 5);
                }else
                {
                    g.DrawString("None", SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushWhite, u.Pos.X * scale + 1, bmp.Height - (u.Pos.Y * scale) - 5 + 1);
                    g.DrawString("None", SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushBlack, u.Pos.X * scale, bmp.Height - (u.Pos.Y * scale) - 5);
                }
            }
            else
            {
                if (cmd != null)
                {
                    g.DrawString(Enum.GetName(typeof(ABILITY_ID), cmd.AbilityId), SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushBlack, u.Pos.X * scale + 1, bmp.Height - (u.Pos.Y * scale) - 5 + 1);
                    g.DrawString(Enum.GetName(typeof(ABILITY_ID), cmd.AbilityId), SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushYellow, u.Pos.X * scale, bmp.Height - (u.Pos.Y * scale) - 5);
                }else
                {
                    g.DrawString("None", SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushBlack, u.Pos.X * scale + 1, bmp.Height - (u.Pos.Y * scale) - 5 + 1);
                    g.DrawString("None", SC2ExtendImageData.drawFont, SC2ExtendImageData.drawBrushYellow, u.Pos.X * scale, bmp.Height - (u.Pos.Y * scale) - 5);
                }
            }
            if (cmd != null)
            {
                Pen targetPen = SC2ExtendImageData.penBlack;
                Point2D targetPoint = cmd.TargetWorldSpacePos;
                if (cmd.TargetUnitTag != 0)
                {
                    Unit targtUint = allUnit.GetUnit(cmd.TargetUnitTag);
                    if (targtUint != null)
                    {
                        targetPoint = targtUint.Pos.ToPoint2D();
                    }
                }
                if (targetPoint != null)
                {
                    g.DrawLine(targetPen, u.Pos.X * scale, bmp.Height - (u.Pos.Y * scale), targetPoint.X * scale, bmp.Height - (targetPoint.Y * scale));
                }
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
            // Draw action
            if (gs.CurrentAction.HasCommand())
            {
                foreach(ulong tag in gs.CurrentAction.ActionRaw.UnitCommand.UnitTags)
                {
                    Unit u = currentBot.allUnits.GetUnit(tag);
                    if (u != null)
                    {
                        DrawUnitAction(bmpHeight, u, picScreenScale, currentBot.allUnits, gs.CurrentAction.ActionRaw.UnitCommand);
                    }
                }

            }
            // Draw AI Action
            if (gs.AIActions.Count > 0)
            {
                foreach (SC2UnitAction aiAction in gs.AIActions)
                {
                    Unit u = currentBot.allUnits.GetUnit(aiAction.Tag);
                    if (u != null)
                    {
                        
                        DrawUnitAction(bmpHeight, u, picScreenScale, currentBot.allUnits, aiAction.action.ActionRaw.UnitCommand,true);
                    }
                }
            }
            picScreen.Image = bmpHeight;
            //bmpHeight.Save("test.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            picScreenScale *= 1.25f;
            if(picScreenScale> 100)
            {
                picScreenScale = 100;
            }
            RefreshPicScreen();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            picScreenScale /= 1.25f;
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

        private void tvGameState_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeView tv = (TreeView)(sender);
            tv.SelectedNode.ImageIndex = 1;
            tv.SelectedNode.SelectedImageIndex = 1;
        }

        private void tvGameState_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            TreeView tv = (TreeView)(sender);
            tv.SelectedNode.ImageIndex = 0;
            tv.SelectedNode.SelectedImageIndex = 0;
            tv.Refresh();
        }
    }


}
