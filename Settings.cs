using BepInEx.Configuration;

namespace DarkwoodRandomizer
{
    internal static class Settings
    {
        internal static ConfigEntry<bool> World_RandomizeChunkBiomes;
        internal static ConfigEntry<string> World_RandomizeChunkBiomesPool;

        internal static ConfigEntry<bool> World_RandomizeChunkGroundSprites;
        internal static ConfigEntry<string> World_RandomizeChunkGroundSpritesPool;



        internal static ConfigEntry<bool> Locations_RandomizeExistingLocations;
        internal static ConfigEntry<bool> Locations_RandomizeExistingLocationsAllowRepeats;
        internal static ConfigEntry<bool> Locations_RandomizeExistingLocationsExcludeHideouts;
        internal static ConfigEntry<string> Locations_RandomizeExistingLocationsPool;

        internal static ConfigEntry<bool> Locations_AddExtraLocations;
        internal static ConfigEntry<bool> Locations_AddExtraLocationsAllowRepeats;
        internal static ConfigEntry<bool> Locations_AddExtraLocationsExcludeHideouts;
        internal static ConfigEntry<string> Locations_AddExtraLocationsPool;



        internal static ConfigEntry<bool> Map_RandomizeBorders;

        internal static ConfigEntry<bool> Map_RevealAllMapElements;
    }
}
