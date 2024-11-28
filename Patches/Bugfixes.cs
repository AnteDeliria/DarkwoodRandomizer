using DarkwoodRandomizer.Settings;
using HarmonyLib;
using Pathfinding;
using System.Linq;
using UnityEngine;

namespace DarkwoodRandomizer.Patches
{
    [HarmonyPatch]
    internal static class Bugfixes
    {
        // Unlocks Piotrek's door - needed because dog gets randomized away
        [HarmonyPatch(typeof(WorldGenerator), "activatePlayer")]
        [HarmonyPostfix]
        private static void OpenPiotrekDoor(WorldGenerator __instance, GameObject ___WorldChunksGO)
        {
            if (!(Plugin.Controller.GameState == GameState.GeneratingCh1))
                return;

            ___WorldChunksGO.GetComponentsInChildren<Locked>(includeInactive: true)
                ?.FirstOrDefault(
                    locked => locked.GetComponentInParent<Location>(includeInactive: true)?.name == "sub_cottage_junk_01_done" &&
                        locked.GetComponent<EventTriggers>()?.remoteTriggers?.Count == 1)
                ?.unlock();
        }


        // Prevents error message spam (needed because this errors every time it is called on a reloaded save)
        [HarmonyPatch(typeof(AstarPath), "GetNodesAround")]
        [HarmonyPrefix]
        private static bool FixGetNodesAround(AstarPath __instance, Vector3 position, int radius, GraphNode[] _nodes, Transform _transform)
        {
            GridGraph nearestGraphFast = __instance.getNearestGraphFast(position);
            if (nearestGraphFast == null || nearestGraphFast.nodes == null || nearestGraphFast.nodes.Length == 0)
                return false;

            Vector3 vector = position - (Vector3)nearestGraphFast.nodes[0].position;
            int num = Mathf.RoundToInt(vector.x / nearestGraphFast.nodeSize);
            int num2 = Mathf.RoundToInt(vector.z / nearestGraphFast.nodeSize) * nearestGraphFast.Width + num;
            int num3 = radius * 2 + 1;
            int num4 = num2 % nearestGraphFast.Width - radius;
            int num5 = num2 / nearestGraphFast.Width - radius;
            int num6 = num4;
            int num7 = num5;
            int graphIndex = (int)nearestGraphFast.graphIndex;
            try
            {
                int num8 = num3 * num3;
                for (int i = 0; i < num8; i++)
                {
                    int num9 = num6 + num7 * nearestGraphFast.Width;
                    bool flag = false;
                    if (!Singleton<OutsideLocations>.Instance.playerInOutsideLocation && !Singleton<Dreams>.Instance.dreaming)
                    {
                        if (num6 >= nearestGraphFast.Width)
                        {
                            if (num7 >= nearestGraphFast.Depth)
                            {
                                if (graphIndex - 2 < 0)
                                {
                                    continue;
                                }

                                GridGraph gridGraph = __instance.graphs[graphIndex - 2] as GridGraph;
                                _nodes[i] = gridGraph.nodes[num6 - gridGraph.Width + (num7 - gridGraph.Depth) * gridGraph.Width];
                                flag = true;
                            }
                            else if (num7 < 0)
                            {
                                if (__instance.graphs.Length <= graphIndex + 4)
                                {
                                    continue;
                                }

                                GridGraph gridGraph2 = __instance.graphs[graphIndex + 4] as GridGraph;
                                _nodes[i] = gridGraph2.nodes[num6 - gridGraph2.Width + (num7 + gridGraph2.Depth) * gridGraph2.Width];
                                flag = true;
                            }
                            else
                            {
                                if (__instance.graphs.Length <= graphIndex + 1)
                                {
                                    continue;
                                }

                                GridGraph gridGraph3 = __instance.graphs[graphIndex + 1] as GridGraph;
                                _nodes[i] = gridGraph3.nodes[num6 - gridGraph3.Width + num7 * gridGraph3.Width];
                                flag = true;
                            }
                        }

                        if (!flag && num6 < 0)
                        {
                            if (num7 >= nearestGraphFast.Depth)
                            {
                                if (graphIndex - 4 < 0)
                                {
                                    continue;
                                }

                                GridGraph gridGraph4 = __instance.graphs[graphIndex - 4] as GridGraph;
                                _nodes[i] = gridGraph4.nodes[num6 + gridGraph4.Width + (num7 - gridGraph4.Depth) * gridGraph4.Width];
                                flag = true;
                            }
                            else if (num7 < 0)
                            {
                                if (__instance.graphs.Length <= graphIndex + 2)
                                {
                                    continue;
                                }

                                GridGraph gridGraph5 = __instance.graphs[graphIndex + 2] as GridGraph;
                                if (num6 + gridGraph5.Width + (num7 + gridGraph5.Depth) * gridGraph5.Width >= 0)
                                {
                                    _nodes[i] = gridGraph5.nodes[num6 + gridGraph5.Width + (num7 + gridGraph5.Depth) * gridGraph5.Width];
                                }

                                flag = true;
                            }
                            else
                            {
                                if (graphIndex - 1 < 0)
                                {
                                    continue;
                                }

                                GridGraph gridGraph6 = __instance.graphs[graphIndex - 1] as GridGraph;
                                _nodes[i] = gridGraph6.nodes[num6 + gridGraph6.Width + num7 * gridGraph6.Width];
                                flag = true;
                            }
                        }

                        if (!flag && num7 >= nearestGraphFast.Depth)
                        {
                            if (graphIndex - 3 < 0)
                            {
                                continue;
                            }

                            GridGraph gridGraph7 = __instance.graphs[graphIndex - 3] as GridGraph;
                            _nodes[i] = gridGraph7.nodes[num6 + (num7 - gridGraph7.Depth) * gridGraph7.Width];
                            flag = true;
                        }

                        if (!flag && num7 < 0)
                        {
                            if (__instance.graphs.Length <= graphIndex + 3)
                            {
                                continue;
                            }

                            GridGraph gridGraph8 = __instance.graphs[graphIndex + 3] as GridGraph;
                            _nodes[i] = gridGraph8.nodes[num6 + (num7 + gridGraph8.Depth) * gridGraph8.Width];
                            flag = true;
                        }
                    }

                    if (!flag && num9 >= 0)
                    {
                        try
                        {
                            _nodes[i] = nearestGraphFast.nodes[num9];
                        }
                        catch
                        {
                        }
                    }

                    num6++;
                    if (num6 == num4 + num3)
                    {
                        num7++;
                        num6 = num4;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        // Prevents null animation clips and error message spam
        [HarmonyPatch(typeof(tk2dSpriteAnimator), "GetClipByNameVerbose")]
        [HarmonyPrefix]
        private static bool FixCharacterDeathClip(tk2dSpriteAnimator __instance, string name, ref tk2dSpriteAnimationClip? __result)
        {
            tk2dSpriteAnimation? library = AccessTools.Field(typeof(tk2dSpriteAnimator), "library").GetValue(__instance) as tk2dSpriteAnimation;

            if (library == null)
            {
                __result = __instance.DefaultClip;
                return false;
            }
            tk2dSpriteAnimationClip clipByName = library.GetClipByName(name);
            if (clipByName == null)
            {
                __result = __instance.DefaultClip;
                return false;
            }
            __result = clipByName;
            return false;
        }


        // Fixes character vaulting over windows
        [HarmonyPatch(typeof(CharBase), "getJumpRotation")]
        [HarmonyPrefix]
        private static bool FixJumpRotation(CharBase __instance, GameObject ___touchedJumpableObject)
        {
            // Unrotate objects so that we can apply relative position check
            // Rotation is reapplied when assigning to __instance.jumpingThroughWindowRotation
            Vector3 unrotatedJumpableObjectPosition = Quaternion.Euler(0, -___touchedJumpableObject.transform.rotation.eulerAngles.y + ___touchedJumpableObject.transform.localEulerAngles.z, 0) * ___touchedJumpableObject.transform.position;
            Vector3 unrotatedPlayerPosition = Quaternion.Euler(0, -___touchedJumpableObject.transform.rotation.eulerAngles.y + ___touchedJumpableObject.transform.localEulerAngles.z, 0) * __instance.transform.position;


            if (___touchedJumpableObject != null)
            {
                float y = ___touchedJumpableObject.transform.localEulerAngles.z;
                if ((y > 89f && y < 91f) || (y > 269f && y < 271f) || (y < -89f && y > -91f) || (y < -269f && y > -271f))
                {
                    if (unrotatedJumpableObjectPosition.x > unrotatedPlayerPosition.x)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, 90f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                    if (unrotatedJumpableObjectPosition.x < unrotatedPlayerPosition.x)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, -90f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                }
                else
                {
                    if (unrotatedJumpableObjectPosition.z > unrotatedPlayerPosition.z)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, 0f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                    if (unrotatedJumpableObjectPosition.z < unrotatedPlayerPosition.z)
                        __instance.jumpingThroughWindowRotation = new Vector3(0f, 180f - ___touchedJumpableObject.transform.localEulerAngles.z + ___touchedJumpableObject.transform.rotation.eulerAngles.y, 0f);
                }
            }

            return false;
        }
    }
}
