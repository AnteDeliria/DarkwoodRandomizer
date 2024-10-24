﻿using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace DarkwoodRandomizer
{
    [HarmonyPatch]
    internal static class Loot
    {


        [HarmonyPatch(typeof(WorldGenerator), "distributeMustSpawnItems")]
        [HarmonyPrefix]
        internal static void RandomizeItemContainers(WorldGenerator __instance)
        {
            if (!Settings.Loot_RandomizeItemContainers.Value)
                return;

            List<Inventory> inventoriesPool = new();

            foreach (Location location in __instance.locations)
                foreach (Item itemContainer in location.inventoriesList)
                {
                    Inventory inventory = (Inventory)AccessTools.Field(typeof(Item), "inventory").GetValue(itemContainer);
                    inventoriesPool.Add(inventory);
                }

            foreach (Location location in __instance.locations)
                foreach (Item itemContainer in location.inventoriesList)
                {
                    Inventory randomInventory = inventoriesPool[UnityEngine.Random.Range(0, inventoriesPool.Count)];
                    inventoriesPool.Remove(randomInventory);

                    Inventory inventory = (Inventory)AccessTools.Field(typeof(Item), "inventory").GetValue(itemContainer);
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
