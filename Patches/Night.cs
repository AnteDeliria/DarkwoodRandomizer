using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Pools;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
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
            if (!SettingsManager.Night_RandomizeCharacters!.Value)
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

            // Injection
            IEnumerable<string>? characterPool = CharacterPools.GetNightPoolForBiome(Player.Instance.whereAmI.bigLocation.biomeType);

            if (characterPool == null)
                return true;

            string randomCharacter = characterPool.RandomItem();
            Character? character = Core.AddPrefab(randomCharacter, vector, Quaternion.Euler(90f, 0f, 0f), gameObject, false).GetComponent<Character>();
            // End Injection

            AccessTools.Field(typeof(CharacterSpawner), "character").SetValue(__instance, character);
            if (Player.Instance.whereAmI.bigLocation != null)
                character.setWaypoints(Player.Instance.whereAmI.bigLocation.waypoints);
            if (attackPlayer)
                character.attackPlayer();
            character.relentlessPursuit = relentlessPursuit;
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
