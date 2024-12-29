using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Vendors
    {
        private static int MaxNPCSlots = 42;


        [HarmonyPatch(typeof(InventoryRandom), "spawnItems")]
        [HarmonyPostfix]
        private static void RandomizeTraderInventory(InventoryRandom __instance, Inventory? ___inventory)
        {
            if (___inventory == null || __instance.gameObject?.GetComponent<NPC>() == null)
                return;
            if (!SettingsManager.Vendors_RandomizeVendorInventory!.Value)
                return;
            SettingsManager.ValidateSettings();


            string npcName = __instance.gameObject.GetComponent<NPC>().name.ToLower();

            IEnumerable<string>? iremPool = npcName switch
            {
                "piotrek" => ItemPools.VendorPiotrek,
                "wolfman" => ItemPools.VendorWolfman,
                "wolfman_att" => ItemPools.VendorWolfman,
                "nighttrader" => ItemPools.VendorNightTrader,
                "thethree" => ItemPools.VendorTheThree,
                _ => null
            };
            if (iremPool == null)
                return;

            ___inventory.clear();

            if (SettingsManager.Vendors_EnsureStaples!.Value)
            {
                if (npcName == "piotrek")
                {
                    ___inventory.addItem(new InvItemClass("cable", 1f, 1), true);
                    ___inventory.addItem(new InvItemClass("chain_well", 1f, 1), true);
                    ___inventory.addItem(new InvItemClass("map_bio3", 1f, 1), true);
                }
                else if (npcName == "wolfman" || npcName == "wolfman_att")
                {
                    ___inventory.addItem(new InvItemClass("chain_well", 1f, 1), true);
                    ___inventory.addItem(new InvItemClass("map_bio3", 1f, 1), true);
                }
                else if (npcName == "nighttrader" || npcName == "thethree")
                {
                    ___inventory.addItem(new InvItemClass("gasoline", UnityEngine.Random.Range(0.7f, 1f), 1), true);
                    ___inventory.addItem(new InvItemClass("gasoline", UnityEngine.Random.Range(0.7f, 1f), 1), true);
                    ___inventory.addItem(new InvItemClass("wood", 1, 10), true);
                    ___inventory.addItem(new InvItemClass("wood", 1, 5), true);
                    ___inventory.addItem(new InvItemClass("nail", 1, 20), true);
                    ___inventory.addItem(new InvItemClass("junk", 1, 3), true);
                    ___inventory.addItem(new InvItemClass("wire", 1, 2), true);
                    ___inventory.addItem(new InvItemClass("rag", 1, 3), true);
                    ___inventory.addItem(new InvItemClass("matchstick", 1, UnityEngine.Random.Range(10, 16)), true);
                    ___inventory.addItem(new InvItemClass("ammo_single_pellet", 1, 1), true);
                    ___inventory.addItem(new InvItemClass("ammo_single_shotgun", 1, 1), true);
                    ___inventory.addItem(new InvItemClass("ammo_single_mediumCal", 1, 1), true);
                    ___inventory.addItem(new InvItemClass("ammo_clip_smallCal", 1, 1), true);
                }
            }

            int assignedSlots = ___inventory.slots.Where(s => !InvItemClass.isNull(s.invItem)).Count();
            int slotsToAssign = UnityEngine.Random.Range(SettingsManager.Vendors_MinRandomSlots!.Value + assignedSlots, SettingsManager.Vendors_MaxRandomSlots!.Value + assignedSlots + 1);
            while (assignedSlots < slotsToAssign && assignedSlots < MaxNPCSlots)
            {
                InvSlot? nextFreeSlot = ___inventory.getNextFreeSlot();
                if (nextFreeSlot == null)
                    break;

                string itemName = iremPool.RandomItem();
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

                InvItemClass createdItem = nextFreeSlot.createItem(itemName, amount, durability, InvItem.ModifierQuality.none, false);
                assignedSlots++;

                if (!SettingsManager.WeaponUpgrades_RandomizeWeaponUpgrades!.Value)
                    continue;

                while (createdItem.upgrades.Count < SettingsManager.WeaponUpgrades_MaxRandomUpgades!.Value &&
                    UnityEngine.Random.Range(0f, 1f) < SettingsManager.WeaponUpgrades_RandomUpgradeChance!.Value)
                {
                    IEnumerable<ItemUpgrade> upgradePool = createdItem.baseClass.upgrades.Where(u => !createdItem.hasUpgrade(u));
                    if (upgradePool.Count() == 0)
                        break;

                    createdItem.addUpgrade(upgradePool.RandomItem());
                }

                while (createdItem.upgrades.Count < SettingsManager.WeaponUpgrades_MinRandomUpgades!.Value)
                {
                    IEnumerable<ItemUpgrade> upgradePool = createdItem.baseClass.upgrades.Where(u => !createdItem.hasUpgrade(u));
                    if (upgradePool.Count() == 0)
                        break;

                    createdItem.addUpgrade(upgradePool.RandomItem());
                }
            }

            return;
        }
    }
}
