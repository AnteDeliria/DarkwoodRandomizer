using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Night
    {
        [HarmonyPatch(typeof(CharacterSpawner), "spawnCharacterAround")]
        [HarmonyPrefix]
        internal static void RandomizeNightEnemies(ref string type)
        {
            if (!SettingsManager.Night_RandomizeEnemies!.Value)
                return;

            IEnumerable<string>? characterPool = CharacterPools.NightCharacters;
            if (characterPool == null)
                return;

            type = characterPool.RandomItem().ToString();
        }
    }
}
