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
    public class TerranBot: SC2Bot
    {
        public Dictionary<Point2D, TerranBuildPattern> rampData = new Dictionary<Point2D, TerranBuildPattern>();

        public SC2APIProtocol.Action MorphOrbital(Unit cc)
        {
            logDebug("MorphOrbital");
            SC2APIProtocol.Action answer = NewAction();
            answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.MORPH_ORBITALCOMMAND;
            answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
            return answer;
        }

        public SC2APIProtocol.Action SetRallyPoint(Unit cc)
        {
            if (!coolDownCommand.IsDelayed("SetRallyPoint"))
            {
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.RALLY_BUILDING;
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                //Point2D pos = FindPlaceable((int)cc.Pos.X, (int)cc.Pos.Y, 10, 3);
                Point2D pos = FindNearestRanmp(cc);
                if (pos != null)
                {
                    Point2D offset = rampData[pos].data["Rally"];
                    Point2D rallyPos = pos.Clone();
                    rallyPos.X += offset.X;
                    rallyPos.Y += offset.Y;
                    coolDownCommand.Add(new CoolDownCommandData() { key = "SetRallyPoint", finishStep = gameLoop + 10 });
                    logDebug("SetRallyPoint at " + rallyPos.ToString());
                    answer.ActionRaw.UnitCommand.TargetWorldSpacePos = rallyPos;
                    return answer;
                }
            }
            return null;
        }

        public SC2APIProtocol.Action TrainSCV(Unit cc)
        {
            if (!coolDownCommand.IsDelayed("TrainSCV"))
            {
                coolDownCommand.Add(new CoolDownCommandData() { key = "TrainSCV", finishStep = gameLoop + 10 });
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.TRAIN_SCV;
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                return answer;
            }
            return null;
        }
        public SC2APIProtocol.Action TrainMarine(Unit cc)
        {
            if (!coolDownCommand.IsDelayed("TrainMarine"))
            {
                coolDownCommand.Add(new CoolDownCommandData() { key = "TrainMarine", finishStep = gameLoop + 10 });
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.TRAIN_MARINE;
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                return answer;
            }
            return null;
        }
        
        public SC2APIProtocol.Action BuildSupply(Unit cc)
        {
            if (!coolDownCommand.IsDelayed("BuildSupply"))
            {
                coolDownCommand.Add(new CoolDownCommandData() { key = "BuildSupply", finishStep = gameLoop + 10 });
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.BUILD_SUPPLYDEPOT;
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                Point2D pos = FindPlaceable((int)cc.Pos.X, (int)cc.Pos.Y, 10, 2);
                if (pos != null)
                {
                    logDebug("BuildSupply at " + pos.ToString());
                    answer.ActionRaw.UnitCommand.TargetWorldSpacePos = pos;
                    return answer;
                }
            }
            return null;
        }
        public SC2APIProtocol.Action BuildBarrak(Unit cc)
        {
            SC2APIProtocol.Action answer = NewAction();
            answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.BUILD_BARRACKS;
            answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
            Point2D pos = FindPlaceable((int)cc.Pos.X, (int)cc.Pos.Y, 10, 3);
            if (pos != null)
            {
                logDebug("BuildBarrak at " + pos.ToString());
                answer.ActionRaw.UnitCommand.TargetWorldSpacePos = pos;
                //DumpImage();
                return answer;
            }
            return null;
        }
        public SC2APIProtocol.Action BuildGas(Unit cc)
        {
            Unit target = FindNearest(cc, UNIT_TYPEID.NEUTRAL_VESPENEGEYSER);
            if (target != null)
            {
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.BUILD_REFINERY;
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                answer.ActionRaw.UnitCommand.TargetUnitTag = target.Tag;
                return answer;
            }
            return null;
        }
        public SC2APIProtocol.Action GatherMineral(Unit cc)
        {
            Unit target = FindNearest(cc, UNIT_TYPEID.NEUTRAL_MINERALFIELD);
            if (target != null)
            {
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.HARVEST_GATHER_SCV;
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                answer.ActionRaw.UnitCommand.TargetUnitTag = target.Tag;
                return answer;
            }
            return null;
        }
        public SC2APIProtocol.Action GatherGas(Unit cc)
        {
            Unit target = FindNearest(cc, UNIT_TYPEID.TERRAN_REFINERY,true);
            if (target != null)
            {
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.HARVEST_GATHER_SCV;
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                answer.ActionRaw.UnitCommand.TargetUnitTag = target.Tag;
                return answer;
            }
            return null;
        }

        public override SC2APIProtocol.Action OnCommand(SC2Command cmd)
        {
            List<Unit> SCVs = GetMyUnits(UNIT_TYPEID.TERRAN_SCV);
            SC2APIProtocol.Action answer = NewAction();
            answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            switch (cmd.type)
            {
                case SC2CommandType.BUILD_SUPPLY: answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.BUILD_SUPPLYDEPOT;break;
                case SC2CommandType.BUILD_BARRAK: answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.BUILD_BARRACKS; break;
            }
            
            answer.ActionRaw.UnitCommand.UnitTags.Add(SCVs[0].Tag);
            logDebug(cmd.ToString());
            answer.ActionRaw.UnitCommand.TargetWorldSpacePos = cmd.targetPos;
            return answer;
        }

        public override SC2APIProtocol.Action DoIdle(Unit u)
        {
            SC2APIProtocol.Action action = null;
            switch ((UNIT_TYPEID)u.UnitType)
            {
                case UNIT_TYPEID.TERRAN_ORBITALCOMMAND:
                    {
                        if (HasResouce(50, 0, 1) && (u.AssignedHarvesters < u.IdealHarvesters))
                        {
                            action = TrainSCV(u);
                        }
                        break;
                    }
                case UNIT_TYPEID.TERRAN_COMMANDCENTER:
                    {
                        /*if (HasResouce(150, 0, 0))
                        {
                            action = MorphOrbital(u);

                        }else*/ if (HasResouce(50,0,1) && (u.AssignedHarvesters < u.IdealHarvesters) )
                        {
                            action = TrainSCV(u);
                        }
                        break;
                    }

                case UNIT_TYPEID.TERRAN_SCV:
                    {
                        action = GatherMineral(u);
                        break;
                    }
                case UNIT_TYPEID.TERRAN_BARRACKS:
                    {
                        if (unitStates[u.Tag].rallyPoint == null)
                        {
                            action = SetRallyPoint(u);
                            if (action != null)
                            {
                                unitStates[u.Tag].rallyPoint = action.ActionRaw.UnitCommand.TargetWorldSpacePos;
                            }
                        }
                        else
                        {
                            if (HasResouce(50, 0, 1))
                            {
                                action = TrainMarine(u);
                            }
                        }
                        break;
                    }
            }
            if(action != null)
            {
                Observation obs = gameState.NewObservation.Observation;
                logDebug("Idle " + u.ToString());
                logDebug(action.ToString());
            }else
            {
                //if (u.UnitType != (int)UNIT_TYPEID.TERRAN_SUPPLYDEPOT)
                //{
                    logDebug("Idle " + u.ToString());
                //}
            }
            return action;
        }
        public int CountBuildingOnProgress(List<Unit> SCVs, ABILITY_ID id)
        {
            int ret = 0;
            foreach(Unit scv in SCVs)
            {
                if (HasOrder(scv, id))
                {
                    ret++;
                }
            }
            return ret;
        }

        public Point2D FindNearestRanmp(Unit u)
        {
            float minDist = 10000;
            Point2D ret = null;
            foreach (Point2D p in rampData.Keys)
            {
                float dist = u.Pos.Dist(p.X, p.Y);
                if (dist < minDist)
                {
                    minDist = dist;
                    ret = p;
                }
            }
            return ret;
        }
        
        public override void OnInit(GameState gameState)
        {
            //Seqarch for ramp
            foreach (TerranBuildPattern tbp in TerranData.rampPattens)
            {
                List<Point2D> ramp = terrainHeightData.imgData.FindPattern(tbp.pattern);
                foreach (Point2D p in ramp)
                {
                    Point2D gamePos = p.Clone();
                    gamePos.Y = terrainHeightData.imgData.Size.Y - gamePos.Y;
                    rampData[gamePos] = tbp;
                    logDebug("OnInit found ramp at " + p.ToString() +  " game at " + gamePos.ToString());
                }
            }
        }




        public override SC2APIProtocol.Action Process()
        {
            SC2APIProtocol.Action answer = NewAction();
            Observation obs = gameState.NewObservation.Observation;
            List<Unit> CCs = GetMyUnits(UNIT_TYPEID.TERRAN_COMMANDCENTER);
            List<Unit> SCVs = GetMyUnits(UNIT_TYPEID.TERRAN_SCV);
            List<Unit> Supplys = GetMyUnits(UNIT_TYPEID.TERRAN_SUPPLYDEPOT,true);
            List<Unit> Refineries = GetMyUnits(UNIT_TYPEID.TERRAN_REFINERY);
            List<Unit> Barracks = GetMyUnits(UNIT_TYPEID.TERRAN_BARRACKS);
            int supplyBuildingProgress = CountBuildingOnProgress(SCVs, ABILITY_ID.BUILD_SUPPLYDEPOT);
            logPrintf("CC {0} SCV {1} SUPPLY {2} + {5} REFINERY {3} BARRAKS {4}", 
                CCs.Count, SCVs.Count, Supplys.Count, Refineries.Count, Barracks.Count,
                supplyBuildingProgress
            );
            Unit scv = null;
            foreach (Unit u in SCVs)
            {
                
                if (HasOrder(u, ABILITY_ID.BUILD_SUPPLYDEPOT)||
                    HasOrder(u, ABILITY_ID.BUILD_REFINERY)
                    )
                {
                    //logDebug("Supply is building by " + u.ToString());
                    //scv = null;
                    //break;
                    continue;
                }
                scv = u;
            }
            if (scv != null)
            {
                //return answer;
                if (HasResouce(100, 0, 0) && (GetAvailableSupplyRoom() < 5) && (supplyBuildingProgress < 1))
                {
                    SC2APIProtocol.Action ret = BuildSupply(scv);
                    if (ret != null)
                    {
                        logDebug("BuildSupply " + scv);
                        return ret;
                    }
                }

                if (HasResouce(75, 0, 0) && (Supplys.Count > 0) && (Refineries.Count < 2) && (Barracks.Count >0 ))
                {
                    SC2APIProtocol.Action ret = BuildGas(scv);
                    if (ret != null)
                    {
                        logDebug("BuildGas " + scv);
                        return ret;
                    }
                }
                if(HasResouce(150, 0, 0) && (Supplys.Count >0) && (Barracks.Count < 3))
                {
                    if (!coolDownCommand.IsDelayed("BuildBarrak"))
                    {
                        SC2APIProtocol.Action ret = BuildBarrak(scv);
                        if (ret != null)
                        {
                            logDebug("BuildBarrak " + scv);
                            coolDownCommand.Add(new CoolDownCommandData() { key = "BuildBarrak", finishStep = gameLoop + 50 });
                            return ret;
                        }
                    }
                }

                if (Refineries.Count > 0) {
                    if (!coolDownCommand.IsDelayed("GatherGas"))
                    {
                        SC2APIProtocol.Action ret = GatherGas(scv);
                        if (ret != null)
                        {
                            logDebug("GatherGas " + scv);
                            coolDownCommand.Add(new CoolDownCommandData() {key= "GatherGas",finishStep=gameLoop + 50});
                            return ret;
                        }
                    }
                }
            }
            return answer;
        }
    }
}
