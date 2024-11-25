using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
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

            if (presetName.Contains("epilog"))
                return true;

            DreamPreset dreamPreset = __instance.presetList.Where(x => !x.name.Contains("epilog")).RandomItem();
            __instance.presetList.Remove(dreamPreset);
            __result = dreamPreset;
            return false;
        }
    }
}
