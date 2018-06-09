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

        public TerranBot(SC2BotOption option = null): base(option)
        {
            
        }

        public SC2APIProtocol.Action Stimpack(List<Unit> units)
        {
            SC2APIProtocol.Action answer = NewAction();
            answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.EFFECT_STIM;
            foreach (Unit cc in units)
            {
                answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
            }
            return answer;
        }

        public SC2APIProtocol.Action BuildOption(Unit u, ABILITY_ID ability)
        {
            if (!coolDownCommand.IsDelayed("BuildOption"))
            {
                coolDownCommand.Add(new CoolDownCommandData() { key = "BuildOption", finishStep = gameLoop + 10 });
                return CreateAction(u, ability);
            }
            return null;
        }

        public SC2APIProtocol.Action AttackAttack(List<Unit> idleArmy)
        {
            if (!coolDownCommand.IsDelayed("AttackAttack"))
            {
                SC2APIProtocol.Action answer = NewAction();
                answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
                answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.ATTACK_ATTACK;
                foreach (Unit cc in idleArmy)
                {
                    answer.ActionRaw.UnitCommand.UnitTags.Add(cc.Tag);
                    logDebug("My Idle Army " + cc.ToString());
                }
                // Find target 
                Point2D targetPos = null;
                //EnemyUnits enemyUnit = GetEnemyUnits();
                if (enemyUnit.baseBuilding.Count > 0)
                {
                    foreach(Unit b in enemyUnit.baseBuilding)
                    {
                        logDebug("baseBuilding " + b.ToStringEx());
                    }
                    targetPos = new Point2D() { X = enemyUnit.baseBuilding[0].Pos.X, Y = enemyUnit.baseBuilding[0].Pos.Y };
                    logDebug("AttackAttack baseBuilding at " + targetPos.ToString());
                    logDebug(enemyUnit.baseBuilding[0].ToStringEx());
                }
                else
                {
                    if (enemyUnit.all.Count > 0){
                        foreach (Unit b in enemyUnit.all)
                        {
                            logDebug("enemyUnit " + b.ToStringEx());
                        }
                        targetPos = new Point2D() { X = enemyUnit.all[0].Pos.X, Y = enemyUnit.all[0].Pos.Y };
                        logDebug("AttackAttack enemyUnit at " + targetPos.ToString());
                        logDebug(enemyUnit.all[0].ToStringEx());

                    }
                    else {
                        float ax = idleArmy.Average(x => x.Pos.X);
                        float ay = idleArmy.Average(x => x.Pos.Y);
                        logPrintf("AttackAttack average at expansion {0},{1}", ax, ay);
                        if (startLocations[0].Dist(ax, ay) < 3)
                        {
                            logDebug("AttackAttack already at expansion");
                            targetPos = startLocations[0];
                        }
                        else
                        {
                            targetPos = GetNextEnemyExpansion(startLocations[0]);
                            logDebug("AttackAttack EnemyExpansion at " + targetPos.ToString());
                        }
                    }
                }
                if (targetPos != null)
                {
                    coolDownCommand.Add(new CoolDownCommandData() { key = "AttackAttack", finishStep = gameLoop + 10 });
                    answer.ActionRaw.UnitCommand.TargetWorldSpacePos = targetPos;
                    return answer;
                }
            }
            return null;
        }
        public SC2APIProtocol.Action CallMule(Unit cc)
        {
            if (!coolDownCommand.IsDelayed("CallMule"))
            {
                Unit target = FindNearest(cc, UNIT_TYPEID.NEUTRAL_MINERALFIELD);
                if (target != null)
                {
                    coolDownCommand.Add(new CoolDownCommandData() { key = "CallMule", finishStep = gameLoop + 50 });
                    SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.EFFECT_CALLDOWNMULE);
                    answer.ActionRaw.UnitCommand.TargetUnitTag = target.Tag;
                    return answer;
                }
            }
            return null;
        }

        public SC2APIProtocol.Action MorphOrbital(Unit cc)
        {
            logDebug("MorphOrbital");
            SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.MORPH_ORBITALCOMMAND);
            return answer;
        }

        public SC2APIProtocol.Action SetRallyPoint(Unit cc)
        {
            if (!coolDownCommand.IsDelayed("SetRallyPoint"))
            {
                SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.RALLY_BUILDING);
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
                if (!cc.HasOrder(ABILITY_ID.TRAIN_SCV))
                {
                    coolDownCommand.Add(new CoolDownCommandData() { key = "TrainSCV", finishStep = gameLoop + 250 });
                    SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.TRAIN_SCV);
                    return answer;
                }
            }
            return null;
        }
        public SC2APIProtocol.Action TrainMarine(Unit cc)
        {
            //if (!coolDownCommand.IsDelayed("TrainFromBarrak"))
            //{
                //coolDownCommand.Add(new CoolDownCommandData() { key = "TrainFromBarrak", finishStep = gameLoop + 10 });
                return TrainUnit(cc, ABILITY_ID.TRAIN_MARINE);
            //}
            //return null;
        }
        public SC2APIProtocol.Action TrainMarauder(Unit cc)
        {
            //if (!coolDownCommand.IsDelayed("TrainFromBarrak"))
            //{
                //coolDownCommand.Add(new CoolDownCommandData() { key = "TrainFromBarrak", finishStep = gameLoop + 10 });
                return TrainUnit(cc, ABILITY_ID.TRAIN_MARAUDER);
            //}
            //return null;
        }

        public SC2APIProtocol.Action TrainUnit(Unit cc, ABILITY_ID ability)
        {
            SC2APIProtocol.Action answer = CreateAction(cc, ability);
            return answer;
        }

        public SC2APIProtocol.Action BuildSupply(Unit cc)
        {
            if (!coolDownCommand.IsDelayed("BuildSupply"))
            {
                coolDownCommand.Add(new CoolDownCommandData() { key = "BuildSupply", finishStep = gameLoop + 10 });
                SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.BUILD_SUPPLYDEPOT);
                List<Point2D> poses = FindPlaceables((int)cc.Pos.X, (int)cc.Pos.Y, 10, UNIT_TYPEID.TERRAN_SUPPLYDEPOT, true);
                if (poses.Count > 0)
                {
                    ScoreDatas scores = new ScoreDatas();
                    foreach (Point2D p in poses)
                    {
                        float dist = cc.Pos.Dist(p.X, p.Y);
                        scores.Add(new ScoreData { data = p, score = dist });
                    }
                    scores.Sort();
                    scores.Reverse();
                    //Point2D pos = FindPlaceable((int)cc.Pos.X, (int)cc.Pos.Y, 10,UNIT_TYPEID.TERRAN_SUPPLYDEPOT,true);
                    Point2D pos = (Point2D)scores[0].data;
                    if (pos != null)
                    {
                        logDebug("BuildSupply at " + pos.ToString());
                        answer.ActionRaw.UnitCommand.TargetWorldSpacePos = pos;
                        return answer;
                    }
                }
            }
            return null;
        }
        public SC2APIProtocol.Action BuildBarrak(Unit cc)
        {
            SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.BUILD_BARRACKS);
            Point2D pos = FindPlaceable((int)cc.Pos.X, (int)cc.Pos.Y, 15,UNIT_TYPEID.TERRAN_BARRACKS, true);
            if (pos != null)
            {
                logDebug("BuildBarrak at " + pos.ToString());
                answer.ActionRaw.UnitCommand.TargetWorldSpacePos = pos;
                return answer;
            }
            return null;
        }
        public SC2APIProtocol.Action BuildFactory(Unit cc)
        {
            SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.BUILD_FACTORY);
            Point2D pos = FindPlaceable((int)cc.Pos.X, (int)cc.Pos.Y, 15, UNIT_TYPEID.TERRAN_FACTORY, true);
            if (pos != null)
            {
                logDebug("BuildFactory at " + pos.ToString());
                answer.ActionRaw.UnitCommand.TargetWorldSpacePos = pos;
                return answer;
            }
            return null;
        }

        public SC2APIProtocol.Action BuildEngineering(Unit cc)
        {
            SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.BUILD_ENGINEERINGBAY);
            Point2D pos = FindPlaceable((int)cc.Pos.X, (int)cc.Pos.Y, 15, UNIT_TYPEID.TERRAN_ENGINEERINGBAY, true);
            if (pos != null)
            {
                logDebug("BuildEngineering at " + pos.ToString());
                answer.ActionRaw.UnitCommand.TargetWorldSpacePos = pos;
                return answer;
            }
            return null;
        }

        

        public SC2APIProtocol.Action BuildGas(Unit cc)
        {
            Unit target = FindNearest(cc, UNIT_TYPEID.NEUTRAL_VESPENEGEYSER);
            if (target != null)
            {
                SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.BUILD_REFINERY);
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
                SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.HARVEST_GATHER_SCV);
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
                SC2APIProtocol.Action answer = CreateAction(cc, ABILITY_ID.HARVEST_GATHER_SCV);
                answer.ActionRaw.UnitCommand.TargetUnitTag = target.Tag;
                return answer;
            }
            return null;
        }

        public override SC2APIProtocol.Action OnCommand(SC2Command cmd)
        {
            SC2APIProtocol.Action answer = NewAction();
            answer.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            switch (cmd.type)
            {
                case SC2CommandType.BUILD_SUPPLY:
                    {
                        List<Unit> SCVs = GetMyUnits(UNIT_TYPEID.TERRAN_SCV);
                        answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.BUILD_SUPPLYDEPOT;
                        answer.ActionRaw.UnitCommand.UnitTags.Add(SCVs[0].Tag);
                        answer.ActionRaw.UnitCommand.TargetWorldSpacePos = cmd.targetPos;
                        break;
                    }
                case SC2CommandType.BUILD_BARRAK:
                    {
                        List<Unit> SCVs = GetMyUnits(UNIT_TYPEID.TERRAN_SCV);
                        answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.BUILD_BARRACKS;
                        answer.ActionRaw.UnitCommand.UnitTags.Add(SCVs[0].Tag);
                        answer.ActionRaw.UnitCommand.TargetWorldSpacePos = cmd.targetPos;
                        break;
                    }

                case SC2CommandType.MORPH_ORBITAL:
                    {
                        List<Unit> CCs = GetMyUnits(UNIT_TYPEID.TERRAN_COMMANDCENTER);
                        answer.ActionRaw.UnitCommand.AbilityId = (int)ABILITY_ID.MORPH_ORBITALCOMMAND;
                        answer.ActionRaw.UnitCommand.UnitTags.Add(CCs[0].Tag);
                        break;
                    }
            }
            logDebug(cmd.ToString());
            logDebug(answer.ToString());
            return answer;
        }
        public override bool IsIdle(Unit u)
        {
            // if has attatchment as REACTOR
            int num = 1;
            if (u.AddOnTag != 0)
            {
                Unit attatch = GetUnitFromTag(u.AddOnTag);
                if (attatch != null)
                {
                    if(/*(attatch.UnitType == (int)UNIT_TYPEID.TERRAN_REACTOR)||*/(attatch.UnitType == (int)UNIT_TYPEID.TERRAN_BARRACKSREACTOR))
                    {
                        num = 2;
                    }
                }
            }
            return (u.Orders.Count < num) && (u.BuildProgress == 1);
        }

        public override SC2APIProtocol.Action DoIdle(Unit u)
        {
            SC2APIProtocol.Action action = null;
            switch ((UNIT_TYPEID)u.UnitType)
            {
                case UNIT_TYPEID.TERRAN_ENGINEERINGBAY:
                    {
                        if (!IsUpgraded(UPGRADE_ID.TERRANINFANTRYWEAPONSLEVEL1))
                        {
                            action = BuildOption(u, ABILITY_ID.RESEARCH_TERRANINFANTRYWEAPONS);
                            break;
                        }
                        if (!IsUpgraded(UPGRADE_ID.TERRANINFANTRYARMORSLEVEL1))
                        {
                            action = BuildOption(u, ABILITY_ID.RESEARCH_TERRANINFANTRYARMOR);
                            break;
                        }
                        break;
                    }
                case UNIT_TYPEID.TERRAN_SUPPLYDEPOT:
                    {
                        action = BuildOption(u, ABILITY_ID.MORPH_SUPPLYDEPOT_LOWER);
                        break;
                    }
                case UNIT_TYPEID.TERRAN_BARRACKSTECHLAB:
                    {
                        if (HasResouce(100, 100, 0))
                        {
                            if((!IsUpgraded(UPGRADE_ID.SHIELDWALL)) && (!myUnit.building.hasOrder(ABILITY_ID.RESEARCH_COMBATSHIELD)))
                            {
                                action = BuildOption(u, ABILITY_ID.RESEARCH_COMBATSHIELD);
                                break;
                            }
                        }
                        if (HasResouce(100, 100, 0))
                        {
                            if ((!IsUpgraded(UPGRADE_ID.STIMPACK)) && (!myUnit.building.hasOrder(ABILITY_ID.RESEARCH_STIMPACK)))
                            {
                                action = BuildOption(u, ABILITY_ID.RESEARCH_STIMPACK);
                                break;
                            }
                        }
                        if (HasResouce(50, 50, 0))
                        {
                            if ((!IsUpgraded(UPGRADE_ID.PUNISHERGRENADES))&& IsUpgraded(UPGRADE_ID.STIMPACK))
                            {
                                action = BuildOption(u, ABILITY_ID.RESEARCH_CONCUSSIVESHELLS);
                                break;
                            }

                        }
                        break;
                    }

                case UNIT_TYPEID.TERRAN_ORBITALCOMMAND:
                    {
                        if (u.Energy > 50)
                        {
                            action = CallMule(u);

                        }
                        if ((action == null) && HasResouce(50, 0, 1) && (u.AssignedHarvesters < u.IdealHarvesters))
                        {
                            action = TrainSCV(u);
                        }
                        break;
                    }
                case UNIT_TYPEID.TERRAN_COMMANDCENTER:
                    {
                        if (HasResouce(150, 25, 0))
                        {
                            action = MorphOrbital(u);

                        }
                        if ((action == null) && HasResouce(50,0,1) && (u.AssignedHarvesters < u.IdealHarvesters) )
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
                case UNIT_TYPEID.TERRAN_FACTORY:
                    {
                        if (unitStates[u.Tag].rallyPoint == null)
                        {
                            action = SetRallyPoint(u);
                            if (action != null)
                            {
                                unitStates[u.Tag].rallyPoint = action.ActionRaw.UnitCommand.TargetWorldSpacePos;
                                return action;
                            }
                        }
                        if (HasResouce(50, 50, 1) && (u.AddOnTag == 0))
                        {
                            if (!HasBuilding(UNIT_TYPEID.TERRAN_FACTORYTECHLAB))
                            {
                                action = BuildOption(u, ABILITY_ID.BUILD_TECHLAB);
                                return action;
                            }
                            else if (!HasBuilding(UNIT_TYPEID.TERRAN_FACTORYREACTOR))
                            {
                                action = BuildOption(u, ABILITY_ID.BUILD_REACTOR);
                                return action;
                            }
                            else
                            {
                                if ((rand.Next() % 2) == 1)
                                {
                                    action = BuildOption(u, ABILITY_ID.BUILD_TECHLAB);
                                    return action;
                                }
                                else
                                {
                                    action = BuildOption(u, ABILITY_ID.BUILD_REACTOR);
                                    return action;
                                }
                            }
                        }
                        if (HasResouce(150, 100, 1))
                        {
                            Unit addOn = GetUnitFromTag(u.AddOnTag);
                            /*if ((addOn != null) && (addOn.UnitType == (int)UNIT_TYPEID.TERRAN_FACTORYTECHLAB))
                            {
                                action = TrainUnit(u, ABILITY_ID.TRAIN_HELLION);
                            }
                            else
                            {*/
                                action = TrainUnit(u, ABILITY_ID.TRAIN_CYCLONE);
                            //}
                        }
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
                                return action;
                            }
                        }
                        
                        if (HasResouce(50, 50, 1) && (u.AddOnTag == 0))
                        {
                            if (!HasBuilding(UNIT_TYPEID.TERRAN_BARRACKSTECHLAB))
                            {
                                action = BuildOption(u, ABILITY_ID.BUILD_TECHLAB_BARRACKS);
                            }
                            else if (!HasBuilding(UNIT_TYPEID.TERRAN_BARRACKSREACTOR))
                            {
                                action = BuildOption(u, ABILITY_ID.BUILD_REACTOR);
                            }
                            else
                            {
                                if ((rand.Next() % 2) == 1)
                                {
                                    action = BuildOption(u, ABILITY_ID.BUILD_TECHLAB_BARRACKS);
                                    //action = BuildOption(u, ABILITY_ID.BUILD_REACTOR);
                                }
                                else
                                {
                                    action = BuildOption(u, ABILITY_ID.BUILD_REACTOR);
                                }
                            }
                        }
                        if ((action == null) && HasResouce(50, 0, 1))
                        {
                            Unit addOn = GetUnitFromTag(u.AddOnTag);
                            if((addOn!=null) && HasResouce(100, 25, 2) && (addOn.UnitType == (int)UNIT_TYPEID.TERRAN_BARRACKSTECHLAB)){
                                action = TrainMarauder(u);
                            } else {
                                action = TrainMarine(u);
                            }
                        }
                        break;
                    }
            }
            return action;
        }
        public int CountBuildingOnProgress(List<Unit> SCVs, ABILITY_ID id)
        {
            int ret = 0;
            foreach(Unit scv in SCVs)
            {
                if (scv.HasOrder(id))
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
        
        public override void OnInit(SC2GameState gameState)
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
            int armyToAttack = 15;
            SC2APIProtocol.Action answer = NewAction();
            Observation obs = newObservation.Observation;

            List<Unit> CCs = GetMyUnits(UNIT_TYPEID.TERRAN_COMMANDCENTER);
            List<Unit> OCCs = GetMyUnits(UNIT_TYPEID.TERRAN_ORBITALCOMMAND);
            List<Unit> SCVs = GetMyUnits(UNIT_TYPEID.TERRAN_SCV);
            List<Unit> Supplys = GetMyUnits(UNIT_TYPEID.TERRAN_SUPPLYDEPOT,true);
            Supplys.AddRange(GetMyUnits(UNIT_TYPEID.TERRAN_SUPPLYDEPOTLOWERED, true));
            List<Unit> Refineries = GetMyUnits(UNIT_TYPEID.TERRAN_REFINERY);
            List<Unit> Barracks = GetMyUnits(UNIT_TYPEID.TERRAN_BARRACKS);
            List<Unit> Factories = GetMyUnits(UNIT_TYPEID.TERRAN_FACTORY);
            List<Unit> Engineerings = GetMyUnits(UNIT_TYPEID.TERRAN_ENGINEERINGBAY);
            //MyArmy ArmyUnits =  GetMyArmyUnits();
            int supplyBuildingProgress = CountBuildingOnProgress(SCVs, ABILITY_ID.BUILD_SUPPLYDEPOT);
            int barrakIdleCount = 0;
            foreach(Unit u in Barracks){
                if (IsIdle(u))
                {
                    barrakIdleCount++;
                }
            }
            logPrintf("CC {0} OCC {6} SCV {1} SUPPLY {2} + {5} REFINERY {3} BARRAKS {4} {8} FACTORY {11} ENG {12} ARMY {7} {9} {10}", 
                CCs.Count, SCVs.Count, Supplys.Count, Refineries.Count, Barracks.Count,
                supplyBuildingProgress, OCCs.Count, myUnit.armyUnit.Count, barrakIdleCount, myUnit.engaging.Count,
                myUnit.engagingUnit.Count, Factories.Count, Engineerings.Count
            );

            if(myUnit.engagingUnit.Count > 0)
            {
                List<Unit> stimUnits = new List<Unit>();
                foreach (Unit u in myUnit.engagingUnit)
                {
                    if (IsUpgraded(UPGRADE_ID.STIMPACK) && u.CanStimpack())
                    {
                        if (u.Health < u.HealthMax)
                        {
                            stimUnits.Add(u);
                        }
                    }
                }
                if (stimUnits.Count > 0)
                {
                    logPrintf("STIM {0}", stimUnits.Count);
                    answer = Stimpack(stimUnits);
                    return answer;
                }
            }

            if(myUnit.armyUnit.Count > 0)
            {
                // Send to Dead
                List<Unit> noOrderUnits =  myUnit.armyUnit.Where(u => u.Orders.Count == 0).ToList();
                if (noOrderUnits.Count > 0)
                {
                    SC2APIProtocol.Action ret = AttackAttack(noOrderUnits);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }

            if (IsUpgraded(UPGRADE_ID.STIMPACK) && myUnit.armyUnit.Count > armyToAttack)
            {
                List<Unit> idleArmy = new List<Unit>();
                foreach (Unit u in myUnit.armyUnit)
                {
                    if (!u.HasOrder(ABILITY_ID.ATTACK_ATTACK))
                    {
                        idleArmy.Add(u);
                    }
                }
                if (idleArmy.Count > 5)
                {
                    SC2APIProtocol.Action ret = AttackAttack(idleArmy);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }

            //Avoid high ground enemy
            if(enemyUnit.armyUnit.Count > 0)
            {

            }

            Unit scv = null;
            foreach (Unit u in SCVs)
            {
                
                if (u.HasOrder(ABILITY_ID.BUILD_SUPPLYDEPOT)||
                    u.HasOrder(ABILITY_ID.BUILD_REFINERY)||
                    u.HasOrder(ABILITY_ID.BUILD_BARRACKS)||
                    u.HasOrder(ABILITY_ID.BUILD_ARMORY) ||
                    u.HasOrder(ABILITY_ID.BUILD_FACTORY) ||
                    u.HasOrder(ABILITY_ID.BUILD_STARPORT) 
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
                if (HasResouce(100, 0, 0) && (GetAvailableSupplyRoom() < 10) && (supplyBuildingProgress < 1))
                {
                    SC2APIProtocol.Action ret = BuildSupply(scv);
                    if (ret != null)
                    {
                        logDebug("BuildSupply " + scv);
                        return ret;
                    }
                }

                if (HasResouce(75, 0, 0) && (Supplys.Count > 0) && (Refineries.Count < 1) && (Barracks.Count >0 ))
                {
                    SC2APIProtocol.Action ret = BuildGas(scv);
                    if (ret != null)
                    {
                        logDebug("BuildGas " + scv);
                        return ret;
                    }
                }
                if (HasResouce(75, 0, 0) && (Supplys.Count > 0) && (Refineries.Count < 2) && (Factories.Count > 0))
                {
                    SC2APIProtocol.Action ret = BuildGas(scv);
                    if (ret != null)
                    {
                        logDebug("BuildGas " + scv);
                        return ret;
                    }
                }

                if (HasResouce(150, 0, 0) && (Supplys.Count >0) && (Barracks.Count < 3))
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

                if (HasResouce(300, 100, 0) && (Supplys.Count > 0) && (Barracks.Count >= 3) && (Factories.Count < 1))
                {
                    if (!coolDownCommand.IsDelayed("BuildFactory"))
                    {
                        SC2APIProtocol.Action ret = BuildFactory(scv);
                        if (ret != null)
                        {
                            logDebug("BuildFactory " + scv);
                            coolDownCommand.Add(new CoolDownCommandData() { key = "BuildFactory", finishStep = gameLoop + 50 });
                            return ret;
                        }
                    }
                }

                if (HasResouce(300, 100, 0) && (Factories.Count >= 1) && (Engineerings.Count <1))
                {
                    if (!coolDownCommand.IsDelayed("BuildEngineering"))
                    {
                        SC2APIProtocol.Action ret = BuildEngineering(scv);
                        if (ret != null)
                        {
                            logDebug("BuildEngineering " + scv);
                            coolDownCommand.Add(new CoolDownCommandData() { key = "BuildEngineering", finishStep = gameLoop + 50 });
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
