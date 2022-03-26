using System.Text.Json.Serialization;

namespace Pokédex
{
    [Serializable]
    public struct PokemonStat
    {
        #region Properties
        [JsonPropertyName("name")]  public string? Name { get; set; }
        [JsonPropertyName("stat")]  public int Stat { get; set; }
        #endregion
    }
}
