using BepInEx.Configuration;

namespace DarkwoodRandomizer.Plugin.Settings
{
    internal static class SettingsManager
    {
        internal static ConfigEntry<bool>? World_RandomizeChunkBiomes;
        internal static ConfigEntry<string>? World_RandomizeChunkBiomesPool;

        internal static ConfigEntry<bool>? World_RandomizeChunkGroundSprites;
        internal static ConfigEntry<string>? World_RandomizeChunkGroundSpritesPool;


        internal static ConfigEntry<bool>? Locations_RandomizeLocationPosition;
        internal static ConfigEntry<bool>? Locations_RandomizeHideoutPosition;
        internal static ConfigEntry<bool>? Locations_RandomizeLocationRotation;
        internal static ConfigEntry<bool>? Locations_RandomizeHideoutRotation;


        internal static ConfigEntry<bool>? GridObjects_RandomizeGridObjects;
        internal static ConfigEntry<bool>? GridObjects_IncludeSwampObjectsInPool;
        internal static ConfigEntry<bool>? GridObjects_RandomizeGridObjectRotation;


        internal static ConfigEntry<bool>? MiscObjects_RandomizeMiscObjects;
        internal static ConfigEntry<bool>? MiscObjects_IncludeSwampObjectsInPool;


        internal static ConfigEntry<bool>? Characters_RandomizeFreeRoamingCharacters;

        internal static ConfigEntry<bool>? Characters_RandomizeLocationCharacters;

        internal static ConfigEntry<bool>? Characters_RandomizeStaticCharacters;

        internal static ConfigEntry<bool>? Characters_RandomizeNPCs;


        internal static ConfigEntry<bool>? Night_RandomizeEnemies;


        internal static ConfigEntry<bool>? Loot_RandomizeItemContainers;
        internal static ConfigEntry<bool>? Loot_RandomizeItemContainersWithinBiomes;


        internal static ConfigEntry<bool>? Map_RandomizeBorders;

        internal static ConfigEntry<bool>? Map_RevealAllMapElements;



        internal static void InitializeConfigs(ConfigFile config)
        {
            World_RandomizeChunkBiomes = config.Bind
                (
                    section: "World",
                    key: "Randomize chunk biomes",
                    defaultValue: false,
                    description: "Whether to randomize chunk biomes. Results in a lot of downstream effects, including randomizing biome specific objects, monsters, and decals"
                );
            World_RandomizeChunkBiomesPool = config.Bind
                (
                    section: "World",
                    key: "Chunk biomes selection",
                    defaultValue: "forest,forest_dense,forest_mutated,meadow",
                    description: "The biomes to choose from when randomizing chunk biomes. Valid values are \"forest\", \"forest_dense\", \"forest_mutated\", \"swamp\", \"meadow\", and \"empty\". Will default to all if no value is specified"
                );


            World_RandomizeChunkGroundSprites = config.Bind
                (
                    section: "World",
                    key: "Randomize chunk ground sprites",
                    defaultValue: false,
                    description: "COSMETIC: Whether to randomize chunk ground sprites"
                );
            World_RandomizeChunkGroundSpritesPool = config.Bind
                (
                    section: "World",
                    key: "Biome ground sprites pool",
                    defaultValue: "forest,forest_dense,forest_mutated,swamp,meadow,empty",
                    description: "COSMETIC: The biomes to choose from when randomizing chunk ground sprites. Valid values are \"forest\", \"forest_dense\", \"forest_mutated\", \"swamp\", \"meadow\", and \"empty\". Will default to all if no value is specified"
                );


            Locations_RandomizeLocationPosition = config.Bind
                (
                    section: "Locations",
                    key: "Randomize location position",
                    defaultValue: false,
                    description: "Whether to randomize the position of locations"
                );
            Locations_RandomizeHideoutPosition = config.Bind
                (
                    section: "Locations",
                    key: "Randomize hideout position",
                    defaultValue: false,
                    description: "Whether to randomize the position of hideouts"
                );
            Locations_RandomizeLocationRotation = config.Bind
                (
                    section: "Locations",
                    key: "Randomize location rotation",
                    defaultValue: false,
                    description: "Whether to randomize location rotation. Also makes vaulting a little janky. Does not affect border locations"
                );
            Locations_RandomizeHideoutRotation = config.Bind
                (
                    section: "Locations",
                    key: "Randomize hideout rotation",
                    defaultValue: false,
                    description: "Whether to randomize hideout rotation. Also makes vaulting a little janky."
                );


            GridObjects_RandomizeGridObjects = config.Bind
                (
                    section: "Grid Objects",
                    key: "Randomize grid objects between biomes",
                    defaultValue: false,
                    description: "Whether to randomize the biome spawning of grid objects (e.g.: shrines, tank wrecks, log piles etc.)"
                );
            GridObjects_IncludeSwampObjectsInPool = config.Bind
                (
                    section: "Grid Objects",
                    key: "Include swamp prefabs in grid objects pool",
                    defaultValue: false,
                    description: "Whether to allow chapter 2 (swamp) prefabs to be generated. These are significantly larger in size than chapter 1 prefabs"
                );
            GridObjects_RandomizeGridObjectRotation = config.Bind
                (
                    section: "Grid Objects",
                    key: "Randomize grid object rotation",
                    defaultValue: false,
                    description: "Whether to randomize grid object rotation"
                );


            MiscObjects_RandomizeMiscObjects = config.Bind
                (
                    section: "Misc Objects",
                    key: "Randomize miscellaneous objects between biomes",
                    defaultValue: false,
                    description: "Whether to randomize the biome spawning of miscellaneous objects (e.g.: crates, rocks, mushrooms etc.)"
                );
            MiscObjects_IncludeSwampObjectsInPool = config.Bind
                (
                    section: "Misc Objects",
                    key: "Include swamp prefabs in miscellaneous objects pool",
                    defaultValue: false,
                    description: "Whether to allow chapter 2 (swamp) prefabs to be generated. These are significantly larger in size than chapter 1 prefabs"
                );


            Characters_RandomizeFreeRoamingCharacters = config.Bind
                (
                    section: "Characters",
                    key: "Randomize free roaming enemies",
                    defaultValue: false,
                    description: "Whether to randomize free roaming enemy spawns"
                );

            Characters_RandomizeLocationCharacters = config.Bind
                (
                    section: "Characters",
                    key: "Randomize location enemies",
                    defaultValue: false,
                    description: "Whether to randomize location enemy spawns"
                );
            
            Characters_RandomizeStaticCharacters = config.Bind
                (
                    section: "Characters",
                    key: "Randomize static characters",
                    defaultValue: false,
                    description: "Whether to randomize static characters spawns"
                );


            Night_RandomizeEnemies = config.Bind
                (
                    section: "Night",
                    key: "Randomize night enemies",
                    defaultValue: false,
                    description: "Whether to randomize night enemies"
                );


            Loot_RandomizeItemContainers = config.Bind
                (
                    section: "Loot",
                    key: "Randomize item containers",
                    defaultValue: false,
                    description: "Whether to randomize item containers by swapping their inventory contents"
                );
            Loot_RandomizeItemContainersWithinBiomes = config.Bind
                (
                    section: "Loot",
                    key: "Apply item container randomization within biomes",
                    defaultValue: false,
                    description: "Whether to randomize item containers within their respective biomes only"
                );



            Map_RevealAllMapElements = config.Bind
                (
                    section: "Map",
                    key: "Reveal all map elements",
                    defaultValue: false,
                    description: "Whether to show all locations on the map. Use this for testing out the mod"
                );


            Map_RandomizeBorders = config.Bind
                (
                    section: "Map",
                    key: "Randomize map borders",
                    defaultValue: false,
                    description: "COSMETIC: Whether to randomize map borders. Makes the map screen more confusing"
                );
        }
    }
}
