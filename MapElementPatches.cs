using HarmonyLib;

namespace DarkwoodRandomizer
{
    [HarmonyPatch(typeof(MapElement))]
    internal class MapElementPatches
    {
        //[HarmonyPatch("Start")]
        //[HarmonyPrefix]
        //internal static void RevealAllMapElements(MapElement __instance)
        //{
        //    __instance.dontInitialize = false;
        //    __instance.isOnMap = true;
        //}

        [HarmonyPatch("initialize")]
        [HarmonyPostfix]
        internal static void RevealAllMapElementsTwo(MapElement __instance)
        {
            if (!Settings.Map_RevealAllMapElements.Value)
                return;


            if (!__instance.elementName.Contains("border"))
                __instance.isOnMap = true;
        }
    }
}
