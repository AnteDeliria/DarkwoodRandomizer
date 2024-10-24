using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer
{
    [HarmonyPatch]
    internal static class FreeRoamingEnemies
    {
        private static CharacterType[] possibleCharacters =
            [CharacterType.Centipede, CharacterType.ChomperRed, CharacterType.ChomperHalf, CharacterType.ChomperBlack,
            CharacterType.Pig, CharacterType.Chicken, CharacterType.Dog, CharacterType.DogMutated,
            CharacterType.Deer, CharacterType.Redneck, CharacterType.Redneck02, CharacterType.Villager,
            CharacterType.Villager3_plank, CharacterType.Banshee, CharacterType.Spider01, CharacterType.Spider03_day,
            CharacterType.Swamper1, CharacterType.HumanSpider, CharacterType.Villager1_Burning];

        private static CharacterType GetRandomCharacterType()
        {
            return possibleCharacters[UnityEngine.Random.Range(0, possibleCharacters.Length)];
        }

        private static CharacterType GetRandomCharacterType(ConfigEntry<string> configEntry)
        {
            if (string.IsNullOrEmpty(configEntry.Value))
                return GetRandomCharacterType();

            IEnumerable<string> characterStrings = configEntry.Value.Split(',').Select(x => x.Trim().ToLower());
            if (characterStrings.Count() == 0)
                return GetRandomCharacterType();

            CharacterType[] characterTypes = possibleCharacters.Where(type => characterStrings.Contains(type.ToString().ToLower())).ToArray();
            return characterTypes[UnityEngine.Random.Range(0, characterTypes.Length)];
        }



        [HarmonyPatch(typeof(WorldChunk), "spawnFreeRoamingCharacters")]
        [HarmonyPrefix]
        internal static bool RandomizeFreeRoamingCharacters(WorldChunk __instance, GameObject ___CharactersFreeRoaming, List<GameObject> ___freeRoamingChars)
        {
            if (!Settings.FreeRoamingEnemies_RandomizeType.Value)
                return true;

            if (__instance.isBorderChunk)
                return false;

            int num;
            for (int i = 0; i < __instance.biome.characters.Count; i = num + 1)
            {
                CharacterToSpawn charToSpawn = __instance.biome.characters[i];
                if (charToSpawn != null && charToSpawn.character != null && !charToSpawn.useCharacterSpawnPoint)
                {
                    for (int j = 0; j < charToSpawn.amount; j = num + 1)
                    {
                        Vector3 pointWithinBounds = (Vector3)AccessTools.Method(typeof(WorldChunk), "getPointWithinBounds").Invoke(__instance, [charToSpawn.minimumDistanceToHideout, 1]);
                        if (pointWithinBounds != Vector3.zero)
                        {
                            // Injection
                            string character;
                            if (__instance.biome.type == Biome.Type.meadow)
                                character = GetRandomCharacterType(Settings.FreeRoamingEnemies_DryMeadowEnemiesPool).ToString();
                            else if (__instance.biome.type == Biome.Type.forest)
                                character = GetRandomCharacterType(Settings.FreeRoamingEnemies_SilentForestEnemiesPool).ToString();
                            else if (__instance.biome.type == Biome.Type.forest_mutated)
                                character = GetRandomCharacterType(Settings.FreeRoamingEnemies_SilentForestEnemiesPool).ToString();
                            else
                                character = GetRandomCharacterType().ToString();

                            Character component = Core.AddPrefab("Characters/" + character, pointWithinBounds, Quaternion.Euler(90f, 0f, 0f), ___CharactersFreeRoaming, true).GetComponent<Character>();
                            // End injection
                            if (component != null)
                            {
                                component.noWaypoints = true;
                                ___freeRoamingChars.Add(component.gameObject);
                                Core.addToSaveable(component.gameObject, true, true);
                                Singleton<WorldGrid>.Instance.registerToNode(component.gameObject, WorldGrid.Cullable.Type.moveable);
                                component.gameObject.SetActive(false);
                                if (component.sleeping)
                                {
                                    component.returnToSleepAfterLosingTarget = true;
                                }
                            }
                        }
                        num = j;
                    }
                }
                charToSpawn = null;
                num = i;
            }

            return false;
        }
    }
}
