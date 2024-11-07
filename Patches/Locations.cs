﻿using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Locations
    {
        // Need to recheck
        // Some are missing like village entrance, zone connections, road to doctor etc

        // "med_cottage_tree_01" beginner hideout
        internal static List<string> HideoutsCh1 = ["med_cottage_tree_01", "big_farm_02", "big_hideout_03"];

        internal static List<string> HideoutsCh2 = ["med_hideout_04", "med_hideout_05"];

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

        internal static List<string> OutsideLocationsCh1 =
            ["outside_bunker_underground_02", "outside_church_underground_01", "outside_church_underground_02",
            "outside_doctor_house_01", "outside_village_ch1_01", "outside_village_ch1_cottage01_underground_01", "outside_well_underground_01"];


        internal static bool OutsideLocationsLoaded =>
            OutsideLocationsCh1.Count == Singleton<OutsideLocations>.Instance.spawnedLocations.Count &&
            !Singleton<OutsideLocations>.Instance.playerInOutsideLocation;


        // Simply calling OutsideLocations.createLocation doesn't work, so we have to visit them one by one
        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPostfix]
        internal static void PreloadOutsideLocations()
        {
            if (OutsideLocationsLoaded)
                return;



            // Generate outside_bunker_underground_02 last to remove issues with leaving nested locations
            foreach (string locationName in OutsideLocationsCh1.Except(["outside_bunker_underground_02"]))
                Utils.RunWhenPredicateMet
                (
                    predicate: () => !Singleton<OutsideLocations>.Instance.loading,
                    action: () => Singleton<OutsideLocations>.Instance.prepareLocation(locationName),
                    exclusive: true
                );

            Utils.RunWhenPredicateMet
            (
                predicate: () => OutsideLocationsCh1.Count - 1 == Singleton<OutsideLocations>.Instance.spawnedLocations.Count && !Singleton<OutsideLocations>.Instance.loading,
                action: () => Singleton<OutsideLocations>.Instance.prepareLocation("outside_bunker_underground_02"),
                exclusive: true
            );

            Utils.RunWhenPredicateMet
            (
                predicate: () => OutsideLocationsCh1.Count == Singleton<OutsideLocations>.Instance.spawnedLocations.Count && !Singleton<OutsideLocations>.Instance.loading,
                action: () =>
                {
                    GameEvents? component =
                        Singleton<OutsideLocations>.Instance
                        .spawnedLocations[Singleton<OutsideLocations>.Instance.currentLocationName]
                        .gameObject
                        .GetComponentsInChildren<GameEvents>()
                        .FirstOrDefault(x => x.events.Any(evnt => evnt.type == GameEvent.Type.returnToWorld))
                        ?.GetComponent<GameEvents>();
                    component?.fire();

                    foreach (Location location in Singleton<OutsideLocations>.Instance.spawnedLocations.Values)
                    {
                        string locationName = location.name.Replace("_done", "");

                        if (locationName == "outside_village_ch1_01" || locationName == "outside_village_ch1_cottage01_underground_01" || locationName == "outside_well_underground_01")
                            location.biomeType = Biome.Type.forest;
                        else if (locationName == "outside_bunker_underground_02" || locationName == "outside_church_underground_01" || locationName == "outside_church_underground_02" || locationName == "outside_doctor_house_01")
                            location.biomeType = Biome.Type.forest_mutated;
                        else
                            location.biomeType = Biome.Type.swamp;
                    }
                },
                exclusive: true
            );
        }



        private static List<string> locationsAlreadySpawned = new();
        // Randomize location position
        [HarmonyPatch(typeof(WorldChunk), "populate")]
        [HarmonyPrefix]
        internal static void RandomizeLocations(WorldChunk __instance)
        {
            if (string.IsNullOrEmpty(__instance.locationName)) // Empty chunk
                return;
            if (__instance.isBorderChunk) // Don't want to clip locations into the wall
                return;
            if (!SettingsManager.Locations_RandomizeLocationPosition!.Value && MustSpawnCh1.Contains(__instance.locationName))
                return;
            if (!SettingsManager.Locations_RandomizeHideoutPosition!.Value && HideoutsCh1.Contains(__instance.locationName))
                return;


            List<string> availableToSpawn = new();

            if (SettingsManager.Locations_RandomizeLocationPosition.Value)
                availableToSpawn = availableToSpawn.Concat(MustSpawnCh1).ToList();
            if (SettingsManager.Locations_RandomizeHideoutPosition.Value)
                availableToSpawn = availableToSpawn.Concat(HideoutsCh1).ToList();

            availableToSpawn = availableToSpawn.Except(locationsAlreadySpawned).ToList();

            if (availableToSpawn.Count == 0)
                return;

            __instance.locationName = availableToSpawn.RandomItem();
            locationsAlreadySpawned.Add(__instance.locationName);
        }

        // Randomizes location rotation
        // Also makes vaulting a little scuffed
        // Does not affect border locations
        [HarmonyPatch(typeof(GameObject), "SetActive")]
        [HarmonyPrefix]
        internal static void RandomizeLocationRotation(GameObject __instance)
        {
            if (SettingsManager.Locations_RandomizeHideoutRotation!.Value && HideoutsCh1.Contains(__instance.name.Replace("_done", "")))
                __instance.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);

            if (SettingsManager.Locations_RandomizeLocationRotation!.Value && MustSpawnCh1.Concat(OutsideLocationsCh1).Contains(__instance.name.Replace("_done", "")))
                __instance.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);
        }
    }
}
