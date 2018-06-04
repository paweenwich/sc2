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

    public class BuildingData
    {
        public byte[][] pattern;
        public float radious;
        public UNIT_TYPEID unitType;
    }

    public static class SC2BuildingData
    {
        public static Dictionary<UNIT_TYPEID, BuildingData> Buildings = new Dictionary<UNIT_TYPEID, BuildingData>
        {
            { UNIT_TYPEID.TERRAN_SUPPLYDEPOT, new BuildingData {radious=1.0f, unitType= UNIT_TYPEID.TERRAN_SUPPLYDEPOT} },
            { UNIT_TYPEID.TERRAN_BARRACKS, new BuildingData {radious=1.8125f, unitType= UNIT_TYPEID.TERRAN_BARRACKS, pattern=SC2Data.block5x3} },
            { UNIT_TYPEID.TERRAN_FACTORY, new BuildingData {radious=1.8125f, unitType= UNIT_TYPEID.TERRAN_FACTORY, pattern=SC2Data.block5x3} },
            { UNIT_TYPEID.TERRAN_ENGINEERINGBAY, new BuildingData {radious=1.8125f, unitType= UNIT_TYPEID.TERRAN_FACTORY, pattern=SC2Data.block3x3} },
        };
    }

    public class UnitProperty
    {
        public float range = 5;
    }


    public static class TerranData
    {
        public static Dictionary<UNIT_TYPEID, UnitProperty> TerranArmy = new Dictionary < UNIT_TYPEID, UnitProperty>
        //public static HashSet<UNIT_TYPEID> TerranArmy = new HashSet<UNIT_TYPEID>
        {
            { UNIT_TYPEID.TERRAN_MARINE, new  UnitProperty() { range=5 } },
            { UNIT_TYPEID.TERRAN_MARAUDER, new  UnitProperty() { range=6 } },
            { UNIT_TYPEID.TERRAN_BATTLECRUISER, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_CYCLONE, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_GHOST, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_HELLION, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_HELLIONTANK, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_LIBERATOR, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_LIBERATORAG, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_MEDIVAC, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_RAVEN, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_REAPER, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_SIEGETANK, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_SIEGETANKSIEGED, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_THOR, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_THORAP, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_VIKINGASSAULT, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_VIKINGFIGHTER, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_WIDOWMINE, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_WIDOWMINEBURROWED, new  UnitProperty()},
            { UNIT_TYPEID.TERRAN_POINTDEFENSEDRONE, new  UnitProperty()},
        };
        public static bool isArmy(Unit u)
        {
            return TerranArmy.Keys.Contains((UNIT_TYPEID)u.UnitType);
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
