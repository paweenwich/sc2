using Accord.MachineLearning;
using MoreLinq;
using Newtonsoft.Json;
using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public static class SC2ExtendUnit
    {

        public static float idealRedius(this Unit self)
        {
            return (float)(Math.Floor(self.Radius * 2.0f) / 2.0f);
        }
        public static bool OverlapWith(this Unit self, Unit other)
        {
            float d = self.Pos.Dist(other.Pos);
            return self.OverlapWith(other.Pos.X, other.Pos.Y, other.idealRedius());
        }

        public static bool OverlapWith(this Unit self, float x, float y, float range)
        {
            float d = self.Pos.Dist(x, y);
            return (d < (self.idealRedius() + range));
        }

        public static bool OverlapWith(this Unit self, List<Unit> allUnits)
        {
            foreach (Unit u in allUnits)
            {
                if (self == u) continue;
                if (u.IsWorker()) continue;
                if (self.OverlapWith(u))
                {
                    return true;
                }
            }
            return false;
        }

        public static String ToStringEx(this Unit self)
        {
            return self.ToSimpleString() + " " + self.ToString();
        }

        public static String ToSimpleString(this Unit self)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Enum.GetName(typeof(UNIT_TYPEID), self.UnitType) + " ");
            if (self.Orders.Count > 0)
            {
                sb.Append("[");
                foreach (UnitOrder uo in self.Orders)
                {
                    sb.Append(Enum.GetName(typeof(ABILITY_ID), uo.AbilityId) + " ");
                }
                sb.Append("]");
            }
            if (self.BuffIds.Count > 0)
            {
                sb.Append("[");
                foreach (uint bufID in self.BuffIds)
                {
                    sb.Append(Enum.GetName(typeof(BUFF_ID), bufID) + " ");
                }
                sb.Append("]");
            }
            return sb.ToString();
        }

        public static String ToJson(this Unit self)
        {
            dynamic obj = JsonConvert.DeserializeObject(self.ToString());
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static bool IsBaseBuilding(this Unit u)
        {
            if ((u.UnitType == (uint)UNIT_TYPEID.TERRAN_COMMANDCENTER) || (u.UnitType == (uint)UNIT_TYPEID.PROTOSS_NEXUS)
                || (u.UnitType == (uint)UNIT_TYPEID.ZERG_HATCHERY) || (u.UnitType == (uint)UNIT_TYPEID.ZERG_LAIR) || (u.UnitType == (uint)UNIT_TYPEID.ZERG_HIVE))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsWorker(this Unit u)
        {
            if ((u.UnitType == (uint)UNIT_TYPEID.TERRAN_SCV) || (u.UnitType == (uint)UNIT_TYPEID.PROTOSS_PROBE) || (u.UnitType == (uint)UNIT_TYPEID.ZERG_DRONE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsArmyUnit(this Unit u)
        {
            return SC2Data.ArmyUnit.Contains((UNIT_TYPEID)u.UnitType);
        }

        public static bool HasOrder(this Unit u, ABILITY_ID ability)
        {
            foreach (UnitOrder o in u.Orders)
            {
                if (o.AbilityId == (int)ability)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetPos(this Unit u, float x, float y)
        {
            u.Pos.X = x;
            u.Pos.Y = y;
        }

        public static byte[][] GetBlock(this Unit u)
        {

            if (SC2BuildingData.Buildings.ContainsKey((UNIT_TYPEID)u.UnitType))
            {
                if (SC2BuildingData.Buildings[(UNIT_TYPEID)u.UnitType].pattern != null)
                {
                    return SC2BuildingData.Buildings[(UNIT_TYPEID)u.UnitType].pattern;
                }
            }
            int size = (int)(u.idealRedius() * 2);
            switch (size)
            {
                case 2: return SC2Data.block2x2;
                case 3: return SC2Data.block3x3;
                case 4: return SC2Data.block4x4;
                case 5: return SC2Data.block5x5;
            }
            return SC2Data.block1x1;
        }

        public static bool HasBuff(this Unit u, BUFF_ID buffID)
        {
            if (u.BuffIds != null)
            {
                return (u.BuffIds.Contains((uint)buffID));
            }
            return false;
        }

        public static bool CanStimpack(this Unit u)
        {
            if ((u.Health > 10))
            {
                if ((u.UnitType == (int)UNIT_TYPEID.TERRAN_MARINE) && (!u.HasBuff(BUFF_ID.STIMPACK)))
                {
                    return true;
                }
                if ((u.UnitType == (int)UNIT_TYPEID.TERRAN_MARAUDER) && (!u.HasBuff(BUFF_ID.STIMPACKMARAUDER)))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Unit> GetUnits(this List<Unit> allUnits, UNIT_TYPEID unitType)
        {
            List<Unit> ret = new List<Unit>();
            foreach (Unit u in allUnits)
            {
                if ((int)unitType == u.UnitType)
                {
                    ret.Add(u);
                }
            }
            return ret;
        }
        public static List<Point2D> FindBaseLocation(this List<Unit> self)
        {
            List<Point2D> ret = new List<Point2D>();
            List<Unit> gas = self.GetUnits(UNIT_TYPEID.NEUTRAL_VESPENEGEYSER);
            double[][] data = new double[gas.Count][];
            for (int i = 0; i < gas.Count; i++)
            {
                data[i] = new double[] { gas[i].Pos.X, gas[i].Pos.Y };
            }
            KMeans kmeans = new KMeans(k: gas.Count / 2);
            KMeansClusterCollection clusters = kmeans.Learn(data);
            for (int i = 0; i < kmeans.Centroids.Length; i++)
            {
                ret.Add(new Point2D { X = (float)kmeans.Centroids[i][0], Y = (float)kmeans.Centroids[i][1] });
            }
            return ret;
        }

        public static bool hasOrder(this List<Unit> self, ABILITY_ID ability)
        {
            return (self.FirstOrDefault(u => u.HasOrder(ability)) != null);
        }

        public static List<Unit> GetUnitInRange(this Unit self, List<Unit> units, float range = 12.0f)
        {
            return units.Where(u => (u != self) && (self.Pos.Dist(u.Pos) <= range)).ToList();
        }

        public static List<Unit> GetUnitInRange(this List<Unit> self, List<Unit> units, float range = 12.0f)
        {
            HashSet<Unit> unitOut = new HashSet<Unit>();
            foreach (Unit u in self)
            {
                unitOut.UnionWith(new HashSet<Unit>(u.GetUnitInRange(units, range)));
            }
            return unitOut.ToList();
        }

        public static Unit GetUnit(this List<Unit> self, ulong tag)
        {
            return self.FirstOrDefault(u => u.Tag == tag);
        }

        public static Unit GetUnit(this List<Unit> self, Point pos)
        {
            Unit ret = self.MinBy(u => u.Pos.Dist(pos));
            float dist = ret.Pos.Dist(pos);
            if (dist <= ret.Radius)
            {
                return ret;
            }
            return null;
        }

        public static Unit GetNearestUnit(this List<Unit> self, Point pos)
        {
            Unit ret = self.MinBy(u => u.Pos.Dist(pos));
            return ret;
        }

        public static String ToStringEx(this float[] self)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (float t in self)
            {
                sb.Append(String.Format(" {0:0.0000}",t));
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
