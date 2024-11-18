using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class GridObjects
    {
        [HarmonyPatch(typeof(WorldGenerator), "onConnectedRoads")]
        [HarmonyPrefix]
        internal static void ShuffleGridObjects(WorldGenerator __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.GridObjects_ShuffleGridObjects!.Value)
                return;


            IEnumerable<Biome> biomesSource;

            if (__instance.chapterID == 1)
                biomesSource = __instance.biomePresets.Where(x => new Biome.Type[] { Biome.Type.meadow, Biome.Type.forest, Biome.Type.forest_mutated }.Contains(x.type));
            else if (__instance.chapterID == 2)
                biomesSource = __instance.biomePresets.Where(x => new Biome.Type[] { Biome.Type.meadow, Biome.Type.forest, Biome.Type.forest_mutated, Biome.Type.swamp }.Contains(x.type));
            else
                return; // unknown chapter ID

            IEnumerable<Biome> biomesDestination = __instance.bigBiomes.Select(biome => __instance.getBiomePreset(biome.type));


            List<GridObject> gridObjectPool = new();

            foreach (Biome biome in biomesSource)
                foreach (GridObject gObject in biome.gObjects)
                    gridObjectPool.Add(gObject);

            foreach (Biome biome in biomesDestination)
                biome.gObjects.Clear();

            while (gridObjectPool.Count > 0)
            {
                GridObject randomGridObject = gridObjectPool.RandomItem();
                Biome randomBiome = biomesDestination.RandomItem();

                randomBiome.gObjects.Add(randomGridObject);
                gridObjectPool.Remove(randomGridObject);
            }

            Plugin.Controller.GridObjectsShuffled = true;
        }



        [HarmonyPatch(typeof(GameObject), "SetActive")]
        [HarmonyPrefix]
        internal static void RandomizeGridObjectRotation(GameObject __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (SettingsManager.GridObjects_RandomizeGridObjectRotation!.Value)
                return;

            if (__instance.GetComponent<Location>()?.isGridObject == true)
                __instance.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        }
    }
}
