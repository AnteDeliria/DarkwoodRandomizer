using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static NightScenario;

namespace DarkwoodRandomizer
{
    [HarmonyPatch]
    internal static class Enemies
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


        // TODO: fix to include all enemies and Core.SpawnPrefab
        [HarmonyPatch(typeof(WorldChunk), "spawnFreeRoamingCharacters")]
        [HarmonyPrefix]
        internal static bool RandomizeFreeRoamingCharacters(WorldChunk __instance, GameObject ___CharactersFreeRoaming, List<GameObject> ___freeRoamingChars)
        {
            if (!Settings.Enemies_RandomizeFreeRoamingEnemies.Value)
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
                                character = GetRandomCharacterType(Settings.Enemies_FreeRoamingEnemiesDryMeadowPool).ToString();
                            else if (__instance.biome.type == Biome.Type.forest)
                                character = GetRandomCharacterType(Settings.Enemies_FreeRoamingEnemiesSilentForestPool).ToString();
                            else if (__instance.biome.type == Biome.Type.forest_mutated)
                                character = GetRandomCharacterType(Settings.Enemies_FreeRoamingEnemiesOldWoodsPool).ToString();
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


        // TODO: fix to include all enemies
        [HarmonyPatch(typeof(WorldGenerator), "activateAllLocations")]
        [HarmonyPostfix]
        internal static void RandomizeLocationEnemies(WorldGenerator __instance)
        {
            if (!Settings.Enemies_RandomizeLocationEnemies.Value)
                return;

            foreach (Location location in __instance.locations)
            {
                if (location.isGridObject)
                    continue;

                foreach (Character character in location.charactersList.ToArray())
                {
                    location.charactersList.Remove(character);
                    UnityEngine.Object.Destroy(character.gameObject);

                    Character component = Core.AddPrefab("Characters/" + GetRandomCharacterTypeForLocation(location).ToString(), character.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false).GetComponent<Character>();
                    location.charactersList.Add(component);
                }

                foreach (CharacterSpawnPoint characterSpawnPoint in location.spawnPoints.ToArray())
                {
                    location.spawnPoints.Remove(characterSpawnPoint);
                    UnityEngine.Object.Destroy(characterSpawnPoint.gameObject);

                    Character component = Core.AddPrefab("Characters/" + GetRandomCharacterTypeForLocation(location).ToString(), characterSpawnPoint.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false).GetComponent<Character>();
                    location.charactersList.Add(component);
                }
            }

            CharacterType GetRandomCharacterTypeForLocation(Location location)
            {
                CharacterType characterToSpawn;

                if (location.biomeType == Biome.Type.meadow)
                    characterToSpawn = GetRandomCharacterType(Settings.Enemies_LocationEnemiesDryMeadowPool);
                else if (location.biomeType == Biome.Type.forest)
                    characterToSpawn = GetRandomCharacterType(Settings.Enemies_LocationEnemiesSilentForestPool);
                else if (location.biomeType == Biome.Type.forest_mutated)
                    characterToSpawn = GetRandomCharacterType(Settings.Enemies_LocationEnemiesOldWoodsPool);
                else
                    characterToSpawn = GetRandomCharacterType();

                return characterToSpawn;
            }
        }
    }
}
