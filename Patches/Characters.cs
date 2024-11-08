using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
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


        [HarmonyPatch(typeof(WorldChunk), "spawnFreeRoamingCharacters")]
        [HarmonyPrefix]
        internal static bool RandomizeFreeRoamingCharacters(WorldChunk __instance, GameObject ___CharactersFreeRoaming, List<GameObject> ___freeRoamingChars)
        {
            if (!Utils.IsNewSave)
                return true;
            if (!SettingsManager.Characters_RandomizeFreeRoamingCharacters!.Value)
                return true;

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
                            IEnumerable<string>? characterPool = CharacterPools.GetFreeRoamingPoolForBiome(__instance.biome.type);
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
        internal static void RandomizeLocationCharacters(WorldGenerator __instance)
        {
            if (!Utils.IsNewSave)
                return;

            // TODO: fix this not randomizing all NPCs
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

            Plugin.Utils.RunWhenPredicateMet
            (
                predicate: () => Locations.OutsideLocationsLoaded,
                action: RandomizeLocationCharacters
            );


            static void RandomizeLocationCharacters()
            {
                foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                {
                    foreach (Character character in location.charactersList.ToArray())
                    {
                        List<string>? characterPool = null;

                        if (SettingsManager.Characters_RandomizeLocationCharacters!.Value && character.npc == null && CharacterPools.ActiveCharacters.Keys.Contains(character.name))
                            characterPool = CharacterPools.GetLocationActivePoolForBiome(location.biomeType)?.ToList();
                        else if (SettingsManager.Characters_RandomizeStaticCharacters!.Value && character.npc == null && CharacterPools.StaticCharacters.Keys.Contains(character.name))
                            characterPool = CharacterPools.GetLocationStaticPoolForBiome(location.biomeType)?.ToList();

                        if (characterPool == null)
                            continue;

                        location.charactersList.Remove(character);
                        Object.Destroy(character.gameObject);

                        Character component = Core.AddPrefab(characterPool.RandomItem(), character.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false).GetComponent<Character>();
                        location.charactersList.Add(component);
                    }

                    foreach (CharacterSpawnPoint characterSpawnPoint in location.spawnPoints.ToArray())
                    {
                        List<string>? characterPool = CharacterPools.GetLocationActivePoolForBiome(location.biomeType)?.ToList();

                        if (characterPool == null)
                            continue;

                        location.spawnPoints.Remove(characterSpawnPoint);
                        Object.Destroy(characterSpawnPoint.gameObject);

                        Character component = Core.AddPrefab(characterPool.RandomItem(), characterSpawnPoint.transform.localPosition, Quaternion.Euler(90f, 0f, 0f), location.characters.gameObject, false).GetComponent<Character>();
                        location.charactersList.Add(component);
                    }
                }
            }
        }
    }
}
