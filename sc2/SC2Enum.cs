﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public enum UNIT_TYPEID
    {
        INVALID = 0, TERRAN_ARMORY = 29, TERRAN_AUTOTURRET = 31, TERRAN_BANSHEE = 55,
        TERRAN_BARRACKS = 21, TERRAN_BARRACKSFLYING = 46, TERRAN_BARRACKSREACTOR = 38, TERRAN_BARRACKSTECHLAB = 37,
        TERRAN_BATTLECRUISER = 57, TERRAN_BUNKER = 24, TERRAN_COMMANDCENTER = 18, TERRAN_COMMANDCENTERFLYING = 36,
        TERRAN_CYCLONE = 692, TERRAN_ENGINEERINGBAY = 22, TERRAN_FACTORY = 27, TERRAN_FACTORYFLYING = 43,
        TERRAN_FACTORYREACTOR = 40, TERRAN_FACTORYTECHLAB = 39, TERRAN_FUSIONCORE = 30, TERRAN_GHOST = 50,
        TERRAN_GHOSTACADEMY = 26, TERRAN_HELLION = 53, TERRAN_HELLIONTANK = 484, TERRAN_LIBERATOR = 689,
        TERRAN_LIBERATORAG = 734, TERRAN_MARAUDER = 51, TERRAN_MARINE = 48, TERRAN_MEDIVAC = 54,
        TERRAN_MISSILETURRET = 23, TERRAN_MULE = 268, TERRAN_ORBITALCOMMAND = 132, TERRAN_ORBITALCOMMANDFLYING = 134,
        TERRAN_PLANETARYFORTRESS = 130, TERRAN_RAVEN = 56, TERRAN_REAPER = 49, TERRAN_REFINERY = 20,
        TERRAN_SCV = 45, TERRAN_SENSORTOWER = 25, TERRAN_SIEGETANK = 33, TERRAN_SIEGETANKSIEGED = 32,
        TERRAN_STARPORT = 28, TERRAN_STARPORTFLYING = 44, TERRAN_STARPORTREACTOR = 42, TERRAN_STARPORTTECHLAB = 41,
        TERRAN_SUPPLYDEPOT = 19, TERRAN_SUPPLYDEPOTLOWERED = 47, TERRAN_THOR = 52, TERRAN_THORAP = 691,
        TERRAN_VIKINGASSAULT = 34, TERRAN_VIKINGFIGHTER = 35, TERRAN_WIDOWMINE = 498, TERRAN_WIDOWMINEBURROWED = 500,
        TERRAN_KD8CHARGE = 830, TERRAN_NUKE = 58, TERRAN_POINTDEFENSEDRONE = 11, TERRAN_REACTOR = 6,
        TERRAN_TECHLAB = 5, ZERG_BANELING = 9, ZERG_BANELINGBURROWED = 115, ZERG_BANELINGCOCOON = 8,
        ZERG_BANELINGNEST = 96, ZERG_BROODLING = 289, ZERG_BROODLORD = 114, ZERG_BROODLORDCOCOON = 113,
        ZERG_CHANGELING = 12, ZERG_CHANGELINGMARINE = 15, ZERG_CHANGELINGMARINESHIELD = 14, ZERG_CHANGELINGZEALOT = 13,
        ZERG_CHANGELINGZERGLING = 17, ZERG_CHANGELINGZERGLINGWINGS = 16, ZERG_CORRUPTOR = 112, ZERG_CREEPTUMOR = 87,
        ZERG_CREEPTUMORBURROWED = 137, ZERG_CREEPTUMORQUEEN = 138, ZERG_DRONE = 104, ZERG_DRONEBURROWED = 116,
        ZERG_EGG = 103, ZERG_EVOLUTIONCHAMBER = 90, ZERG_EXTRACTOR = 88, ZERG_GREATERSPIRE = 102,
        ZERG_HATCHERY = 86, ZERG_HIVE = 101, ZERG_HYDRALISK = 107, ZERG_HYDRALISKBURROWED = 117,
        ZERG_HYDRALISKDEN = 91, ZERG_INFESTATIONPIT = 94, ZERG_INFESTEDTERRANSEGG = 150, ZERG_INFESTOR = 111,
        ZERG_INFESTORBURROWED = 127, ZERG_INFESTORTERRAN = 7, ZERG_LAIR = 100, ZERG_LARVA = 151,
        ZERG_LOCUSTMP = 489, ZERG_LOCUSTMPFLYING = 693, ZERG_LURKERDENMP = 504, ZERG_LURKERMP = 502,
        ZERG_LURKERMPBURROWED = 503, ZERG_LURKERMPEGG = 501, ZERG_MUTALISK = 108, ZERG_NYDUSCANAL = 142,
        ZERG_NYDUSNETWORK = 95, ZERG_OVERLORD = 106, ZERG_OVERLORDCOCOON = 128, ZERG_OVERLORDTRANSPORT = 893,
        ZERG_OVERSEER = 129, ZERG_QUEEN = 126, ZERG_QUEENBURROWED = 125, ZERG_RAVAGER = 688,
        ZERG_RAVAGERCOCOON = 687, ZERG_ROACH = 110, ZERG_ROACHBURROWED = 118, ZERG_ROACHWARREN = 97,
        ZERG_SPAWNINGPOOL = 89, ZERG_SPINECRAWLER = 98, ZERG_SPINECRAWLERUPROOTED = 139, ZERG_SPIRE = 92,
        ZERG_SPORECRAWLER = 99, ZERG_SPORECRAWLERUPROOTED = 140, ZERG_SWARMHOSTBURROWEDMP = 493, ZERG_SWARMHOSTMP = 494,
        ZERG_TRANSPORTOVERLORDCOCOON = 892, ZERG_ULTRALISK = 109, ZERG_ULTRALISKCAVERN = 93, ZERG_VIPER = 499,
        ZERG_ZERGLING = 105, ZERG_ZERGLINGBURROWED = 119, ZERG_PARASITICBOMBDUMMY = 824, PROTOSS_ADEPT = 311,
        PROTOSS_ADEPTPHASESHIFT = 801, PROTOSS_ARCHON = 141, PROTOSS_ASSIMILATOR = 61, PROTOSS_CARRIER = 79,
        PROTOSS_COLOSSUS = 4, PROTOSS_CYBERNETICSCORE = 72, PROTOSS_DARKSHRINE = 69, PROTOSS_DARKTEMPLAR = 76,
        PROTOSS_DISRUPTOR = 694, PROTOSS_DISRUPTORPHASED = 733, PROTOSS_FLEETBEACON = 64, PROTOSS_FORGE = 63,
        PROTOSS_GATEWAY = 62, PROTOSS_HIGHTEMPLAR = 75, PROTOSS_IMMORTAL = 83, PROTOSS_INTERCEPTOR = 85,
        PROTOSS_MOTHERSHIP = 10, PROTOSS_MOTHERSHIPCORE = 488, PROTOSS_NEXUS = 59, PROTOSS_OBSERVER = 82,
        PROTOSS_ORACLE = 495, PROTOSS_ORACLESTASISTRAP = 732, PROTOSS_PHOENIX = 78, PROTOSS_PHOTONCANNON = 66,
        PROTOSS_PROBE = 84, PROTOSS_PYLON = 60, PROTOSS_PYLONOVERCHARGED = 894, PROTOSS_ROBOTICSBAY = 70,
        PROTOSS_ROBOTICSFACILITY = 71, PROTOSS_SENTRY = 77, PROTOSS_SHIELDBATTERY = 1910, PROTOSS_STALKER = 74,
        PROTOSS_STARGATE = 67, PROTOSS_TEMPEST = 496, PROTOSS_TEMPLARARCHIVE = 68, PROTOSS_TWILIGHTCOUNCIL = 65,
        PROTOSS_VOIDRAY = 80, PROTOSS_WARPGATE = 133, PROTOSS_WARPPRISM = 81, PROTOSS_WARPPRISMPHASING = 136,
        PROTOSS_ZEALOT = 73, NEUTRAL_BATTLESTATIONMINERALFIELD = 886, NEUTRAL_BATTLESTATIONMINERALFIELD750 = 887, NEUTRAL_COLLAPSIBLEROCKTOWERDEBRIS = 490,
        NEUTRAL_COLLAPSIBLEROCKTOWERDIAGONAL = 588, NEUTRAL_COLLAPSIBLEROCKTOWERPUSHUNIT = 561, NEUTRAL_COLLAPSIBLETERRANTOWERDEBRIS = 485, NEUTRAL_COLLAPSIBLETERRANTOWERDIAGONAL = 589,
        NEUTRAL_COLLAPSIBLETERRANTOWERPUSHUNIT = 562, NEUTRAL_COLLAPSIBLETERRANTOWERPUSHUNITRAMPLEFT = 559, NEUTRAL_COLLAPSIBLETERRANTOWERPUSHUNITRAMPRIGHT = 560, NEUTRAL_COLLAPSIBLETERRANTOWERRAMPLEFT = 590,
        NEUTRAL_COLLAPSIBLETERRANTOWERRAMPRIGHT = 591, NEUTRAL_DEBRISRAMPLEFT = 486, NEUTRAL_DEBRISRAMPRIGHT = 487, NEUTRAL_DESTRUCTIBLEDEBRIS6X6 = 365,
        NEUTRAL_DESTRUCTIBLEDEBRISRAMPDIAGONALHUGEBLUR = 377, NEUTRAL_DESTRUCTIBLEDEBRISRAMPDIAGONALHUGEULBR = 376, NEUTRAL_DESTRUCTIBLEROCK6X6 = 371, NEUTRAL_DESTRUCTIBLEROCKEX1DIAGONALHUGEBLUR = 641,
        NEUTRAL_FORCEFIELD = 135, NEUTRAL_KARAKFEMALE = 324, NEUTRAL_LABMINERALFIELD = 665, NEUTRAL_LABMINERALFIELD750 = 666,
        NEUTRAL_MINERALFIELD = 341, NEUTRAL_MINERALFIELD750 = 483, NEUTRAL_PROTOSSVESPENEGEYSER = 608, NEUTRAL_PURIFIERMINERALFIELD = 884,
        NEUTRAL_PURIFIERMINERALFIELD750 = 885, NEUTRAL_PURIFIERRICHMINERALFIELD = 796, NEUTRAL_PURIFIERRICHMINERALFIELD750 = 797, NEUTRAL_PURIFIERVESPENEGEYSER = 880,
        NEUTRAL_RICHMINERALFIELD = 146, NEUTRAL_RICHMINERALFIELD750 = 147, NEUTRAL_RICHVESPENEGEYSER = 344, NEUTRAL_SCANTIPEDE = 335,
        NEUTRAL_SHAKURASVESPENEGEYSER = 881, NEUTRAL_SPACEPLATFORMGEYSER = 343, NEUTRAL_UNBUILDABLEBRICKSDESTRUCTIBLE = 473, NEUTRAL_UNBUILDABLEPLATESDESTRUCTIBLE = 474,
        NEUTRAL_UTILITYBOT = 330, NEUTRAL_VESPENEGEYSER = 342, NEUTRAL_XELNAGATOWER = 149
    }

    public  enum ABILITY_ID
    {
        INVALID = 0, SMART = 1, ATTACK = 3674, ATTACK_ATTACK = 23,
        ATTACK_ATTACKBUILDING = 2048, ATTACK_REDIRECT = 1682, BEHAVIOR_BUILDINGATTACKOFF = 2082, BEHAVIOR_BUILDINGATTACKON = 2081,
        BEHAVIOR_CLOAKOFF = 3677, BEHAVIOR_CLOAKOFF_BANSHEE = 393, BEHAVIOR_CLOAKOFF_GHOST = 383, BEHAVIOR_CLOAKON = 3676,
        BEHAVIOR_CLOAKON_BANSHEE = 392, BEHAVIOR_CLOAKON_GHOST = 382, BEHAVIOR_GENERATECREEPOFF = 1693, BEHAVIOR_GENERATECREEPON = 1692,
        BEHAVIOR_HOLDFIREOFF = 3689, BEHAVIOR_HOLDFIREOFF_LURKER = 2552, BEHAVIOR_HOLDFIREON = 3688, BEHAVIOR_HOLDFIREON_GHOST = 36,
        BEHAVIOR_HOLDFIREON_LURKER = 2550, BEHAVIOR_PULSARBEAMOFF = 2376, BEHAVIOR_PULSARBEAMON = 2375, BUILD_ARMORY = 331,
        BUILD_ASSIMILATOR = 882, BUILD_BANELINGNEST = 1162, BUILD_BARRACKS = 321, BUILD_BUNKER = 324,
        BUILD_COMMANDCENTER = 318, BUILD_CREEPTUMOR = 3691, BUILD_CREEPTUMOR_QUEEN = 1694, BUILD_CREEPTUMOR_TUMOR = 1733,
        BUILD_CYBERNETICSCORE = 894, BUILD_DARKSHRINE = 891, BUILD_ENGINEERINGBAY = 322, BUILD_EVOLUTIONCHAMBER = 1156,
        BUILD_EXTRACTOR = 1154, BUILD_FACTORY = 328, BUILD_FLEETBEACON = 885, BUILD_FORGE = 884,
        BUILD_FUSIONCORE = 333, BUILD_GATEWAY = 883, BUILD_GHOSTACADEMY = 327, BUILD_HATCHERY = 1152,
        BUILD_HYDRALISKDEN = 1157, BUILD_INFESTATIONPIT = 1160, BUILD_INTERCEPTORS = 1042, BUILD_MISSILETURRET = 323,
        BUILD_NEXUS = 880, BUILD_NUKE = 710, BUILD_NYDUSNETWORK = 1161, BUILD_NYDUSWORM = 1768,
        BUILD_PHOTONCANNON = 887, BUILD_PYLON = 881, BUILD_REACTOR = 3683, BUILD_REACTOR_BARRACKS = 422,
        BUILD_REACTOR_FACTORY = 455, BUILD_REACTOR_STARPORT = 488, BUILD_REFINERY = 320, BUILD_ROACHWARREN = 1165,
        BUILD_ROBOTICSBAY = 892, BUILD_ROBOTICSFACILITY = 893, BUILD_SENSORTOWER = 326, BUILD_SHIELDBATTERY = 895,
        BUILD_SPAWNINGPOOL = 1155, BUILD_SPINECRAWLER = 1166, BUILD_SPIRE = 1158, BUILD_SPORECRAWLER = 1167,
        BUILD_STARGATE = 889, BUILD_STARPORT = 329, BUILD_STASISTRAP = 2505, BUILD_SUPPLYDEPOT = 319,
        BUILD_TECHLAB = 3682, BUILD_TECHLAB_BARRACKS = 421, BUILD_TECHLAB_FACTORY = 454, BUILD_TECHLAB_STARPORT = 487,
        BUILD_TEMPLARARCHIVE = 890, BUILD_TWILIGHTCOUNCIL = 886, BUILD_ULTRALISKCAVERN = 1159, BURROWDOWN = 3661,
        BURROWDOWN_BANELING = 1374, BURROWDOWN_DRONE = 1378, BURROWDOWN_HYDRALISK = 1382, BURROWDOWN_INFESTOR = 1444,
        BURROWDOWN_LURKER = 2108, BURROWDOWN_QUEEN = 1433, BURROWDOWN_RAVAGER = 2340, BURROWDOWN_ROACH = 1386,
        BURROWDOWN_SWARMHOST = 2014, BURROWDOWN_WIDOWMINE = 2095, BURROWDOWN_ZERGLING = 1390, BURROWUP = 3662,
        BURROWUP_BANELING = 1376, BURROWUP_DRONE = 1380, BURROWUP_HYDRALISK = 1384, BURROWUP_INFESTOR = 1446,
        BURROWUP_LURKER = 2110, BURROWUP_QUEEN = 1435, BURROWUP_RAVAGER = 2342, BURROWUP_ROACH = 1388,
        BURROWUP_SWARMHOST = 2016, BURROWUP_WIDOWMINE = 2097, BURROWUP_ZERGLING = 1392, CANCEL = 3659,
        CANCELSLOT_ADDON = 313, CANCELSLOT_QUEUE1 = 305, CANCELSLOT_QUEUE5 = 307, CANCELSLOT_QUEUECANCELTOSELECTION = 309,
        CANCELSLOT_QUEUEPASSIVE = 1832, CANCEL_ADEPTPHASESHIFT = 2594, CANCEL_ADEPTSHADEPHASESHIFT = 2596, CANCEL_BARRACKSADDON = 451,
        CANCEL_BUILDINPROGRESS = 314, CANCEL_CREEPTUMOR = 1763, CANCEL_FACTORYADDON = 484, CANCEL_GRAVITONBEAM = 174,
        CANCEL_LAST = 3671, CANCEL_MORPHBROODLORD = 1373, CANCEL_MORPHLAIR = 1217, CANCEL_MORPHLURKER = 2333,
        CANCEL_MORPHLURKERDEN = 2113, CANCEL_MORPHMOTHERSHIP = 1848, CANCEL_MORPHORBITAL = 1517, CANCEL_MORPHOVERLORDTRANSPORT = 2709,
        CANCEL_MORPHOVERSEER = 1449, CANCEL_MORPHPLANETARYFORTRESS = 1451, CANCEL_MORPHRAVAGER = 2331, CANCEL_QUEUE1 = 304,
        CANCEL_QUEUE5 = 306, CANCEL_QUEUEADDON = 312, CANCEL_QUEUECANCELTOSELECTION = 308, CANCEL_QUEUEPASIVE = 1831,
        CANCEL_QUEUEPASSIVECANCELTOSELECTION = 1833, CANCEL_SPINECRAWLERROOT = 1730, CANCEL_STARPORTADDON = 517, EFFECT_ABDUCT = 2067,
        EFFECT_ADEPTPHASESHIFT = 2544, EFFECT_AUTOTURRET = 1764, EFFECT_BLINDINGCLOUD = 2063, EFFECT_BLINK = 3687,
        EFFECT_BLINK_STALKER = 1442, EFFECT_CALLDOWNMULE = 171, EFFECT_CAUSTICSPRAY = 2324, EFFECT_CHARGE = 1819,
        EFFECT_CHRONOBOOST = 261, EFFECT_CONTAMINATE = 1825, EFFECT_CORROSIVEBILE = 2338, EFFECT_EMP = 1628,
        EFFECT_EXPLODE = 42, EFFECT_FEEDBACK = 140, EFFECT_FORCEFIELD = 1526, EFFECT_FUNGALGROWTH = 74,
        EFFECT_GHOSTSNIPE = 2714, EFFECT_GRAVITONBEAM = 173, EFFECT_GUARDIANSHIELD = 76, EFFECT_HEAL = 386,
        EFFECT_HUNTERSEEKERMISSILE = 169, EFFECT_IMMORTALBARRIER = 2328, EFFECT_INFESTEDTERRANS = 247, EFFECT_INJECTLARVA = 251,
        EFFECT_KD8CHARGE = 2588, EFFECT_LOCKON = 2350, EFFECT_LOCUSTSWOOP = 2387, EFFECT_MASSRECALL = 3686,
        EFFECT_MASSRECALL_MOTHERSHIP = 2368, EFFECT_MASSRECALL_MOTHERSHIPCORE = 1974, EFFECT_MEDIVACIGNITEAFTERBURNERS = 2116, EFFECT_NEURALPARASITE = 249,
        EFFECT_NUKECALLDOWN = 1622, EFFECT_ORACLEREVELATION = 2146, EFFECT_PARASITICBOMB = 2542, EFFECT_PHOTONOVERCHARGE = 2162,
        EFFECT_POINTDEFENSEDRONE = 144, EFFECT_PSISTORM = 1036, EFFECT_PURIFICATIONNOVA = 2346, EFFECT_REPAIR = 3685,
        EFFECT_REPAIR_MULE = 78, EFFECT_REPAIR_SCV = 316, EFFECT_RESTORE = 3765, EFFECT_SALVAGE = 32,
        EFFECT_SCAN = 399, EFFECT_SHADOWSTRIDE = 2700, EFFECT_SPAWNCHANGELING = 181, EFFECT_SPAWNLOCUSTS = 2704,
        EFFECT_SPRAY = 3684, EFFECT_SPRAY_PROTOSS = 30, EFFECT_SPRAY_TERRAN = 26, EFFECT_SPRAY_ZERG = 28,
        EFFECT_STIM = 3675, EFFECT_STIM_MARAUDER = 253, EFFECT_STIM_MARINE = 380, EFFECT_STIM_MARINE_REDIRECT = 1683,
        EFFECT_SUPPLYDROP = 255, EFFECT_TACTICALJUMP = 2358, EFFECT_TEMPESTDISRUPTIONBLAST = 2698, EFFECT_TIMEWARP = 2244,
        EFFECT_TRANSFUSION = 1664, EFFECT_VIPERCONSUME = 2073, EFFECT_VOIDRAYPRISMATICALIGNMENT = 2393, EFFECT_WIDOWMINEATTACK = 2099,
        EFFECT_YAMATOGUN = 401, HALLUCINATION_ADEPT = 2391, HALLUCINATION_ARCHON = 146, HALLUCINATION_COLOSSUS = 148,
        HALLUCINATION_DISRUPTOR = 2389, HALLUCINATION_HIGHTEMPLAR = 150, HALLUCINATION_IMMORTAL = 152, HALLUCINATION_ORACLE = 2114,
        HALLUCINATION_PHOENIX = 154, HALLUCINATION_PROBE = 156, HALLUCINATION_STALKER = 158, HALLUCINATION_VOIDRAY = 160,
        HALLUCINATION_WARPPRISM = 162, HALLUCINATION_ZEALOT = 164, HALT = 3660, HALT_BUILDING = 315,
        HALT_TERRANBUILD = 348, HARVEST_GATHER = 3666, HARVEST_GATHER_DRONE = 1183, HARVEST_GATHER_PROBE = 298,
        HARVEST_GATHER_SCV = 295, HARVEST_RETURN = 3667, HARVEST_RETURN_DRONE = 1184, HARVEST_RETURN_MULE = 167,
        HARVEST_RETURN_PROBE = 299, HARVEST_RETURN_SCV = 296, HOLDPOSITION = 18, LAND = 3678,
        LAND_BARRACKS = 554, LAND_COMMANDCENTER = 419, LAND_FACTORY = 520, LAND_ORBITALCOMMAND = 1524,
        LAND_STARPORT = 522, LIFT = 3679, LIFT_BARRACKS = 452, LIFT_COMMANDCENTER = 417,
        LIFT_FACTORY = 485, LIFT_ORBITALCOMMAND = 1522, LIFT_STARPORT = 518, LOAD = 3668,
        LOADALL = 3663, LOADALL_COMMANDCENTER = 416, LOAD_BUNKER = 407, LOAD_MEDIVAC = 394,
        MORPH_ARCHON = 1766, MORPH_BROODLORD = 1372, MORPH_GATEWAY = 1520, MORPH_GREATERSPIRE = 1220,
        MORPH_HELLBAT = 1998, MORPH_HELLION = 1978, MORPH_HIVE = 1218, MORPH_LAIR = 1216,
        MORPH_LIBERATORAAMODE = 2560, MORPH_LIBERATORAGMODE = 2558, MORPH_LURKER = 2332, MORPH_LURKERDEN = 2112,
        MORPH_MOTHERSHIP = 1847, MORPH_ORBITALCOMMAND = 1516, MORPH_OVERLORDTRANSPORT = 2708, MORPH_OVERSEER = 1448,
        MORPH_PLANETARYFORTRESS = 1450, MORPH_RAVAGER = 2330, MORPH_ROOT = 3680, MORPH_SIEGEMODE = 388,
        MORPH_SPINECRAWLERROOT = 1729, MORPH_SPINECRAWLERUPROOT = 1725, MORPH_SPORECRAWLERROOT = 1731, MORPH_SPORECRAWLERUPROOT = 1727,
        MORPH_SUPPLYDEPOT_LOWER = 556, MORPH_SUPPLYDEPOT_RAISE = 558, MORPH_THOREXPLOSIVEMODE = 2364, MORPH_THORHIGHIMPACTMODE = 2362,
        MORPH_UNSIEGE = 390, MORPH_UPROOT = 3681, MORPH_VIKINGASSAULTMODE = 403, MORPH_VIKINGFIGHTERMODE = 405,
        MORPH_WARPGATE = 1518, MORPH_WARPPRISMPHASINGMODE = 1528, MORPH_WARPPRISMTRANSPORTMODE = 1530, MOVE = 16,
        PATROL = 17, RALLY_BUILDING = 195, RALLY_COMMANDCENTER = 203, RALLY_HATCHERY_UNITS = 211,
        RALLY_HATCHERY_WORKERS = 212, RALLY_MORPHING_UNIT = 199, RALLY_NEXUS = 207, RALLY_UNITS = 3673,
        RALLY_WORKERS = 3690, RESEARCH_ADEPTRESONATINGGLAIVES = 1594, RESEARCH_ADVANCEDBALLISTICS = 805, RESEARCH_BANSHEECLOAKINGFIELD = 790,
        RESEARCH_BANSHEEHYPERFLIGHTROTORS = 799, RESEARCH_BATTLECRUISERWEAPONREFIT = 1532, RESEARCH_BLINK = 1593, RESEARCH_BURROW = 1225,
        RESEARCH_CENTRIFUGALHOOKS = 1482, RESEARCH_CHARGE = 1592, RESEARCH_CHITINOUSPLATING = 265, RESEARCH_COMBATSHIELD = 731,
        RESEARCH_CONCUSSIVESHELLS = 732, RESEARCH_DRILLINGCLAWS = 764, RESEARCH_ENHANCEDMUNITIONS = 806, RESEARCH_EXTENDEDTHERMALLANCE = 1097,
        RESEARCH_GLIALREGENERATION = 216, RESEARCH_GRAVITICBOOSTER = 1093, RESEARCH_GRAVITICDRIVE = 1094, RESEARCH_GROOVEDSPINES = 1282,
        RESEARCH_HIGHCAPACITYFUELTANKS = 804, RESEARCH_HISECAUTOTRACKING = 650, RESEARCH_INFERNALPREIGNITER = 761, RESEARCH_INTERCEPTORGRAVITONCATAPULT = 44,
        RESEARCH_MAGFIELDLAUNCHERS = 766, RESEARCH_MUSCULARAUGMENTS = 1283, RESEARCH_NEOSTEELFRAME = 655, RESEARCH_NEURALPARASITE = 1455,
        RESEARCH_PATHOGENGLANDS = 1454, RESEARCH_PERSONALCLOAKING = 820, RESEARCH_PHOENIXANIONPULSECRYSTALS = 46, RESEARCH_PNEUMATIZEDCARAPACE = 1223,
        RESEARCH_PROTOSSAIRARMOR = 3692, RESEARCH_PROTOSSAIRARMORLEVEL1 = 1565, RESEARCH_PROTOSSAIRARMORLEVEL2 = 1566, RESEARCH_PROTOSSAIRARMORLEVEL3 = 1567,
        RESEARCH_PROTOSSAIRWEAPONS = 3693, RESEARCH_PROTOSSAIRWEAPONSLEVEL1 = 1562, RESEARCH_PROTOSSAIRWEAPONSLEVEL2 = 1563, RESEARCH_PROTOSSAIRWEAPONSLEVEL3 = 1564,
        RESEARCH_PROTOSSGROUNDARMOR = 3694, RESEARCH_PROTOSSGROUNDARMORLEVEL1 = 1065, RESEARCH_PROTOSSGROUNDARMORLEVEL2 = 1066, RESEARCH_PROTOSSGROUNDARMORLEVEL3 = 1067,
        RESEARCH_PROTOSSGROUNDWEAPONS = 3695, RESEARCH_PROTOSSGROUNDWEAPONSLEVEL1 = 1062, RESEARCH_PROTOSSGROUNDWEAPONSLEVEL2 = 1063, RESEARCH_PROTOSSGROUNDWEAPONSLEVEL3 = 1064,
        RESEARCH_PROTOSSSHIELDS = 3696, RESEARCH_PROTOSSSHIELDSLEVEL1 = 1068, RESEARCH_PROTOSSSHIELDSLEVEL2 = 1069, RESEARCH_PROTOSSSHIELDSLEVEL3 = 1070,
        RESEARCH_PSISTORM = 1126, RESEARCH_RAPIDFIRELAUNCHERS = 768, RESEARCH_RAVENCORVIDREACTOR = 793, RESEARCH_RAVENRECALIBRATEDEXPLOSIVES = 803,
        RESEARCH_SHADOWSTRIKE = 2720, RESEARCH_SMARTSERVOS = 766, RESEARCH_STIMPACK = 730, RESEARCH_TERRANINFANTRYARMOR = 3697,
        RESEARCH_TERRANINFANTRYARMORLEVEL1 = 656, RESEARCH_TERRANINFANTRYARMORLEVEL2 = 657, RESEARCH_TERRANINFANTRYARMORLEVEL3 = 658, RESEARCH_TERRANINFANTRYWEAPONS = 3698,
        RESEARCH_TERRANINFANTRYWEAPONSLEVEL1 = 652, RESEARCH_TERRANINFANTRYWEAPONSLEVEL2 = 653, RESEARCH_TERRANINFANTRYWEAPONSLEVEL3 = 654, RESEARCH_TERRANSHIPWEAPONS = 3699,
        RESEARCH_TERRANSHIPWEAPONSLEVEL1 = 861, RESEARCH_TERRANSHIPWEAPONSLEVEL2 = 862, RESEARCH_TERRANSHIPWEAPONSLEVEL3 = 863, RESEARCH_TERRANSTRUCTUREARMORUPGRADE = 651,
        RESEARCH_TERRANVEHICLEANDSHIPPLATING = 3700, RESEARCH_TERRANVEHICLEANDSHIPPLATINGLEVEL1 = 864, RESEARCH_TERRANVEHICLEANDSHIPPLATINGLEVEL2 = 865, RESEARCH_TERRANVEHICLEANDSHIPPLATINGLEVEL3 = 866,
        RESEARCH_TERRANVEHICLEWEAPONS = 3701, RESEARCH_TERRANVEHICLEWEAPONSLEVEL1 = 855, RESEARCH_TERRANVEHICLEWEAPONSLEVEL2 = 856, RESEARCH_TERRANVEHICLEWEAPONSLEVEL3 = 857,
        RESEARCH_TUNNELINGCLAWS = 217, RESEARCH_WARPGATE = 1568, RESEARCH_ZERGFLYERARMOR = 3702, RESEARCH_ZERGFLYERARMORLEVEL1 = 1315,
        RESEARCH_ZERGFLYERARMORLEVEL2 = 1316, RESEARCH_ZERGFLYERARMORLEVEL3 = 1317, RESEARCH_ZERGFLYERATTACK = 3703, RESEARCH_ZERGFLYERATTACKLEVEL1 = 1312,
        RESEARCH_ZERGFLYERATTACKLEVEL2 = 1313, RESEARCH_ZERGFLYERATTACKLEVEL3 = 1314, RESEARCH_ZERGGROUNDARMOR = 3704, RESEARCH_ZERGGROUNDARMORLEVEL1 = 1189,
        RESEARCH_ZERGGROUNDARMORLEVEL2 = 1190, RESEARCH_ZERGGROUNDARMORLEVEL3 = 1191, RESEARCH_ZERGLINGADRENALGLANDS = 1252, RESEARCH_ZERGLINGMETABOLICBOOST = 1253,
        RESEARCH_ZERGMELEEWEAPONS = 3705, RESEARCH_ZERGMELEEWEAPONSLEVEL1 = 1186, RESEARCH_ZERGMELEEWEAPONSLEVEL2 = 1187, RESEARCH_ZERGMELEEWEAPONSLEVEL3 = 1188,
        RESEARCH_ZERGMISSILEWEAPONS = 3706, RESEARCH_ZERGMISSILEWEAPONSLEVEL1 = 1192, RESEARCH_ZERGMISSILEWEAPONSLEVEL2 = 1193, RESEARCH_ZERGMISSILEWEAPONSLEVEL3 = 1194,
        SCAN_MOVE = 19, STOP = 3665, STOP_BUILDING = 2057, STOP_CHEER = 6,
        STOP_DANCE = 7, STOP_REDIRECT = 1691, STOP_STOP = 4, TRAINWARP_ADEPT = 1419,
        TRAINWARP_DARKTEMPLAR = 1417, TRAINWARP_HIGHTEMPLAR = 1416, TRAINWARP_SENTRY = 1418, TRAINWARP_STALKER = 1414,
        TRAINWARP_ZEALOT = 1413, TRAIN_ADEPT = 922, TRAIN_BANELING = 80, TRAIN_BANSHEE = 621,
        TRAIN_BATTLECRUISER = 623, TRAIN_CARRIER = 948, TRAIN_COLOSSUS = 978, TRAIN_CORRUPTOR = 1353,
        TRAIN_CYCLONE = 597, TRAIN_DARKTEMPLAR = 920, TRAIN_DISRUPTOR = 994, TRAIN_DRONE = 1342,
        TRAIN_GHOST = 562, TRAIN_HELLBAT = 596, TRAIN_HELLION = 595, TRAIN_HIGHTEMPLAR = 919,
        TRAIN_HYDRALISK = 1345, TRAIN_IMMORTAL = 979, TRAIN_INFESTOR = 1352, TRAIN_LIBERATOR = 626,
        TRAIN_MARAUDER = 563, TRAIN_MARINE = 560, TRAIN_MEDIVAC = 620, TRAIN_MOTHERSHIP = 110,
        TRAIN_MOTHERSHIPCORE = 1853, TRAIN_MUTALISK = 1346, TRAIN_OBSERVER = 977, TRAIN_ORACLE = 954,
        TRAIN_OVERLORD = 1344, TRAIN_PHOENIX = 946, TRAIN_PROBE = 1006, TRAIN_QUEEN = 1632,
        TRAIN_RAVEN = 622, TRAIN_REAPER = 561, TRAIN_ROACH = 1351, TRAIN_SCV = 524,
        TRAIN_SENTRY = 921, TRAIN_SIEGETANK = 591, TRAIN_STALKER = 917, TRAIN_SWARMHOST = 1356,
        TRAIN_TEMPEST = 955, TRAIN_THOR = 594, TRAIN_ULTRALISK = 1348, TRAIN_VIKINGFIGHTER = 624,
        TRAIN_VIPER = 1354, TRAIN_VOIDRAY = 950, TRAIN_WARPPRISM = 976, TRAIN_WIDOWMINE = 614,
        TRAIN_ZEALOT = 916, TRAIN_ZERGLING = 1343, UNLOADALL = 3664, UNLOADALLAT = 3669,
        UNLOADALLAT_MEDIVAC = 396, UNLOADALLAT_OVERLORD = 1408, UNLOADALLAT_WARPPRISM = 913, UNLOADALL_BUNKER = 408,
        UNLOADALL_COMMANDCENTER = 413, UNLOADALL_NYDASNETWORK = 1438, UNLOADALL_NYDUSWORM = 2371, UNLOADUNIT_BUNKER = 410,
        UNLOADUNIT_COMMANDCENTER = 415, UNLOADUNIT_MEDIVAC = 397, UNLOADUNIT_NYDASNETWORK = 1440, UNLOADUNIT_OVERLORD = 1409,
        UNLOADUNIT_WARPPRISM = 914
    }

    public enum UPGRADE_ID
    {
        INVALID = 0, CARRIERLAUNCHSPEEDUPGRADE = 1, GLIALRECONSTITUTION = 2, TUNNELINGCLAWS = 3,
        CHITINOUSPLATING = 4, HISECAUTOTRACKING = 5, TERRANBUILDINGARMOR = 6, TERRANINFANTRYWEAPONSLEVEL1 = 7,
        TERRANINFANTRYWEAPONSLEVEL2 = 8, TERRANINFANTRYWEAPONSLEVEL3 = 9, NEOSTEELFRAME = 10, TERRANINFANTRYARMORSLEVEL1 = 11,
        TERRANINFANTRYARMORSLEVEL2 = 12, TERRANINFANTRYARMORSLEVEL3 = 13, STIMPACK = 15, SHIELDWALL = 16,
        PUNISHERGRENADES = 17, HIGHCAPACITYBARRELS = 19, BANSHEECLOAK = 20, RAVENCORVIDREACTOR = 22,
        PERSONALCLOAKING = 25, TERRANVEHICLEWEAPONSLEVEL1 = 30, TERRANVEHICLEWEAPONSLEVEL2 = 31, TERRANVEHICLEWEAPONSLEVEL3 = 32,
        TERRANSHIPWEAPONSLEVEL1 = 36, TERRANSHIPWEAPONSLEVEL2 = 37, TERRANSHIPWEAPONSLEVEL3 = 38, PROTOSSGROUNDWEAPONSLEVEL1 = 39,
        PROTOSSGROUNDWEAPONSLEVEL2 = 40, PROTOSSGROUNDWEAPONSLEVEL3 = 41, PROTOSSGROUNDARMORSLEVEL1 = 42, PROTOSSGROUNDARMORSLEVEL2 = 43,
        PROTOSSGROUNDARMORSLEVEL3 = 44, PROTOSSSHIELDSLEVEL1 = 45, PROTOSSSHIELDSLEVEL2 = 46, PROTOSSSHIELDSLEVEL3 = 47,
        OBSERVERGRAVITICBOOSTER = 48, GRAVITICDRIVE = 49, EXTENDEDTHERMALLANCE = 50, PSISTORMTECH = 52,
        ZERGMELEEWEAPONSLEVEL1 = 53, ZERGMELEEWEAPONSLEVEL2 = 54, ZERGMELEEWEAPONSLEVEL3 = 55, ZERGGROUNDARMORSLEVEL1 = 56,
        ZERGGROUNDARMORSLEVEL2 = 57, ZERGGROUNDARMORSLEVEL3 = 58, ZERGMISSILEWEAPONSLEVEL1 = 59, ZERGMISSILEWEAPONSLEVEL2 = 60,
        ZERGMISSILEWEAPONSLEVEL3 = 61, OVERLORDSPEED = 62, BURROW = 64, ZERGLINGATTACKSPEED = 65,
        ZERGLINGMOVEMENTSPEED = 66, ZERGFLYERWEAPONSLEVEL1 = 68, ZERGFLYERWEAPONSLEVEL2 = 69, ZERGFLYERWEAPONSLEVEL3 = 70,
        ZERGFLYERARMORSLEVEL1 = 71, ZERGFLYERARMORSLEVEL2 = 72, ZERGFLYERARMORSLEVEL3 = 73, INFESTORENERGYUPGRADE = 74,
        CENTRIFICALHOOKS = 75, BATTLECRUISERENABLESPECIALIZATIONS = 76, PROTOSSAIRWEAPONSLEVEL1 = 78, PROTOSSAIRWEAPONSLEVEL2 = 79,
        PROTOSSAIRWEAPONSLEVEL3 = 80, PROTOSSAIRARMORSLEVEL1 = 81, PROTOSSAIRARMORSLEVEL2 = 82, PROTOSSAIRARMORSLEVEL3 = 83,
        WARPGATERESEARCH = 84, CHARGE = 86, BLINKTECH = 87, PHOENIXRANGEUPGRADE = 99,
        NEURALPARASITE = 101, TERRANVEHICLEANDSHIPARMORSLEVEL1 = 116, TERRANVEHICLEANDSHIPARMORSLEVEL2 = 117, TERRANVEHICLEANDSHIPARMORSLEVEL3 = 118,
        DRILLCLAWS = 122, ADEPTPIERCINGATTACK = 130, MAGFIELDLAUNCHERS = 133, EVOLVEGROOVEDSPINES = 134,
        EVOLVEMUSCULARAUGMENTS = 135, BANSHEESPEED = 136, RAVENRECALIBRATEDEXPLOSIVES = 138, MEDIVACINCREASESPEEDBOOST = 139,
        LIBERATORAGRANGEUPGRADE = 140, DARKTEMPLARBLINKUPGRADE = 141, SMARTSERVOS = 289, RAPIDFIRELAUNCHERS = 291,
        ENHANCEDMUNITIONS = 292
    }

    public enum BUFF_ID
    {
        INVALID = 0, GRAVITONBEAM = 5, GHOSTCLOAK = 6, BANSHEECLOAK = 7,
        POWERUSERWARPABLE = 8, QUEENSPAWNLARVATIMER = 11, GHOSTHOLDFIRE = 12, GHOSTHOLDFIREB = 13,
        EMPDECLOAK = 16, FUNGALGROWTH = 17, GUARDIANSHIELD = 18, TIMEWARPPRODUCTION = 20,
        NEURALPARASITE = 22, STIMPACKMARAUDER = 24, SUPPLYDROP = 25, STIMPACK = 27,
        PSISTORM = 28, CLOAKFIELDEFFECT = 29, CHARGING = 30, SLOW = 33,
        CONTAMINATED = 36, BLINDINGCLOUDSTRUCTURE = 38, ORACLEREVELATION = 49, VIPERCONSUMESTRUCTURE = 59,
        BLINDINGCLOUD = 83, MEDIVACSPEEDBOOST = 89, PURIFY = 97, ORACLEWEAPON = 99,
        IMMORTALOVERLOAD = 102, LOCKON = 116, SEEKERMISSILE = 120, TEMPORALFIELD = 121,
        VOIDRAYSWARMDAMAGEBOOST = 122, ORACLESTASISTRAPTARGET = 129, PARASITICBOMB = 132, PARASITICBOMBUNITKU = 133,
        PARASITICBOMBSECONDARYUNITSEARCH = 134, LURKERHOLDFIREB = 137, CHANNELSNIPECOMBAT = 145, TEMPESTDISRUPTIONBLASTSTUNBEHAVIOR = 146,
        CARRYMINERALFIELDMINERALS = 271, CARRYHIGHYIELDMINERALFIELDMINERALS = 272, CARRYHARVESTABLEVESPENEGEYSERGAS = 273, CARRYHARVESTABLEVESPENEGEYSERGASPROTOSS = 274,
        CARRYHARVESTABLEVESPENEGEYSERGASZERG = 275
    }

    public static class SC2Enum
    {
        public static String ToName<T>(T d)
        {
            return Enum.GetName(typeof(T), d);
        }
    }

}