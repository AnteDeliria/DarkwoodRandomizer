using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Night
    {
        [HarmonyPatch(typeof(CharacterSpawner), "spawnCharacterAround")]
        [HarmonyPrefix]
        private static bool RandomizeNightEnemies(CharacterSpawner __instance, ref Character? __result, GameObject destGO, Vector3 offset, float distance, string type, bool nocturnal, bool attackPlayer = false, bool relentlessPursuit = false, bool canSpawnInside = false)
        {
            if (!SettingsManager.Night_RandomizeCharacters!.Value || Core.isDay())
                return true;



            GameObject gameObject = __instance.holder;
            Vector3 vector = Vector3.zero;
            if (offset == Vector3.zero)
            {
                vector = (Vector3)AccessTools.Method(typeof(CharacterSpawner), "getFreeSpotAround").Invoke(__instance, [destGO, distance, canSpawnInside, 0]);
                if (vector == Vector3.zero)
                {
                    Debug.LogError("Aborting character spawn " + type + " " + destGO.name, destGO);
                    __result = null;
                    return false;
                }
            }
            else
                vector = offset * distance;

            IEnumerable<string>? characterPool = CharacterPools.GetNightCharacterPathsForBiome(Player.Instance.whereAmI.bigLocation.biomeType);

            if (characterPool == null)
                return true;

            Character? character = Core.AddPrefab(characterPool.RandomItem(), vector, Quaternion.Euler(90f, 0f, 0f), gameObject, false)?.GetComponent<Character>();

            if (character == null)
                return true;

            character.gameObject.SetActive(true);
            character.enableComponents(true);

            AccessTools.Field(typeof(CharacterSpawner), "character").SetValue(__instance, character);
            if (Player.Instance.whereAmI.bigLocation != null)
                character.setWaypoints(Player.Instance.whereAmI.bigLocation.waypoints);

            if (attackPlayer || SettingsManager.Night_AlwaysAttackPlayer!.Value)
                character.attackPlayer();
            if (relentlessPursuit || SettingsManager.Night_RelentlessPursuit!.Value)
                character.relentlessPursuit = true;

            character.nocturnal = nocturnal;
            character.temporarySpawned = true;
            if (!Singleton<Dreams>.Instance.dreaming)
            {
                if (nocturnal && !__instance.nocturnalCharacters.Contains(character.gameObject))
                {
                    __instance.nocturnalCharacters.Add(character.gameObject);
                }
                if (!__instance.spawnedCharacters.Contains(character.gameObject))
                {
                    __instance.spawnedCharacters.Add(character.gameObject);
                }
                Core.addToSaveable(character.gameObject, true, false);
            }
            character.isActive = true;
            

            __result = character;
            return false;
        }



        [HarmonyPatch(typeof(NightScenarios), "setCurrentScenario")]
        [HarmonyPrefix]
        private static void EnsureRandomScenarioFromNight2Prefix(NightScenarios __instance, ref int __state)
        {
            if (!SettingsManager.Night_RandomizeScenarioDifficulty!.Value)
                return;

            if (Player.Instance?.whereAmI?.currentBiome?.type == Biome.Type.meadow && __instance.scenarioId == 3)
            {
                __state = __instance.scenarioId;
                __instance.scenarioId = 4;
            }
        }

        [HarmonyPatch(typeof(NightScenarios), "setCurrentScenario")]
        [HarmonyPostfix]
        private static void EnsureRandomScenarioFromNight2Postfix(NightScenarios __instance, ref int __state)
        {
            if (!SettingsManager.Night_RandomizeScenarioDifficulty!.Value)
                return;

            if (Player.Instance?.whereAmI?.currentBiome?.type == Biome.Type.meadow && __state == 3)
                __instance.scenarioId = __state;
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
