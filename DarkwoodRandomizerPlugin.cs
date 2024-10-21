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

        Settings.Locations_RandomizeExistingLocations = Config.Bind
            (
                section: "Locations",
                key: "Randomize existing locations",
                defaultValue: false,
                description: "Whether to randomize existing locations"
            );
        Settings.Locations_RandomizeExistingLocationsAllowRepeats = Config.Bind
            (
                section: "Locations",
                key: "Allow repeats when randomizing existing locations",
                defaultValue: false,
                description: "Whether to allow locations to be repeated when randomizing existing locations"
            );
        Settings.Locations_RandomizeExistingLocationsExcludeHideouts = Config.Bind
            (
                section: "Locations",
                key: "Exclude hideouts when randomizing existing locations",
                defaultValue: false,
                description: "Whether to exclude hideouts when randomizing existing locations"
            );
        Settings.Locations_RandomizeExistingLocationsPool = Config.Bind
            (
                section: "Locations",
                key: "Randomize existing locations pool",
                defaultValue: "chapter1",
                description: "The location pools to choose from when randomizing locations. Valid values are \"chapter1\", \"wolf_camps\", \"chapter2\", \"hideouts\". Will default to all if no value is specified"
            );

        Settings.Locations_AddExtraLocations = Config.Bind
            (
                section: "Locations",
                key: "Add extra locations",
                defaultValue: false,
                description: "Whether to add extra locations"
            );
        Settings.Locations_AddExtraLocationsAllowRepeats = Config.Bind
            (
                section: "Locations",
                key: "Allow repeats when adding extra locations",
                defaultValue: false,
                description: "Whether to allow locations to be repeated when adding extra locations"
            );
        Settings.Locations_AddExtraLocationsExcludeHideouts = Config.Bind
            (
                section: "Locations",
                key: "Exclude hideouts when adding extra locations",
                defaultValue: false,
                description: "Whether to exclude hideouts when adding extra locations"
            );
        Settings.Locations_AddExtraLocationsPool = Config.Bind
            (
                section: "Locations",
                key: "Add extra locations pool",
                defaultValue: "chapter1",
                description: "The location pools to choose from when adding extra locations. Valid values are \"chapter1\", \"wolf_camps\", \"chapter2\", \"hideouts\". Will default to all if no value is specified"
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
