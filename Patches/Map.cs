using DarkwoodRandomizer.Settings;
using HarmonyLib;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal class Map
    {
        [HarmonyPatch(typeof(MapElement), "initialize")]
        [HarmonyPostfix]
        internal static void RevealAllMapElements(MapElement __instance)
        {
            if (!(Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh1 || Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.Map_RevealAllMapElements!.Value)
                return;

            if (!__instance.elementName.Contains("border"))
                __instance.isOnMap = true;
        }
    }
}
