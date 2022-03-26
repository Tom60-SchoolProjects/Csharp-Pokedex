using System.Text.Json.Serialization;

namespace Pokédex
{
    [Serializable]
    public struct PokemonEntry
    {
        #region Properties
        [JsonPropertyName("id")]    public int Id { get; set; }
        [JsonPropertyName("url")]    public string? Url { get; set; }
        [JsonPropertyName("lastEdit")]    public long LastEdit { get; set; }
        #endregion
    }
}
