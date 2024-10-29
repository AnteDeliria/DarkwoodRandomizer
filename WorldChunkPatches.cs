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
            if (!Settings.World_RandomizeChunkBiomes!.Value)
                return;

            __instance.biome = Biomes.GetRandomBiome(Settings.World_RandomizeChunkBiomesPool!);
        }


        /*  
         *  Randomizes ground sprites
         *  Purely cosmetic and often gets covered by other decals
         */
        [HarmonyPatch("createGroundSprites")]
        [HarmonyPrefix]
        internal static void RandomizeGroundSpritesPrefix(WorldChunk __instance, ref Biome __state)
        {
            if (!Settings.World_RandomizeChunkGroundSprites!.Value)
                return;

            __state = __instance.biome;
            __instance.biome = Biomes.GetRandomBiome(Settings.World_RandomizeChunkGroundSpritesPool!);
        }
        [HarmonyPatch("createGroundSprites")]
        [HarmonyPostfix]
        internal static void RandomizeGroundSpritesPostfix(WorldChunk __instance, ref Biome __state)
        {
            if (!Settings.World_RandomizeChunkGroundSprites!.Value)
                return;

            __instance.biome = __state;
        }
    }
}
