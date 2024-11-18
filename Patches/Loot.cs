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
        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPrefix]
        private static void RandomizeCharacterLoot(GameObject ___WorldChunksGO)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1 || Plugin.Controller.GameState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.Loot_RandomizeCharacterDrops!.Value)
                return;
            if (Plugin.Controller.OutsideLocationsLoaded)
                return;

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => Plugin.Controller.OutsideLocationsLoaded && Plugin.Controller.FreeRoamingCharactersRandomized && Plugin.Controller.LocationCharactersRandomized,
                action: () =>
                {
                    IEnumerable<string>? itemPool = ItemPools.CharacterLoot;
                    if (itemPool == null)
                        return;

                    IEnumerable<Inventory> deathDrops = ___WorldChunksGO
                        .GetComponentsInChildren<Inventory>(includeInactive: true)
                        .Concat(Singleton<OutsideLocations>.Instance.gameObject
                            .GetComponentsInChildren<Inventory>(includeInactive: true))
                        .Where(inv => inv.invType == Inventory.InvType.deathDrop);

                    foreach (Inventory inventory in deathDrops)
                        foreach (InvSlot slot in inventory.slots.Where(slot => !string.IsNullOrEmpty(slot?.invItem?.type)))
                        {
                            string itemName = itemPool.RandomItem();

                            InvItem item = Singleton<ItemsDatabase>.Instance.getItem(itemName, false);

                            int amount;
                            if (item.hasAmmo)
                                amount = UnityEngine.Random.Range(0, item.clipSize + 1);
                            else if (item.stackable)
                                amount = UnityEngine.Random.Range(1, item.maxAmount + 1);
                            else
                                amount = 1;

                            float durability;
                            if (item.hasDurability)
                                durability = UnityEngine.Random.Range(0.7f, 1f);
                            else
                                durability = 1;

                            slot.createItem(itemName, amount, durability);
                        }

                    Plugin.Controller.CharacterLootRandomized = true;
                },
                exclusive: false
            );
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
                        .Where(inv => inv.gameObject.GetComponent<Saw>() == null)
                        .Where(inv => inv.gameObject.GetComponent<Workbench>() == null)
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
                        .Where(inv => inv.gameObject.GetComponent<Saw>() == null)
                        .Where(inv => inv.gameObject.GetComponent<Workbench>() == null)
                        .ToList();

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
