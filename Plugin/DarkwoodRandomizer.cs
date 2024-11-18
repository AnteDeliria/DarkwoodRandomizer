using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using DarkwoodRandomizer.Plugin.Settings;
using System.IO;

namespace DarkwoodRandomizer.Plugin;

[BepInPlugin("deliria.darkwood.darkwoodrandomizer", "Darkwood Randomizer", "1.0.0")]
[BepInProcess("Darkwood.exe")]
public class DarkwoodRandomizer : BaseUnityPlugin
{
    internal static string PluginPath => Path.Combine(Paths.PluginPath, "DarkwoodRandomizer");

    internal new static ManualLogSource Logger;


    private void Awake()
    {
        Logger = base.Logger;
        SettingsManager.InitializeConfigs(Config);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    private void Update()
    {
        Controller.Update();
    }
}
