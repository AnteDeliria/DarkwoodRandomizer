using BepInEx.Configuration;
using DarkwoodRandomizer.Plugin;

namespace DarkwoodRandomizer.Settings
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


        internal static ConfigEntry<bool>? Characters_RandomizeGlobalCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeLocationActiveCharacters;
        internal static ConfigEntry<bool>? Characters_RandomizeLocationStaticCharacters;
        internal static ConfigEntry<float>? Characters_HealthVarianceRange;
        internal static ConfigEntry<bool>? Characters_PreventInfighting;


        internal static ConfigEntry<bool>? Night_RandomizeCharacters;
        internal static ConfigEntry<bool>? Night_RandomizeScenarioDifficulty;


        internal static ConfigEntry<bool>? Vendors_RandomizeVendorInventory;
        internal static ConfigEntry<bool>? Vendors_EnsureStaples;
        internal static ConfigEntry<int>? Vendors_MinRandomSlots;
        internal static ConfigEntry<int>? Vendors_MaxRandomSlots;


        internal static ConfigEntry<bool>? ItemShuffle_ShuffleItemContainers;
        internal static ConfigEntry<BiomeRandomizationType>? ItemShuffle_ShuffleItemContainersType;
        internal static ConfigEntry<bool>? ItemShuffle_ShuffleItemContainersIncludeOutsideLocations;
        internal static ConfigEntry<bool>? ItemShuffle_ShuffleItemContainersIncludeEmptyContainers;
        internal static ConfigEntry<bool>? ItemShuffle_ShuffleItemContainersIncludeKeyAndQuestItems;


        internal static ConfigEntry<float>? ItemDrops_RandomDropChance;
        internal static ConfigEntry<int>? ItemDrops_MinRandomDrops;
        internal static ConfigEntry<int>? ItemDrops_MaxRandomDrops;


        internal static ConfigEntry<float>? ItemUpgrades_RandomUpgradeChance;
        internal static ConfigEntry<int>? ItemUpgrades_MinRandomUpgades;
        internal static ConfigEntry<int>? ItemUpgrades_MaxRandomUpgades;


        internal static ConfigEntry<bool>? Map_RevealAllMapElements;

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
                    description: "Shuffles the position of non-border locations (excluding hideouts) by allowing them to be placed anywhere on the map"
                );
            Locations_RandomizeHideoutPosition = config.Bind
                (
                    section: "Locations",
                    key: "Randomize hideout position",
                    defaultValue: true,
                    description: "Shuffles the position of hideout locations by allowing them to be placed anywhere within their respective biome"
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
                    description: "Shuffles the position of grid objects, allowing them to spawn outside their target biome. Does not affect chapter 2"
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
                    description: "Shuffles biome specific misc objects, allowing them to spawn outside their target biome. Does not affect chapter 2"
                );


            Characters_RandomizeGlobalCharacters = config.Bind
                (
                    section: "Characters",
                    key: "Randomize global characters",
                    defaultValue: true,
                    description: "Randomizes spawns for global characters - characters that are not tied to any location or grid object. Character pools are defined within DarkwoodRandomizer/CharacterPools/[GlobalCharactersDryMeadow.txt, GlobalCharactersSilentForest.txt, GlobalCharactersOldWoods.txt, GlobalCharactersSwamp.txt]"
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
            Characters_HealthVarianceRange = config.Bind
                (
                    section: "Characters",
                    key: "Character health variance range",
                    defaultValue: 50f,
                    description: "The percentage of character health that is allowed to vary. Character health is uniformly distributed between BaseHealth ± BaseHealth * HealthVarianceRange/100"
                );
            Characters_PreventInfighting = config.Bind
                (
                    section: "Characters",
                    key: "Prevent character infighting",
                    defaultValue: true,
                    description: "Prevents characters from different factions from attacking and killing each other"
                );


            Night_RandomizeCharacters = config.Bind
                (
                    section: "Night",
                    key: "Randomize night characters",
                    defaultValue: true,
                    description: "Randomizes spawns for night characters - characters that spawn during night scenarios. Character pools are defined within DarkwoodRandomizer/CharacterPools/[NightCharactersDryMeadow.txt, NightCharactersSilentForest.txt, NightCharactersOldWoods.txt, NightCharactersSwamp.txt]"
                );
            Night_RandomizeScenarioDifficulty = config.Bind
                (
                    section: "Night",
                    key: "Randomize night scenario difficulty",
                    defaultValue: true,
                    description: "From night 4 onwards, expands the hideout-specific night scenario pools to include scenarios from every hideout. Some night events will fail to spawn due to missing hideout-specific elements"
                );


            Vendors_RandomizeVendorInventory = config.Bind
                (
                    section: "Vendors",
                    key: "Randomize vendor inventory",
                    defaultValue: true,
                    description: "Randomizes the items sold by vendors. Item pools are defined within DarkwoodRandomizer/ItemPools/[VendorWolfman.txt, VendorPiotrek.txt, VendorNightTrader.txt, VendorTheThree.txt]"
                );
            Vendors_EnsureStaples = config.Bind
                (
                    section: "Vendors",
                    key: "Guarantee vendor staples",
                    defaultValue: true,
                    description: "Guarantee certain items in trader inventories. Wolfman - chain, old woods map; Piotrek - chain, cable, old woods map; NightTrader/TheThree - gasoline, planks, nails, scrap, wires, rags, matchsticks, pellet ammo, shotgun shell, medium caliber bullet, small caliber magazine"
                );
            Vendors_MinRandomSlots = config.Bind
                (
                    section: "Vendors",
                    key: "Vendor inventory min random slots",
                    defaultValue: 6,
                    description: "Minimum number of randomized inventory slots that will be added to trader inventories"
                );
            Vendors_MaxRandomSlots = config.Bind
                (
                    section: "Vendors",
                    key: "Trader inventory max random slots",
                    defaultValue: 18,
                    description: "Maximum number of randomized inventory slots that will added to trader inventories"
                );


            ItemShuffle_ShuffleItemContainers = config.Bind
                (
                    section: "Item Shuffle",
                    key: "Shuffle item containers",
                    defaultValue: true,
                    description: "Shuffles item containers"
                );
            ItemShuffle_ShuffleItemContainersType = config.Bind
                (
                    section: "Item Shuffle",
                    key: "Item container shuffle type",
                    defaultValue: BiomeRandomizationType.WithinBiome,
                    description: "Global - shuffle item containers globally\nWithin Biome - shuffle item containers within each biome"
                );
            ItemShuffle_ShuffleItemContainersIncludeEmptyContainers = config.Bind
                (
                    section: "Item Shuffle",
                    key: "Shuffle item containers - include empty containers",
                    defaultValue: false,
                    description: "Whether to include empty item containers in the shuffle pool"
                );
            ItemShuffle_ShuffleItemContainersIncludeKeyAndQuestItems = config.Bind
                (
                    section: "Item Shuffle",
                    key: "Shuffle item containers - include key and quest items",
                    defaultValue: false,
                    description: "Whether to include item containers containing key and quest items in the shuffle pool. WARNING: This may result in key/quest items ending up in inaccessible locations"
                );
            ItemShuffle_ShuffleItemContainersIncludeOutsideLocations = config.Bind
                (
                    section: "Item Shuffle",
                    key: "Shuffle item containers - include outside locations",
                    defaultValue: true,
                    description: "Whether to include item containers from outside locations in the shuffle pool"
                );


            ItemDrops_RandomDropChance = config.Bind
                (
                    section: "Item Drops",
                    key: "Random drop chance",
                    defaultValue: 0.5f,
                    description: "The chance (0-1) of adding a random drop to killed characters. This is rolled multiple times per character until a single failed roll occurs, or the maximum number of random items is reached"
                );
            ItemDrops_MinRandomDrops = config.Bind
                (
                    section: "Item Drops",
                    key: "Min random drops",
                    defaultValue: 1,
                    description: "The minimum number of random drops that can be dropped by killed characters. If the number of items resulting from the drop chance roll is less than this, the number of items will be set to this minimum"
                );
            ItemDrops_MaxRandomDrops = config.Bind
                (
                    section: "Item Drops",
                    key: "Max random drops",
                    defaultValue: 3,
                    description: "The maximum number of random drops that can be dropped by killed characters"
                );


            ItemUpgrades_RandomUpgradeChance = config.Bind
                (
                    section: "Item Upgrades",
                    key: "Random upgrade chance",
                    defaultValue: 0.3f,
                    description: "The chance (0-1) of adding a random upgrade to items sold by vendors or dropped by characters. This is rolled multiple times per item until a single failed roll occurs, or the maximum number of random upgrades is reached"
                );
            ItemUpgrades_MinRandomUpgades = config.Bind
                (
                    section: "Item Upgrades",
                    key: "Min random upgrades",
                    defaultValue: 0,
                    description: "The minimum number of random upgrades that can be added to items sold by vendors or dropped by characters. If the number of upgrades resulting from the upgrade chance roll is less than this, the number of upgrades will be set to this minimum"
                );
            ItemUpgrades_MaxRandomUpgades = config.Bind
                (
                    section: "Item Upgrades",
                    key: "Max random upgrades",
                    defaultValue: 3,
                    description: "The maximum number of random upgrades that can be added to items sold by vendors or dropped by characters"
                );



            Map_RevealAllMapElements = config.Bind
                (
                    section: "Map",
                    key: "Reveal all map elements",
                    defaultValue: false,
                    description: "Whether to show all locations on the map. Use this for testing out the mod"
                );
        }


        internal static void ValidateSettings()
        {
            if (ItemUpgrades_RandomUpgradeChance!.Value < 0 || ItemUpgrades_RandomUpgradeChance!.Value > 1)
            {
                DarkwoodRandomizerPlugin.Logger.LogError("Items_RandomUpgradeChance is not within [0, 1] - defaulting to 0");
                ItemUpgrades_RandomUpgradeChance!.Value = 0;
            }

            if (ItemDrops_RandomDropChance!.Value < 0 || ItemDrops_RandomDropChance!.Value > 1)
            {
                DarkwoodRandomizerPlugin.Logger.LogError("ItemDrops_RandomDropChance is not between 0 and 1 - defaulting to 0");
                ItemDrops_RandomDropChance!.Value = 0;
            }

            if (ItemUpgrades_RandomUpgradeChance!.Value < 0 || ItemUpgrades_RandomUpgradeChance!.Value > 1)
            {
                DarkwoodRandomizerPlugin.Logger.LogError("Items_RandomUpgradeChance is not within [0, 1] - defaulting to 0");
                ItemUpgrades_RandomUpgradeChance!.Value = 0;
            }

            if (Characters_HealthVarianceRange!.Value < 0)
            {
                DarkwoodRandomizerPlugin.Logger.LogError("Characters_HealthVarianceRange is negative - defaulting to 0");
                Characters_HealthVarianceRange!.Value = 0;
            }
        }
    }
}
