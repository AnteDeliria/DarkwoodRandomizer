using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Traders
    {
        private static int MaxNPCSlots = 42;


        // TODO: tweak stack size for each item individually
        // Do not randomize for some NPCs?
        [HarmonyPatch(typeof(InventoryRandom), "randomize")]
        [HarmonyPrefix]
        internal static bool RandomizeTraderInventory(InventoryRandom __instance, Inventory? ___inventory)
        {
            if (!SettingsManager.Traders_RandomizeTraderInventory!.Value)
                return true;
            if (___inventory == null || __instance.gameObject?.GetComponent<NPC>() == null)
                return true;

            string npcName = __instance.gameObject.GetComponent<NPC>().name;

            IEnumerable<string>? iremPool = npcName switch
            {
                "piotrek" => ItemPools.VendorPiotrek,
                "wolfman" => ItemPools.VendorWolfman,
                "nightTrader" => ItemPools.VendorNightTrader,
                "theThree" => ItemPools.VendorNightTrader,
                _ => null
            };
            if (iremPool == null)
                return true;

            int assignedSlots = 0;

            if (SettingsManager.Traders_TraderInventoryEnsureStaples!.Value)
            {
                if (npcName == "piotrek")
                {
                    ___inventory.addItem(new InvItemClass("cable", 1f, 1), true);
                    ___inventory.addItem(new InvItemClass("map_bio3", 1f, 1), true);
                    assignedSlots += 2;
                }
                else if (npcName == "wolfman")
                {
                    ___inventory.addItem(new InvItemClass("map_bio3", 1f, 1), true);
                    assignedSlots += 1;
                }
                else if (npcName == "nightTrader" || npcName == "theThree")
                {
                    ___inventory.addItem(new InvItemClass("gasoline", UnityEngine.Random.Range(0.7f, 1f), 1), true);
                    ___inventory.addItem(new InvItemClass("wood", 1, UnityEngine.Random.Range(1, 11)), true);
                    ___inventory.addItem(new InvItemClass("wood", 1, UnityEngine.Random.Range(1, 11)), true);
                    ___inventory.addItem(new InvItemClass("nail", 1, UnityEngine.Random.Range(1, 51)), true);
                    ___inventory.addItem(new InvItemClass("junk", 1, UnityEngine.Random.Range(1, 11)), true);
                    ___inventory.addItem(new InvItemClass("wire", 1, UnityEngine.Random.Range(1, 3)), true);
                    ___inventory.addItem(new InvItemClass("rag", 1, UnityEngine.Random.Range(1, 6)), true);
                    ___inventory.addItem(new InvItemClass("matchstick", 1, UnityEngine.Random.Range(1, 21)), true);
                    assignedSlots += 8;
                }
            }

            int slotsToAssign = UnityEngine.Random.Range(SettingsManager.Traders_TraderInventoryMinRandomSlots!.Value + assignedSlots, SettingsManager.Traders_TraderInventoryMaxRandomSlots!.Value + assignedSlots + 1);
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

                nextFreeSlot.createItem(itemName, amount, durability, InvItem.ModifierQuality.none, false);
                assignedSlots++;
            }

            return false;
        }
    }
}
