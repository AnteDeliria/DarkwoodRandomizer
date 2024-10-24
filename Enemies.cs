using HarmonyLib;

namespace DarkwoodRandomizer
{
    [HarmonyPatch]
    internal static class Enemies
    {
        [HarmonyPatch(typeof(WorldChunk), "spawnChars")]
        [HarmonyPrefix]
        internal static void RandomizeFreeRoamingCharacters(WorldChunk __instance)
        {
            if (!Settings.Enemies_RandomizeFreeRoamingCharacters.Value)
                return;
    }
}
