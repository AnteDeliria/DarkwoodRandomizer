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
        internal static ConfigEntry<bool>? Characters_RandomizeLocationActiveCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeLocationStaticCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeNPCs;
        internal static ConfigEntry<bool>? Characters_PreventInfighting;

        internal static ConfigEntry<bool>? CharacterAttributes_ScaleHealthByBiome;
        internal static ConfigEntry<float>? CharacterAttributes_HealthScalingRatio;
        internal static ConfigEntry<float>? CharacterAttributes_HealthScalingThresholdDryMeadow;
        internal static ConfigEntry<float>? CharacterAttributes_HealthScalingThresholdSilentForest;
        internal static ConfigEntry<float>? CharacterAttributes_HealthScalingThresholdOldWoods;
        internal static ConfigEntry<float>? CharacterAttributes_HealthScalingThresholdSwamp;
        internal static ConfigEntry<float>? CharacterAttributes_HealthVarianceRange;


        internal static ConfigEntry<bool>? Night_RandomizeCharacters;
        internal static ConfigEntry<bool>? Night_RandomizeScenarioDifficulty;


        internal static ConfigEntry<bool>? Vendors_RandomizeVendorInventory;
        internal static ConfigEntry<bool>? Vendors_EnsureStaples;
        internal static ConfigEntry<int>? Vendors_MinRandomSlots;
        internal static ConfigEntry<int>? Vendors_MaxRandomSlots;


        internal static ConfigEntry<bool>? Loot_ShuffleItemContainers;
        internal static ConfigEntry<BiomeRandomizationType>? Loot_ShuffleItemContainersType;
        internal static ConfigEntry<bool>? Loot_RandomizeCharacterDrops;


        //internal static ConfigEntry<bool>? Map_RandomizeBorders;
        //internal static ConfigEntry<bool>? Map_RevealAllMapElements;



        internal static void InitializeConfigs(ConfigFile config)
        {
            Decals_RandomizeChunkGroundSprites = config.Bind
                (
                    section: "Decals",
                    key: "Randomize ground decals",
                    defaultValue: true,
                    description: "COSMETIC: Expands the biome-specific ground decal pools to include sprites from every biome"
                );


            Locations_RandomizeLocationPosition = config.Bind
                (
                    section: "Locations",
                    key: "Randomize location position",
                    defaultValue: true,
                    description: "Randomizes the position of non-border locations (excluding hideouts) by allowing them to be placed anywhere on the map"
                );
            Locations_RandomizeHideoutPosition = config.Bind
                (
                    section: "Locations",
                    key: "Randomize hideout position",
                    defaultValue: false,
                    description: "Randomizes the position of hideout locations by allowing them to be placed anywhere on the map"
                );
            Locations_RandomizeLocationRotation = config.Bind
                (
                    section: "Locations",
                    key: "Randomize location rotation",
                    defaultValue: true,
                    description: "Allows non-border locations (excluding hideouts) to be rotated a full range of 360 degrees"
                );
            Locations_RandomizeHideoutRotation = config.Bind
                (
                    section: "Locations",
                    key: "Randomize hideout rotation",
                    defaultValue: true,
                    description: "Allows hideout locations to be rotated a full range of 360 degrees"
                );


            GridObjects_ShuffleGridObjects = config.Bind
                (
                    section: "Grid Objects",
                    key: "Shuffle grid objects",
                    defaultValue: true,
                    description: "Shuffles the position of grid objects, allowing them to spawn outside their target biome"
                );
            GridObjects_RandomizeGridObjectRotation = config.Bind
                (
                    section: "Grid Objects",
                    key: "Randomize grid object rotation",
                    defaultValue: true,
                    description: "Allows grid objects to be rotated a full range of 360 degrees"
                );


            MiscObjects_RandomizeMiscObjects = config.Bind
                (
                    section: "Misc Objects",
                    key: "Shuffle misc objects",
                    defaultValue: false,
                    description: "Shuffles the position of misc objects, allowing them to spawn outside their target biome"
                );


            Characters_RandomizeFreeRoamingCharacters = config.Bind
                (
                    section: "Characters",
                    key: "Randomize free roaming characters",
                    defaultValue: true,
                    description: "Randomizes spawns for free roaming characters - characters that are not tied to any location or grid object. Character pools are defined within DarkwoodRandomizer/CharacterPools/[LocationFreeRoamingCharactersDryMeadow.txt, LocationFreeRoamingCharactersSilentForest.txt, LocationFreeRoamingCharactersOldWoods.txt, LocationFreeRoamingCharactersSwamp.txt]"
                );
            Characters_RandomizeLocationActiveCharacters = config.Bind
                (
                    section: "Characters",
                    key: "Randomize location active characters",
                    defaultValue: true,
                    description: "Randomizes spawns for location active characters - characters that are tied to a location and exhibit movement. Character pools are defined within DarkwoodRandomizer/CharacterPools/[LocationActiveCharactersDryMeadow.txt, LocationActiveCharactersSilentForest.txt, LocationActiveCharactersOldWoods.txt, LocationActiveCharactersSwamp.txt]"
                );
            Characters_RandomizeLocationStaticCharacters = config.Bind
                (
                    section: "Characters",
                    key: "Randomize location static characters",
                    defaultValue: true,
                    description: "Randomizes spawns for location static characters - characters that are tied to a location and do not exhibit any movement. Character pools are defined within DarkwoodRandomizer/CharacterPools/[LocationStaticCharactersDryMeadow.txt, LocationStaticCharactersSilentForest.txt, LocationStaticCharactersOldWoods.txt, LocationStaticCharactersSwamp.txt]"
                );
            Characters_PreventInfighting = config.Bind
                (
                    section: "Characters",
                    key: "Prevent character infighting",
                    defaultValue: false,
                    description: "Prevents characters from different factions from attacking and killing each other"
                );


            CharacterAttributes_ScaleHealthByBiome = config.Bind
                (
                    section: "Character.Attributes",
                    key: "Scale character health",
                    defaultValue: true,
                    description: "Reduces the health of characters which are above a biome-specific threshold, making tough enemies easier to kill in earlier biomes. Character health is calculated as BaseHealth + (BaseHealth - Threshold) / ScalingRatio"
                );
            CharacterAttributes_HealthScalingRatio = config.Bind
                (
                    section: "Character.Attributes",
                    key: "Health scaling ratio",
                    defaultValue: 2f,
                    description: "Any health above biome-specific character health thresholds will be scaled down by this value"
                );
            CharacterAttributes_HealthScalingThresholdDryMeadow = config.Bind
                (
                    section: "Character.Attributes",
                    key: "Health scaling threshold - Dry Meadow",
                    defaultValue: 20f,
                    description: "Characters above this threshold in the Dry Meadow biome will have their health scaled down"
                );
            CharacterAttributes_HealthScalingThresholdSilentForest = config.Bind
                (
                    section: "Character.Attributes",
                    key: "Health scaling threshold - Silent Forest",
                    defaultValue: 60f,
                    description: "Characters above this threshold in the Silent Forest biome will have their health scaled down"
                );
            CharacterAttributes_HealthScalingThresholdOldWoods = config.Bind
                (
                    section: "Character.Attributes",
                    key: "Health scaling threshold - Old Woods",
                    defaultValue: 90f,
                    description: "Characters above this threshold in the Old Woods biome will have their health scaled down"
                );
            CharacterAttributes_HealthScalingThresholdSwamp = config.Bind
                (
                    section: "Character.Attributes",
                    key: "Health scaling threshold - Swamp",
                    defaultValue: 120f,
                    description: "Characters above this threshold in the Swamp biome will have their health scaled down"
                );
            CharacterAttributes_HealthVarianceRange = config.Bind
                (
                    section: "Character.Attributes",
                    key: "Character health variance percentage",
                    defaultValue: 50f,
                    description: "The percentage of character health that is allowed to vary. Character health is uniformly distributed between BaseHealth ± BaseHealth * HealthVarianceRange/100"
                );


            Night_RandomizeCharacters = config.Bind
                (
                    section: "Night",
                    key: "Randomize night characters",
                    defaultValue: true,
                    description: "Randomizes spawns for night characters - characters that spawn during night scenarios. Character pools are defined within DarkwoodRandomizer/CharacterPools/NightCharacters.txt"
                );
            Night_RandomizeScenarioDifficulty = config.Bind
                (
                    section: "Night",
                    key: "Randomize night scenario difficulty",
                    defaultValue: true,
                    description: "Expands the hideout-specific night scenario pools to include scenarios from every hideout. Some night events will fail to spawn due to missing hideout-specific elements"
                );


            Vendors_RandomizeVendorInventory = config.Bind
                (
                    section: "Vendors",
                    key: "Randomize vendor inventory",
                    defaultValue: true,
                    description: "Randomizes the items sold by vendors. Item pools are defined within DarkwoodRandomizer/ItemPools/[VendorWolfman.txt, VendorPiotrek.txt, VendorNightTrader.txt]"
                );
            Vendors_EnsureStaples = config.Bind
                (
                    section: "Vendors",
                    key: "Guarantee vendor staples",
                    defaultValue: true,
                    description: "Guarantee key items in trader inventories. Wolfman - old woods map; Piotrek - cable, old woods map; Night Trader - gasoline, planks, nails, scrap, wires, rags, matchsticks"
                );
            Vendors_MinRandomSlots = config.Bind
                (
                    section: "Vendors",
                    key: "Vendor inventory min random slots",
                    defaultValue: 6,
                    description: "Minimum number of randomized inventory slots that will be present in trader inventories"
                );
            Vendors_MaxRandomSlots = config.Bind
                (
                    section: "Vendors",
                    key: "Trader inventory max random slots",
                    defaultValue: 18,
                    description: "Maximum number of randomized inventory slots that will be present in trader inventories"
                );


            Loot_ShuffleItemContainers = config.Bind
                (
                    section: "Loot",
                    key: "Shuffle item containers",
                    defaultValue: true,
                    description: "Shuffles the contents of item containers found within locations and grid objects"
                );
            Loot_ShuffleItemContainersType = config.Bind
                (
                    section: "Loot",
                    key: "Item container shuffle type",
                    defaultValue: BiomeRandomizationType.WithinBiome,
                    description: "Global - shuffle item containers globally\nWithin Biome - shuffle item containers within each biome"
                );
            Loot_RandomizeCharacterDrops = config.Bind
                (
                    section: "Loot",
                    key: "Randomize character drops",
                    defaultValue: true,
                    description: "Randomizes the items dropped by killed characters. Item pools are defined within DarkwoodRandomizer/ItemPools/CharacterLoot.txt"
                );



            //Map_RevealAllMapElements = config.Bind
            //    (
            //        section: "Map",
            //        key: "Reveal all map elements",
            //        defaultValue: false,
            //        description: "Whether to show all locations on the map. Use this for testing out the mod"
            //    );


            //Map_RandomizeBorders = config.Bind
            //    (
            //        section: "Map",
            //        key: "Randomize map borders",
            //        defaultValue: false,
            //        description: "COSMETIC: Whether to randomize map borders. Makes the map screen more confusing"
            //    );
        }
    }
}
