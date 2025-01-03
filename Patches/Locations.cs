﻿using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Locations
    {
        // Simply calling OutsideLocations.createLocation doesn't work, so we have to visit them one by one
        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPostfix]
        private static void PreloadOutsideLocations(WorldGenerator __instance)
        {
            if (!(Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh1 || Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh2))
                return;
            if (Plugin.Controller.OutsideLocationsLoaded)
                return;

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => !Singleton<OutsideLocations>.Instance.playerInOutsideLocation,
                action: () =>
                {
                    List<string> locationsToGenerate;
                    string LastLocation;

                    switch (__instance.chapterID)
                    {
                        case 1:
                            locationsToGenerate = LocationPools.OUTSIDE_LOCATIONS_CH1;
                            LastLocation = "outside_bunker_underground_02";
                            break;
                        case 2:
                            locationsToGenerate = LocationPools.OUTSIDE_LOCATIONS_CH2;
                            LastLocation = "outside_village_ch2_cellar_01";
                            break;
                        default:
                            return;
                    }

                    // Generate outside_bunker_underground_02 last to remove issues with leaving nested locations
                    foreach (string locationName in locationsToGenerate.Except([LastLocation]))
                        Plugin.Controller.RunWhenPredicateMet
                        (
                            predicate: () => !Singleton<OutsideLocations>.Instance.loading,
                            action: () => Singleton<OutsideLocations>.Instance.prepareLocation(locationName),
                            exclusive: true
                        );

                    Plugin.Controller.RunWhenPredicateMet
                    (
                        predicate: () => locationsToGenerate.Count - 1 == Singleton<OutsideLocations>.Instance.spawnedLocations.Count && !Singleton<OutsideLocations>.Instance.loading,
                        action: () => Singleton<OutsideLocations>.Instance.prepareLocation(LastLocation),
                        exclusive: true
                    );

                    Plugin.Controller.RunWhenPredicateMet
                    (
                        predicate: () => locationsToGenerate.Count == Singleton<OutsideLocations>.Instance.spawnedLocations.Count && !Singleton<OutsideLocations>.Instance.loading,
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

                            Plugin.Controller.OutsideLocationsLoaded = true;
                        },
                        exclusive: true
                    );
                }
            );
        }


        // Randomize location position
        [HarmonyPatch(typeof(WorldGenerator), "onPrepareTutorial")]
        [HarmonyPrefix]
        private static void RandomizeLocationPosition(WorldGenerator __instance)
        {
            if (!(Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh1 || Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh2))
                return;


            Dictionary<string, Biome.Type?> locationsToShuffle = new();

            foreach (WorldChunk worldChunk in __instance.worldChunks.Where(x => !x.isBorderChunk))
            {
                if (SettingsManager.Locations_RandomizeLocationPosition!.Value && LocationPools.NON_BORDER_LOCATIONS.Contains(worldChunk.locationName))
                {
                    locationsToShuffle.Add(worldChunk.locationName, null); // Shuffle to any biome
                    worldChunk.locationName = "";
                }
                
                if (SettingsManager.Locations_RandomizeHideoutPosition!.Value && LocationPools.HIDEOUTS.Contains(worldChunk.locationName))
                {
                    locationsToShuffle.Add(worldChunk.locationName, worldChunk.biome.type); // Shuffle to same biome
                    worldChunk.locationName = "";
                }
            }


            List<WorldChunk> chunkPool = __instance.worldChunks.Where(x => !x.isBorderChunk && x.biome.type != Biome.Type.empty).ToList();
            while (locationsToShuffle.Count > 0)
            {
                KeyValuePair<string, Biome.Type?> locationToShuffle = locationsToShuffle.RandomItem();
                string randomLocationName = locationToShuffle.Key;
                Biome.Type? randomLocationBiome = locationToShuffle.Value;

                WorldChunk randomWorldChunk;
                if (randomLocationBiome != null)
                    randomWorldChunk = chunkPool.Where(x => x.biome.type == randomLocationBiome).RandomItem();
                else
                    randomWorldChunk = chunkPool.RandomItem();

                if (LocationPools.HIDEOUTS.Contains(randomLocationName) || string.IsNullOrEmpty(randomWorldChunk.locationName)) // Hideouts can replace any location
                {
                    if (!string.IsNullOrEmpty(randomWorldChunk.locationName))
                        locationsToShuffle.Add(randomWorldChunk.locationName, randomWorldChunk.biome.type);

                    locationsToShuffle.Remove(randomLocationName);
                    randomWorldChunk.locationName = randomLocationName;
                }
            }

            Plugin.Controller.LocationPositionsRandomized = true;
        }



        // Randomizes location rotation
        // Does not affect border locations
        [HarmonyPatch(typeof(GameObject), "SetActive")]
        [HarmonyPrefix]
        private static void RandomizeLocationRotation(GameObject __instance)
        {
            if (!(Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh1 || Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh2))
                return;
            if (__instance.GetComponentInParent<WorldChunk>()?.isBorderChunk == true)
                return;

            string locationName = __instance.name.Replace("_done", "");

            if (SettingsManager.Locations_RandomizeHideoutRotation!.Value && LocationPools.HIDEOUTS_CH1.Concat(LocationPools.HIDEOUTS_CH2).Contains(locationName))
                __instance.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);

            if (SettingsManager.Locations_RandomizeLocationRotation!.Value && LocationPools.NON_BORDER_LOCATIONS_CH1.Concat(LocationPools.OUTSIDE_LOCATIONS_CH1).Concat(LocationPools.NON_BORDER_LOCATIONS_CH2.Concat(LocationPools.OUTSIDE_LOCATIONS_CH2)).Contains(locationName))
                __instance.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);
        }
    }
}
