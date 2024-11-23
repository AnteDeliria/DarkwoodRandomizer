using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Night
    {
        [HarmonyPatch(typeof(CharacterSpawner), "spawnCharacterAround")]
        [HarmonyPrefix]
        private static void RandomizeNightEnemies(ref string type)
        {
            if (!SettingsManager.Night_RandomizeCharacters!.Value)
                return;

            IEnumerable<string>? characterPool = CharacterPools.NightCharacters.Select(path => path.Replace("characters/", ""));
            if (characterPool == null)
                return;

            type = characterPool.RandomItem().ToString();
        }


        [HarmonyPatch(typeof(NightScenarios), "getRandomScenarioOfDifficulty")]
        [HarmonyPrefix]
        private static void RandomizeNightScenarioDifficulty(ref int _diff)
        {
            if (!SettingsManager.Night_RandomizeScenarioDifficulty!.Value)
                return;

            _diff = UnityEngine.Random.Range(1, 5);
        }
    }
}
