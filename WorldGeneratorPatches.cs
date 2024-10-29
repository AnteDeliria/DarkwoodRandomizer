using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DarkwoodRandomizer
{
    [HarmonyPatch(typeof(WorldGenerator))]
    internal class WorldGeneratorPatches
    {
        /* Crashes */
        //[HarmonyPatch("generateWorld")]
        //[HarmonyPrefix]
        internal static void EnableBiomeTesting(WorldGenerator __instance)
        {
            __instance.biomeTesting = true;
        }


        /* Crashes */
        //[HarmonyPatch("generateWorld")]
        //[HarmonyPrefix]
        internal static void DisablePredefinedGrid(WorldGenerator __instance)
        {
            __instance.predefinedGrid = false;
            __instance.chunkAmount = 49;
        }


        /*  Prevents map borders from being drawn
         *  Purely cosmetic
         */
        //[HarmonyPatch("createPredefinedChunks", MethodType.Enumerator)]
        //[HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> HideMapBorders(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.StoresField(AccessTools.Field(typeof(LocationMarker), "addMapElement")))
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                }
                yield return instruction;
            }
        }


        /*  Randomly rotates map borders
         *  Is confusing but purely cosmetic
         */
        [HarmonyPatch("createPredefinedChunks", MethodType.Enumerator)]
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> RandomizeMapBorders(IEnumerable<CodeInstruction> instructions)
        {
            if (!Settings.Map_RandomizeBorders!.Value)
                foreach (var instruction in instructions)
                    yield return instruction;

            foreach (var instruction in instructions)
            {
                if (instruction.StoresField(AccessTools.Field(typeof(LocationMarker), "mapElementNoRandomRot")))
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                }
                yield return instruction;
            }
        }
    }
}
