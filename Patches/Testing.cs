using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    public static class Testing
    {
        // Test spawn characters
        public static void SpawnCharacter(string name)
        {
            Core.AddPrefab(CharacterPools.ALL_CHARACTERS[name], Player.Instance.transform.position + new Vector3(100, 0, 0), Quaternion.Euler(90f, 0f, 0f), Singleton<WorldGenerator>.Instance.gameObject, true);
            DarkwoodRandomizerPlugin.Logger.LogInfo($"Spawning {name}");
        }

        public static void SpawnCharacter(int index)
        {
            Core.AddPrefab(CharacterPools.ALL_CHARACTERS.Values.ElementAt(index), Player.Instance.transform.position + new Vector3(100, 0, 0), Quaternion.Euler(90f, 0f, 0f), Singleton<WorldGenerator>.Instance.gameObject, true);
            DarkwoodRandomizerPlugin.Logger.LogInfo($"Spawning {CharacterPools.ALL_CHARACTERS.Keys.ElementAt(index)}");
        }
    }
}
