using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DarkwoodRandomizer
{
    [HarmonyPatch(typeof(WorldChunk))]
    internal class WorldChunkPatches
    {
        /*  
         *  Randomizes chunk biomes
         *  Affects all downstream biome dependencies
         */
        [HarmonyPatch("populate")]
        [HarmonyPrefix]
        internal static void RandomizeChunkBiome(WorldChunk __instance)
        {
            if (!Settings.World_RandomizeChunkBiomes.Value)
                return;

            __instance.biome = Biomes.GetRandomBiome(Settings.World_RandomizeChunkBiomesPool);
        }


        /*  
         *  Randomizes ground sprites
         *  Purely cosmetic and often gets covered by other decals
         */
        [HarmonyPatch("createGroundSprites")]
        [HarmonyPrefix]
        internal static void RandomizeGroundSpritesPrefix(WorldChunk __instance, ref Biome __state)
        {
            if (!Settings.World_RandomizeChunkGroundSprites.Value)
                return;

            __state = __instance.biome;
            __instance.biome = Biomes.GetRandomBiome(Settings.World_RandomizeChunkGroundSpritesPool);
        }
        [HarmonyPatch("createGroundSprites")]
        [HarmonyPostfix]
        internal static void RandomizeGroundSpritesPostfix(WorldChunk __instance, ref Biome __state)
        {
            if (!Settings.World_RandomizeChunkGroundSprites.Value)
                return;

            __instance.biome = __state;
        }

        //[HarmonyPatch("spawnNextLocation", MethodType.Enumerator)]
        //[HarmonyTranspiler]
        //internal static IEnumerable<CodeInstruction> RandomizeMapBorders(IEnumerable<CodeInstruction> instructions)
        //{
        //    foreach (var instruction in instructions)
        //    {
        //        if (instruction.Calls(AccessTools.Method(typeof(WorldChunk), "getLocationName")))
        //        {
        //            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(WorldChunkPatches), nameof(IWasHere)));
        //        }
        //        yield return instruction;
        //    }
        //}

        //internal static void IWasHere()
        //{
        //    DarkwoodRandomizerPlugin.Logger.LogInfo("I was here");
        //}


        /*  
         *  Spawns locations in chunks without a predefined location
         *  Mostly spawns around the edges but can also block off paths
         */
        internal static List<string> alreadySpawned = new();

        [HarmonyPatch("populate")]
        [HarmonyPrefix]
        internal static void PopulatePrefix(WorldChunk __instance)
        {
            //DarkwoodRandomizerPlugin.Logger.LogInfo("Old location name: " + __instance.locationName ?? "null");

            //if (!string.IsNullOrEmpty(__instance.locationName) || __instance.isBorderChunk)
            //    return;

            

            if (__instance.isBorderChunk || Locations.Hideouts.Contains(__instance.locationName)) // Don't want to clip locations into the wall or rearrange hideouts
                return;

            if (Settings.Locations_RandomizeExistingLocations.Value && !string.IsNullOrEmpty(__instance.locationName))
            {
                if (Settings.Locations_RandomizeExistingLocationsExcludeHideouts.Value && Locations.Hideouts.Contains(__instance.locationName))
                    return;

                List<string> availableToSpawn = Locations.GetOverworldLocationPool(Settings.Locations_RandomizeExistingLocationsPool);

                if (Settings.Locations_RandomizeExistingLocationsExcludeHideouts.Value)
                    availableToSpawn = availableToSpawn.Except(Locations.Hideouts).ToList();
                if (!Settings.Locations_RandomizeExistingLocationsAllowRepeats.Value)
                    availableToSpawn = availableToSpawn.Except(alreadySpawned).ToList();

                if (availableToSpawn.Count > 0)
                {
                    __instance.locationName = availableToSpawn[UnityEngine.Random.Range(0, availableToSpawn.Count)];
                    alreadySpawned.Add(__instance.locationName);
                }
            }
            else if (Settings.Locations_AddExtraLocations.Value && !string.IsNullOrEmpty(__instance.locationName))
            {
                List<string> availableToSpawn = Locations.GetOverworldLocationPool(Settings.Locations_AddExtraLocationsPool);

                if (Settings.Locations_RandomizeExistingLocationsExcludeHideouts.Value)
                    availableToSpawn = availableToSpawn.Except(Locations.Hideouts).ToList();
                if (!Settings.Locations_AddExtraLocationsAllowRepeats.Value)
                    availableToSpawn = availableToSpawn.Except(alreadySpawned).ToList();

                if (availableToSpawn.Count > 0)
                {
                    __instance.locationName = availableToSpawn[UnityEngine.Random.Range(0, availableToSpawn.Count)];
                    alreadySpawned.Add(__instance.locationName);
                }
            }

            //List<LocationPreset> presets = Singleton<WorldGenerator>.Instance.locationPresets.ToList();

            //foreach (LocationPreset preset in presets.ToArray())
            //    if (unloadedPresets.Contains(preset.name) || hideouts.Contains(preset.name))
            //        presets.Remove(preset);



            //DarkwoodRandomizerPlugin.Logger.LogInfo("Spawned location: " + __instance.locationName);
        }
    }
}
