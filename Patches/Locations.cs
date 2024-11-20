using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Locations
    {
        internal static List<string> HIDEOUTS_CH1 = ["med_cottage_tree_01", "big_farm_02", "big_hideout_03"];

        internal static List<string> HIDEOUTS_CH2 = ["med_hideout_04", "med_hideout_05"];

        internal static List<string> NON_BORDER_LOCATIONS_CH1 =
            ["big_farm_02", "med_bunker_enter_01", "big_hunter_01", "med_musican_house_01",
                "big_burned_houses_01", "big_hideout_03", "big_church_ruins_01", "big_piotrek_01",
                "med_musicianHideout_01", "big_pigsheds_01", "med_cottage_tree_01"];

        internal static List<string> NON_BORDER_LOCATIONS_CH2 =
            ["med_mushroomGranny_01", "med_hideout_05", "med_junkyard_01", "med_maskFamily_01", "big_swamp_lake_02"];

        internal static List<string> WOLF_CAMPS_CH1 = ["med_wolfman_camp_01", "med_wolfman_camp_02"];

        internal static List<string> BORDER_LOCATIONS_CH1 = ["med_wolfmanHideout_01"]; // unfinished

        internal static List<string> BORDER_LOCATIONS_CH2 = []; // unfinished

        internal static List<string> MustSpawnCh2 = // Remove
            ["big_swamp_lake_02", "big_village_tree_01",
                "med_junkyard", "med_maskFamily_01", "med_mushroomGranny_01", "med_undergroundCh2Entrance_01",
                "big_baba_01", "big_farm_03", "big_pigmen_cottage_01", "big_trainWreck_01",
                "med_butcher_house_01", "med_cottage_dogs_01", "med_cottage_tree_02", "med_larva_01",
                "mini_bodies_01", "mini_burned_house_01", "mini_mutated_roots_01", "mini_mutated_roots_02",
                "mini_rocks_01", "mini_rocks_02", "mini_tank_wreck_01", "mini_tank_wreck_02", "mini_tent_camp"];

        internal static List<string> DREAMS =
            ["dream_undergroundCh2_01", "dream_bunker_underground_01", "dream_church_ruins_01", "dream_doctor_01",
                "dream_doctor_02", "dream_grave_meadow", "dream_oneChance_01_2", "dream_village_cellar"];

        internal static List<string> TUTORIAL = ["dream_tutorial_00", "dream_tutorial_01"];

        internal static List<string> EPILOGUE = ["epilog_part1b_corridor_dream", "epilog_part2_crater"];

        internal static List<string> OUTSIDE_LOCATIONS_CH1 =
            ["outside_bunker_underground_02", "outside_church_underground_01", "outside_church_underground_02",
            "outside_doctor_house_01", "outside_village_ch1_01", "outside_village_ch1_cottage01_underground_01", "outside_well_underground_01"];

        internal static List<string> OUTSIDE_LOCATIONS_CH2 =
            ["outside_bunker_underground_03", "outside_cottage_snail_01", "outside_oneChance_01", "outside_roadToHome_01",
                "outside_undergroundCh2_01", "outside_village_ch2_cellar_01"];

        internal static string OUTSIDE_LOCATION_TRANSITION = "outside_bunker_underground_part2_01";




        // Simply calling OutsideLocations.createLocation doesn't work, so we have to visit them one by one
        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPostfix]
        private static void PreloadOutsideLocations(WorldGenerator __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (Plugin.Controller.OutsideLocationsLoaded)
                return;


            IEnumerable<string> locationsToGenerate;
            string LastLocation;
            
            switch (__instance.chapterID)
            {
                case 1:
                    locationsToGenerate = OUTSIDE_LOCATIONS_CH1;
                    LastLocation = "outside_bunker_underground_02";
                    break;
                case 2:
                    locationsToGenerate = OUTSIDE_LOCATIONS_CH2;
                    LastLocation = null; // TODO
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
                predicate: () => locationsToGenerate.Count() - 1 == Singleton<OutsideLocations>.Instance.spawnedLocations.Count && !Singleton<OutsideLocations>.Instance.loading,
                action: () => Singleton<OutsideLocations>.Instance.prepareLocation(LastLocation),
                exclusive: true
            );

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => locationsToGenerate.Count() == Singleton<OutsideLocations>.Instance.spawnedLocations.Count && !Singleton<OutsideLocations>.Instance.loading,
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
        private static void RandomizeLocationPosition(WorldChunk __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (string.IsNullOrEmpty(__instance.locationName)) // Empty chunk
                return;

            if (!SettingsManager.Locations_RandomizeLocationPosition!.Value && NON_BORDER_LOCATIONS_CH1.Concat(NON_BORDER_LOCATIONS_CH2).Contains(__instance.locationName))
                return;
            if (!SettingsManager.Locations_RandomizeHideoutPosition!.Value && HIDEOUTS_CH1.Concat(HIDEOUTS_CH2).Contains(__instance.locationName))
                return;


            List<string> availableToSpawn = new();

            if (SettingsManager.Locations_RandomizeLocationPosition.Value)
            {
                if (Plugin.Controller.GameState == GameState.GeneratingCh1)
                    availableToSpawn = availableToSpawn.Concat(NON_BORDER_LOCATIONS_CH1).ToList();
                else if (Plugin.Controller.GameState == GameState.GeneratingCh2)
                    availableToSpawn = availableToSpawn.Concat(NON_BORDER_LOCATIONS_CH2).ToList();
            }
            if (SettingsManager.Locations_RandomizeHideoutPosition.Value)
            {
                if (Plugin.Controller.GameState == GameState.GeneratingCh1)
                    availableToSpawn = availableToSpawn.Concat(HIDEOUTS_CH1).ToList();
                else if (Plugin.Controller.GameState == GameState.GeneratingCh2)
                    availableToSpawn = availableToSpawn.Concat(HIDEOUTS_CH2).ToList();
            }

            List<string> remainingAvailableToSpawn = availableToSpawn.Except(locationsAlreadySpawned).ToList();

            if (remainingAvailableToSpawn.Count == 0)
            {
                Plugin.Controller.LocationPositionsRandomized = true;
                return;
            }

            __instance.locationName = remainingAvailableToSpawn.RandomItem();
            locationsAlreadySpawned.Add(__instance.locationName);
        }



        // Randomizes location rotation
        // Also makes vaulting a little scuffed
        // Does not affect border locations
        [HarmonyPatch(typeof(GameObject), "SetActive")]
        [HarmonyPrefix]
        private static void RandomizeLocationRotation(GameObject __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (__instance.GetComponentInParent<WorldChunk>()?.isBorderChunk == true)
                return;

            string locationName = __instance.name.Replace("_done", "");

            if (SettingsManager.Locations_RandomizeHideoutRotation!.Value && HIDEOUTS_CH1.Concat(HIDEOUTS_CH2).Contains(locationName))
                __instance.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);

            if (SettingsManager.Locations_RandomizeLocationRotation!.Value && NON_BORDER_LOCATIONS_CH1.Concat(OUTSIDE_LOCATIONS_CH1).Concat(NON_BORDER_LOCATIONS_CH2.Concat(OUTSIDE_LOCATIONS_CH2)).Contains(locationName))
                __instance.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);
        }



        // Fixes vaulting
        [HarmonyPatch(typeof(CharBase), "getJumpRotation")]
        [HarmonyPrefix]
        private static bool FixJumpRotation(CharBase __instance, GameObject ___touchedJumpableObject)
        {
            // Unrotate objects so that we can apply relative position check
            // Rotation is reapplied when assigning to __instance.jumpingThroughWindowRotation
            Vector3 unrotatedJumpableObjectPosition = Quaternion.Euler(0, -___touchedJumpableObject.transform.rotation.eulerAngles.y + ___touchedJumpableObject.transform.localEulerAngles.z, 0) * ___touchedJumpableObject.transform.position;
            Vector3 unrotatedPlayerPosition = Quaternion.Euler(0, -___touchedJumpableObject.transform.rotation.eulerAngles.y + ___touchedJumpableObject.transform.localEulerAngles.z, 0) * __instance.transform.position;


            if (___touchedJumpableObject != null)
            {
                float y = ___touchedJumpableObject.transform.localEulerAngles.z;
                if ((y > 89f && y < 91f) || (y > 269f && y < 271f) || (y < -89f && y > -91f) || (y < -269f && y > -271f))
                {
                    if (unrotatedJumpableObjectPosition.x > unrotatedPlayerPosition.x)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, 90f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                    if (unrotatedJumpableObjectPosition.x < unrotatedPlayerPosition.x)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, -90f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                }
                else
                {
                    if (unrotatedJumpableObjectPosition.z > unrotatedPlayerPosition.z)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, 0f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                    if (unrotatedJumpableObjectPosition.z < unrotatedPlayerPosition.z)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, 180f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                }
            }

            return false;
        }
    }
}
