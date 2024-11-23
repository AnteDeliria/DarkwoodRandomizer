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
        internal static ConfigEntry<bool>? GridObjects_RandomizeGridObjectRotation;


        internal static ConfigEntry<bool>? MiscObjects_RandomizeMiscObjects;


        internal static ConfigEntry<bool>? Characters_RandomizeFreeRoamingCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeLocationCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeStaticCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeNPCs;
        internal static ConfigEntry<bool>? Characters_PreventInfighting;

        internal static ConfigEntry<bool>? CharacterScaling_AdjustHealth;
        internal static ConfigEntry<float>? CharacterScaling_MeanHealthDryMeadow;
        internal static ConfigEntry<float>? CharacterScaling_MeanHealthSilentForest;
        internal static ConfigEntry<float>? CharacterScaling_MeanHealthOldWoods;
        internal static ConfigEntry<float>? CharacterScaling_MeanHealthSwamp;
        internal static ConfigEntry<float>? CharacterScaling_StdevHealthDryMeadow;
        internal static ConfigEntry<float>? CharacterScaling_StdevHealthSilentForest;
        internal static ConfigEntry<float>? CharacterScaling_StdevHealthOldWoods;
        internal static ConfigEntry<float>? CharacterScaling_StdevHealthSwamp;


        internal static ConfigEntry<bool>? Night_RandomizeCharacters;
        internal static ConfigEntry<bool>? Night_RandomizeScenarioDifficulty;


        internal static ConfigEntry<bool>? Traders_RandomizeTraderInventory;
        internal static ConfigEntry<bool>? Traders_TraderInventoryEnsureStaples;
        internal static ConfigEntry<int>? Traders_TraderInventoryMinRandomSlots;
        internal static ConfigEntry<int>? Traders_TraderInventoryMaxRandomSlots;


        internal static ConfigEntry<bool>? Loot_ShuffleItemContainers;
        internal static ConfigEntry<BiomeRandomizationType>? Loot_ShuffleItemContainersType;
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
            Characters_PreventInfighting = config.Bind
                (
                    section: "Characters",
                    key: "Prevent infighting",
                    defaultValue: false,
                    description: "Prevent characters from fighting each other"
                );


            CharacterScaling_AdjustHealth = config.Bind
                (
                    section: "Character Scaling",
                    key: "Adjust health by biome",
                    defaultValue: false,
                    description: "Adjust character health according to biome"
                );
            CharacterScaling_MeanHealthDryMeadow = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Dry Meadow",
                    defaultValue: 20f,
                    description: ""
                );
            CharacterScaling_MeanHealthSilentForest = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Silent Forest",
                    defaultValue: 60f,
                    description: ""
                );
            CharacterScaling_MeanHealthOldWoods = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Old Woods",
                    defaultValue: 90f,
                    description: ""
                );
            CharacterScaling_MeanHealthSwamp = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Swamp",
                    defaultValue: 120f,
                    description: ""
                );
            CharacterScaling_StdevHealthDryMeadow = config.Bind
                (
                    section: "Character Scaling",
                    key: "Standard deviation of character health - Dry Meadow",
                    defaultValue: 5f,
                    description: ""
                );
            CharacterScaling_StdevHealthSilentForest = config.Bind
                (
                    section: "Character Scaling",
                    key: "Standard deviation of character health - Silent Forest",
                    defaultValue: 10f,
                    description: ""
                );
            CharacterScaling_StdevHealthOldWoods = config.Bind
                (
                    section: "Character Scaling",
                    key: "Standard deviation of character health - Old Woods",
                    defaultValue: 15f,
                    description: ""
                );
            CharacterScaling_StdevHealthSwamp = config.Bind
                (
                    section: "Character Scaling",
                    key: "Standard deviation of character health - Swamp",
                    defaultValue: 20f,
                    description: ""
                );


            Night_RandomizeCharacters = config.Bind
                (
                    section: "Night",
                    key: "Randomize night enemies",
                    defaultValue: false,
                    description: "Whether to randomize night enemies"
                );
            Night_RandomizeScenarioDifficulty = config.Bind
                (
                    section: "Night",
                    key: "Randomize scenario difficulty",
                    defaultValue: false,
                    description: "Allow night scenarios from any difficulty level to be picked"
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
            Loot_ShuffleItemContainersType = config.Bind
                (
                    section: "Loot",
                    key: "How to shuffle item containers",
                    defaultValue: BiomeRandomizationType.WithinBiome,
                    description: "Global - Shuffle item containers globally\nWithin Biome - Shuffle item containers within each biome"
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
