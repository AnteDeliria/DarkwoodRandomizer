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
        internal static void RandomizeCharacterLoot(GameObject ___WorldChunksGO)
        {
            if (!SettingsManager.Loot_RandomizeCharacterDrops!.Value)
                return;
            if (Plugin.Controller.OutsideLocationsLoaded || !Plugin.Controller.IsNewSave)
                return;

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => Plugin.Controller.OutsideLocationsLoaded,
                action: () =>
                {
                    IEnumerable<string>? itemPool = ItemPools.CharacterLoot;
                    if (itemPool == null)
                        return;

                    IEnumerable<Inventory> deathDrops = ___WorldChunksGO
                        .GetComponentsInChildren<Inventory>(includeInactive: true)
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
                },
                exclusive: false
            );
        }


        [HarmonyPatch(typeof(WorldGenerator), "distributeMustSpawnItems")]
        [HarmonyPrefix]
        internal static void RandomizeItemContainers(WorldGenerator __instance)
        {
            if (!Plugin.Controller.IsNewSave)
                return;
            if (!SettingsManager.Loot_ShuffleItemContainers!.Value)
                return;

            if (SettingsManager.Loot_ShuffleItemContainersType!.Value == BiomeRandomizationType.WithinBiome)
                Plugin.Controller.RunWhenPredicateMet
                (
                    predicate: () => Plugin.Controller.OutsideLocationsLoaded,
                    action: RandomizeItemContainersWithinBiomes
                );
            else if (SettingsManager.Loot_ShuffleItemContainersType!.Value == BiomeRandomizationType.Global)
                Plugin.Controller.RunWhenPredicateMet
                (
                    predicate: () => Plugin.Controller.OutsideLocationsLoaded,
                    action: RandomizeItemContainersGlobally
                );
        }

        private static void RandomizeItemContainersGlobally()
        {
            List<Inventory> inventoriesPool = new();

            foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                foreach (Item itemContainer in location.inventoriesList) // Sublocations don't have containers in inventoriesList, it is instead tied to the main location
                {
                    if (itemContainer.name.Contains("workbench"))
                        continue;

                    Inventory inventory = (Inventory)AccessTools.Field(typeof(Item), "inventory").GetValue(itemContainer);
                    inventoriesPool.Add(inventory);
                }

            foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                foreach (Item itemContainer in location.inventoriesList)
                {
                    if (itemContainer.name.Contains("workbench"))
                        continue;

                    Inventory inventory = (Inventory)AccessTools.Field(typeof(Item), "inventory").GetValue(itemContainer);

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
            Dictionary<Biome.Type, List<Inventory>> inventoriesPool = new();

            foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                foreach (Item itemContainer in location.inventoriesList)
                {
                    if (itemContainer.name.Contains("workbench"))
                        continue;

                    Inventory inventory = (Inventory)AccessTools.Field(typeof(Item), "inventory").GetValue(itemContainer);

                    if (!inventoriesPool.ContainsKey(location.biomeType))
                        inventoriesPool.Add(location.biomeType, new List<Inventory>());

                    inventoriesPool[location.biomeType].Add(inventory);
                }

            foreach (Location location in Singleton<WorldGenerator>.Instance.locations.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values))
                foreach (Item itemContainer in location.inventoriesList)
                {
                    if (itemContainer.name.Contains("workbench"))
                        continue;

                    Inventory inventory = (Inventory)AccessTools.Field(typeof(Item), "inventory").GetValue(itemContainer);

                    List<Inventory> biomePool = inventoriesPool[location.biomeType];
                    Inventory randomInventory = biomePool.RandomItem();
                    inventoriesPool[location.biomeType].Remove(randomInventory);

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
