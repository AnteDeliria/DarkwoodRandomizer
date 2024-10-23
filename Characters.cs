using HarmonyLib;
using System;

namespace DarkwoodRandomizer
{
    [HarmonyPatch]
    internal class Characters
    {
        private static CharacterType[] possibleCharacters = (CharacterType[])Enum.GetValues(typeof(CharacterType));


        [HarmonyPatch(typeof(CharacterSpawnPoint), "actuallySpawn")]
        [HarmonyPrefix]
        internal static void RandomizeCharacters(CharacterSpawnPoint __instance)
        {
            if (!Settings.Enemies_RandomizeEnemies.Value)
                return;

            __instance.type = possibleCharacters[UnityEngine.Random.Range(0, possibleCharacters.Length)];
        }
    }
}
