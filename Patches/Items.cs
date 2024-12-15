using HarmonyLib;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Items
    {
        // No way to serialize these so they are lost on save/load

        //[HarmonyPatch(typeof(InvItemClass), "assignClass")]
        //[HarmonyPostfix]
        //private static void AlwaysInstantiateItems(InvItemClass __instance)
        //{
        //    __instance.baseClass = Singleton<ItemsDatabase>.Instance.getItem(__instance.type, true);
        //}

        //[HarmonyPatch(typeof(ItemPopup), "show", typeof(InvSlot), typeof(Inventory.InvType))]
        //[HarmonyPostfix]
        //private static void ModifyItemDescription(ItemPopup __instance, InvSlot slot)
        //{
        //    InvItem? baseItem = Singleton<ItemsDatabase>.Instance.getItem(slot.invItem.type, false);
        //    if (baseItem == null)
        //        return;

        //    if (baseItem.staminaAttackDrain != slot.invItem.baseClass.staminaAttackDrain)
        //    {
        //        ((tk2dTextMesh)AccessTools.Field(typeof(ItemPopup), "descriptionText").GetValue(__instance)).text +=
        //            $"\n{((slot.invItem.baseClass.staminaAttackDrain - baseItem.staminaAttackDrain) / baseItem.staminaAttackDrain * 100).ToString("+#;-#;0")}% Stamina Drain";
        //    }
        //}
    }
}
