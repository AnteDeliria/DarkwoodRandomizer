using BepInEx.Configuration;

namespace DarkwoodRandomizer.Plugin.Settings
{
    internal static class SettingsManager
    {
        internal static ConfigEntry<bool>? Decals_RandomizeChunkGroundSprites;


        internal static ConfigEntry<bool>? Locations_RandomizeLocationPosition;
        internal static ConfigEntry<bool>? Locations_RandomizeHideoutPosition;
        internal static ConfigEntry<bool>? Locations_RandomizeLocationRotation;
        internal static ConfigEntry<bool>? Locations_RandomizeHideoutRotation;


        internal static ConfigEntry<bool>? GridObjects_ShuffleGridObjects;
        internal static ConfigEntry<bool>? GridObjects_IncludeSwampObjectsInCh1Pool;
        internal static ConfigEntry<bool>? GridObjects_RandomizeGridObjectRotation;


        internal static ConfigEntry<bool>? MiscObjects_RandomizeMiscObjects;
        internal static ConfigEntry<bool>? MiscObjects_IncludeSwampObjectsInCh1Pool;


        internal static ConfigEntry<bool>? Characters_RandomizeFreeRoamingCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeLocationCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeStaticCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeNPCs;


        internal static ConfigEntry<bool>? Night_RandomizeEnemies;


        internal static ConfigEntry<bool>? Traders_RandomizeTraderInventory;
        internal static ConfigEntry<bool>? Traders_TraderInventoryEnsureStaples;
        internal static ConfigEntry<TraderInventoryRandomizationType>? Traders_TraderInventoryRandomizationType;
        internal static ConfigEntry<int>? Traders_TraderInventoryMinRandomSlots;
        internal static ConfigEntry<int>? Traders_TraderInventoryMaxRandomSlots;


        internal static ConfigEntry<bool>? Loot_ShuffleItemContainers;
        internal static ConfigEntry<bool>? Loot_ShuffleItemContainersWithinBiomes;
        internal static ConfigEntry<bool>? Loot_RandomizeCharacterDrops;


        internal static ConfigEntry<bool>? Map_RandomizeBorders;
        internal static ConfigEntry<bool>? Map_RevealAllMapElements;



        internal static void InitializeConfigs(ConfigFile config)
        {
            Decals_RandomizeChunkGroundSprites = config.Bind
                (
                    section: "Decals",
                    key: "Randomize chunk ground sprites",
                    defaultValue: false,
                    description: "COSMETIC: Whether to randomize chunk ground sprites"
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


            GridObjects_ShuffleGridObjects = config.Bind
                (
                    section: "Grid Objects",
                    key: "Randomize grid objects between biomes",
                    defaultValue: false,
                    description: "Whether to randomize the biome spawning of grid objects (e.g.: shrines, tank wrecks, log piles etc.)"
                );
            GridObjects_IncludeSwampObjectsInCh1Pool = config.Bind
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
            MiscObjects_IncludeSwampObjectsInCh1Pool = config.Bind
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


            Traders_RandomizeTraderInventory = config.Bind
                (
                    section: "Traders",
                    key: "Randomize trader inventory",
                    defaultValue: false,
                    description: "Whether to randomize trader inventory contents"
                );
            Traders_TraderInventoryEnsureStaples = config.Bind
                (
                    section: "Traders",
                    key: "Ensure staple items in trader inventories",
                    defaultValue: false,
                    description: "Ensure key/staple items are always available in trader inventories"
                );
            Traders_TraderInventoryRandomizationType = config.Bind
                (
                    section: "Traders",
                    key: "Trader inventory randomization type",
                    defaultValue: TraderInventoryRandomizationType.Themed,
                    description: ""
                );
            Traders_TraderInventoryMinRandomSlots = config.Bind
                (
                    section: "Traders",
                    key: "Trader inventory minimum number of random slots",
                    defaultValue: 6,
                    description: ""
                );
            Traders_TraderInventoryMaxRandomSlots = config.Bind
                (
                    section: "Traders",
                    key: "Trader inventory maximum number of random slots",
                    defaultValue: 18,
                    description: ""
                );


            Loot_ShuffleItemContainers = config.Bind
                (
                    section: "Loot",
                    key: "Randomize item containers",
                    defaultValue: false,
                    description: "Whether to randomize item containers by swapping their inventory contents"
                );
            Loot_ShuffleItemContainersWithinBiomes = config.Bind
                (
                    section: "Loot",
                    key: "Apply item container randomization within biomes",
                    defaultValue: false,
                    description: "Whether to randomize item containers within their respective biomes only"
                );
            Loot_RandomizeCharacterDrops = config.Bind
                (
                    section: "Loot",
                    key: "Randomize character drops",
                    defaultValue: false,
                    description: "Whether to randomize character drops"
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
