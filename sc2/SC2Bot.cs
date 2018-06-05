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
    public abstract class ISC2Bot
    {
        public abstract void Init(GameState gameState);
        public abstract SC2APIProtocol.Action Update(GameState gameState);
        public abstract void SendCommand(SC2Command cmd);
        public abstract void SetBoolProperty(String name,bool value);
        public abstract bool GetBoolProperty(String name);
        public abstract List<String> GetBoolProperty();
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

    public class ScoreData : IComparable<ScoreData>
    {
        public float score;
        public object data;

        int IComparable<ScoreData>.CompareTo(ScoreData other)
        {
            return score.CompareTo(other.score);
        }
    }

    public class ScoreDatas : List<ScoreData>
    {
    }

    public class MyUnits
    {
        public List<Unit> all = new List<Unit>();
        public List<Unit> armyUnit = new List<Unit>();          // 
        public List<Unit> engaging = new List<Unit>();          // All engaging
        public List<Unit> engagingUnit = new List<Unit>();      // engaging enemy unit
        public List<Unit> building = new List<Unit>();          // building
    }

    public class EnemyUnits
    {
        public List<Unit> all = new List<Unit>();
        public List<Unit> baseBuilding = new List<Unit>();
        public List<Unit> armyUnit = new List<Unit>();
        //public List<Unit> engaging = new List<Unit>();          // All engaging
        //public List<Unit> engagingUnit = new List<Unit>();      // engaging enemy unit
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
        public Unit unit;
        //public bool isHPDecrease = false;
        public Point2D rallyPoint;
        public HashSet<ABILITY_ID> researched = new HashSet<ABILITY_ID>();
    }

    public class SC2Bot : ISC2Bot
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
        public List<Unit> allUnits;
        public RepeatedField<uint> upgradeIDs;
        public Dictionary<ulong, SC2UnitState> unitStates= new Dictionary<ulong, SC2UnitState>();
        public Dictionary<String, bool> boolProperty = new Dictionary<String, bool>();
        public Point2D[] startLocations;
        public List<Point2D> baseLocations;
        public EnemyUnits enemyUnit ;
        public MyUnits myUnit;

        public void logDebug(String data)
        {
            if (GetBoolProperty("Log"))
            {
                Console.WriteLine(data);
            }
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
            SetBoolProperty("Log", false);
            SetBoolProperty("DumpNetural", false);
            SetBoolProperty("DumpSelf", true);
            SetBoolProperty("DumpEnemy", true);
            SetBoolProperty("DumpWorker", false);
        }

        public override void Init(GameState gameState)
        {
            logDebug(this.GetType().Name);
            startLocations = gameState.GameInfo.StartRaw.StartLocations.ToArray();


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
            this.allUnits = gameState.NewObservation.Observation.RawData.Units.ToList();
            this.upgradeIDs = gameState.NewObservation.Observation.RawData.Player.UpgradeIds;
            this.enemyUnit = GetEnemyUnits();
            this.myUnit = GetMyUnits();
            this.baseLocations = allUnits.FindBaseLocation();

            coolDownCommand.Update(this.gameLoop);
            logPrintf("\n{0} Update {1} {2} {3}", this.GetType().Name, this.gameLoop, coolDownCommand.ToString(), this.upgradeIDs.ToString());
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
            foreach (Unit a in allUnits)
            { 
                if (!unitStates.ContainsKey(a.Tag))
                {
                    unitStates[a.Tag] = new SC2UnitState { unit = a };
                }else
                {
                    Unit u = unitStates[a.Tag].unit;
                    // update unit 
                    /*if ((a.Health < u.Health) && a.HealthMax == u.HealthMax)
                    {
                        logPrintf("HP DECREASE {0}",a.ToStringEx());
                        unitStates[a.Tag].isHPDecrease = true;
                    }else
                    {
                        unitStates[a.Tag].isHPDecrease = false;
                    }*/
                    unitStates[a.Tag].unit = a;
                }
            }

            if (GetBoolProperty("Auto"))
            {
                //DoIdle
                foreach (Unit a in myUnit.all)
                {
                    if (IsIdle(a))
                    {
                        SC2APIProtocol.Action action = DoIdle(a);
                        if (action != null)
                        {
                            logDebug("ACTION " + action.ToStringEx());
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
                if (ret.HasCommand())
                {
                    logDebug(ret.ToStringEx());
                }
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
            foreach(Unit u in allUnits)
            {
                if (u.IsWorker()&&(GetBoolProperty("DumpWorker") == false))
                {
                    continue;
                }
                if((u.Alliance == Alliance.Neutral) && (GetBoolProperty("DumpNetural")))
                {
                    logDebug(u.ToStringEx());
                    continue;
                }
                if ((u.Alliance == Alliance.Self) && (GetBoolProperty("DumpSelf")))
                {
                    logDebug(u.ToStringEx());
                    continue;
                }
                if ((u.Alliance == Alliance.Enemy) && (GetBoolProperty("DumpEnemy")))
                {
                    logDebug(u.ToStringEx());
                    continue;
                }
            }
        }

        public bool IsNotOverlapWithUnit(Point2D point,float rad)
        {
            foreach (Unit u in allUnits)
            {
                if (u.OverlapWith(point.X,point.Y,rad))
                {
                    return false;
                }
            }
            return true;
        }

        public static Unit FakeBuildingUnit(UNIT_TYPEID unitType)
        {
            if (SC2BuildingData.Buildings.ContainsKey(unitType))
            {
                Unit u = new Unit();
                u.Pos = new SC2APIProtocol.Point();
                u.Radius = SC2BuildingData.Buildings[unitType].radious;
                u.UnitType = (uint)unitType;
                return u;
            }
            return null;
        }

        public Point2D FindPlaceable(int X, int Y, int rad, UNIT_TYPEID unitType, bool flgSameLevel = true)
        {
            List<Point2D> lstPoint = FindPlaceables(X, Y, rad, unitType, flgSameLevel);
            logPrintf("FindPlaceable found {0}", lstPoint.Count);
            if (lstPoint.Count > 0)
            {
                return lstPoint[rand.Next(lstPoint.Count)];
            }
            return null;
        }
        public static bool IsPointTaken(float x, float y, List<Unit> allUnits)
        {
            foreach (Unit u in allUnits)
            {
                if (u.IsWorker()) continue;
                if (u.OverlapWith(x, y, 0.01f))
                {
                    return true;
                }
                if ((u.UnitType == (int)UNIT_TYPEID.TERRAN_BARRACKS)|| (u.UnitType == (int)UNIT_TYPEID.TERRAN_FACTORY)
                    || (u.UnitType == (int)UNIT_TYPEID.TERRAN_STARPORT))
                {
                    if(u.OverlapWith(x-2, y, 0.01f))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static ImageData CreatePlaceableImageData(ImageData placeable, List<Unit> allUnits)
        {
            ImageData ret = placeable.Clone();
            byte[] data = ret.Data.ToArray();
            for (float x = 0.5f; x < ret.Size.X; x += 1)
            {
                for (float y = 0.5f; y < ret.Size.Y; y += 1)
                {
                    if (IsPointTaken(x, y, allUnits))
                    {
                        int addr = (int)((Math.Floor(ret.Size.Y - y) * ret.Size.X) + Math.Floor(x));
                        data[addr] = 127;
                    }
                }
            }
            ret.Data = Google.Protobuf.ByteString.CopyFrom(data);
            return ret;
        }

        public List<Point2D> FindPlaceables(int X, int Y, int rad, UNIT_TYPEID unitType, bool flgSameLevel = true)
        {
            ImageData placeMap = new ImageData();
            placeMap = CreatePlaceableImageData(placementData.imgData, allUnits);

            int r = rad;
            Unit testUnit = FakeBuildingUnit(unitType);
            byte[][] pattern = testUnit.GetBlock();
            int ccHeight = terrainHeightData.imgData.GetValue((int)(X), terrainHeightData.imgData.Size.Y - (int)(Y));
            List<Point2D> lstPoint = new List<Point2D>();
            for (int y = (int)(Y - r); y < (int)(Y + r); y++)
            {
                if ((y < 0) || (y > placeMap.Size.Y)) continue;
                for (int x = (int)(X - r); x < (int)(X + r); x++)
                {
                    if ((x < 0) || (x > placeMap.Size.X)) continue;
                    if (placeMap.IsPlaceable(x, y, pattern))
                    {
                        if (flgSameLevel)
                        {
                            if (!terrainHeightData.imgData.IsPlaceable(x, y, pattern, ccHeight))
                            {
                                continue;
                            }
                        }
                        float tx = x + testUnit.idealRedius();
                        float ty = y - testUnit.idealRedius();
                        testUnit.SetPos(x + testUnit.idealRedius(), y - testUnit.idealRedius());
                        if (!isPlaceable(testUnit, placeMap, allUnits))
                        {
                            continue;
                        }
                        Point2D ret = new Point2D();
                        ret.X = tx;
                        ret.Y = ty;
                        lstPoint.Add(ret);
                    }
                }
            }
            return lstPoint;
        }


        public bool isPlaceable(Unit u, ImageData placeMap, List<Unit> allUnits, byte[][] pattern = null)
        {
            if (pattern == null)
            {
                pattern = u.GetBlock();
            }
            float dx = (float)(pattern[0].Length / 2.0);
            float dy = (float)(pattern.Length / 2.0);
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

        private EnemyUnits GetEnemyUnits()
        {
            EnemyUnits ret = new EnemyUnits();
            foreach (Unit u in allUnits)
            {
                if (u.Alliance == Alliance.Enemy)
                {
                    ret.all.Add(u);
                    if (u.IsBaseBuilding())
                    {
                        ret.baseBuilding.Add(u);
                    }else
                    {
                        if (u.IsArmyUnit())
                        {
                            ret.armyUnit.Add(u);
                        }
                    }
                }
            }
            return ret;
        }

        public MyUnits GetMyUnits()
        {
            MyUnits ret = new MyUnits();
            foreach (Unit u in allUnits)
            {
                if (u.Alliance == Alliance.Self) {
                    ret.all.Add(u);
                    if (u.IsArmyUnit())
                    {
                        ret.armyUnit.Add(u);
                        if (u.EngagedTargetTag != 0)
                        {
                            ret.engaging.Add(u);
                            Unit targetUnit = GetUnitFromTag(u.EngagedTargetTag);
                            if (targetUnit != null)
                            {
                                if (targetUnit.IsArmyUnit())
                                {
                                    ret.engagingUnit.Add(u);
                                }
                            }
                        }
                    }else
                    {
                        ret.building.Add(u);
                    }
                }
            }
            return ret;
        }

        public List<Unit> GetMyUnits(UNIT_TYPEID unitType = UNIT_TYPEID.INVALID,bool countBuilding = false)
        {
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
       /* public Unit GetUnitFromTag(ulong tag)
        {
            foreach(Unit u in allUnits)
            {
                if(u.Tag == tag)
                {
                    return u;
                }
            }
            return null;
        }*/
        public virtual bool IsUpgraded(UPGRADE_ID upgrade)
        {
            return (upgradeIDs.Contains((uint)upgrade));
        }

        public virtual bool IsIdle(Unit u)
        {
            return (u.Orders.Count == 0) && (u.BuildProgress == 1);
        }

        /*
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
        }*/

        public bool HasBuilding(UNIT_TYPEID unit)
        {
            foreach(Unit u in myUnit.building)
            {
                if(u.UnitType == (int)unit)
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

        public virtual SC2APIProtocol.Action CreateAction(Unit u, ABILITY_ID ability)
        {
            SC2APIProtocol.Action answer = NewAction();
            answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            answer.ActionRaw.UnitCommand.AbilityId = (int)ability;
            answer.ActionRaw.UnitCommand.UnitTags.Add(u.Tag);
            return answer;
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

        public override List<string> GetBoolProperty()
        {
            return boolProperty.Keys.ToList();
        }

        public Unit GetUnitFromTag(ulong tag)
        {
            if (unitStates.ContainsKey(tag))
            {
                return unitStates[tag].unit;
            }
            return null;
        }
        public Point2D GetNextEnemyExpansion(Point2D fromPos)
        {
            float minDist = 1000000;
            Point2D ret = null;
            foreach(Point2D p in baseLocations)
            {
                float dist = p.Dist(fromPos);
                if ( dist < 10) continue;
                if ( dist < minDist)
                {
                    ret = p;
                    minDist = dist;
                }
            }
            return ret;
        }
    }
}
