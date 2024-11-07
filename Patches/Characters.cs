using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Characters
    {
        private static readonly Dictionary<string, string> allCharacters = new()
        {
            ["character_dummy_02"] = "characters/_test/character_dummy_02",
            ["event_spawn_swamper1"] = "characters/_test/event_spawn_swamper1",
            ["forestspirit_onechance_02"] = "characters/_test/forestspirit_onechance_02",
            ["trigger_event_spawn_swamper1"] = "characters/_test/trigger_event_spawn_swamper1",
            ["villager3_plank_burning"] = "characters/_test/villager3_plank_burning",
            ["weszenie"] = "characters/_test/weszenie",
            ["antagonistact2lv4"] = "characters/antagonistact2lv4",
            ["banshee"] = "characters/banshee",
            ["bansheebaby"] = "characters/bansheebaby",
            ["brat_babykury"] = "characters/brat_babykury",
            ["bride"] = "characters/bride",
            ["bride_dress_old"] = "characters/bride_dress_old",
            ["bride_running"] = "characters/bride_running",
            ["bride_test"] = "characters/bride_test",
            ["centipede"] = "characters/centipede",
            ["chicken"] = "characters/chicken",
            ["chomperblack"] = "characters/chomperblack",
            ["chomperbride"] = "characters/chomperbride",
            ["chomperhalf"] = "characters/chomperhalf",
            ["chomperred"] = "characters/chomperred",
            ["chomperred_small"] = "characters/chomperred_small",
            ["clone_villager_01"] = "characters/clones/clone_villager_01",
            ["deer"] = "characters/deer",
            ["dog"] = "characters/dog",
            ["dogmutated"] = "characters/dogmutated",
            ["antagonistact2lv4_epilog"] = "characters/epilogue/antagonistact2lv4_epilog",
            ["epilog_dozorca"] = "characters/epilogue/epilog_dozorca",
            ["epilog_man_burning_1"] = "characters/epilogue/epilog_man_burning_1",
            ["epilog_man_burning_2"] = "characters/epilogue/epilog_man_burning_2",
            ["epilog_man_burning_3"] = "characters/epilogue/epilog_man_burning_3",
            ["epilog_man_crawl_1"] = "characters/epilogue/epilog_man_crawl_1",
            ["epilog_man_crawl_1_fake"] = "characters/epilogue/epilog_man_crawl_1_fake",
            ["epilog_man_crawl_1_fake_burn"] = "characters/epilogue/epilog_man_crawl_1_fake_burn",
            ["epilog_man_crawl_1_fake_slow"] = "characters/epilogue/epilog_man_crawl_1_fake_slow",
            ["epilog_man_crawl_2"] = "characters/epilogue/epilog_man_crawl_2",
            ["epilog_man_crawl_3"] = "characters/epilogue/epilog_man_crawl_3",
            ["epilog_man_idle_1"] = "characters/epilogue/epilog_man_idle_1",
            ["epilog_man_idle_1_burned"] = "characters/epilogue/epilog_man_idle_1_burned",
            ["epilog_man_idle_1_first"] = "characters/epilogue/epilog_man_idle_1_first",
            ["epilog_man_idle_10_standing"] = "characters/epilogue/epilog_man_idle_10_standing",
            ["epilog_man_idle_11_crawl"] = "characters/epilogue/epilog_man_idle_11_crawl",
            ["epilog_man_idle_2_burned"] = "characters/epilogue/epilog_man_idle_2_burned",
            ["epilog_man_idle_2_burned_randomizer"] = "characters/epilogue/epilog_man_idle_2_burned_randomizer",
            ["epilog_man_idle_3"] = "characters/epilogue/epilog_man_idle_3",
            ["epilog_man_idle_3_burned"] = "characters/epilogue/epilog_man_idle_3_burned",
            ["epilog_man_idle_3_first"] = "characters/epilogue/epilog_man_idle_3_first",
            ["epilog_man_idle_4"] = "characters/epilogue/epilog_man_idle_4",
            ["epilog_man_idle_5_standing"] = "characters/epilogue/epilog_man_idle_5_standing",
            ["epilog_man_idle_6"] = "characters/epilogue/epilog_man_idle_6",
            ["epilog_man_idle_7_wakeup"] = "characters/epilogue/epilog_man_idle_7_wakeup",
            ["epilog_man_idle_8_wakeup"] = "characters/epilogue/epilog_man_idle_8_wakeup",
            ["epilog_man_idle_9_wakeup"] = "characters/epilogue/epilog_man_idle_9_wakeup",
            ["epilog_man_villager_1_burned"] = "characters/epilogue/epilog_man_villager_1_burned",
            ["epilog_villager_idle_1"] = "characters/epilogue/epilog_villager_idle_1",
            ["epilog_villager_idle_2"] = "characters/epilogue/epilog_villager_idle_2",
            ["epilog_villager_idle_3"] = "characters/epilogue/epilog_villager_idle_3",
            ["areabird"] = "characters/fakechars/areabird",
            ["banshee_cry"] = "characters/fakechars/banshee_cry",
            ["banshee_fake"] = "characters/fakechars/banshee_fake",
            ["bug_cockroach_big"] = "characters/fakechars/bug_cockroach_big",
            ["bug_cockroach_huge"] = "characters/fakechars/bug_cockroach_huge",
            ["bug_cockroach_small"] = "characters/fakechars/bug_cockroach_small",
            ["centipede_hole"] = "characters/fakechars/centipede_hole",
            ["centipede_hole_2"] = "characters/fakechars/centipede_hole_2",
            ["characterspawnpoint"] = "characters/fakechars/characterspawnpoint",
            ["crazy_villager_mininpc_01"] = "characters/fakechars/crazy_villager_mininpc_01",
            ["crazy_villager_mininpc_01_radiotower"] = "characters/fakechars/crazy_villager_mininpc_01_radiotower",
            ["crazy_villager_mininpc_02"] = "characters/fakechars/crazy_villager_mininpc_02",
            ["crazy_villager_mininpc_gridobj_rednecks_2_01"] = "characters/fakechars/crazy_villager_mininpc_gridobj_rednecks_2_01",
            ["crazy_villager_mininpc_onechance"] = "characters/fakechars/crazy_villager_mininpc_onechance",
            ["dog_cottagetrailer"] = "characters/fakechars/dog_cottagetrailer",
            ["forestspirit2"] = "characters/fakechars/forestspirit2",
            ["larva_big_01"] = "characters/fakechars/larva_big_01",
            ["nightworms_01"] = "characters/fakechars/nightworms_01",
            ["nightworms_02"] = "characters/fakechars/nightworms_02",
            ["pig_big_mutant"] = "characters/fakechars/pig_big_mutant",
            ["priest_mininpc_01"] = "characters/fakechars/priest_mininpc_01",
            ["raven_dummy_01"] = "characters/fakechars/raven_dummy_01",
            ["shadow"] = "characters/fakechars/shadow",
            ["shadow_immortal"] = "characters/fakechars/shadow_immortal",
            ["worms_enemy_01"] = "characters/fakechars/worms_enemy_01",
            ["zombie_female_bathing"] = "characters/fakechars/zombie_female_bathing",
            ["zombie_male_sitting"] = "characters/fakechars/zombie_male_sitting",
            ["forestspirit_bunkerdream"] = "characters/forestspirit_bunkerdream",
            ["humanspider"] = "characters/humanspider",
            ["humanspiderminion"] = "characters/humanspiderminion",
            ["kamikaze"] = "characters/kamikaze",
            ["cripple_npc_preepilogue"] = "characters/locationchars/cripple_npc_preepilogue",
            ["dog_01_wolfmanhideout_01"] = "characters/locationchars/dog_01_wolfmanhideout_01",
            ["dog_02_wolfmanhideout_01"] = "characters/locationchars/dog_02_wolfmanhideout_01",
            ["dog_03_wolfmanhideout_01"] = "characters/locationchars/dog_03_wolfmanhideout_01",
            ["dog_doctor"] = "characters/locationchars/dog_doctor",
            ["redneckbury"] = "characters/locationchars/redneckbury",
            ["dodger"] = "characters/notused/dodger",
            ["doppelganger"] = "characters/notused/doppelganger",
            ["robber"] = "characters/notused/robber",
            ["tank"] = "characters/notused/tank",
            ["villager_pistol"] = "characters/notused/villager_pistol",
            ["baba"] = "characters/npc/baba",
            ["bed_npc"] = "characters/npc/bed_npc",
            ["brother_baba"] = "characters/npc/brother_baba",
            ["child"] = "characters/npc/child",
            ["cripple"] = "characters/npc/cripple",
            ["doctor_act2"] = "characters/npc/doctor_act2",
            ["doctor_confronted"] = "characters/npc/doctor_confronted",
            ["doctor_confronted2"] = "characters/npc/doctor_confronted2",
            ["doctor_follower"] = "characters/npc/doctor_follower",
            ["doctor_idle"] = "characters/npc/doctor_idle",
            ["doctor_trapset"] = "characters/npc/doctor_trapset",
            ["door_talkable_outside_bunker_underground_02"] = "characters/npc/door_talkable_outside_bunker_underground_02",
            ["elephants"] = "characters/npc/elephants",
            ["figurine_doctor"] = "characters/npc/figurine_doctor",
            ["istota_lv1"] = "characters/npc/istota_lv1",
            ["istota_lv1_events"] = "characters/npc/istota_lv1_events",
            ["istota_lv1_events_podmianagrafikiistoty"] = "characters/npc/istota_lv1_events_podmianagrafikiistoty",
            ["istota_lv1_events_sounds"] = "characters/npc/istota_lv1_events_sounds",
            ["istota_lv2"] = "characters/npc/istota_lv2",
            ["istota_lv2_events"] = "characters/npc/istota_lv2_events",
            ["kapelusznik_dummy"] = "characters/npc/kapelusznik_dummy",
            ["kapelusznik_dummy_01"] = "characters/npc/kapelusznik_dummy_01",
            ["kapelusznik_dummy_02"] = "characters/npc/kapelusznik_dummy_02",
            ["kapelusznik_dummy_end_dreamdoctor_01"] = "characters/npc/kapelusznik_dummy_end_dreamdoctor_01",
            ["maciek"] = "characters/npc/maciek",
            ["maciek_noflamethrower"] = "characters/npc/maciek_noflamethrower",
            ["mushroomgranny"] = "characters/npc/mushroomgranny",
            ["muzyk"] = "characters/npc/muzyk",
            ["muzyk_duzy"] = "characters/npc/muzyk_duzy",
            ["muzyk_huge"] = "characters/npc/muzyk_huge",
            ["muzyk_runaway"] = "characters/npc/muzyk_runaway",
            ["nighttrader"] = "characters/npc/nighttrader",
            ["pigshedlever"] = "characters/npc/pigshedlever",
            ["piotrek"] = "characters/npc/piotrek",
            ["porter"] = "characters/npc/porter",
            ["scarecrow"] = "characters/npc/scarecrow",
            ["shrine_03"] = "characters/npc/shrine_03",
            ["shrine_03_pigdead"] = "characters/npc/shrine_03_pigdead",
            ["shrine_mushroomgranny"] = "characters/npc/shrine_mushroomgranny",
            ["shrine_treevillage"] = "characters/npc/shrine_treevillage",
            ["snail"] = "characters/npc/snail",
            ["snail_smoke_01"] = "characters/npc/snail_smoke_01",
            ["soldier_underground"] = "characters/npc/soldier_underground",
            ["soldier_underground_old"] = "characters/npc/soldier_underground_old",
            ["stove_npc"] = "characters/npc/stove_npc",
            ["talkingtree"] = "characters/npc/talkingtree",
            ["talkingtree_burned_roadtohome"] = "characters/npc/talkingtree_burned_roadtohome",
            ["talkingtree_darkside"] = "characters/npc/talkingtree_darkside",
            ["thethree"] = "characters/npc/thethree",
            ["tractor_muzyk"] = "characters/npc/tractor_muzyk",
            ["tree_dry_medium_deadpig_01_villageentrance"] = "characters/npc/tree_dry_medium_deadpig_01_villageentrance",
            ["tree_dry_medium_deadpig_01_villagersswamp"] = "characters/npc/tree_dry_medium_deadpig_01_villagersswamp",
            ["valve"] = "characters/npc/valve",
            ["wardrobe_npc"] = "characters/npc/wardrobe_npc",
            ["wolf"] = "characters/npc/wolf",
            ["wolfman_att"] = "characters/npc/wolfman_att",
            ["pig"] = "characters/pig",
            ["player_cutscene_soundtest"] = "characters/player_cutscene_soundtest",
            ["rabbit"] = "characters/rabbit",
            ["raven"] = "characters/raven",
            ["redneck"] = "characters/redneck",
            ["redneck02"] = "characters/redneck02",
            ["redneck03"] = "characters/redneck03",
            ["spider01"] = "characters/spider01",
            ["spider02"] = "characters/spider02",
            ["spider03_day"] = "characters/spider03_day",
            ["swamper1"] = "characters/swamper1",
            ["villager"] = "characters/villager",
            ["villager1_burning"] = "characters/villager1_burning",
            ["villager3_plank"] = "characters/villager3_plank",
            ["villagerpitchfork"] = "characters/villagerpitchfork",
            ["villager1granny"] = "characters/villagers_mushroom_granny/villager1granny",
            ["villager2granny"] = "characters/villagers_mushroom_granny/villager2granny",
            ["villager3granny"] = "characters/villagers_mushroom_granny/villager3granny",
            ["crazy_villager_cottageruin"] = "characters/villagers/crazy_villager_cottageruin",
            ["villager_bride_01"] = "characters/villagers/villager_bride_01",
            ["villager_infected1"] = "characters/villagers/villager_infected1",
            ["villager_infected1_ch2"] = "characters/villagers/villager_infected1_ch2",
            ["villager_infected1b_ch2"] = "characters/villagers/villager_infected1b_ch2",
            ["villager_infected2"] = "characters/villagers/villager_infected2",
            ["villager_infected2_ch2"] = "characters/villagers/villager_infected2_ch2",
            ["villager_infected3"] = "characters/villagers/villager_infected3",
            ["villager_infected3_ch2"] = "characters/villagers/villager_infected3_ch2",
            ["villager_infected4"] = "characters/villagers/villager_infected4",
            ["villager_normal1"] = "characters/villagers/villager_normal1",
            ["villager_normal10"] = "characters/villagers/villager_normal10",
            ["villager_normal11"] = "characters/villagers/villager_normal11",
            ["villager_normal12_babaplacze"] = "characters/villagers/villager_normal12_babaplacze",
            ["villager_normal2"] = "characters/villagers/villager_normal2",
            ["villager_normal3"] = "characters/villagers/villager_normal3",
            ["villager_normal4"] = "characters/villagers/villager_normal4",
            ["villager_normal5"] = "characters/villagers/villager_normal5",
            ["villager_normal6"] = "characters/villagers/villager_normal6",
            ["villager_normal7"] = "characters/villagers/villager_normal7",
            ["villager_normal8"] = "characters/villagers/villager_normal8",
            ["villager_normal9"] = "characters/villagers/villager_normal9",
            ["villagertorch"] = "characters/villagertorch"
        };

        private static readonly Dictionary<string, string> nonBrokenCharacters = new()
        {
            ["character_dummy_02"] = "characters/_test/character_dummy_02",
            ["forestspirit_onechance_02"] = "characters/_test/forestspirit_onechance_02",
            ["villager3_plank_burning"] = "characters/_test/villager3_plank_burning",
            ["banshee"] = "characters/banshee",
            ["bansheebaby"] = "characters/bansheebaby",
            ["brat_babykury"] = "characters/brat_babykury",
            ["bride"] = "characters/bride",
            ["bride_running"] = "characters/bride_running",
            ["bride_test"] = "characters/bride_test",
            ["centipede"] = "characters/centipede",
            ["chicken"] = "characters/chicken",
            ["chomperblack"] = "characters/chomperblack",
            ["chomperbride"] = "characters/chomperbride",
            ["chomperhalf"] = "characters/chomperhalf",
            ["chomperred"] = "characters/chomperred",
            ["chomperred_small"] = "characters/chomperred_small",
            ["clone_villager_01"] = "characters/clones/clone_villager_01",
            ["deer"] = "characters/deer",
            ["dog"] = "characters/dog",
            ["dogmutated"] = "characters/dogmutated",
            ["antagonistact2lv4_epilog"] = "characters/epilogue/antagonistact2lv4_epilog",
            ["epilog_dozorca"] = "characters/epilogue/epilog_dozorca",
            ["epilog_man_burning_1"] = "characters/epilogue/epilog_man_burning_1",
            ["epilog_man_burning_2"] = "characters/epilogue/epilog_man_burning_2",
            ["epilog_man_burning_3"] = "characters/epilogue/epilog_man_burning_3",
            ["epilog_man_crawl_1"] = "characters/epilogue/epilog_man_crawl_1",
            ["epilog_man_crawl_1_fake"] = "characters/epilogue/epilog_man_crawl_1_fake",
            ["epilog_man_crawl_1_fake_slow"] = "characters/epilogue/epilog_man_crawl_1_fake_slow",
            ["epilog_man_crawl_2"] = "characters/epilogue/epilog_man_crawl_2",
            ["epilog_man_crawl_3"] = "characters/epilogue/epilog_man_crawl_3",
            ["epilog_man_idle_1"] = "characters/epilogue/epilog_man_idle_1",
            ["epilog_man_idle_1_burned"] = "characters/epilogue/epilog_man_idle_1_burned",
            ["epilog_man_idle_1_first"] = "characters/epilogue/epilog_man_idle_1_first",
            ["epilog_man_idle_10_standing"] = "characters/epilogue/epilog_man_idle_10_standing",
            ["epilog_man_idle_11_crawl"] = "characters/epilogue/epilog_man_idle_11_crawl",
            ["epilog_man_idle_2_burned"] = "characters/epilogue/epilog_man_idle_2_burned",
            ["epilog_man_idle_2_burned_randomizer"] = "characters/epilogue/epilog_man_idle_2_burned_randomizer",
            ["epilog_man_idle_3"] = "characters/epilogue/epilog_man_idle_3",
            ["epilog_man_idle_3_burned"] = "characters/epilogue/epilog_man_idle_3_burned",
            ["epilog_man_idle_3_first"] = "characters/epilogue/epilog_man_idle_3_first",
            ["epilog_man_idle_4"] = "characters/epilogue/epilog_man_idle_4",
            ["epilog_man_idle_5_standing"] = "characters/epilogue/epilog_man_idle_5_standing",
            ["epilog_man_idle_6"] = "characters/epilogue/epilog_man_idle_6",
            ["epilog_man_idle_7_wakeup"] = "characters/epilogue/epilog_man_idle_7_wakeup",
            ["epilog_man_idle_8_wakeup"] = "characters/epilogue/epilog_man_idle_8_wakeup",
            ["epilog_man_idle_9_wakeup"] = "characters/epilogue/epilog_man_idle_9_wakeup",
            ["epilog_man_villager_1_burned"] = "characters/epilogue/epilog_man_villager_1_burned",
            ["epilog_villager_idle_1"] = "characters/epilogue/epilog_villager_idle_1",
            ["epilog_villager_idle_2"] = "characters/epilogue/epilog_villager_idle_2",
            ["epilog_villager_idle_3"] = "characters/epilogue/epilog_villager_idle_3",
            ["areabird"] = "characters/fakechars/areabird",
            ["banshee_cry"] = "characters/fakechars/banshee_cry",
            ["banshee_fake"] = "characters/fakechars/banshee_fake",
            ["bug_cockroach_big"] = "characters/fakechars/bug_cockroach_big",
            ["bug_cockroach_huge"] = "characters/fakechars/bug_cockroach_huge",
            ["bug_cockroach_small"] = "characters/fakechars/bug_cockroach_small",
            ["centipede_hole"] = "characters/fakechars/centipede_hole",
            ["centipede_hole_2"] = "characters/fakechars/centipede_hole_2",
            ["crazy_villager_mininpc_01"] = "characters/fakechars/crazy_villager_mininpc_01",
            ["crazy_villager_mininpc_01_radiotower"] = "characters/fakechars/crazy_villager_mininpc_01_radiotower",
            ["crazy_villager_mininpc_02"] = "characters/fakechars/crazy_villager_mininpc_02",
            ["crazy_villager_mininpc_gridobj_rednecks_2_01"] = "characters/fakechars/crazy_villager_mininpc_gridobj_rednecks_2_01",
            ["crazy_villager_mininpc_onechance"] = "characters/fakechars/crazy_villager_mininpc_onechance",
            ["larva_big_01"] = "characters/fakechars/larva_big_01",
            ["nightworms_01"] = "characters/fakechars/nightworms_01",
            ["nightworms_02"] = "characters/fakechars/nightworms_02",
            ["pig_big_mutant"] = "characters/fakechars/pig_big_mutant",
            ["raven_dummy_01"] = "characters/fakechars/raven_dummy_01",
            ["worms_enemy_01"] = "characters/fakechars/worms_enemy_01",
            ["zombie_female_bathing"] = "characters/fakechars/zombie_female_bathing",
            ["zombie_male_sitting"] = "characters/fakechars/zombie_male_sitting",
            ["forestspirit_bunkerdream"] = "characters/forestspirit_bunkerdream",
            ["humanspider"] = "characters/humanspider",
            ["humanspiderminion"] = "characters/humanspiderminion",
            ["kamikaze"] = "characters/kamikaze",
            ["cripple_npc_preepilogue"] = "characters/locationchars/cripple_npc_preepilogue",
            ["dog_01_wolfmanhideout_01"] = "characters/locationchars/dog_01_wolfmanhideout_01",
            ["dog_02_wolfmanhideout_01"] = "characters/locationchars/dog_02_wolfmanhideout_01",
            ["dog_03_wolfmanhideout_01"] = "characters/locationchars/dog_03_wolfmanhideout_01",
            ["dog_doctor"] = "characters/locationchars/dog_doctor",
            ["dodger"] = "characters/notused/dodger",
            ["doppelganger"] = "characters/notused/doppelganger",
            ["robber"] = "characters/notused/robber",
            ["tank"] = "characters/notused/tank",
            ["villager_pistol"] = "characters/notused/villager_pistol",
            ["baba"] = "characters/npc/baba",
            ["bed_npc"] = "characters/npc/bed_npc",
            ["child"] = "characters/npc/child",
            ["cripple"] = "characters/npc/cripple",
            ["doctor_act2"] = "characters/npc/doctor_act2",
            ["doctor_confronted"] = "characters/npc/doctor_confronted",
            ["doctor_confronted2"] = "characters/npc/doctor_confronted2",
            ["doctor_follower"] = "characters/npc/doctor_follower",
            ["doctor_idle"] = "characters/npc/doctor_idle",
            ["doctor_trapset"] = "characters/npc/doctor_trapset",
            ["door_talkable_outside_bunker_underground_02"] = "characters/npc/door_talkable_outside_bunker_underground_02",
            ["elephants"] = "characters/npc/elephants",
            ["figurine_doctor"] = "characters/npc/figurine_doctor",
            ["istota_lv1"] = "characters/npc/istota_lv1",
            ["kapelusznik_dummy"] = "characters/npc/kapelusznik_dummy",
            ["kapelusznik_dummy_01"] = "characters/npc/kapelusznik_dummy_01",
            ["kapelusznik_dummy_02"] = "characters/npc/kapelusznik_dummy_02",
            ["maciek"] = "characters/npc/maciek",
            ["maciek_noflamethrower"] = "characters/npc/maciek_noflamethrower",
            ["mushroomgranny"] = "characters/npc/mushroomgranny",
            ["muzyk"] = "characters/npc/muzyk",
            ["muzyk_duzy"] = "characters/npc/muzyk_duzy",
            ["muzyk_huge"] = "characters/npc/muzyk_huge",
            ["muzyk_runaway"] = "characters/npc/muzyk_runaway",
            ["nighttrader"] = "characters/npc/nighttrader",
            ["pigshedlever"] = "characters/npc/pigshedlever",
            ["piotrek"] = "characters/npc/piotrek",
            ["porter"] = "characters/npc/porter",
            ["scarecrow"] = "characters/npc/scarecrow",
            ["shrine_03"] = "characters/npc/shrine_03",
            ["shrine_03_pigdead"] = "characters/npc/shrine_03_pigdead",
            ["shrine_mushroomgranny"] = "characters/npc/shrine_mushroomgranny",
            ["shrine_treevillage"] = "characters/npc/shrine_treevillage",
            ["snail"] = "characters/npc/snail",
            ["soldier_underground"] = "characters/npc/soldier_underground",
            ["soldier_underground_old"] = "characters/npc/soldier_underground_old",
            ["stove_npc"] = "characters/npc/stove_npc",
            ["talkingtree"] = "characters/npc/talkingtree",
            ["talkingtree_burned_roadtohome"] = "characters/npc/talkingtree_burned_roadtohome",
            ["talkingtree_darkside"] = "characters/npc/talkingtree_darkside",
            ["thethree"] = "characters/npc/thethree",
            ["tractor_muzyk"] = "characters/npc/tractor_muzyk",
            ["tree_dry_medium_deadpig_01_villageentrance"] = "characters/npc/tree_dry_medium_deadpig_01_villageentrance",
            ["tree_dry_medium_deadpig_01_villagersswamp"] = "characters/npc/tree_dry_medium_deadpig_01_villagersswamp",
            ["valve"] = "characters/npc/valve",
            ["wardrobe_npc"] = "characters/npc/wardrobe_npc",
            ["wolf"] = "characters/npc/wolf",
            ["wolfman_att"] = "characters/npc/wolfman_att",
            ["pig"] = "characters/pig",
            ["player_cutscene_soundtest"] = "characters/player_cutscene_soundtest",
            ["rabbit"] = "characters/rabbit",
            ["raven"] = "characters/raven",
            ["redneck"] = "characters/redneck",
            ["redneck02"] = "characters/redneck02",
            ["redneck03"] = "characters/redneck03",
            ["spider01"] = "characters/spider01",
            ["spider02"] = "characters/spider02",
            ["spider03_day"] = "characters/spider03_day",
            ["swamper1"] = "characters/swamper1",
            ["villager"] = "characters/villager",
            ["villager1_burning"] = "characters/villager1_burning",
            ["villager3_plank"] = "characters/villager3_plank",
            ["villagerpitchfork"] = "characters/villagerpitchfork",
            ["villager1granny"] = "characters/villagers_mushroom_granny/villager1granny",
            ["villager2granny"] = "characters/villagers_mushroom_granny/villager2granny",
            ["villager3granny"] = "characters/villagers_mushroom_granny/villager3granny",
            ["crazy_villager_cottageruin"] = "characters/villagers/crazy_villager_cottageruin",
            ["villager_bride_01"] = "characters/villagers/villager_bride_01",
            ["villager_infected1"] = "characters/villagers/villager_infected1",
            ["villager_infected1_ch2"] = "characters/villagers/villager_infected1_ch2",
            ["villager_infected1b_ch2"] = "characters/villagers/villager_infected1b_ch2",
            ["villager_infected2"] = "characters/villagers/villager_infected2",
            ["villager_infected2_ch2"] = "characters/villagers/villager_infected2_ch2",
            ["villager_infected3"] = "characters/villagers/villager_infected3",
            ["villager_infected3_ch2"] = "characters/villagers/villager_infected3_ch2",
            ["villager_infected4"] = "characters/villagers/villager_infected4",
            ["villager_normal1"] = "characters/villagers/villager_normal1",
            ["villager_normal10"] = "characters/villagers/villager_normal10",
            ["villager_normal11"] = "characters/villagers/villager_normal11",
            ["villager_normal12_babaplacze"] = "characters/villagers/villager_normal12_babaplacze",
            ["villager_normal2"] = "characters/villagers/villager_normal2",
            ["villager_normal3"] = "characters/villagers/villager_normal3",
            ["villager_normal4"] = "characters/villagers/villager_normal4",
            ["villager_normal5"] = "characters/villagers/villager_normal5",
            ["villager_normal6"] = "characters/villagers/villager_normal6",
            ["villager_normal7"] = "characters/villagers/villager_normal7",
            ["villager_normal9"] = "characters/villagers/villager_normal9",
            ["villagertorch"] = "characters/villagertorch"
        };

        private static readonly Dictionary<string, string> activeCharacters = new()
        {
            ["forestspirit_onechance_02"] = "characters/_test/forestspirit_onechance_02",
            ["villager3_plank_burning"] = "characters/_test/villager3_plank_burning",
            ["banshee"] = "characters/banshee",
            ["bansheebaby"] = "characters/bansheebaby",
            ["brat_babykury"] = "characters/brat_babykury",
            ["bride"] = "characters/bride",
            ["centipede"] = "characters/centipede",
            ["chicken"] = "characters/chicken",
            ["chomperblack"] = "characters/chomperblack",
            ["chomperbride"] = "characters/chomperbride",
            ["chomperhalf"] = "characters/chomperhalf",
            ["chomperred"] = "characters/chomperred",
            ["chomperred_small"] = "characters/chomperred_small",
            ["deer"] = "characters/deer",
            ["dog"] = "characters/dog",
            ["dogmutated"] = "characters/dogmutated",
            ["nightworms_02"] = "characters/fakechars/nightworms_02",
            ["worms_enemy_01"] = "characters/fakechars/worms_enemy_01",
            ["forestspirit_bunkerdream"] = "characters/forestspirit_bunkerdream",
            ["humanspider"] = "characters/humanspider",
            ["humanspiderminion"] = "characters/humanspiderminion",
            ["kamikaze"] = "characters/kamikaze",
            ["dodger"] = "characters/notused/dodger",
            ["doppelganger"] = "characters/notused/doppelganger",
            ["robber"] = "characters/notused/robber",
            ["tank"] = "characters/notused/tank",
            ["villager_pistol"] = "characters/notused/villager_pistol",
            ["doctor_follower"] = "characters/npc/doctor_follower",
            ["istota_lv1"] = "characters/npc/istota_lv1",
            ["muzyk_runaway"] = "characters/npc/muzyk_runaway",
            ["pig"] = "characters/pig",
            ["rabbit"] = "characters/rabbit",
            ["raven"] = "characters/raven",
            ["redneck"] = "characters/redneck",
            ["redneck02"] = "characters/redneck02",
            ["redneck03"] = "characters/redneck03",
            ["spider02"] = "characters/spider02",
            ["spider03_day"] = "characters/spider03_day",
            ["swamper1"] = "characters/swamper1",
            ["villager"] = "characters/villager",
            ["villager1_burning"] = "characters/villager1_burning",
            ["villager3_plank"] = "characters/villager3_plank",
            ["villagerpitchfork"] = "characters/villagerpitchfork",
            ["villagertorch"] = "characters/villagertorch"
        };

        private static readonly Dictionary<string, string> specialCharacters = new()
        {
            ["larva_big_01"] = "characters/fakechars/larva_big_01",
            ["pig_big_mutant"] = "characters/fakechars/pig_big_mutant", // The Sow
            ["istota_lv1"] = "characters/npc/istota_lv1", // The Being
            ["muzyk_huge"] = "characters/npc/muzyk_huge",
            ["talkingtree"] = "characters/npc/talkingtree", // Ch2 Big tree
            ["talkingtree_burned_roadtohome"] = "characters/npc/talkingtree_burned_roadtohome", // Ch2 Big tree
            ["talkingtree_darkside"] = "characters/npc/talkingtree_darkside" // Ch2 Big tree
        };

        private static readonly Dictionary<string, string> staticCharacters = new()
        {
            ["character_dummy_02"] = "characters/_test/character_dummy_02",
            ["clone_villager_01"] = "characters/clones/clone_villager_01",
            ["antagonistact2lv4_epilog"] = "characters/epilogue/antagonistact2lv4_epilog",
            ["epilog_dozorca"] = "characters/epilogue/epilog_dozorca",
            ["epilog_man_burning_1"] = "characters/epilogue/epilog_man_burning_1",
            ["epilog_man_burning_2"] = "characters/epilogue/epilog_man_burning_2",
            ["epilog_man_burning_3"] = "characters/epilogue/epilog_man_burning_3",
            ["epilog_man_crawl_1"] = "characters/epilogue/epilog_man_crawl_1",
            ["epilog_man_crawl_2"] = "characters/epilogue/epilog_man_crawl_2",
            ["epilog_man_crawl_3"] = "characters/epilogue/epilog_man_crawl_3",
            ["epilog_man_idle_1"] = "characters/epilogue/epilog_man_idle_1",
            ["epilog_man_idle_1_burned"] = "characters/epilogue/epilog_man_idle_1_burned",
            ["epilog_man_idle_1_first"] = "characters/epilogue/epilog_man_idle_1_first",
            ["epilog_man_idle_10_standing"] = "characters/epilogue/epilog_man_idle_10_standing",
            ["epilog_man_idle_11_crawl"] = "characters/epilogue/epilog_man_idle_11_crawl",
            ["epilog_man_idle_2_burned"] = "characters/epilogue/epilog_man_idle_2_burned",
            ["epilog_man_idle_2_burned_randomizer"] = "characters/epilogue/epilog_man_idle_2_burned_randomizer",
            ["epilog_man_idle_3"] = "characters/epilogue/epilog_man_idle_3",
            ["epilog_man_idle_3_burned"] = "characters/epilogue/epilog_man_idle_3_burned",
            ["epilog_man_idle_3_first"] = "characters/epilogue/epilog_man_idle_3_first",
            ["epilog_man_idle_4"] = "characters/epilogue/epilog_man_idle_4",
            ["epilog_man_idle_5_standing"] = "characters/epilogue/epilog_man_idle_5_standing",
            ["epilog_man_idle_6"] = "characters/epilogue/epilog_man_idle_6",
            ["epilog_man_idle_7_wakeup"] = "characters/epilogue/epilog_man_idle_7_wakeup",
            ["epilog_man_idle_8_wakeup"] = "characters/epilogue/epilog_man_idle_8_wakeup",
            ["epilog_man_idle_9_wakeup"] = "characters/epilogue/epilog_man_idle_9_wakeup",
            ["epilog_man_villager_1_burned"] = "characters/epilogue/epilog_man_villager_1_burned",
            ["epilog_villager_idle_1"] = "characters/epilogue/epilog_villager_idle_1",
            ["epilog_villager_idle_2"] = "characters/epilogue/epilog_villager_idle_2",
            ["epilog_villager_idle_3"] = "characters/epilogue/epilog_villager_idle_3",
            ["areabird"] = "characters/fakechars/areabird",
            ["banshee_cry"] = "characters/fakechars/banshee_cry",
            ["banshee_fake"] = "characters/fakechars/banshee_fake",
            ["bug_cockroach_big"] = "characters/fakechars/bug_cockroach_big",
            ["bug_cockroach_huge"] = "characters/fakechars/bug_cockroach_huge",
            ["bug_cockroach_small"] = "characters/fakechars/bug_cockroach_small",
            ["centipede_hole"] = "characters/fakechars/centipede_hole",
            ["centipede_hole_2"] = "characters/fakechars/centipede_hole_2",
            ["crazy_villager_mininpc_01"] = "characters/fakechars/crazy_villager_mininpc_01",
            ["crazy_villager_mininpc_01_radiotower"] = "characters/fakechars/crazy_villager_mininpc_01_radiotower",
            ["crazy_villager_mininpc_02"] = "characters/fakechars/crazy_villager_mininpc_02",
            ["crazy_villager_mininpc_gridobj_rednecks_2_01"] = "characters/fakechars/crazy_villager_mininpc_gridobj_rednecks_2_01",
            ["crazy_villager_mininpc_onechance"] = "characters/fakechars/crazy_villager_mininpc_onechance",
            ["raven_dummy_01"] = "characters/fakechars/raven_dummy_01",
            ["zombie_female_bathing"] = "characters/fakechars/zombie_female_bathing",
            ["zombie_male_sitting"] = "characters/fakechars/zombie_male_sitting",
            ["cripple_npc_preepilogue"] = "characters/locationchars/cripple_npc_preepilogue",
            ["dog_01_wolfmanhideout_01"] = "characters/locationchars/dog_01_wolfmanhideout_01",
            ["dog_02_wolfmanhideout_01"] = "characters/locationchars/dog_02_wolfmanhideout_01",
            ["dog_03_wolfmanhideout_01"] = "characters/locationchars/dog_03_wolfmanhideout_01",
            ["dog_doctor"] = "characters/locationchars/dog_doctor",
            ["istota_lv1"] = "characters/npc/istota_lv1",
            ["kapelusznik_dummy"] = "characters/npc/kapelusznik_dummy",
            ["kapelusznik_dummy_01"] = "characters/npc/kapelusznik_dummy_01",
            ["kapelusznik_dummy_02"] = "characters/npc/kapelusznik_dummy_02",
            ["maciek_noflamethrower"] = "characters/npc/maciek_noflamethrower",
            ["player_cutscene_soundtest"] = "characters/player_cutscene_soundtest",
            ["spider01"] = "characters/spider01",
            ["villager1granny"] = "characters/villagers_mushroom_granny/villager1granny",
            ["villager2granny"] = "characters/villagers_mushroom_granny/villager2granny",
            ["villager3granny"] = "characters/villagers_mushroom_granny/villager3granny",
            ["crazy_villager_cottageruin"] = "characters/villagers/crazy_villager_cottageruin",
            ["villager_bride_01"] = "characters/villagers/villager_bride_01",
            ["villager_infected1"] = "characters/villagers/villager_infected1",
            ["villager_infected1_ch2"] = "characters/villagers/villager_infected1_ch2",
            ["villager_infected1b_ch2"] = "characters/villagers/villager_infected1b_ch2",
            ["villager_infected2"] = "characters/villagers/villager_infected2",
            ["villager_infected2_ch2"] = "characters/villagers/villager_infected2_ch2",
            ["villager_infected3"] = "characters/villagers/villager_infected3",
            ["villager_infected3_ch2"] = "characters/villagers/villager_infected3_ch2",
            ["villager_infected4"] = "characters/villagers/villager_infected4",
            ["villager_normal1"] = "characters/villagers/villager_normal1",
            ["villager_normal10"] = "characters/villagers/villager_normal10",
            ["villager_normal11"] = "characters/villagers/villager_normal11",
            ["villager_normal12_babaplacze"] = "characters/villagers/villager_normal12_babaplacze",
            ["villager_normal2"] = "characters/villagers/villager_normal2",
            ["villager_normal3"] = "characters/villagers/villager_normal3",
            ["villager_normal4"] = "characters/villagers/villager_normal4",
            ["villager_normal5"] = "characters/villagers/villager_normal5",
            ["villager_normal6"] = "characters/villagers/villager_normal6",
            ["villager_normal7"] = "characters/villagers/villager_normal7",
            ["villager_normal9"] = "characters/villagers/villager_normal9",
        };

        private static readonly Dictionary<string, string> npcCharactersCh1 = new()
        {
            ["baba"] = "characters/npc/baba",
            ["doctor_confronted"] = "characters/npc/doctor_confronted",
            ["doctor_confronted2"] = "characters/npc/doctor_confronted2",
            ["doctor_idle"] = "characters/npc/doctor_idle",
            ["doctor_trapset"] = "characters/npc/doctor_trapset",
            ["door_talkable_outside_bunker_underground_02"] = "characters/npc/door_talkable_outside_bunker_underground_02",
            ["muzyk"] = "characters/npc/muzyk",
            ["nighttrader"] = "characters/npc/nighttrader",
            ["pigshedlever"] = "characters/npc/pigshedlever",
            ["piotrek"] = "characters/npc/piotrek",
            ["porter"] = "characters/npc/porter",
            ["shrine_03"] = "characters/npc/shrine_03",
            ["shrine_03_pigdead"] = "characters/npc/shrine_03_pigdead",
            ["soldier_underground"] = "characters/npc/soldier_underground",
            ["soldier_underground_old"] = "characters/npc/soldier_underground_old",
            ["tractor_muzyk"] = "characters/npc/tractor_muzyk",
            ["tree_dry_medium_deadpig_01_villageentrance"] = "characters/npc/tree_dry_medium_deadpig_01_villageentrance",
            ["wolf"] = "characters/npc/wolf",
            ["wolfman_att"] = "characters/npc/wolfman_att",
        };

        private static readonly Dictionary<string, string> npcCharactersCh2 = new()
        {
            ["child"] = "characters/npc/child",
            ["cripple"] = "characters/npc/cripple",
            ["doctor_act2"] = "characters/npc/doctor_act2",
            ["elephants"] = "characters/npc/elephants",
            ["figurine_doctor"] = "characters/npc/figurine_doctor",
            ["mushroomgranny"] = "characters/npc/mushroomgranny",
            ["muzyk_duzy"] = "characters/npc/muzyk_duzy",
            ["shrine_mushroomgranny"] = "characters/npc/shrine_mushroomgranny",
            ["shrine_treevillage"] = "characters/npc/shrine_treevillage",
            ["snail"] = "characters/npc/snail",
            ["thethree"] = "characters/npc/thethree",
            ["tree_dry_medium_deadpig_01_villagersswamp"] = "characters/npc/tree_dry_medium_deadpig_01_villagersswamp",
            ["valve"] = "characters/npc/valve",
        };

        private static readonly Dictionary<string, string> npcCharactersEpilogue = new()
        {
            ["bed_npc"] = "characters/npc/bed_npc",
            ["maciek"] = "characters/npc/maciek",
            ["scarecrow"] = "characters/npc/scarecrow",
            ["stove_npc"] = "characters/npc/stove_npc",
            ["wardrobe_npc"] = "characters/npc/wardrobe_npc",
        };


        // Test spawn characters
        private static int Index = 0;
        [HarmonyPatch(typeof(Player), "onInstantClick")]
        [HarmonyPostfix]
        internal static void SpawnCharacterTest(Player __instance)
        {
            if (!(bool)AccessTools.Field(typeof(Player), "rmbDown").GetValue(__instance))
                return;

            GameObject obj = Core.AddPrefab(allCharacters.Values.ToArray()[Index], __instance.transform.position + new Vector3(100, 0, 0), Quaternion.Euler(90f, 0f, 0f), Singleton<WorldGenerator>.Instance.gameObject, true);
            Index++;
            DarkwoodRandomizerPlugin.Logger.LogInfo($"Spawning Index {Index}: {allCharacters.Values.ToArray()[Index]}, Immobile: {obj.GetComponent<Character>().immobile}");
        }


        [HarmonyPatch(typeof(WorldChunk), "spawnFreeRoamingCharacters")]
        [HarmonyPrefix]
        internal static bool RandomizeFreeRoamingCharacters(WorldChunk __instance, GameObject ___CharactersFreeRoaming, List<GameObject> ___freeRoamingChars)
        {
            if (!SettingsManager.Enemies_RandomizeFreeRoamingEnemies!.Value)
                return true;
            if (__instance.isBorderChunk)
                return false;

            int num;
            for (int i = 0; i < __instance.biome.characters.Count; i = num + 1)
            {
                CharacterToSpawn charToSpawn = __instance.biome.characters[i];
                if (charToSpawn != null && charToSpawn.character != null && !charToSpawn.useCharacterSpawnPoint)
                {
                    for (int j = 0; j < charToSpawn.amount; j = num + 1)
                    {
                        Vector3 pointWithinBounds = (Vector3)AccessTools.Method(typeof(WorldChunk), "getPointWithinBounds").Invoke(__instance, [charToSpawn.minimumDistanceToHideout, 1]);
                        if (pointWithinBounds != Vector3.zero)
                        {
                            // Injection
                            Character component = Core.AddPrefab(activeCharacters.Values.RandomItem(), pointWithinBounds, Quaternion.Euler(90f, 0f, 0f), ___CharactersFreeRoaming, true).GetComponent<Character>();
                            // End injection
                            if (component != null)
                            {
                                component.noWaypoints = true;
                                ___freeRoamingChars.Add(component.gameObject);
                                Core.addToSaveable(component.gameObject, true, true);
                                Singleton<WorldGrid>.Instance.registerToNode(component.gameObject, WorldGrid.Cullable.Type.moveable);
                                component.gameObject.SetActive(false);
                                if (component.sleeping)
                                {
                                    component.returnToSleepAfterLosingTarget = true;
                                }
                            }
                        }
                        num = j;
                    }
                }
                num = i;
            }

            return false;
        }


        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPostfix]
        internal static void RandomizeLocationCharacters(WorldGenerator __instance)
        {
            // TODO: fix this not randomizing all NPCs
            //if (Settings.Enemies_RandomizeNPCs!.Value)
            //{
            //    List<NPC> npcPool = new();
            //    foreach (Location location in __instance.locations)
            //        foreach (NPC npc in location.gameObject.GetComponentsInChildren<NPC>())
            //            npcPool.Add(npc);

            //    DarkwoodRandomizerPlugin.Logger.LogInfo($"NPC pool: {string.Join(", ", npcPool.Select(npc => npc.name))}");

            //    foreach (Location location in __instance.locations)
            //        foreach (NPC npc in location.gameObject.GetComponentsInChildren<NPC>())
            //        {
            //            NPC newNPC = npcPool[UnityEngine.Random.Range(0, npcPool.Count)];
            //            npcPool.Remove(newNPC);

            //            DarkwoodRandomizerPlugin.Logger.LogInfo($"Old: {npc.name}, New: {newNPC.name}");

            //            npc.name = newNPC.name;
            //            npc.inventory = newNPC.inventory;
            //            npc.characterDialogue = newNPC.characterDialogue;
            //        }
            //}

            
            Utils.RunWhenPredicateMet
            (
                predicate: () => Locations.OutsideLocationsLoaded,
                action: RandomizeLocationCharacters
            );


            static void RandomizeLocationCharacters()
            {
                foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                {
                    foreach (Character character in location.charactersList.ToArray())
                    {
                        List<string>? characterPool = null;

                        if (specialCharacters.Keys.Contains(character.name)) // Huge characters that may block paths if placed in a different location
                            continue;
                        else if (SettingsManager.Enemies_RandomizeLocationEnemies!.Value && character.npc == null && !character.immobile)
                            characterPool = activeCharacters.Values.ToList();
                        else if (SettingsManager.Enemies_RandomizeStaticCharacters!.Value && character.npc == null && character.immobile)
                            characterPool = staticCharacters.Values.ToList();

                        if (characterPool == null)
                            continue;

                        location.charactersList.Remove(character);
                        Object.Destroy(character.gameObject);

                        Character component = Core.AddPrefab(characterPool.RandomItem(), character.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false).GetComponent<Character>();
                        location.charactersList.Add(component);
                    }

                    foreach (CharacterSpawnPoint characterSpawnPoint in location.spawnPoints.ToArray())
                    {
                        if (characterSpawnPoint.spawnedCharacter == null)
                            continue;

                        List<string>? characterPool = null;

                        if (specialCharacters.Keys.Contains(characterSpawnPoint.spawnedCharacter.name)) // Huge characters that may block paths if placed in a different location
                            continue;
                        else if (SettingsManager.Enemies_RandomizeLocationEnemies!.Value && characterSpawnPoint.spawnedCharacter.npc == null && !characterSpawnPoint.spawnedCharacter.immobile)
                            characterPool = activeCharacters.Values.ToList();
                        else if (SettingsManager.Enemies_RandomizeStaticCharacters!.Value && characterSpawnPoint.spawnedCharacter.npc == null && characterSpawnPoint.spawnedCharacter.immobile)
                            characterPool = staticCharacters.Values.ToList();

                        if (characterPool == null)
                            continue;

                        location.spawnPoints.Remove(characterSpawnPoint);
                        Object.Destroy(characterSpawnPoint.gameObject);

                        Character component = Core.AddPrefab(characterPool.RandomItem(), characterSpawnPoint.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false).GetComponent<Character>();
                        location.charactersList.Add(component);
                    }
                }
            }
        }
    }
}
