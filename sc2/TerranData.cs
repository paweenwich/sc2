using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{

    public class TerranBuildPattern
    {
        public byte[][] pattern;
        public Dictionary<String, Point2D> data;
    }

    public static class TerranData
    {
        public static HashSet<UNIT_TYPEID> TerranArmy = new HashSet<UNIT_TYPEID>
        {
            UNIT_TYPEID.TERRAN_BATTLECRUISER,
            UNIT_TYPEID.TERRAN_CYCLONE,
            UNIT_TYPEID.TERRAN_GHOST,
            UNIT_TYPEID.TERRAN_HELLION,
            UNIT_TYPEID.TERRAN_HELLIONTANK,
            UNIT_TYPEID.TERRAN_LIBERATOR,
            UNIT_TYPEID.TERRAN_LIBERATORAG,
            UNIT_TYPEID.TERRAN_MARAUDER,
            UNIT_TYPEID.TERRAN_MARINE,
            UNIT_TYPEID.TERRAN_MEDIVAC,
            UNIT_TYPEID.TERRAN_RAVEN,
            UNIT_TYPEID.TERRAN_REAPER,
            UNIT_TYPEID.TERRAN_SIEGETANK,
            UNIT_TYPEID.TERRAN_SIEGETANKSIEGED,
            UNIT_TYPEID.TERRAN_THOR,
            UNIT_TYPEID.TERRAN_THORAP,
            UNIT_TYPEID.TERRAN_VIKINGASSAULT,
            UNIT_TYPEID.TERRAN_VIKINGFIGHTER,
            UNIT_TYPEID.TERRAN_WIDOWMINE,
            UNIT_TYPEID.TERRAN_WIDOWMINEBURROWED,
            UNIT_TYPEID.TERRAN_POINTDEFENSEDRONE,
        };
        public static bool isArmy(Unit u)
        {
            return TerranArmy.Contains((UNIT_TYPEID)u.UnitType);
        }
        public static TerranBuildPattern[] rampPattens = new TerranBuildPattern[] {
                new TerranBuildPattern()
                {
                    pattern = new byte[4][]
                    {
                        new byte[4] {142,143,143,143},
                        new byte[4] {142,143,143,143},
                        new byte[4] {142,142,143,143},
                        new byte[4] {142,142,142,142},
                    },
                    data = new Dictionary<String, Point2D>
                    {
                        { "Rally", new Point2D() {X = +2, Y=-1 } },
                    },
                },
                new TerranBuildPattern()
                {
                    pattern = new byte[4][]
                    {
                        new byte[4] {143,142,142,142},
                        new byte[4] {143,143,143,142},
                        new byte[4] {143,143,143,143},
                        new byte[4] {143,143,143,143},
                    },
                    data = new Dictionary<String, Point2D>
                    {
                        { "Rally", new Point2D() {X = +2, Y=-2 } },
                    },
                },
        };
    }
}
