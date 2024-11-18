using DarkwoodRandomizer.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace DarkwoodRandomizer.Plugin
{
    [HarmonyPatch]
    internal static class Controller
    {
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

        internal static bool IsNewSave { get; private set; } = false;

        internal static bool OutsideLocationsLoaded =>
            Locations.OutsideLocationsCh1.Count == Singleton<OutsideLocations>.Instance.spawnedLocations.Count &&
            !Singleton<OutsideLocations>.Instance.playerInOutsideLocation;

        internal static bool LocationCharactersRandomized { get; set; } = false;
        internal static bool FreeRoamingCharactersRandomized { get; set; } = false;



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

        [HarmonyPatch(typeof(WorldGenerator), "generateWorld")]
        [HarmonyPrefix]
        internal static void RegisterIsNewSave()
        {
            IsNewSave = !Core.doLoadChapterSave;
        }
    }
}
