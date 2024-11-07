using DarkwoodRandomizer.Plugin;
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
            if (!Settings.MiscObjects_RandomizeMiscObjects!.Value)
                return;

            WorldGenerator worldGenerator = Singleton<WorldGenerator>.Instance;
            List<Biome> biomes;

            if (worldGenerator.chapterID == 1)
                biomes = worldGenerator.biomePresets.Where(x => x.type == Biome.Type.meadow || x.type == Biome.Type.forest || x.type == Biome.Type.forest_mutated).ToList();
            else if (worldGenerator.chapterID == 2)
                biomes = worldGenerator.biomePresets.Where(x => x.type == Biome.Type.swamp).ToList();
            else
                return; // unknown chapter ID


            List<BiomePrefabsPreset> miscPrefabPool = new();

            foreach (Biome biome in biomes)
                foreach (BiomePrefabsPreset prefab in biome.miscPrefabs)
                    miscPrefabPool.Add(prefab);

            if (worldGenerator.chapterID == 1 && Settings.MiscObjects_IncludeSwampObjectsInPool!.Value)
                foreach (BiomePrefabsPreset prefab in worldGenerator.biomePresets.First(x => x.type == Biome.Type.swamp).miscPrefabs)
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
