﻿using DarkwoodRandomizer.Plugin;
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
            if (!SettingsManager.GridObjects_ShuffleGridObjects!.Value)
                return;


            IEnumerable<Biome> biomes;

            if (__instance.chapterID == 1)
                biomes = __instance.biomePresets.Where(x => x.type == Biome.Type.meadow || x.type == Biome.Type.forest || x.type == Biome.Type.forest_mutated);
            else if (__instance.chapterID == 2)
                biomes = __instance.biomePresets.Where(x => x.type == Biome.Type.swamp);
            else
                return; // unknown chapter ID


            List<GridObject> gridObjectPool = new();

            foreach (Biome biome in biomes)
            {
                foreach (GridObject gObject in biome.gObjects)
                    gridObjectPool.Add(gObject);

                biome.gObjects.Clear();
            }

            if (__instance.chapterID == 1 && SettingsManager.GridObjects_IncludeSwampObjectsInCh1Pool!.Value)
                foreach (GridObject gObject in __instance.biomePresets.First(x => x.type == Biome.Type.swamp).gObjects)
                    gridObjectPool.Add(gObject);

            while (gridObjectPool.Count > 0)
            {
                GridObject randomGridObject = gridObjectPool.RandomItem();
                Biome biome = biomes.RandomItem();

                biome.gObjects.Add(randomGridObject);
                gridObjectPool.Remove(randomGridObject);
            }
        }


        [HarmonyPatch(typeof(GameObject), "SetActive")]
        [HarmonyPrefix]
        internal static void RandomizeGridObjectRotation(GameObject __instance)
        {
            if (SettingsManager.GridObjects_RandomizeGridObjectRotation!.Value && __instance.GetComponent<Location>()?.isGridObject == true)
                __instance.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        }
    }
}
