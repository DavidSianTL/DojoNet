using System.Collections.Generic;

namespace PokeApiConsumer.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Id { get; set; }
        public List<PokemonType> Types { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Sprites Sprites { get; set; }
    }

    public class PokemonType
    {
        public TypeDetail Type { get; set; }
    }

    public class TypeDetail
    {
        public string Name { get; set; }
    }

    public class Sprites
    {
        public string Front_Default { get; set; }
    }

    public class PokemonListResponse
    {
        public List<PokemonListItem> Results { get; set; }
    }

    public class PokemonListItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
