using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
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


        internal static void TryAdjustCharacterHealth(Character character, Biome.Type biome)
        {
            float healthVarianceRange = SettingsManager.CharacterAttributes_HealthVarianceRange!.Value / 100;
            if (healthVarianceRange < 0)
            {
                DarkwoodRandomizerPlugin.Logger.LogError("CharacterAttributes_HealthVarianceRange is negative - defaulting to 0");
                healthVarianceRange = 0;
            }

            character.maxHealth = character.maxHealth * (1 + UnityEngine.Random.Range(-healthVarianceRange, healthVarianceRange));
            character.health = character.maxHealth;
        }

        internal static void TryPreventInfighting(Character character)
        {
            if (SettingsManager.Characters_PreventInfighting!.Value)
                foreach (Character.EnemyType enemyType in character.enemyTypes)
                    if (enemyType.faction != Faction.player)
                        enemyType.attacks = false;
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



        [HarmonyPatch(typeof(WorldChunk), "spawnFreeRoamingCharacters")]
        [HarmonyPrefix]
        private static bool RandomizeFreeRoamingCharacters(WorldChunk __instance, GameObject ___CharactersFreeRoaming, List<GameObject> ___freeRoamingChars)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return true;
            if (!SettingsManager.Characters_RandomizeFreeRoamingCharacters!.Value)
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
                            IEnumerable<string>? characterPool = CharacterPools.GetFreeRoamingPoolForBiome(__instance.biome.type);
                            Character? component = null;
                            if (characterPool != null)
                                component = Core.AddPrefab(characterPool.RandomItem(), pointWithinBounds, Quaternion.Euler(90f, 0f, 0f), ___CharactersFreeRoaming, true).GetComponent<Character>();
                            
                            if (component != null)
                            {
                                TryPreventInfighting(component);
                                TryAdjustCharacterHealth(component, __instance.biome.type);
                                // End injection

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

            Plugin.Controller.FreeRoamingCharactersRandomized = true;
            return false;
        }



        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPostfix]
        private static void RandomizeLocationCharacters(WorldGenerator __instance)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;

            //if (Settings.Enemies_RandomizeNPCs!.Value)
            //{
            //    List<NPC> npcPool = new();
            //    foreach (Location location in __instance.locations)
            //        foreach (NPC npc in location.gameObject.GetComponentsInChildren<NPC>())
            //            npcPool.Add(npc);

            //    DarkwoodRandomizerPlugin.Logger.LogInfo($"NPC pool: {string.Join(", ", npcPool.Select(npc => npc.name))}");

            //    foreach (Location location in __instance.locations)
            //        foreach (NPC npc in location.gameObject.GetComponentsInChildren<NPC>())
            //        {
            //            NPC newNPC = npcPool[UnityEngine.Random.Range(0, npcPool.Count)];
            //            npcPool.Remove(newNPC);

            //            DarkwoodRandomizerPlugin.Logger.LogInfo($"Old: {npc.name}, New: {newNPC.name}");

            //            npc.name = newNPC.name;
            //            npc.inventory = newNPC.inventory;
            //            npc.characterDialogue = newNPC.characterDialogue;
            //        }
            //}

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => Plugin.Controller.OutsideLocationsLoaded && Plugin.Controller.LocationPositionsRandomized,
                action: () =>
                {
                    foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                    {
                        foreach (Character oldCharacter in location.charactersList.ToArray())
                        {
                            IEnumerable<string>? characterPool = null;

                            if (SettingsManager.Characters_RandomizeLocationActiveCharacters!.Value && oldCharacter.npc == null && CharacterPools.ACTIVE_CHARACTERS.Keys.Contains(oldCharacter.name.ToLower()))
                                characterPool = CharacterPools.GetLocationActivePoolForBiome(location.biomeType);
                            else if (SettingsManager.Characters_RandomizeLocationStaticCharacters!.Value && oldCharacter.npc == null && CharacterPools.STATIC_CHARACTERS.Keys.Contains(oldCharacter.name.ToLower()))
                                characterPool = CharacterPools.GetLocationStaticPoolForBiome(location.biomeType);

                            Character? newCharacter;

                            if (characterPool == null)
                                newCharacter = oldCharacter;
                            else
                            {
                                GameObject newCharacterObject = Core.AddPrefab(characterPool.RandomItem(), oldCharacter.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false);
                                Core.addToSaveable(newCharacterObject, true, true);
                                UnityEngine.Object.Destroy(oldCharacter.gameObject);

                                newCharacter = newCharacterObject.GetComponent<Character>();
                            }

                            if (newCharacter != null)
                            {
                                TryPreventInfighting(newCharacter);
                                TryAdjustCharacterHealth(newCharacter, location.biomeType);
                            }

                            location.charactersList.Remove(oldCharacter);
                            location.charactersList.Add(newCharacter);
                        }

                        foreach (CharacterSpawnPoint characterSpawnPoint in location.spawnPoints.ToArray())
                        {
                            IEnumerable<string>? characterPool = null;

                            if (SettingsManager.Characters_RandomizeLocationActiveCharacters!.Value && CharacterPools.ACTIVE_CHARACTERS.Keys.Contains(characterSpawnPoint.type.ToString().ToLower()))
                                characterPool = CharacterPools.GetLocationActivePoolForBiome(location.biomeType);
                            if (SettingsManager.Characters_RandomizeLocationStaticCharacters!.Value && CharacterPools.STATIC_CHARACTERS.Keys.Contains(characterSpawnPoint.type.ToString().ToLower()))
                                characterPool = CharacterPools.GetLocationStaticPoolForBiome(location.biomeType);

                            string newCharacterName;

                            if (characterPool == null)
                                newCharacterName = CharacterPools.ALL_CHARACTERS[characterSpawnPoint.type.ToString().ToLower()];
                            else
                                newCharacterName = characterPool.RandomItem();

                            GameObject newCharacterObject = Core.AddPrefab(newCharacterName, characterSpawnPoint.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false);
                            Core.addToSaveable(newCharacterObject, true, true);
                            UnityEngine.Object.Destroy(characterSpawnPoint.gameObject);

                            Character? newCharacter = newCharacterObject.GetComponent<Character>();

                            if (newCharacter != null)
                            {
                                TryPreventInfighting(newCharacter);
                                TryAdjustCharacterHealth(newCharacter, location.biomeType);
                            }

                            location.spawnPoints.Remove(characterSpawnPoint);
                            location.charactersList.Add(newCharacter);
                        }
                    }

                    Plugin.Controller.LocationCharactersRandomized = true;
                }
            );
        }
    }
}
