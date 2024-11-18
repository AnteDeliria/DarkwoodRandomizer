using DarkwoodRandomizer.Patches;
using DarkwoodRandomizer.Plugin.Settings;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace DarkwoodRandomizer.Plugin
{
    [HarmonyPatch]
    internal static class Controller
    {
        internal static GameState GameState { get; private set; } = GameState.Unknown;

        internal static bool OutsideLocationsLoaded =>
            Locations.OutsideLocationsCh1.Count == Singleton<OutsideLocations>.Instance.spawnedLocations.Count &&
            !Singleton<OutsideLocations>.Instance.playerInOutsideLocation;

        internal static bool LocationCharactersRandomized { get; set; } = false;

        internal static bool FreeRoamingCharactersRandomized { get; set; } = false;

        internal static bool GridObjectsShuffled { get; set; } = false;

        internal static bool LocationPositionsRandomized { get; set; } = false;

        internal static bool CharacterLootRandomized { get; set; } = false;

        internal static bool ItemContainersRandomized { get; set; } = false;


        [HarmonyPatch(typeof(WorldGenerator), "generateWorld")]
        [HarmonyPrefix]
        internal static void RegisterGameState()
        {
            GameState = GameState.Unknown;

            if (Core.doLoadChapterSave)
                if (Singleton<WorldGenerator>.Instance.chapterID == 1)
                    GameState = GameState.LoadingCh1;
                else if (Singleton<WorldGenerator>.Instance.chapterID == 2)
                    GameState = GameState.LoadingCh2;
            else
                if (Singleton<WorldGenerator>.Instance.chapterID == 1)
                    GameState = GameState.GeneratingCh1;
                else if (Singleton<WorldGenerator>.Instance.chapterID == 2)
                    GameState = GameState.GeneratingCh2;
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
