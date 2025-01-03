﻿using BepInEx;
using BepInEx.Logging;
using DarkwoodRandomizer.Settings;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace DarkwoodRandomizer.Plugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Darkwood.exe")]
[HarmonyPatch]
public class DarkwoodRandomizerPlugin : BaseUnityPlugin
{
    internal static string PluginPath => Path.Combine(Paths.PluginPath, "DarkwoodRandomizer");

    internal new static ManualLogSource Logger;


    private void Awake()
    {
        Logger = base.Logger;
        SettingsManager.InitializeConfigs(Config);
        SettingsManager.ValidateSettings();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    private void Update()
    {
        Controller.Update();
    }


    [HarmonyPatch(typeof(MainMenu), "Start")]
    [HarmonyPostfix]
    private static void AddModName(MainMenu __instance)
    {
        __instance.CurrentVersion.text += $"\nDarkwood Randomizer {MyPluginInfo.PLUGIN_VERSION}";
    }
}
