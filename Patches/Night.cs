using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal class Night
    {
        private static CharacterType[] possibleCharacters = (CharacterType[])Enum.GetValues(typeof(CharacterType));


        [HarmonyPatch(typeof(CharacterSpawner), "spawnCharacterAround")]
        [HarmonyPrefix]
        internal static void RandomizeNightEnemies(ref string type)
        {
            if (!SettingsManager.Night_RandomizeEnemies!.Value)
                return;

            type = possibleCharacters.RandomItem().ToString();
        }
    }
}
