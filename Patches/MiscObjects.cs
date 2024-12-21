using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class MiscObjects
    {
        [HarmonyPatch(typeof(WorldGenerator), "spawnMiscObjects")]
        [HarmonyPrefix]
        internal static void RandomizeMiscObjects(WorldGenerator __instance)
        {
            if (!(Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh1 || Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.MiscObjects_RandomizeMiscObjects!.Value)
                return;

            IEnumerable<Biome> biomesSource;

            if (__instance.chapterID == 1)
                biomesSource = __instance.biomePresets.Where(x => new Biome.Type[] { Biome.Type.meadow, Biome.Type.forest, Biome.Type.forest_mutated }.Contains(x.type));
            else
                return;

            List<Biome> biomesDestination = __instance.bigBiomes
                .Where(biome => biome.type != Biome.Type.empty)
                .Select(biome => __instance.getBiomePreset(biome.type)).ToList();


            List<BiomePrefabsPreset> miscPrefabPool = new();

            foreach (Biome biome in biomesSource)
                foreach (BiomePrefabsPreset prefab in biome.miscPrefabs)
                    miscPrefabPool.Add(prefab);


            foreach (Biome biome in biomesDestination)
                biome.miscPrefabs.Clear();

            foreach (BiomePrefabsPreset prefab in miscPrefabPool)
                biomesDestination.RandomItem().miscPrefabs.Add(prefab);

            Plugin.Controller.MiscObjectsShuffled = true;
        }
    }
}
