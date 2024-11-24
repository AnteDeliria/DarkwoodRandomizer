using HarmonyLib;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class UI
    {
        [HarmonyPatch(typeof(MainMenu), "Start")]
        [HarmonyPostfix]
        private static void AddModName(MainMenu __instance)
        {
            __instance.CurrentVersion.text += $"\nDarkwood Randomizer {MyPluginInfo.PLUGIN_VERSION}";
        }
    }
}
