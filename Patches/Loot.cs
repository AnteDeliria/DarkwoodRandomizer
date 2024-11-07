using DarkwoodRandomizer.Plugin;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Loot
    {


        [HarmonyPatch(typeof(WorldGenerator), "distributeMustSpawnItems")]
        [HarmonyPrefix]
        internal static void RandomizeItemContainers(WorldGenerator __instance)
        {
            if (!Settings.Loot_RandomizeItemContainers!.Value)
                return;

            if (Settings.Loot_RandomizeItemContainersWithinBiomes!.Value)
                Utils.RunWhenPredicateMet
                (
                    predicate: () => Locations.OutsideLocationsLoaded,
                    action: RandomizeItemContainersWithinBiomes
                );
            else
                Utils.RunWhenPredicateMet
                (
                    predicate: () => Locations.OutsideLocationsLoaded,
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
