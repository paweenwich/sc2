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
        public static bool OverlapWith(this Unit self, Unit other)
        {
            float d = self.Pos.Dist(other.Pos);
            return self.OverlapWith(other.Pos.X, other.Pos.Y, other.Radius);
        }

        public static bool OverlapWith(this Unit self, float x, float y, float range)
        {
            float d = self.Pos.Dist(x, y);
            return (d < (self.Radius + range + 1));
        }

        public static String ToStringEx(this Unit self)
        {
            return Enum.GetName(typeof(UNIT_TYPEID), self.UnitType) + " " + self.ToString();
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


    }
}
