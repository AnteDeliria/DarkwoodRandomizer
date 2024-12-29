using DarkwoodRandomizer.Plugin;
using DarkwoodRandomizer.Pools;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class ItemDrops
    {
        [HarmonyPatch(typeof(Character), "die")]
        [HarmonyPrefix]
        private static void RandomizeCharacterLoot(Character __instance)
        {
            if (!SettingsManager.ItemDrops_RandomizeItemDrops!.Value)
                return;

            if (Singleton<Dreams>.Instance.dreaming)
                return;

            if (new string[] { "doctor_confronted", "doctor_confronted2", "doctor_idle", "doctor_trapset" }.Contains(__instance.name.ToLower()))
                return; // Do not randomize Big Metal Key

            Inventory? inventory = __instance.GetComponent<Inventory>();
            if (inventory == null || inventory?.invType != Inventory.InvType.deathDrop)
                return;

            IEnumerable<string>? itemPool = ItemPools.CharacterLoot;
            if (itemPool == null)
                return;

            SettingsManager.ValidateSettings();



            __instance.notSelectableWhenDead = false;
            inventory.clear();

            while (inventory.slots.Where(slot => !InvItemClass.isNull(slot.invItem)).Count() < SettingsManager.ItemDrops_MaxRandomDrops!.Value &&
                UnityEngine.Random.Range(0f, 1f) < SettingsManager.ItemDrops_RandomDropChance!.Value)
            {
                AddRandomItemToInventory(inventory, itemPool);
            }

            while (inventory.slots.Where(slot => !InvItemClass.isNull(slot.invItem)).Count() < SettingsManager.ItemDrops_MinRandomDrops!.Value)
            {
                AddRandomItemToInventory(inventory, itemPool);
            }
        }

        private static void AddRandomItemToInventory(Inventory inventory, IEnumerable<string> itemPool)
        {
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

            InvItemClass createdItem = inventory.addItem(new InvItemClass(itemName, durability, amount), true);

            if (!SettingsManager.WeaponUpgrades_RandomizeWeaponUpgrades!.Value)
                return;

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
    }
}
