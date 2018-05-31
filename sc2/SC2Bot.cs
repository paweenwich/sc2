using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using SC2APIProtocol;
using Starcraft2;
using System.Drawing;
using System.Drawing.Imaging;

namespace sc2
{
    public abstract class ISCBot
    {
        public abstract void Init(GameState gameState);
        public abstract SC2APIProtocol.Action Update(GameState gameState);
        public abstract void SendCommand(SC2Command cmd);
        public abstract void SetBoolProperty(String name,bool value);
        public abstract bool GetBoolProperty(String name);
    }

    public enum SC2CommandType 
    {
        BUILD_SUPPLY=1,BUILD_BARRAK,MORPH_ORBITAL
    }

    public class SC2Command
    {
        public SC2CommandType type;
        public Point2D targetPos;
        public override string ToString()
        {
            if (targetPos != null)
            {
                return type.ToString() + " " + targetPos.ToString();
            }else
            {
                return type.ToString();
            }
        }
    }


    public class CoolDownCommandData
    {
        public String key;
        public int finishStep;
    }
    public class CoolDownCommand : Dictionary<String, CoolDownCommandData>
    {
        public void Add(CoolDownCommandData data)
        {
            this[data.key] = data;
        }
        public void Update(int step)
        {
            List<String> lstRemove = new List<string>();
            foreach (CoolDownCommandData d in this.Values)
            {
                if (d.finishStep < step)
                {
                    lstRemove.Add(d.key);
                }
            }
            foreach (String k in lstRemove)
            {
                this.Remove(k);
            }
        }
        public bool IsDelayed(String cmd)
        {
            return this.ContainsKey(cmd);
        }
        public override String ToString()
        {
            String ret = "{" + String.Join(",", this.Select(x => x.Value.key + " " + x.Value.finishStep).ToArray()) + "}";
            return ret;
        }
    }

    public class SC2ImageData
    {
        public Bitmap bmp;
        public ImageData imgData;
        //public byte[] data;
        public SC2ImageData(ImageData imgData)
        {
            bmp = imgData.ToBitmap();
            //data = imgData.Data.ToByteArray();
            this.imgData = imgData;
        }
    }

    public class SC2UnitState
    {
        public Point2D rallyPoint; 
    }

    public class SC2Bot : ISCBot
    {
        protected int prevStep = -1;
        protected bool first = true;
        public int gameLoop = 0;
        public GameState gameState;
        public CoolDownCommand coolDownCommand = new CoolDownCommand();
        public List<SC2Command> commandQueue  = new List<SC2Command>();
        public Bitmap bmpTerrainHeight;
        //public Bitmap bmpPlacementGrid;
        public SC2ImageData terrainHeightData;
        public SC2ImageData placementData;
        public SC2ImageData pathingGridData; 
        public Dictionary<ulong, SC2UnitState> unitStates= new Dictionary<ulong, SC2UnitState>();
        public Dictionary<String, bool> boolProperty = new Dictionary<String, bool>();

        public void logDebug(String data)
        {
            Console.WriteLine(data);
        }
        public void logPrintf(String format, params object[] param)
        {
            logDebug(String.Format(format, param));
        }

        public SC2APIProtocol.Action NewAction()
        {
            SC2APIProtocol.Action answer = new SC2APIProtocol.Action();
            answer.ActionRaw = new ActionRaw();
            answer.ActionRaw.ClearAction();
            return answer;
        }

        public override void SendCommand(SC2Command cmd)
        {
            commandQueue.Add(cmd);
        }

        public override void SetBoolProperty(string name, bool value)
        {
            boolProperty[name] = value;
        }
        public override bool GetBoolProperty(string name)
        {
            return boolProperty[name];
        }

        public SC2Bot()
        {
            SetBoolProperty("Auto", true);
        }

        public override void Init(GameState gameState)
        {
            logDebug(this.GetType().Name);
            terrainHeightData = new SC2ImageData(gameState.GameInfo.StartRaw.TerrainHeight);
            terrainHeightData.bmp.Save(@"TerrainHeight.png", ImageFormat.Png);
            terrainHeightData.imgData.Save(@"TerrainHeight.bin");
            terrainHeightData.imgData.ToDebugBitmap().Save(@"TerrainHeightDebug.png", ImageFormat.Png);

            placementData = new SC2ImageData(gameState.GameInfo.StartRaw.PlacementGrid);
            placementData.bmp.Save(@"PlacementGrid.png", ImageFormat.Png);
            placementData.imgData.Save(@"PlacementGrid.bin");
            placementData.imgData.ToDebugBitmap().Save(@"PlacementGridDebug.png", ImageFormat.Png);

            pathingGridData = new SC2ImageData(gameState.GameInfo.StartRaw.PathingGrid);
            pathingGridData.bmp.Save(@"PathingGrid.png", ImageFormat.Png);
            pathingGridData.imgData.Save(@"PathingGrid.bin");
            pathingGridData.imgData.ToDebugBitmap().Save(@"PathingGridDebug.png", ImageFormat.Png);

            OnInit(gameState);
        }

        public override SC2APIProtocol.Action Update(GameState gameState)
        {
            SC2APIProtocol.Action answer = NewAction();
            this.gameState = gameState;
            this.gameLoop = (int)gameState.NewObservation.Observation.GameLoop;
            coolDownCommand.Update(this.gameLoop);
            logPrintf("{0} Update {1} {2}", this.GetType().Name, this.gameLoop, coolDownCommand.ToString());
            if (gameState.NewObservation.Observation.GameLoop == prevStep)
            {
                logPrintf("Skip same step {0}", prevStep);
                return answer;
            }
            prevStep = (int)gameState.NewObservation.Observation.GameLoop;
            logDebug(gameState.NewObservation.Observation.PlayerCommon.ToString());
            if (gameState.NewObservation.ActionErrors.Count > 0)
            {
                logDebug("ActionErrors " + gameState.NewObservation.ActionErrors.ToString());
            }
            if (first)
            {
                first = false;
                Init(gameState);
                return answer;
            }
            if (gameLoop % 50 == 0)
            {
                DumpUnits();
                DumpImage();
            }
            //
            if (commandQueue.Count > 0)
            {
                SC2Command cmd = commandQueue[0];
                commandQueue.RemoveAt(0);
                logPrintf("onCommand {0}", cmd.ToString());
                SC2APIProtocol.Action action = OnCommand(cmd);
                if (action != null)
                {
                    return action;
                }
            }
            // Create Tag here
            RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
            foreach (Unit a in allUnits)
            { 
                if (!unitStates.ContainsKey(a.Tag))
                {
                    unitStates[a.Tag] = new SC2UnitState();
                }
            }

            if (GetBoolProperty("Auto"))
            {
                //DoIdle
                foreach (Unit a in GetMyUnits())
                {
                    if (IsIdle(a))
                    {
                        SC2APIProtocol.Action action = DoIdle(a);
                        if (action != null)
                        {
                            return action;
                        }
                    }
                }
                SC2APIProtocol.Action ret = Process();
                if (ret == null)
                {
                    logDebug("Warning: process() return null");
                    ret = answer;
                }
                logDebug(ret.ToString());
                return ret;
            }else
            {
                return answer;
            }
        }

        public Pen penRed = new Pen(System.Drawing.Color.Red,2);
        public Pen penGreen = new Pen(System.Drawing.Color.Green,2);
        public Pen penBlue = new Pen(System.Drawing.Color.Blue,2);
        public Pen penWhite = new Pen(System.Drawing.Color.White, 2);
        public Pen penYellow = new Pen(System.Drawing.Color.Yellow, 2);
        public Random rand = new Random();
        public void DumpImage()
        {
            {
                Bitmap b = gameState.NewObservation.Observation.RawData.MapState.Visibility.ToBitmap();
                b.Save(@"Visibility.bmp", ImageFormat.Bmp);

                //Pen myPen = new Pen(System.Drawing.Color.Green, 1);
                Bitmap bg = placementData.bmp;
                float scale = 50.0f;
                RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
                Bitmap bv = terrainHeightData.imgData.ToDebugBitmap(scale, true);
                //Bitmap bv = new Bitmap((int)(bg.Width * scale),(int)(bg.Height * scale), PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bv);
                //g.Clear(System.Drawing.Color.Black);
                foreach (Unit u in allUnits)
                {
                    Pen pen = penWhite;
                    switch (u.Alliance)
                    {
                        case Alliance.Enemy: pen = penRed; break;
                        case Alliance.Neutral: pen = penGreen; break;
                        case Alliance.Self: pen = penBlue;break;
                    }
                    g.DrawCircle(pen, u.Pos.X * scale,(b.Height - u.Pos.Y) * scale, u.Radius * scale);
                }
                g.Save();
                g.Dispose();
                bv.Save(@"Units.png", ImageFormat.Png);
            }

        }

        public void DumpUnits()
        {
            RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
            foreach(Unit u in allUnits)
            {
                logDebug(u.ToString());
            }
        }

        public bool IsNotOverlapWithUnit(Point2D point,float rad)
        {
            RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
            foreach (Unit u in allUnits)
            {
                if (u.OverlapWith(point.X,point.Y,rad))
                {
                    return false;
                }
            }
            return true;
        }

        public Point2D FindPlaceable(int x,int y,int rad,int size)
        {
            for(int i = 0; i < 100; i++)
            {
                int px = rand.Next(x - rad, x + rad);
                int py = rand.Next(y - rad, y + rad);
                if (placementData.imgData.IsPlaceable(px, py, size))
                {
                    Point2D ret = new Point2D();
                    ret.X = px + (float) (size/2.0);
                    ret.Y = py + (float) (size / 2.0);
                    if (IsNotOverlapWithUnit(ret, (float)(size / 2.0)))
                    {
                        return ret;
                    }
                }
            }
            return null;
        }

        public List<Unit> GetMyUnits(UNIT_TYPEID unitType = UNIT_TYPEID.INVALID,bool countBuilding = false)
        {
            RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
            List<Unit> ret = new List<Unit>();
            foreach (Unit u in allUnits)
            {
                if (
                    (u.Alliance == Alliance.Self) && ((u.UnitType == (int)unitType) || (unitType == UNIT_TYPEID.INVALID))
                    &&((countBuilding == false) || ((countBuilding == true) && (u.BuildProgress == 1)))
                ){
                    ret.Add(u);
                }
            }
            return ret;
        }

      
        public Unit FindNearest(Unit myUnit, UNIT_TYPEID unitType,bool checkHarvest = false)
        {
            Unit ret = null;
            float minDist = 10000;
            RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
            foreach (Unit u in allUnits)
            {
                if (checkHarvest)
                {
                    if(u.AssignedHarvesters >= u.IdealHarvesters)
                    {
                        continue;
                    }
                }
                if((u.UnitType == (int)unitType)&&(u.Tag != myUnit.Tag))
                {
                    float dist = myUnit.Pos.Dist(u.Pos);
                    if(dist < minDist)
                    {
                        ret = u;
                        minDist = dist;
                    }
                }
            }
            return ret;
        }

        public bool IsIdle(Unit u)
        {
            return (u.Orders.Count == 0) && (u.BuildProgress == 1);
        }
        public bool HasOrder(Unit u, ABILITY_ID ability)
        {
            foreach(UnitOrder o in u.Orders)
            {
                if(o.AbilityId == (int)ability)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetAvailableSupplyRoom()
        {
            PlayerCommon common = gameState.NewObservation.Observation.PlayerCommon;
            return (int)(common.FoodCap - common.FoodUsed);
        }

        public bool HasResouce(int mineral,int gas=0,int food=0)
        {
            PlayerCommon common = gameState.NewObservation.Observation.PlayerCommon;
            if ((common.Minerals >= mineral)
                && (common.Vespene >= gas)
                && (GetAvailableSupplyRoom() >= food)
                ) 
            {
                return true;
            }
            return false;
        }

        public virtual SC2APIProtocol.Action Process()
        {
            SC2APIProtocol.Action answer = NewAction();

            Observation obs = gameState.NewObservation.Observation;
            logPrintf("Game loop {0}", obs.GameLoop);
            DumpUnits();
            return answer;
        }

        public virtual SC2APIProtocol.Action OnCommand(SC2Command cmd)
        {
            return null;
        }

        public virtual SC2APIProtocol.Action DoIdle(Unit u)
        {
            return null;
        }

        public virtual void OnInit(GameState gameState)
        {

        }

    }
}
