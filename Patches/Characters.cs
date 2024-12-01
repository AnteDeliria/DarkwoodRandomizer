using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Characters
    {
        // Test spawn characters
        //private static int Index = 0;
        //[HarmonyPatch(typeof(Player), "onInstantClick")]
        //[HarmonyPostfix]
        //internal static void SpawnCharacterTest(Player __instance)
        //{
        //    if (!(bool)AccessTools.Field(typeof(Player), "rmbDown").GetValue(__instance))
        //        return;

        //    GameObject obj = Core.AddPrefab(allCharacters.Values.ToArray()[Index], __instance.transform.position + new Vector3(100, 0, 0), Quaternion.Euler(90f, 0f, 0f), Singleton<WorldGenerator>.Instance.gameObject, true);
        //    Index++;
        //    DarkwoodRandomizerPlugin.Logger.LogInfo($"Spawning Index {Index}: {allCharacters.Values.ToArray()[Index]}, Immobile: {obj.GetComponent<Character>().immobile}");
        //}


        [HarmonyPatch(typeof(Character), "init")]
        [HarmonyPostfix]
        private static void ModifyCharacter(Character __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;

            if (SettingsManager.CharacterStats_HealthVarianceRange!.Value != 0)
            {
                float healthVarianceRange = SettingsManager.CharacterStats_HealthVarianceRange!.Value / 100;
                if (healthVarianceRange < 0)
                {
                    DarkwoodRandomizerPlugin.Logger.LogError("CharacterAttributes_HealthVarianceRange is negative - defaulting to 0");
                    healthVarianceRange = 0;
                }

                __instance.maxHealth = __instance.maxHealth * (1 + UnityEngine.Random.Range(-healthVarianceRange, healthVarianceRange));
                __instance.health = __instance.maxHealth;
            }

            if (SettingsManager.Characters_PreventInfighting!.Value)
            {
                foreach (Character.EnemyType enemyType in __instance.enemyTypes)
                    if (enemyType.faction != Faction.player)
                        enemyType.attacks = false;
            }
        }


        internal static void TryRandomizeCharacterProperties(Character character)
        {
            foreach (Character.EnemyType enemyType in character.enemyTypes)
            {
                bool randomBool = UnityEngine.Random.Range(0f, 1f) > 0.5f;
                enemyType.attacks = randomBool;
                enemyType.runsAwayFrom = !randomBool;
            }
            character.seekerType = Enum.GetValues(typeof(Character.SeekerType)).Cast<Character.SeekerType>().RandomItem();
            //character.blind
            //character.deaf
            //character.ethereal
            //character.fieldOfViewRange
            //character.wantToAttackDistance
            //character.wantToRangedAttackDistance
            //character.accuracy
            //character.enemyInSightCounterTarget
            //character.enemyInSightCounterFade
            //character.hearingQuality

            //foreach (FieldInfo field in typeof(Character).GetFields())
            //{
            //    if (field.FieldType == typeof(bool))
            //        field.SetValue(character, UnityEngine.Random.Range(0f, 1f) > 0.5f);
            //    else if (field.FieldType == typeof(float))
            //        field.SetValue(character, (float)field.GetValue(character) * UnityEngine.Random.Range(0.5f, 1.5f));
            //    else if (field.FieldType == typeof(double))
            //        field.SetValue(character, (double)field.GetValue(character) * UnityEngine.Random.Range(0.5f, 1.5f));
            //    else if (field.FieldType == typeof(int))
            //        field.SetValue(character, (int)((int)field.GetValue(character) * UnityEngine.Random.Range(0.5f, 1.5f)));
            //    else if (field.FieldType == typeof(Enum))
            //        field.SetValue(character, Enum.GetValues(field.FieldType).Cast<Enum>().RandomItem());
            //}
        }


        [HarmonyPatch(typeof(CharacterSpawnPoint), "actuallySpawn")]
        [HarmonyPrefix]
        private static bool RandomizeCharacterSpawners(CharacterSpawnPoint __instance)
        {
            IEnumerable<string>? characterPool;

            if (__instance.location != null && SettingsManager.Characters_RandomizeLocationActiveCharacters!.Value)
                characterPool = CharacterPools.GetGlobalCharacterPathsForBiome(__instance.location.biomeType);
            else if (__instance.location == null && SettingsManager.Characters_RandomizeGlobalCharacters!.Value)
            {
                Biome.Type? biome = Singleton<WorldGenerator>.Instance.bigBiomes.Where(biome => biome.globalCharacterSpawnPoints.Contains(__instance)).FirstOrDefault()?.type;
                if (biome == null)
                    return true;

                characterPool = CharacterPools.GetGlobalCharacterPathsForBiome((Biome.Type)biome);
            }
            else
                return true;

            if (characterPool == null)
                return true;


            float num = UnityEngine.Random.Range(0f, 1f);
            if (__instance.spawnChance >= num)
            {
                Character component = Core.AddPrefab(characterPool.RandomItem(), __instance.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), __instance.transform.parent.gameObject, false).GetComponent<Character>();
                if (component != null)
                {
                    __instance.spawnedCharacter = component;
                    if (__instance.useInsideWaypoints)
                    {
                        component.usesInsideWaypoints = true;
                    }
                    if (__instance.waypoints.Count > 0)
                    {
                        component.setWaypoints(__instance.waypoints);
                        component.usesCustomWaypoints = true;
                    }
                    component.spawnPoint = __instance.transform.position;
                    Core.addToSaveable(component.gameObject, true, true);
                    if (__instance.location != null)
                    {
                        if (!__instance.location.charactersList.Contains(component))
                        {
                            __instance.location.charactersList.Add(component);
                        }
                        component.isActive = true;
                        if (Core.randomGeneration && !__instance.location.entered)
                        {
                            component.gameObject.SetActive(false);
                            component.enableComponents(false);
                        }
                    }
                }
            }

            return false;
        }



        [HarmonyPatch(typeof(WorldChunk), "spawnFreeRoamingCharacters")]
        [HarmonyPrefix]
        private static bool RandomizeFreeRoamingCharacters(WorldChunk __instance, GameObject ___CharactersFreeRoaming, List<GameObject> ___freeRoamingChars)
        {
            if (!SettingsManager.Characters_RandomizeGlobalCharacters!.Value)
                return true;

            int num = 0;
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
                            IEnumerable<string>? characterPool = CharacterPools.GetGlobalCharacterPathsForBiome(__instance.biome.type);
                            Character? component = null;
                            if (characterPool != null)
                                component = Core.AddPrefab(characterPool.RandomItem(), pointWithinBounds, Quaternion.Euler(90f, 0f, 0f), ___CharactersFreeRoaming, true).GetComponent<Character>();
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
                num = i;
            }

            return false;
        }



        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPostfix]
        private static void RandomizeLocationCharacters(WorldGenerator __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => Plugin.Controller.OutsideLocationsLoaded && Plugin.Controller.LocationPositionsRandomized,
                action: () =>
                {
                    foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                    {
                        foreach (Character oldCharacter in location.GetComponentsInChildren<Character>(includeInactive: true))
                        {
                            IEnumerable<string>? characterPool = null;

                            if (SettingsManager.Characters_RandomizeLocationActiveCharacters!.Value && oldCharacter.npc == null && CharacterPools.ACTIVE_CHARACTERS.Keys.Contains(oldCharacter.name.ToLower()))
                                characterPool = CharacterPools.GetLocationActivePathsForBiome(location.biomeType);
                            else if (SettingsManager.Characters_RandomizeLocationStaticCharacters!.Value && oldCharacter.npc == null && CharacterPools.STATIC_CHARACTERS.Keys.Contains(oldCharacter.name.ToLower()))
                                characterPool = CharacterPools.GetLocationStaticPathsForBiome(location.biomeType);

                            Character? newCharacter;

                            if (characterPool == null)
                                newCharacter = oldCharacter;
                            else
                            {
                                GameObject? newCharacterObject = Core.AddPrefab(characterPool.RandomItem(), oldCharacter.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false);
                                Core.addToSaveable(newCharacterObject, true, true);
                                UnityEngine.Object.Destroy(oldCharacter.gameObject);
                                newCharacter = newCharacterObject?.GetComponent<Character>();
                            }

                            if (location.charactersList.Contains(oldCharacter))
                            {
                                location.charactersList.Remove(oldCharacter);
                                if (newCharacter != null)
                                    location.charactersList.Add(newCharacter);
                            }
                        }
                    }
                }
            );
        }
    }
}
