using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Loot
    {
        [HarmonyPatch(typeof(Character), "die")]
        [HarmonyPrefix]
        private static void RandomizeCharacterLoot(Character __instance)
        {
            if (!SettingsManager.Loot_RandomizeCharacterDrops!.Value)
                return;

            Inventory? inventory = __instance.gameObject.GetComponent<Inventory>();
            if (inventory == null || inventory.invType != Inventory.InvType.deathDrop)
                return;

            IEnumerable<string>? itemPool = ItemPools.CharacterLoot?.Keys;
            if (itemPool == null)
                return;

            inventory.clearSlots();

            InvSlot? firstSlot = inventory.slots.FirstOrDefault();
            if (firstSlot == null)
                return;

            string itemName = itemPool.RandomItem();

            InvItem item = Singleton<ItemsDatabase>.Instance.getItem(itemName, false);

            int amount;
            if (item.hasAmmo)
                amount = UnityEngine.Random.Range(0, item.clipSize + 1);
            else
                amount = 1;

            float durability;
            if (item.hasDurability)
                durability = UnityEngine.Random.Range(0.1f, 0.4f);
            else
                durability = 1;

            firstSlot.createItem(itemName, amount, durability);
        }


        [HarmonyPatch(typeof(WorldGenerator), "distributeMustSpawnItems")]
        [HarmonyPrefix]
        private static void RandomizeItemContainers(WorldGenerator __instance, GameObject ___WorldChunksGO)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.Loot_ShuffleItemContainers!.Value)
                return;

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => Plugin.Controller.OutsideLocationsLoaded && Plugin.Controller.LocationPositionsRandomized,
                action: () =>
                {
                    if (SettingsManager.Loot_ShuffleItemContainersType!.Value == BiomeRandomizationType.WithinBiome)
                        RandomizeItemContainersWithinBiomes();
                    else if (SettingsManager.Loot_ShuffleItemContainersType!.Value == BiomeRandomizationType.Global)
                        RandomizeItemContainersGlobally();

                    Plugin.Controller.ItemContainersRandomized = true;
                }
            );
        }

        private static void RandomizeItemContainersGlobally()
        {
            GameObject worldChunksGO = (GameObject)AccessTools.Field(typeof(WorldGenerator), "WorldChunksGO").GetValue(Singleton<WorldGenerator>.Instance);

            List<Inventory> inventoriesPool = worldChunksGO
                        .GetComponentsInChildren<Inventory>(includeInactive: true)
                        .Concat(Singleton<OutsideLocations>.Instance.gameObject
                            .GetComponentsInChildren<Inventory>(includeInactive: true))
                        .Where(inv => inv.invType == Inventory.InvType.itemInv)
                        .Where(inv => inv.gameObject.GetComponent<Trigger>() == null)
                        .Where(inv => inv.gameObject.GetComponent<Padlock>() == null)
                        .Where(inv => inv.gameObject.GetComponent<Item>()?.switchable == false)
                        .Where(inv => inv.gameObject.GetComponent<Item>()?.isDroppedItem == false)
                        .Where(inv => !inv.inSaw && !inv.isWorkbench)
                        .Where(inv => inv.slots.Any(slot => !string.IsNullOrEmpty(slot?.invItem?.type)))
                        .ToList();

            foreach (Inventory inventory in inventoriesPool.ToArray())
            {
                Inventory randomInventory = inventoriesPool.RandomItem();
                inventoriesPool.Remove(randomInventory);

                // I don't know if setting all of them is necessary
                inventory.maxColumns = randomInventory.maxColumns;
                inventory.currentColumn = randomInventory.currentColumn;
                inventory.currentRow = randomInventory.currentRow;
                AccessTools.Field(typeof(Inventory), "slotSize").SetValue(inventory, AccessTools.Field(typeof(Inventory), "slotSize").GetValue(randomInventory));
                inventory.position = randomInventory.position;
                inventory.slots = randomInventory.slots;
            }
        }

        private static void RandomizeItemContainersWithinBiomes()
        {
            GameObject worldChunksGO = (GameObject)AccessTools.Field(typeof(WorldGenerator), "WorldChunksGO").GetValue(Singleton<WorldGenerator>.Instance);

            IEnumerable<Inventory> inventoriesList = worldChunksGO
                        .GetComponentsInChildren<Inventory>(includeInactive: true)
                        .Concat(Singleton<OutsideLocations>.Instance.gameObject
                            .GetComponentsInChildren<Inventory>(includeInactive: true))
                        .Where(inv => inv.invType == Inventory.InvType.itemInv)
                        .Where(inv => inv.gameObject.GetComponent<Trigger>() == null)
                        .Where(inv => inv.gameObject.GetComponent<Padlock>() == null)
                        .Where(inv => inv.gameObject.GetComponent<Item>()?.switchable == false)
                        .Where(inv => inv.gameObject.GetComponent<Item>()?.isDroppedItem == false)
                        .Where(inv => !inv.inSaw && !inv.isWorkbench)
                        .Where(inv => inv.slots.Any(slot => !string.IsNullOrEmpty(slot?.invItem?.type)));

            DarkwoodRandomizerPlugin.Logger.LogInfo($"Item containers pool size: {inventoriesList.Count()}");

            Dictionary<Biome.Type, List<Inventory>> inventoriesPool = new()
            {
                [Biome.Type.meadow] = new(),
                [Biome.Type.forest] = new(),
                [Biome.Type.forest_mutated] = new(),
                [Biome.Type.swamp] = new(),
            };

            foreach (Inventory inventory in inventoriesList)
            {
                Biome.Type? biome = inventory.gameObject.GetComponentInParent<WorldChunk>()?.biome?.type ?? inventory.gameObject.GetComponentInParent<Location>()?.biomeType;
                if (biome != null)
                    inventoriesPool[(Biome.Type)biome].Add(inventory);
            }

            foreach (Biome.Type biome in inventoriesPool.Keys)
                foreach (Inventory inventory in inventoriesPool[biome].ToArray())
                {
                    Inventory randomInventory = inventoriesPool[biome].RandomItem();
                    inventoriesPool[biome].Remove(randomInventory);

                    // I don't know if setting all of them is necessary
                    inventory.maxColumns = randomInventory.maxColumns;
                    inventory.currentColumn = randomInventory.currentColumn;
                    inventory.currentRow = randomInventory.currentRow;
                    AccessTools.Field(typeof(Inventory), "slotSize").SetValue(inventory, AccessTools.Field(typeof(Inventory), "slotSize").GetValue(randomInventory));
                    inventory.position = randomInventory.position;
                    inventory.slots = randomInventory.slots;
                }
        }
    }
}
