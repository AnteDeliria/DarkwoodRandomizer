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

        public static void SpawnItem(string name, int amount = 1)
        {
            Player.Instance.Inventory.addItemType(name, amount);
            DarkwoodRandomizerPlugin.Logger.LogInfo($"Adding {name}");
        }

        public static void SpawnItem(int index, int amount = 1)
        {
            Player.Instance.Inventory.addItemType(ItemPools.ALL_ITEMS.Keys.ElementAt(index), amount);
            DarkwoodRandomizerPlugin.Logger.LogInfo($"Adding {ItemPools.ALL_ITEMS.Keys.ElementAt(index)}");
        }
    }
}
