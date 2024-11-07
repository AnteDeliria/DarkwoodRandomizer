﻿using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;

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
            if (!SettingsManager.World_RandomizeChunkGroundSprites!.Value)
                return;

            __state = __instance.biome;
            __instance.biome = Biomes.GetRandomBiome(SettingsManager.World_RandomizeChunkGroundSpritesPool!);
        }
        [HarmonyPatch(typeof(WorldChunk), "createGroundSprites")]
        [HarmonyPostfix]
        internal static void RandomizeGroundSpritesPostfix(WorldChunk __instance, ref Biome __state)
        {
            if (!SettingsManager.World_RandomizeChunkGroundSprites!.Value)
                return;

            __instance.biome = __state;
        }
    }
}