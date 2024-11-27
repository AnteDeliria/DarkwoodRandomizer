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

            // Must drop key
            if (new string[] { "doctor_confronted", "doctor_confronted2", "doctor_idle", "doctor_trapset" }.Contains(__instance.name.ToLower()))
                return;

            Inventory? inventory = __instance.gameObject.GetComponent<Inventory>();
            if (inventory == null || inventory.invType != Inventory.InvType.deathDrop)
                return;

            IEnumerable<string>? itemPool = ItemPools.CharacterLoot?.Keys;
            if (itemPool == null)
                return;

            InvSlot? firstSlot = inventory.slots.FirstOrDefault();
            if (firstSlot == null)
                return;

            inventory.clear();

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

            if (inventory.GetComponent<Selectable>() == null)
            {
                inventory.gameObject.AddComponent<Selectable>();
            }
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
                        .Where(inv => !(inv.gameObject.name == "LootContainer_MetalCrate_big_2A")) // These are normally unreachable
                        .Where(inv => inv.slots.Any(slot => !string.IsNullOrEmpty(slot?.invItem?.type)))
                        .ToList();

            foreach (Inventory sourceInventory in inventoriesPool.ToArray())
            {
                Inventory targetInventory = inventoriesPool.RandomItem();

                (targetInventory.maxColumns, sourceInventory.maxColumns) = (sourceInventory.maxColumns, targetInventory.maxColumns);
                (targetInventory.currentColumn, sourceInventory.currentColumn) = (sourceInventory.currentColumn, targetInventory.currentColumn);
                (targetInventory.currentRow, sourceInventory.currentRow) = (sourceInventory.currentRow, targetInventory.currentRow);
                (targetInventory.position, sourceInventory.position) = (sourceInventory.position, targetInventory.position);
                (targetInventory.slots, sourceInventory.slots) = (sourceInventory.slots, targetInventory.slots);
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
                        .Where(inv => !(inv.gameObject.name == "LootContainer_MetalCrate_big_2A")) // These are normally unreachable
                        .Where(inv => inv.slots.Any(slot => !string.IsNullOrEmpty(slot?.invItem?.type)));

            foreach (Inventory inventory in inventoriesList)
            {
                if (inventory.slots.Count == 18 && inventory.slots[0].invItem.type == "barrelExploding")
                    DarkwoodRandomizerPlugin.Logger.LogInfo($"Name: {inventory.gameObject.name}, Location: {inventory.gameObject.transform.position}");
            }

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
                foreach (Inventory sourceInventory in inventoriesPool[biome].ToArray())
                {
                    Inventory targetInventory = inventoriesPool[biome].RandomItem();

                    (targetInventory.maxColumns, sourceInventory.maxColumns) = (sourceInventory.maxColumns, targetInventory.maxColumns);
                    (targetInventory.currentColumn, sourceInventory.currentColumn) = (sourceInventory.currentColumn, targetInventory.currentColumn);
                    (targetInventory.currentRow, sourceInventory.currentRow) = (sourceInventory.currentRow, targetInventory.currentRow);
                    (targetInventory.position, sourceInventory.position) = (sourceInventory.position, targetInventory.position);
                    (targetInventory.slots, sourceInventory.slots) = (sourceInventory.slots, targetInventory.slots);
                }
        }
    }
}
