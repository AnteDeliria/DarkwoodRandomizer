using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace DarkwoodRandomizer;

[BepInPlugin("deliria.darkwood.darkwoodrandomizer", "Darkwood Randomizer", "1.0.0")]
[BepInProcess("Darkwood.exe")]
public class DarkwoodRandomizerPlugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;



        
    private void Awake()
    {
        Logger = base.Logger;
        InitializeConfigs();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }



    private void InitializeConfigs()
    {
        Settings.World_RandomizeChunkBiomes = Config.Bind
            (
                section: "World",
                key: "Randomize chunk biomes",
                defaultValue: false,
                description: "Whether to randomize chunk biomes. Results in a lot of downstream effects, including randomizing biome specific objects, monsters, and decals"
            );
        Settings.World_RandomizeChunkBiomesPool = Config.Bind
            (
                section: "World",
                key: "Chunk biomes selection",
                defaultValue: "forest,forest_dense,forest_mutated,meadow",
                description: "The biomes to choose from when randomizing chunk biomes. Valid values are \"forest\", \"forest_dense\", \"forest_mutated\", \"swamp\", \"meadow\", and \"empty\". Will default to all if no value is specified"
            );


        Settings.World_RandomizeChunkGroundSprites = Config.Bind
            (
                section: "World",
                key: "Randomize chunk ground sprites",
                defaultValue: false,
                description: "COSMETIC: Whether to randomize chunk ground sprites"
            );
        Settings.World_RandomizeChunkGroundSpritesPool = Config.Bind
            (
                section: "World",
                key: "Biome ground sprites pool",
                defaultValue: "forest,forest_dense,forest_mutated,swamp,meadow,empty",
                description: "COSMETIC: The biomes to choose from when randomizing chunk ground sprites. Valid values are \"forest\", \"forest_dense\", \"forest_mutated\", \"swamp\", \"meadow\", and \"empty\". Will default to all if no value is specified"
            );


        Settings.Locations_RandomizeLocations = Config.Bind
            (
                section: "Locations",
                key: "Randomize locations",
                defaultValue: false,
                description: "Whether to randomize locations"
            );
        Settings.Locations_RandomizeLocationsExcludeHideouts = Config.Bind
            (
                section: "Locations",
                key: "Exclude hideouts when randomizing locations",
                defaultValue: false,
                description: "Whether to exclude hideouts when randomizing locations"
            );
        Settings.Locations_RandomizeLocationRotation = Config.Bind
            (
                section: "Locations",
                key: "Randomize location rotation",
                defaultValue: false,
                description: "Whether to randomize location rotation. Also makes vaulting a little janky. Does not affect border locations"
            );
        Settings.Locations_RandomizeHideoutRotation = Config.Bind
            (
                section: "Locations",
                key: "Randomize hideout rotation",
                defaultValue: false,
                description: "Whether to randomize hideout rotation. Also makes vaulting a little janky."
            );


        Settings.Enemies_RandomizeEnemies = Config.Bind
            (
                section: "Enemies",
                key: "Randomize enemies",
                defaultValue: false,
                description: "Whether to randomize enemies"
            );

        Settings.Night_RandomizeEnemies = Config.Bind
            (
                section: "Night",
                key: "Randomize enemies",
                defaultValue: false,
                description: "Whether to randomize night enemies"
            );




        Settings.Map_RevealAllMapElements = Config.Bind
            (
                section: "Map",
                key: "Reveal all map elements",
                defaultValue: false,
                description: "Whether to show all locations on the map. Use this for testing out the mod"
            );


        Settings.Map_RandomizeBorders = Config.Bind
            (
                section: "Map",
                key: "Randomize map borders",
                defaultValue: false,
                description: "COSMETIC: Whether to randomize map borders. Makes the map screen more confusing"
            );
    }
}
