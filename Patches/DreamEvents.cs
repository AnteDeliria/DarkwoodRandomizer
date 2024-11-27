using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Pools;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class DreamEvents
    {
        [HarmonyPatch(typeof(Dreams), "getPreset")]
        [HarmonyPrefix]
        private static bool RandomizeDreams(Dreams __instance, string presetName, ref DreamPreset __result)
        {
            if (!SettingsManager.Dreams_RandomizeDreams!.Value)
                return true;

            if (DreamPools.PROLOGUE_DREAMS.Contains(presetName) || DreamPools.EPILOGUE_DREAMS.Contains(presetName) || DreamPools.STORY_DREAMS.Contains(presetName))
                return true;

            List<string> dreamPool = DreamPools.LEVEL_UP_DREAMS.Concat(DreamPools.UNUSED_DREAMS).ToList();
            if (!Core.currentProfile.backerOnlyContent)
                dreamPool.Remove("dream_acid");

            DreamPreset dreamPreset = __instance.presetList.Where(x => dreamPool.Contains(x.name)).RandomItem();
            __instance.presetList.Remove(dreamPreset);
            __result = dreamPreset;
            return false;
        }
    }
}
