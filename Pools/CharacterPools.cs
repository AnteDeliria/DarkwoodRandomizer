using DarkwoodRandomizer.Plugin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DarkwoodRandomizer.Pools
{
    internal static class CharacterPools
    {
        private static readonly string CharacterPoolsDirectory = Path.Combine(DarkwoodRandomizerPlugin.PluginPath, "CharacterPools");

        internal static IEnumerable<string> PoolNames =
            [nameof(ALL_CHARACTERS), nameof(ACTIVE_CHARACTERS), nameof(STATIC_CHARACTERS), nameof(COLOSSAL_CHARACTERS),
                nameof(NPC_CHARACTERS_CH1), nameof(NPC_CHARACTERS_CH2), nameof(NPC_CHARACTERS_EPILOGUE)];


        internal static List<string>? LocationActiveCharactersDryMeadow => GetPoolFromFile(nameof(LocationActiveCharactersDryMeadow));
        internal static List<string>? LocationActiveCharactersSilentForest => GetPoolFromFile(nameof(LocationActiveCharactersSilentForest));
        internal static List<string>? LocationActiveCharactersOldWoods => GetPoolFromFile(nameof(LocationActiveCharactersOldWoods));
        internal static List<string>? LocationActiveCharactersSwamp => GetPoolFromFile(nameof(LocationActiveCharactersSwamp));

        internal static List<string>? LocationStaticCharactersDryMeadow => GetPoolFromFile(nameof(LocationStaticCharactersDryMeadow));
        internal static List<string>? LocationStaticCharactersSilentForest => GetPoolFromFile(nameof(LocationStaticCharactersSilentForest));
        internal static List<string>? LocationStaticCharactersOldWoods => GetPoolFromFile(nameof(LocationStaticCharactersOldWoods));
        internal static List<string>? LocationStaticCharactersSwamp => GetPoolFromFile(nameof(LocationStaticCharactersSwamp));

        internal static List<string>? GlobalCharactersDryMeadow => GetPoolFromFile(nameof(GlobalCharactersDryMeadow));
        internal static List<string>? GlobalCharactersSilentForest => GetPoolFromFile(nameof(GlobalCharactersSilentForest));
        internal static List<string>? GlobalCharactersOldWoods => GetPoolFromFile(nameof(GlobalCharactersOldWoods));
        internal static List<string>? GlobalCharactersSwamp => GetPoolFromFile(nameof(GlobalCharactersSwamp));

        internal static List<string>? NightCharactersDryMeadow => GetPoolFromFile(nameof(NightCharactersDryMeadow));
        internal static List<string>? NightCharactersSilentForest => GetPoolFromFile(nameof(NightCharactersSilentForest));
        internal static List<string>? NightCharactersOldWoods => GetPoolFromFile(nameof(NightCharactersOldWoods));
        internal static List<string>? NightCharactersSwamp => GetPoolFromFile(nameof(NightCharactersSwamp));



        private static List<string>? GetPoolFromFile(string poolName)
        {
            string path = Path.Combine(CharacterPoolsDirectory, $"{poolName}.txt");
            if (!File.Exists(path))
                return null;

            StreamReader reader = new StreamReader(path);
            List<string> items = new();
            while (!reader.EndOfStream)
            {
                string[] tokens = reader.ReadLine().Trim().Split();
                if (tokens.Length == 0)
                    continue;

                if (PoolNames.Contains(tokens[0]))
                {
                    if (tokens.Length > 1 && int.TryParse(tokens[1], out int timesToAdd))
                    {
                        for (int i = 0; i < timesToAdd; i++)
                        {
                            Dictionary<string, string>? pool = typeof(CharacterPools).GetField(tokens[0], BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as Dictionary<string, string>;
                            if (pool != null)
                                items.AddRange(pool.Keys);
                        }
                    }
                    else
                    {
                        Dictionary<string, string>? pool = typeof(CharacterPools).GetField(tokens[0], BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as Dictionary<string, string>;
                        if (pool != null)
                            items.AddRange(pool.Keys);
                    }
                }
                else if (ALL_CHARACTERS.ContainsKey(tokens[0]))
                {
                    if (tokens.Length > 1 && int.TryParse(tokens[1], out int timesToAdd))
                        for (int i = 0; i < timesToAdd; i++)
                            items.Add(tokens[0]);
                    else
                        items.Add(tokens[0]);
                }
            }
            reader.Close();

            if (items.Count == 0)
                return null;
            else
                return items;
        }

        internal static IEnumerable<string>? GetLocationActivePathsForBiome(Biome.Type biome)
        {
            List<string>? pool = biome switch
            {
                Biome.Type.meadow => LocationActiveCharactersDryMeadow,
                Biome.Type.forest => LocationActiveCharactersSilentForest,
                Biome.Type.forest_mutated => LocationActiveCharactersOldWoods,
                Biome.Type.swamp => LocationActiveCharactersSwamp,
                _ => null,
            };

            return pool?.Select(x => ALL_CHARACTERS[x]);
        }

        internal static IEnumerable<string>? GetLocationStaticPathsForBiome(Biome.Type biome)
        {
            List<string>? pool = biome switch
            {
                Biome.Type.meadow => LocationStaticCharactersDryMeadow,
                Biome.Type.forest => LocationStaticCharactersSilentForest,
                Biome.Type.forest_mutated => LocationStaticCharactersOldWoods,
                Biome.Type.swamp => LocationStaticCharactersSwamp,
                _ => null,
            };

            return pool?.Select(x => ALL_CHARACTERS[x]);
        }

        internal static IEnumerable<string>? GetGlobalCharacterPathsForBiome(Biome.Type biome)
        {
            List<string>? pool = biome switch
            {
                Biome.Type.meadow => GlobalCharactersDryMeadow,
                Biome.Type.forest => GlobalCharactersSilentForest,
                Biome.Type.forest_mutated => GlobalCharactersOldWoods,
                Biome.Type.swamp => GlobalCharactersSwamp,
                _ => null,
            };

            return pool?.Select(x => ALL_CHARACTERS[x]);
        }

        internal static IEnumerable<string>? GetNightPathsForBiome(Biome.Type biome)
        {
            List<string>? pool = biome switch
            {
                Biome.Type.meadow => NightCharactersDryMeadow,
                Biome.Type.forest => NightCharactersSilentForest,
                Biome.Type.forest_mutated => NightCharactersOldWoods,
                Biome.Type.swamp => NightCharactersSwamp,
                _ => null,
            };

            return pool?.Select(x => ALL_CHARACTERS[x]);
        }

        internal static readonly Dictionary<string, string> ALL_CHARACTERS = new()
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

        internal static readonly Dictionary<string, string> NON_BROKEN_CHARACTERS = new()
        {
            ["character_dummy_02"] = "characters/_test/character_dummy_02",
            ["forestspirit_onechance_02"] = "characters/_test/forestspirit_onechance_02",
            ["villager3_plank_burning"] = "characters/_test/villager3_plank_burning",
            ["antagonistact2lv4"] = "characters/antagonistact2lv4",
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
            ["crazy_villager_mininpc_01"] = "characters/fakechars/crazy_villager_mininpc_01",
            ["crazy_villager_mininpc_01_radiotower"] = "characters/fakechars/crazy_villager_mininpc_01_radiotower",
            ["crazy_villager_mininpc_02"] = "characters/fakechars/crazy_villager_mininpc_02",
            ["crazy_villager_mininpc_gridobj_rednecks_2_01"] = "characters/fakechars/crazy_villager_mininpc_gridobj_rednecks_2_01",
            ["crazy_villager_mininpc_onechance"] = "characters/fakechars/crazy_villager_mininpc_onechance",
            ["dog_cottagetrailer"] = "characters/fakechars/dog_cottagetrailer",
            ["larva_big_01"] = "characters/fakechars/larva_big_01",
            ["nightworms_01"] = "characters/fakechars/nightworms_01",
            ["nightworms_02"] = "characters/fakechars/nightworms_02",
            ["pig_big_mutant"] = "characters/fakechars/pig_big_mutant",
            ["priest_mininpc_01"] = "characters/fakechars/priest_mininpc_01",
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
            ["redneckbury"] = "characters/locationchars/redneckbury",
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
            ["villager_normal8"] = "characters/villagers/villager_normal8",
            ["villager_normal9"] = "characters/villagers/villager_normal9",
            ["villagertorch"] = "characters/villagertorch"
        };

        internal static readonly Dictionary<string, string> ACTIVE_CHARACTERS = new()
        {
            ["forestspirit_onechance_02"] = "characters/_test/forestspirit_onechance_02",
            ["villager3_plank_burning"] = "characters/_test/villager3_plank_burning",
            ["antagonistact2lv4"] = "characters/antagonistact2lv4",
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
            ["dog_cottagetrailer"] = "characters/fakechars/dog_cottagetrailer",
            ["nightworms_01"] = "characters/fakechars/nightworms_01",
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

        internal static readonly Dictionary<string, string> COLOSSAL_CHARACTERS = new()
        {
            ["larva_big_01"] = "characters/fakechars/larva_big_01", // Larva Ch2
            ["pig_big_mutant"] = "characters/fakechars/pig_big_mutant", // The Sow
            ["istota_lv1"] = "characters/npc/istota_lv1", // The Being
            ["muzyk_huge"] = "characters/npc/muzyk_huge", // Musician Ch2
            ["talkingtree"] = "characters/npc/talkingtree", // Ch2 Big tree
            ["talkingtree_burned_roadtohome"] = "characters/npc/talkingtree_burned_roadtohome", // Ch2 Big tree
            ["talkingtree_darkside"] = "characters/npc/talkingtree_darkside" // Ch2 Big tree
        };

        internal static readonly Dictionary<string, string> STATIC_CHARACTERS = new()
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
            ["priest_mininpc_01"] = "characters/fakechars/priest_mininpc_01",
            ["raven_dummy_01"] = "characters/fakechars/raven_dummy_01",
            ["zombie_female_bathing"] = "characters/fakechars/zombie_female_bathing",
            ["zombie_male_sitting"] = "characters/fakechars/zombie_male_sitting",
            ["cripple_npc_preepilogue"] = "characters/locationchars/cripple_npc_preepilogue",
            ["dog_01_wolfmanhideout_01"] = "characters/locationchars/dog_01_wolfmanhideout_01",
            ["dog_02_wolfmanhideout_01"] = "characters/locationchars/dog_02_wolfmanhideout_01",
            ["dog_03_wolfmanhideout_01"] = "characters/locationchars/dog_03_wolfmanhideout_01",
            ["dog_doctor"] = "characters/locationchars/dog_doctor",
            ["redneckbury"] = "characters/locationchars/redneckbury",
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
            ["villager_normal8"] = "characters/villagers/villager_normal8",
            ["villager_normal9"] = "characters/villagers/villager_normal9"
        };

        internal static readonly Dictionary<string, string> NPC_CHARACTERS_CH1 = new()
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
            ["wolfman_att"] = "characters/npc/wolfman_att"
        };

        internal static readonly Dictionary<string, string> NPC_CHARACTERS_CH2 = new()
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
            ["valve"] = "characters/npc/valve"
        };

        internal static readonly Dictionary<string, string> NPC_CHARACTERS_EPILOGUE = new()
        {
            ["bed_npc"] = "characters/npc/bed_npc",
            ["maciek"] = "characters/npc/maciek",
            ["scarecrow"] = "characters/npc/scarecrow",
            ["stove_npc"] = "characters/npc/stove_npc",
            ["wardrobe_npc"] = "characters/npc/wardrobe_npc"
        };
    }
}
