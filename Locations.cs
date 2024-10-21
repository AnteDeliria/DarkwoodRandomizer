using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace DarkwoodRandomizer
{
    internal static class Locations
    {
        // Need to recheck
        // Some are missing like village entrance, zone connections, road to doctor etc

        // "med_cottage_tree_01" beginner hideout
        internal static List<string> Hideouts = ["med_cottage_tree_01", "big_farm_02", "big_hideout_03", "med_hideout_04", "med_hideout_05"];

        internal static List<string> MustSpawnCh1 =
            ["big_burned_houses_01", "med_bunker_enter_01",
                "big_pigsheds_01", "big_piotrek_01", "med_musicianHideout_01", "big_church_ruins_01",
                "big_hunter_01", "med_musican_house_01"];

        internal static List<string> WolfCamps = ["med_wolfman_camp_01", "med_wolfman_camp_02"]; // Spawned separately

        internal static List<string> BorderLocationsCh1 = ["med_wolfmanHideout_01"]; // unfinished

        internal static List<string> MustSpawnCh2 =
            ["big_swamp_lake_02", "big_village_tree_01",
                "med_junkyard", "med_maskFamily_01", "med_mushroomGranny_01", "med_undergroundCh2Entrance_01",
                "big_baba_01", "big_farm_03", "big_pigmen_cottage_01", "big_trainWreck_01",
                "med_butcher_house_01", "med_cottage_dogs_01", "med_cottage_tree_02", "med_larva_01",
                "mini_bodies_01", "mini_burned_house_01", "mini_mutated_roots_01", "mini_mutated_roots_02",
                "mini_rocks_01", "mini_rocks_02", "mini_tank_wreck_01", "mini_tank_wreck_02", "mini_tent_camp"];

        internal static List<string> DreamPresets =
            ["dream_undergroundCh2_01", "dream_bunker_underground_01", "dream_church_ruins_01", "dream_doctor_01",
                "dream_doctor_02", "dream_grave_meadow", "dream_oneChance_01_2", "dream_village_cellar"];

        internal static List<string> Tutorial = ["dream_tutorial_00", "dream_tutorial_01"];

        internal static List<string> Epilog = ["epilog_part1b_corridor_dream", "epilog_part2_crater"];

        internal static List<string> InstancePresets =
            ["outside_bunker_underground_03", "outside_bunker_underground_part2_01", "outside_cottage_snail_01", "outside_oneChance_01",
                "outside_roadToHome_01", "outside_undergroundCh2_01", "outside_village_ch2_cellar_01", "outside_bunker_underground_02",
                "outside_church_underground_01", "outside_church_underground_02", "outside_doctor_house_01", "outside_village_ch1_01",
                "outside_village_ch1_cottage01_underground_01", "outside_well_underground_01"];

        internal static List<string> LoadedChTwoPresets =
            ["dream_village_cellar", "dream_church_ruins_01", "med_mushroomGranny_01", "outside_cottage_snail_01",
                "outside_village_ch2_cellar_01", "outside_well_underground_01"];

        internal static List<string> UnloadedPresets =
            ["big_baba_01", "big_trainWreck_01", "med_wolfmanHideout_01", "mini_rocks_02", "med_cottage_tree_02",
                "big_village_tree_01", "mini_burned_house_01", "med_cottage_dogs_01", "big_farm_03", "big_pigmen_cottage_01",
                "outside_bunker_underground_03", "outside_undergroundCh2_01", "dream_undergroundCh2_01", "mini_rocks_01",
                "med_undergroundCh2Entrance_01", "mini_tent_camp"];



        internal static List<string> GetAllOverworldLocations()
        {
            return MustSpawnCh1.Concat(WolfCamps).Concat(MustSpawnCh2).Concat(Hideouts).ToList();
        }

        internal static List<string> GetOverworldLocationPool(ConfigEntry<string> configEntry)
        {
            IEnumerable<string> locationStrings = configEntry.Value.Split(',').Select(x => x.Trim().ToLower());
            List<string> locationChoices = new();

            foreach (string location in locationStrings)
            {
                switch (location)
                {
                    case "chapter1":
                        locationChoices.AddRange(MustSpawnCh1);
                        break;
                    case "chapter2": // Locations are not added to build settings 
                        //foreach (string sceneName in MustSpawnCh2)
                        //    if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                        //    {
                        //        DarkwoodRandomizerPlugin.Logger.LogInfo("test");
                        //        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                        //    }
                        locationChoices.AddRange(MustSpawnCh2.Except(UnloadedPresets));
                        break;
                    case "wolf_camps":
                        locationChoices.AddRange(WolfCamps);
                        break;
                    case "hideouts":
                        locationChoices.AddRange(Hideouts);
                        break;
                }
            }

            if (locationChoices.Count == 0)
                return GetAllOverworldLocations();
            else
                return locationChoices;
        }
    }
}
