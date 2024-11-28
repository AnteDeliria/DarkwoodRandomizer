using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Decals
    {
        // Randomizes ground sprites
        // Purely cosmetic
        [HarmonyPatch(typeof(WorldChunk), "createGroundSprites")]
        [HarmonyPrefix]
        internal static void RandomizeGroundSpritesPrefix(WorldChunk __instance, ref Biome __state)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.Decals_RandomizeChunkGroundSprites!.Value)
                return;

            __state = __instance.biome;
            List<Biome> biomes = (List<Biome>)AccessTools.Field(typeof(WorldGenerator), "biomePresets").GetValue(Singleton<WorldGenerator>.Instance);
            __instance.biome = biomes.Where(biome => biome.type != Biome.Type.empty).RandomItem();
        }

        [HarmonyPatch(typeof(WorldChunk), "createGroundSprites")]
        [HarmonyPostfix]
        internal static void RandomizeGroundSpritesPostfix(WorldChunk __instance, ref Biome __state)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.Decals_RandomizeChunkGroundSprites!.Value)
                return;

            __instance.biome = __state;
        }
    }
}
