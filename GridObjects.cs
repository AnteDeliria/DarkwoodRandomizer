using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer
{
    [HarmonyPatch]
    internal static class GridObjects
    {
        [HarmonyPatch(typeof(WorldGenerator), "onConnectedRoads")]
        [HarmonyPrefix]
        internal static void RandomizeGridObjects(WorldGenerator __instance)
        {
            if (!Settings.GridObjects_RandomizeGridObjects!.Value)
                return;


            List<Biome> biomes;

            if (__instance.chapterID == 1)
                biomes = __instance.biomePresets.Where(x => x.type == Biome.Type.meadow || x.type == Biome.Type.forest || x.type == Biome.Type.forest_mutated).ToList();
            else if (__instance.chapterID == 2)
                biomes = __instance.biomePresets.Where(x => x.type == Biome.Type.swamp).ToList();
            else
                return; // unknown chapter ID


            List<GridObject> gridObjectPool = new();

            foreach (Biome biome in biomes)
            {
                foreach (GridObject gObject in biome.gObjects)
                    gridObjectPool.Add(gObject);

                biome.gObjects.Clear();
            }

            if (__instance.chapterID == 1 && Settings.GridObjects_IncludeSwampObjectsInPool!.Value)
                foreach (GridObject gObject in __instance.biomePresets.First(x => x.type == Biome.Type.swamp).gObjects)
                    gridObjectPool.Add(gObject);

            while (gridObjectPool.Count > 0)
            {
                GridObject randomGridObject = gridObjectPool[UnityEngine.Random.Range(0, gridObjectPool.Count)];
                Biome biome = biomes[UnityEngine.Random.Range(0, biomes.Count)];
                
                biome.gObjects.Add(randomGridObject);
                gridObjectPool.Remove(randomGridObject);
            }
        }


        [HarmonyPatch(typeof(GameObject), "SetActive")]
        [HarmonyPrefix]
        internal static void RandomizeGridObjectRotation(GameObject __instance)
        {
            if (Settings.GridObjects_RandomizeGridObjectRotation!.Value && __instance.GetComponent<Location>()?.isGridObject == true)
                __instance.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);
        }
    }
}
