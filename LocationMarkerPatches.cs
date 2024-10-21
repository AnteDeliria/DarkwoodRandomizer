using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace DarkwoodRandomizer
{
    [HarmonyPatch(typeof(LocationMarker))]
    internal class LocationMarkerPatches
    {
        /*  Doesn't work for some reason */
        //[HarmonyPatch("spawnScene", MethodType.Enumerator)]
        //[HarmonyTranspiler]
        //internal static IEnumerable<CodeInstruction> RevealAllLocationsOnMap(IEnumerable<CodeInstruction> instructions)
        //{
        //    foreach (var instruction in instructions)
        //    {
        //        if (instruction.LoadsField(AccessTools.Field(typeof(MapElement), "rotation")))
        //        {
        //            yield return new CodeInstruction(OpCodes.Pop);
        //            yield return new CodeInstruction(OpCodes.Ldc_I4_1);
        //            yield return instruction;
        //        }
        //        //else if (instruction.Calls(AccessTools.Method(typeof(Map), "showElement", [typeof(MapElement)])))
        //        //{
        //        //    yield return instruction;
        //        //    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Singleton<Map>), "get_Instance"));
        //        //    yield return new CodeInstruction(OpCodes.Ldloc_3);
        //        //    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Map), "addElement"));
        //        //}
        //        else
        //            yield return instruction;
        //    }
        //}
    }
}
