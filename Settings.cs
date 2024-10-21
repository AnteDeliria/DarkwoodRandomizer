using BepInEx.Configuration;

namespace DarkwoodRandomizer
{
    internal static class Settings
    {
        internal static ConfigEntry<bool> World_RandomizeChunkBiomes;
        internal static ConfigEntry<string> World_RandomizeChunkBiomesPool;

        internal static ConfigEntry<bool> World_RandomizeChunkGroundSprites;
        internal static ConfigEntry<string> World_RandomizeChunkGroundSpritesPool;



        internal static ConfigEntry<bool> Locations_RandomizeLocations;
        internal static ConfigEntry<bool> Locations_RandomizeLocationsExcludeHideouts;



        internal static ConfigEntry<bool> Map_RandomizeBorders;

        internal static ConfigEntry<bool> Map_RevealAllMapElements;
    }
}
