using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public static class SC2Data
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
        public static byte[][] block5x3 = new byte[][]
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

        public static HashSet<UNIT_TYPEID> ArmyUnit = new HashSet<UNIT_TYPEID>
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

            UNIT_TYPEID.ZERG_BANELING,
            UNIT_TYPEID.ZERG_BANELINGBURROWED,
            UNIT_TYPEID.ZERG_BANELINGCOCOON,
            UNIT_TYPEID.ZERG_BROODLING,
            UNIT_TYPEID.ZERG_BROODLORD,
            UNIT_TYPEID.ZERG_BROODLORDCOCOON,
            UNIT_TYPEID.ZERG_CHANGELING,
            UNIT_TYPEID.ZERG_CHANGELINGMARINE,
            UNIT_TYPEID.ZERG_CHANGELINGMARINESHIELD,
            UNIT_TYPEID.ZERG_CHANGELINGZEALOT,
            UNIT_TYPEID.ZERG_CHANGELINGZERGLING,
            UNIT_TYPEID.ZERG_CHANGELINGZERGLINGWINGS,
            UNIT_TYPEID.ZERG_CORRUPTOR,
            UNIT_TYPEID.ZERG_CREEPTUMORQUEEN,
            UNIT_TYPEID.ZERG_DRONE,
            UNIT_TYPEID.ZERG_DRONEBURROWED,
            UNIT_TYPEID.ZERG_EGG,
            UNIT_TYPEID.ZERG_HYDRALISK,
            UNIT_TYPEID.ZERG_HYDRALISKBURROWED,
            UNIT_TYPEID.ZERG_INFESTOR,
            UNIT_TYPEID.ZERG_INFESTORBURROWED,
            UNIT_TYPEID.ZERG_INFESTORTERRAN,
            UNIT_TYPEID.ZERG_LARVA,
            UNIT_TYPEID.ZERG_LOCUSTMP,
            UNIT_TYPEID.ZERG_LOCUSTMPFLYING,
            UNIT_TYPEID.ZERG_LURKERDENMP,
            UNIT_TYPEID.ZERG_LURKERMP,
            UNIT_TYPEID.ZERG_LURKERMPBURROWED,
            UNIT_TYPEID.ZERG_LURKERMPEGG,
            UNIT_TYPEID.ZERG_MUTALISK,
            UNIT_TYPEID.ZERG_OVERLORD,
            UNIT_TYPEID.ZERG_OVERLORDCOCOON,
            UNIT_TYPEID.ZERG_OVERLORDTRANSPORT,
            UNIT_TYPEID.ZERG_OVERSEER,
            UNIT_TYPEID.ZERG_QUEEN,
            UNIT_TYPEID.ZERG_QUEENBURROWED,
            UNIT_TYPEID.ZERG_RAVAGER,
            UNIT_TYPEID.ZERG_RAVAGERCOCOON,
            UNIT_TYPEID.ZERG_ROACH,
            UNIT_TYPEID.ZERG_ROACHBURROWED,
            UNIT_TYPEID.ZERG_SPINECRAWLER,
            UNIT_TYPEID.ZERG_SPINECRAWLERUPROOTED,
            UNIT_TYPEID.ZERG_SPORECRAWLER,
            UNIT_TYPEID.ZERG_SPORECRAWLERUPROOTED,
            UNIT_TYPEID.ZERG_SWARMHOSTBURROWEDMP,
            UNIT_TYPEID.ZERG_SWARMHOSTMP,
            UNIT_TYPEID.ZERG_TRANSPORTOVERLORDCOCOON,
            UNIT_TYPEID.ZERG_ULTRALISK,
            UNIT_TYPEID.ZERG_VIPER,
            UNIT_TYPEID.ZERG_ZERGLING,
            UNIT_TYPEID.ZERG_ZERGLINGBURROWED,
            UNIT_TYPEID.ZERG_PARASITICBOMBDUMMY,

            UNIT_TYPEID.PROTOSS_ADEPT,
            UNIT_TYPEID.PROTOSS_ADEPTPHASESHIFT,
            UNIT_TYPEID.PROTOSS_ARCHON,
            UNIT_TYPEID.PROTOSS_CARRIER,
            UNIT_TYPEID.PROTOSS_COLOSSUS,
            UNIT_TYPEID.PROTOSS_DARKTEMPLAR,
            UNIT_TYPEID.PROTOSS_DISRUPTOR,
            UNIT_TYPEID.PROTOSS_DISRUPTORPHASED,
            UNIT_TYPEID.PROTOSS_HIGHTEMPLAR,
            UNIT_TYPEID.PROTOSS_IMMORTAL,
            UNIT_TYPEID.PROTOSS_INTERCEPTOR,
            UNIT_TYPEID.PROTOSS_MOTHERSHIP,
            UNIT_TYPEID.PROTOSS_MOTHERSHIPCORE,
            UNIT_TYPEID.PROTOSS_OBSERVER,
            UNIT_TYPEID.PROTOSS_ORACLE,
            UNIT_TYPEID.PROTOSS_ORACLESTASISTRAP,
            UNIT_TYPEID.PROTOSS_PHOENIX,
            UNIT_TYPEID.PROTOSS_PROBE,
            UNIT_TYPEID.PROTOSS_SENTRY,
            UNIT_TYPEID.PROTOSS_STALKER,
            UNIT_TYPEID.PROTOSS_TEMPEST,
            UNIT_TYPEID.PROTOSS_VOIDRAY,
            UNIT_TYPEID.PROTOSS_WARPPRISM,
            UNIT_TYPEID.PROTOSS_WARPPRISMPHASING,
            UNIT_TYPEID.PROTOSS_ZEALOT,
        };
    }
}
