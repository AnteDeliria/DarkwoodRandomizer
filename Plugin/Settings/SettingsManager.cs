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
                    defaultValue: true,
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


            CharacterScaling_AdjustHealth = config.Bind
                (
                    section: "Character Scaling",
                    key: "Adjust character health",
                    defaultValue: true,
                    description: "Adjusts character health according to specified settings. Health is normally distributed around a biome-specific mean and standard deviation"
                );
            CharacterScaling_MeanHealthDryMeadow = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Dry Meadow",
                    defaultValue: 20f,
                    description: "Mean character health for characters in the Dry Meadow biome"
                );
            CharacterScaling_MeanHealthSilentForest = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Silent Forest",
                    defaultValue: 60f,
                    description: "Mean character health for characters in the Silent Forest biome"
                );
            CharacterScaling_MeanHealthOldWoods = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Old Woods",
                    defaultValue: 90f,
                    description: "Mean character health for characters in the Old Woods biome"
                );
            CharacterScaling_MeanHealthSwamp = config.Bind
                (
                    section: "Character Scaling",
                    key: "Mean character health - Swamp",
                    defaultValue: 120f,
                    description: "Mean character health for characters in the Swamp biome"
                );
            CharacterScaling_StdevHealthDryMeadow = config.Bind
                (
                    section: "Character Scaling",
                    key: "Stdev character health - Dry Meadow",
                    defaultValue: 5f,
                    description: "Character health standard deviation for characters in the Dry Meadow biome. 68%/95%/99.7% of health values will lie within 1*Stdev/2*Stdev/3*Stdev of the Mean"
                );
            CharacterScaling_StdevHealthSilentForest = config.Bind
                (
                    section: "Character Scaling",
                    key: "Standard deviation of character health - Silent Forest",
                    defaultValue: 10f,
                    description: "Character health standard deviation for characters in the Silent Forest biome. 68%/95%/99.7% of health values will lie within 1*Stdev/2*Stdev/3*Stdev of the Mean"
                );
            CharacterScaling_StdevHealthOldWoods = config.Bind
                (
                    section: "Character Scaling",
                    key: "Standard deviation of character health - Old Woods",
                    defaultValue: 15f,
                    description: "Character health standard deviation for characters in the Old Woods biome. 68%/95%/99.7% of health values will lie within 1*Stdev/2*Stdev/3*Stdev of the Mean"
                );
            CharacterScaling_StdevHealthSwamp = config.Bind
                (
                    section: "Character Scaling",
                    key: "Standard deviation of character health - Swamp",
                    defaultValue: 20f,
                    description: "Character health standard deviation for characters in the Swamp biome. 68%/95%/99.7% of health values will lie within 1*Stdev/2*Stdev/3*Stdev of the Mean"
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
                    description: "Minimum number of random inventory slots that will be added to trader inventories"
                );
            Vendors_MaxRandomSlots = config.Bind
                (
                    section: "Vendors",
                    key: "Trader inventory max random slots",
                    defaultValue: 18,
                    description: "Maximum number of random inventory slots that will be added to trader inventories"
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
