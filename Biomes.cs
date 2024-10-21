using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer
{
    internal static class Biomes
    {
        private static List<Biome> biomes;

        internal static Biome BiomeForest { get; }
        internal static Biome BiomeForestDense { get; }
        internal static Biome BiomeForestMutated { get; }
        internal static Biome BiomeSwamp { get; }
        internal static Biome BiomeMeadow { get; }
        internal static Biome BiomeEmpty { get; }



        static Biomes()
        {
            biomes = new List<Biome>();

            WorldGenerator worldGenerator = Singleton<WorldGenerator>.Instance;
            List<Biome> biomePresets = (List<Biome>)AccessTools.Field(typeof(WorldGenerator), "biomePresets").GetValue(worldGenerator);

            foreach (Biome biome in biomePresets)
            {
                biomes.Add(biome);

                switch (biome.type)
                {
                    case Biome.Type.forest:
                        BiomeForest = biome;
                        break;
                    case Biome.Type.forest_dense:
                        BiomeForestDense = biome;
                        break;
                    case Biome.Type.forest_mutated:
                        BiomeForestMutated = biome;
                        break;
                    case Biome.Type.swamp:
                        BiomeSwamp = biome;
                        break;
                    case Biome.Type.meadow:
                        BiomeMeadow = biome;
                        break;
                    case Biome.Type.empty:
                        BiomeEmpty = biome;
                        break;
                }
            }
        }


        internal static Biome GetRandomBiome()
        {
            return biomes[UnityEngine.Random.Range(0, biomes.Count)];
        }

        internal static Biome GetRandomBiome(ConfigEntry<string> configEntry)
        {
            IEnumerable<string> biomeStrings = configEntry.Value.Split(',').Select(x => x.Trim().ToLower());
            List<Biome> biomeChoices = new();

            foreach (string biome in biomeStrings)
            {
                Biome? biomeChoice = biome switch
                {
                    "forest" => BiomeForest,
                    "forest_dense" => BiomeForestDense,
                    "forest_mutated" => BiomeForestMutated,
                    "swamp" => BiomeSwamp,
                    "meadow" => BiomeMeadow,
                    "empty" => BiomeEmpty,
                    _ => null
                };

                if (biomeChoice != null)
                    biomeChoices.Add(biomeChoice);
            }

            if (biomeChoices.Count == 0)
                return GetRandomBiome();
            else
                return biomeChoices[UnityEngine.Random.Range(0, biomeChoices.Count)];
        }
    }
}
