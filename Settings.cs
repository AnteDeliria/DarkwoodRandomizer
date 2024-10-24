﻿using BepInEx.Configuration;

namespace DarkwoodRandomizer
{
    internal static class Settings
    {
        internal static ConfigEntry<bool> World_RandomizeChunkBiomes;
        internal static ConfigEntry<string> World_RandomizeChunkBiomesPool;

        internal static ConfigEntry<bool> World_RandomizeChunkGroundSprites;
        internal static ConfigEntry<string> World_RandomizeChunkGroundSpritesPool;


        internal static ConfigEntry<bool> Locations_RandomizeLocationPosition;
        internal static ConfigEntry<bool> Locations_RandomizeHideoutPosition;
        internal static ConfigEntry<bool> Locations_RandomizeLocationRotation;
        internal static ConfigEntry<bool> Locations_RandomizeHideoutRotation;


        internal static ConfigEntry<bool> GridObjects_RandomizeGridObjects;
        internal static ConfigEntry<bool> GridObjects_IncludeSwampObjectsInPool;
        internal static ConfigEntry<bool> GridObjects_RandomizeGridObjectRotation;


        internal static ConfigEntry<bool> MiscObjects_RandomizeMiscObjects;
        internal static ConfigEntry<bool> MiscObjects_IncludeSwampObjectsInPool;


        internal static ConfigEntry<bool> FreeRoamingEnemies_RandomizeType;
        internal static ConfigEntry<string> FreeRoamingEnemies_DryMeadowEnemiesPool;
        internal static ConfigEntry<string> FreeRoamingEnemies_SilentForestEnemiesPool;
        internal static ConfigEntry<string> FreeRoamingEnemies_OldWoodsEnemiesPool;


        internal static ConfigEntry<bool> Night_RandomizeEnemies;


        internal static ConfigEntry<bool> Loot_RandomizeItemContainers;


        internal static ConfigEntry<bool> Map_RandomizeBorders;

        internal static ConfigEntry<bool> Map_RevealAllMapElements;
    }
}
