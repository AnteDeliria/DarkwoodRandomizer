﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using DarkwoodRandomizer.Plugin.Settings;

namespace DarkwoodRandomizer.Plugin;

[BepInPlugin("deliria.darkwood.darkwoodrandomizer", "Darkwood Randomizer", "1.0.0")]
[BepInProcess("Darkwood.exe")]
public class DarkwoodRandomizerPlugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;


    private void Awake()
    {
        Logger = base.Logger;

        SettingsManager.InitializeConfigs(Config);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    private void Update()
    {
        Utils.Update();
    }
}
