using System.Text.Json.Serialization;

namespace Pokédex
{
    [Serializable]
    public class Pokemon
    {
        #region Properties
        [JsonPropertyName("id")]            public int Id { get; set; }
        [JsonPropertyName("name")]          public Dictionary<string, string>? Name { get; set; }
        [JsonPropertyName("types")]         public string[]? Types { get; set; }
        [JsonPropertyName("height")]        public int Height { get; set; }
        [JsonPropertyName("weight")]        public int Weight { get; set; }
        [JsonPropertyName("genus")]         public Dictionary<string, string>? Genus { get; set; }
        [JsonPropertyName("description")]   public Dictionary<string, string>? Description { get; set; }
        [JsonPropertyName("stats")]         public PokemonStat[]? Stats { get; set; }
        [JsonPropertyName("lastEdit")]      public long LastEdit { get; set; }
        #endregion

        #region Methods
        public int GetGeneration()
        {
            if (Id is >= 1 and <= 151)
                return 1;
            else if (Id is >= 152 and <= 251)
                return 2;
            else if (Id is >= 252 and <= 386)
                return 3;
            else if (Id is >= 387 and <= 493)
                return 4;
            else if (Id is >= 494 and <= 649)
                return 5;
            else if (Id is >= 650 and <= 721)
                return 6;
            else if (Id is >= 722 and <= 802)
                return 7;
            else if (Id is >= 802 and <= 898)
                return 8;
            else return 0;

            #endregion
        }
    }
}
