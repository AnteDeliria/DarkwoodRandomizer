using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Crafting
    {
        // Doesn't save on reload
        //[HarmonyPatch(typeof(Workbench), "Start")]
        //[HarmonyPostfix]
        //private static void RandomizeRecipes(Workbench __instance)
        //{
        //    if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
        //        return;

        //    DarkwoodRandomizerPlugin.Logger.LogInfo("Randomizing Crafting Recipes");
        //    List<CraftingRecipes> recipePool = UnityEngine.Resources.FindObjectsOfTypeAll<CraftingRecipes>().ToList();

        //    foreach (Workbench.Level level in __instance.levels)
        //    {
        //        for (int i = 0; i < level.recipes.Count; i++)
        //        {
        //            CraftingRecipes randomRecipes = recipePool.RandomItem();
        //            level.recipes[i] = randomRecipes;
        //            DarkwoodRandomizerPlugin.Logger.LogInfo(level.recipes[i].name);
        //            recipePool.Remove(randomRecipes);
        //        }
        //    }
        //}
    }
}
