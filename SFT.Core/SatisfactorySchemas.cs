using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameDataSchema
{
    public class GameData
    {
        [JsonPropertyName("belts")]
        public required List<Belt> Belts { get; set; }

        [JsonPropertyName("pipes")]
        public required List<Pipe> Pipes { get; set; }

        [JsonPropertyName("buildings")]
        public required List<Building> Buildings { get; set; }

        [JsonPropertyName("miners")]
        public required List<Miner> Miners { get; set; }

        [JsonPropertyName("items")]
        public required List<Item> Items { get; set; }

        [JsonPropertyName("fluids")]
        public required List<Fluid> Fluids { get; set; }

        [JsonPropertyName("recipes")]
        public required List<Recipe> Recipes { get; set; }

        [JsonPropertyName("resources")]
        public required List<Resource> Resources { get; set; }
    }

    public class Belt
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("rate")]
        public int Rate { get; set; }
    }

    public class Pipe
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("rate")]
        public int Rate { get; set; }
    }

    public class Building
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("category")]
        public required string Category { get; set; }

        [JsonPropertyName("power")]
        public double Power { get; set; }

        [JsonPropertyName("somersloop_slots")]
        public int? SomersloopSlots { get; set; }

        [JsonPropertyName("max")]
        public int? Max { get; set; }

        [JsonPropertyName("power_range")]
        public List<int>? PowerRange { get; set; }
    }

    public class Miner
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("category")]
        public required string Category { get; set; }

        [JsonPropertyName("base_rate")]
        public double BaseRate { get; set; }

        [JsonPropertyName("power")]
        public double Power { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("tier")]
        public int Tier { get; set; }

        [JsonPropertyName("stack_size")]
        public int? StackSize { get; set; }
    }

    public class Fluid
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("tier")]
        public int Tier { get; set; }
    }

    public class Recipe
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("category")]
        public required string Category { get; set; }

        [JsonPropertyName("time")]
        public double Time { get; set; }

        [JsonPropertyName("power_range")]
        public List<int>? PowerRange { get; set; }

        // Ingredients and Products are represented as lists of mixed arrays (e.g., ["iron-ore", 1]).
        // JsonElement handles the mixed string/number types smoothly without requiring a custom JsonConverter.
        [JsonPropertyName("ingredients")]
        public required List<List<JsonElement>> Ingredients { get; set; }

        [JsonPropertyName("products")]
        public required List<List<JsonElement>> Products { get; set; }
    }

    public class Resource
    {
        [JsonPropertyName("key_name")]
        public required string KeyName { get; set; }

        [JsonPropertyName("category")]
        public required string Category { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }
    }
}