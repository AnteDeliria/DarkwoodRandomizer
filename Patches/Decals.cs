using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal class Decals

    {
        //Randomizes ground sprites
        //Purely cosmetic and often gets covered by other decals
        [HarmonyPatch(typeof(WorldChunk), "createGroundSprites")]
        [HarmonyPrefix]
        internal static void RandomizeGroundSpritesPrefix(WorldChunk __instance, ref Biome __state)
        {
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
            if (!SettingsManager.Decals_RandomizeChunkGroundSprites!.Value)
                return;

            __instance.biome = __state;
        }
    }
}
