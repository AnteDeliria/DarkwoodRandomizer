using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace DarkwoodRandomizer;

[BepInPlugin("deliria.darkwood.darkwoodrandomizer", "Darkwood Randomizer", "1.0.0")]
[BepInProcess("Darkwood.exe")]
public class DarkwoodRandomizerPlugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;


        
    private void Awake()
    {
        Logger = base.Logger;
        
        Settings.InitializeConfigs(Config);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }



    
}
