﻿using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public static class SC2ExtendUnit
    {
        public static byte[][] block1x1 = new byte[][]
        {
            new byte[] {1},
        };
        public static byte[][] block2x2 = new byte[][]
        {
            new byte[] {1,1},
            new byte[] {1,1},
        };
        public static byte[][] block3x5 = new byte[][]
        {
            new byte[] {1,1,1,0,0},
            new byte[] {1,1,1,1,1},
            new byte[] {1,1,1,1,1},
        };
        public static byte[][] block3x3 = new byte[][]
        {
            new byte[] {1,1,1},
            new byte[] {1,1,1},
            new byte[] {1,1,1},
        };
        public static byte[][] block4x4 = new byte[][]
        {
            new byte[] {1,1,1,1},
            new byte[] {1,1,1,1},
            new byte[] {1,1,1,1},
            new byte[] {1,1,1,1},
        };
        public static byte[][] block5x5 = new byte[][]
        {
            new byte[] {1,1,1,1,1},
            new byte[] {1,1,1,1,1},
            new byte[] {1,1,1,1,1},
            new byte[] {1,1,1,1,1},
            new byte[] {1,1,1,1,1},
        };

        public static int IntSize(this Unit self)
        {
            return (int)(float)(Math.Floor(self.Radius * 2) / 2.0);
        }
        public static bool OverlapWith(this Unit self, Unit other)
        {
            float d = self.Pos.Dist(other.Pos);
            return self.OverlapWith(other.Pos.X, other.Pos.Y, other.IntSize());
        }

        public static bool OverlapWith(this Unit self, float x, float y, float range)
        {
            float d = self.Pos.Dist(x, y);
            return (d < (self.IntSize() + range));
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

        public static byte[][] GetBlock(this Unit u)
        {
            switch(u.IntSize()*2)
            {
                case 2: return block2x2;
                case 3: return block3x3;
                case 4: return block4x4;
                case 5: return block5x5;
            }
            return block1x1;
        }

    }
}