using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;

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

            string npcName = __instance.gameObject.GetComponent<NPC>().name.ToLower();

            IEnumerable<string>? iremPool = npcName switch
            {
                "piotrek" => ItemPools.VendorPiotrek?.Keys,
                "wolfman" => ItemPools.VendorWolfman?.Keys,
                "wolfman_att" => ItemPools.VendorWolfman?.Keys,
                "nighttrader" => ItemPools.VendorNightTrader?.Keys,
                "thethree" => ItemPools.VendorTheThree?.Keys,
                "doctor_act2" => ItemPools.VendorDoctor?.Keys,
                _ => null
            };
            if (iremPool == null)
                return;

            ___inventory.clear();
            int assignedSlots = 0;

            if (SettingsManager.Vendors_EnsureStaples!.Value)
            {
                if (npcName == "piotrek")
                {
                    ___inventory.addItem(new InvItemClass("cable", 1f, 1), true);
                    ___inventory.addItem(new InvItemClass("chain_well", 1f, 1), true);
                    ___inventory.addItem(new InvItemClass("map_bio3", 1f, 1), true);
                    assignedSlots += 3;
                }
                else if (npcName == "wolfman" || npcName == "wolfman_att")
                {
                    ___inventory.addItem(new InvItemClass("chain_well", 1f, 1), true);
                    ___inventory.addItem(new InvItemClass("map_bio3", 1f, 1), true);
                    assignedSlots += 2;
                }
                else if (npcName == "nighttrader" || npcName == "thethree")
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

                nextFreeSlot.createItem(itemName, amount, durability, InvItem.ModifierQuality.none, false);
                assignedSlots++;
            }

            return;
        }
    }
}
