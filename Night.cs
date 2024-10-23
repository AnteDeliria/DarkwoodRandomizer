using HarmonyLib;
using System;

namespace DarkwoodRandomizer
{
    [HarmonyPatch]
    internal class Night
    {
        private static CharacterType[] possibleCharacters = (CharacterType[])Enum.GetValues(typeof(CharacterType));


        [HarmonyPatch(typeof(CharacterSpawner), "spawnCharacterAround")]
        [HarmonyPrefix]
        internal static void RandomizeNightEnemies(ref string type)
        {
            if (!Settings.Night_RandomizeEnemies.Value)
                return;

            type = possibleCharacters[UnityEngine.Random.Range(0, possibleCharacters.Length)].ToString();
        }
    }
}
