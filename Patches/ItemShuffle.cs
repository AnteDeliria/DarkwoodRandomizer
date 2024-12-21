using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class ItemShuffle
    {
        [HarmonyPatch(typeof(WorldGenerator), "distributeMustSpawnItems")]
        [HarmonyPrefix]
        private static void RandomizeItemContainers(WorldGenerator __instance, GameObject ___WorldChunksGO)
        {
            if (!(Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh1 || Plugin.Controller.WorldGeneratorState == GameState.GeneratingCh2))
                return;
            if (!SettingsManager.ItemShuffle_ShuffleItemContainers!.Value)
                return;

            Plugin.Controller.RunWhenPredicateMet
            (
                predicate: () => Plugin.Controller.OutsideLocationsLoaded && Plugin.Controller.LocationPositionsRandomized,
                action: () =>
                {
                    if (SettingsManager.ItemShuffle_ShuffleItemContainersType!.Value == BiomeRandomizationType.WithinBiome)
                        RandomizeItemContainersWithinBiomes();
                    else if (SettingsManager.ItemShuffle_ShuffleItemContainersType!.Value == BiomeRandomizationType.Global)
                        RandomizeItemContainersGlobally();

                    Plugin.Controller.ItemContainersRandomized = true;
                }
            );
        }


        private static IEnumerable<Inventory> GetItemContainers()
        {
            GameObject worldChunksGO = (GameObject)AccessTools.Field(typeof(WorldGenerator), "WorldChunksGO").GetValue(Singleton<WorldGenerator>.Instance);
            IEnumerable<Inventory> containers = worldChunksGO.GetComponentsInChildren<Inventory>(includeInactive: true);

            if (SettingsManager.ItemShuffle_ShuffleItemContainersIncludeOutsideLocations!.Value)
                containers = containers.Concat(Singleton<OutsideLocations>.Instance.spawnedLocations.Values.SelectMany(loc => loc.GetComponentsInChildren<Inventory>(includeInactive: true)));
            if (!SettingsManager.ItemShuffle_ShuffleItemContainersIncludeEmptyContainers!.Value)
                containers = containers.Where(inv => inv.slots.Any(slot => !InvItemClass.isNull(slot.invItem)));
            if (!SettingsManager.ItemShuffle_ShuffleItemContainersIncludeKeyAndQuestItems!.Value)
                containers = containers.Where(inv => !inv.slots.Any(slot => ItemPools.KEY_ITEMS.Keys.Concat(ItemPools.QUEST_ITEMS.Keys).Contains(slot.invItem?.type)));

            containers = containers
                .Where(inv => inv.invType == Inventory.InvType.itemInv)
                .Where(inv => inv.gameObject.GetComponent<Workbench>() == null)
                .Where(inv => inv.gameObject.GetComponent<Saw>() == null)
                .Where(inv => inv.gameObject.GetComponent<Trigger>() == null)
                .Where(inv => inv.gameObject.GetComponent<Padlock>() == null)
                .Where(inv => inv.gameObject.GetComponent<Item>()?.switchable == false)
                .Where(inv => inv.gameObject.GetComponent<Item>()?.isDroppedItem == false)
                .Where(inv => !(inv.gameObject.name == "LootContainer_MetalCrate_big_2A")); // These are normally unreachable

            return containers;
        }


        private static void RandomizeItemContainersGlobally()
        {
            GameObject worldChunksGO = (GameObject)AccessTools.Field(typeof(WorldGenerator), "WorldChunksGO").GetValue(Singleton<WorldGenerator>.Instance);

            IEnumerable<Inventory> inventoriesPool = GetItemContainers();

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

            IEnumerable<Inventory> inventoriesList = GetItemContainers();

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
