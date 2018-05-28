using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using SC2APIProtocol;
using Starcraft2;

namespace sc2
{
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
            String ret = "{" + String.Join(",", this.Select( x => x.Value.key + " " + x.Value.finishStep).ToArray()) + "}";
            return ret;
        }
    }

    public class SC2Bot : ISCBot
    {
        protected int prevStep = -1;
        protected bool first = true;
        public int gameLoop = 0;
        public GameState gameState;
        public CoolDownCommand coolDownCommand = new CoolDownCommand();
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

        public override void Init(GameState gameState)
        {
            logDebug(this.GetType().Name);
        }

        public override SC2APIProtocol.Action Update(GameState gameState)
        {
            SC2APIProtocol.Action answer = NewAction();
            this.gameState = gameState;
            this.gameLoop = (int)gameState.NewObservation.Observation.GameLoop;
            coolDownCommand.Update(this.gameLoop);
            logPrintf("{0} Update {1} {2}", this.GetType().Name, this.gameLoop, coolDownCommand.ToString());
            if(gameState.NewObservation.Observation.GameLoop == prevStep)
            {
                logPrintf("Skip same step {0}",prevStep);
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
            //DoIdle
            foreach(Unit a in GetMyUnits())
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
            if(ret == null)
            {
                logDebug("Warning: process() return null");
                ret = answer;
            }
            logDebug(ret.ToString());
            return ret;
        }



        public void DumpUnits()
        {
            RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
            foreach(Unit u in allUnits)
            {
                logDebug(u.ToString());
            }
        }

        public List<Unit> GetMyUnits(UNIT_TYPEID unitType = UNIT_TYPEID.INVALID)
        {
            RepeatedField<Unit> allUnits = gameState.NewObservation.Observation.RawData.Units;
            List<Unit> ret = new List<Unit>();
            foreach (Unit u in allUnits)
            {
                if ((u.Alliance == Alliance.Self) && ((u.UnitType == (int)unitType) || (unitType == UNIT_TYPEID.INVALID)))
                {
                    ret.Add(u);
                }
            }
            return ret;
        }

        public float Dist(Point p1, Point p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
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
                    float dist = Dist(myUnit.Pos, u.Pos);
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

        public virtual SC2APIProtocol.Action DoIdle(Unit u)
        {
            return null;
        }
    }
}
