using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class MiscObjects
    {
        [HarmonyPatch(typeof(WorldChunk), "distributeObjects")]
        [HarmonyPrefix]
        internal static void RandomizeMiscObjects(ref List<UnityEngine.Object> objectsToDistribute, ref float height, ref int density, ref int gridSize)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.MiscObjects_RandomizeMiscObjects!.Value)
                return;

            WorldGenerator worldGenerator = Singleton<WorldGenerator>.Instance;

            IEnumerable<Biome> biomesSource;

            if (worldGenerator.chapterID == 1)
                biomesSource = worldGenerator.biomePresets.Where(x => new Biome.Type[] { Biome.Type.meadow, Biome.Type.forest, Biome.Type.forest_mutated }.Contains(x.type));
            else if (worldGenerator.chapterID == 2)
                biomesSource = worldGenerator.biomePresets.Where(x => new Biome.Type[] { Biome.Type.meadow, Biome.Type.forest, Biome.Type.forest_mutated, Biome.Type.swamp }.Contains(x.type));
            else
                return; // unknown chapter ID


            List<BiomePrefabsPreset> miscPrefabPool = new();

            foreach (Biome biome in biomesSource)
                foreach (BiomePrefabsPreset prefab in biome.miscPrefabs)
                    miscPrefabPool.Add(prefab);


            BiomePrefabsPreset randomMiscPrefab = miscPrefabPool.RandomItem();
            objectsToDistribute = randomMiscPrefab.prefabs;
            height = randomMiscPrefab.height;
            density = randomMiscPrefab.density;
            gridSize = randomMiscPrefab.gridDensity;
            // Unlike grid objects, misc objects are not removed from the pool once spawned as different chunks can normally spawn repeats of the same object
        }
    }
}
