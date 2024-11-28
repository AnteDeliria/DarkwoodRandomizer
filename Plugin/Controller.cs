using DarkwoodRandomizer.Patches;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace DarkwoodRandomizer.Plugin
{
    [HarmonyPatch]
    internal static class Controller
    {
        internal static GameState GameState { get; private set; } = GameState.Unknown;

        internal static bool OutsideLocationsLoaded { get; set; }

        internal static bool LocationCharactersRandomized { get; set; }

        internal static bool FreeRoamingCharactersRandomized { get; set; }

        internal static bool GridObjectsShuffled { get; set; }

        internal static bool LocationPositionsRandomized { get; set; }

        internal static bool ItemContainersRandomized { get; set; }
        


        [HarmonyPatch(typeof(WorldGenerator), "Start")]
        [HarmonyPrefix]
        private static void ResetState(WorldGenerator __instance)
        {
            //__instance.chapterID = 2;

            OutsideLocationsLoaded = false;
            LocationCharactersRandomized = false;
            FreeRoamingCharactersRandomized = false;
            GridObjectsShuffled = false;
            LocationPositionsRandomized = false;
            ItemContainersRandomized = false;

            GameState = GameState.Unknown;

            if (Core.loadingGame)
            {
                if (__instance.chapterID == 1)
                    GameState = GameState.LoadingCh1;
                else if (__instance.chapterID == 2)
                    GameState = GameState.LoadingCh2;
            }
            else
            {
                if (__instance.chapterID == 1)
                    GameState = GameState.GeneratingCh1;
                else if (__instance.chapterID == 2)
                    GameState = GameState.GeneratingCh2;
            }

            DarkwoodRandomizerPlugin.Logger.LogInfo($"GameState: {GameState}");
        }



        private class PredicateActionTuple
        {
            internal PredicateActionTuple(Func<bool> predicate, Action action, bool exclusive)
            {
                Predicate = predicate;
                Action = action;
                Exclusive = exclusive;
            }

            internal Func<bool> Predicate;
            internal Action Action;
            internal bool Exclusive;
        }

        private static List<PredicateActionTuple> runOnUpdate = new();

        internal static void RunWhenPredicateMet(Func<bool> predicate, Action action, bool exclusive = false)
        {
            runOnUpdate.Add(new PredicateActionTuple(predicate, action, exclusive));
        }



        // Called via DarkwoodRandomizerPlugin.Update()
        internal static void Update()
        {
            List<PredicateActionTuple> toRun = new();

            foreach (PredicateActionTuple predicateActionTuple in runOnUpdate)
                if (predicateActionTuple.Predicate())
                {
                    if (predicateActionTuple.Exclusive)
                    {
                        toRun.Clear();
                        toRun.Add(predicateActionTuple);
                        break;
                    }
                    else
                        toRun.Add(predicateActionTuple);
                }

            foreach (PredicateActionTuple predicateActionTuple in toRun)
            {
                predicateActionTuple.Action();
                runOnUpdate.Remove(predicateActionTuple);
            }
        }
    }
}
