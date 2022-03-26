using System.Diagnostics;
using System.Text.Json;

namespace Pokédex
{
    public static class PokemonCache
    {
        #region Variables
        private readonly static string PokemonsPath = AppPath.GetFullPath(@"/Pokedex.json");
        private const string PokemonsUri = "https://tmare.ndelpech.fr/tps/pokemons";
        #endregion

        #region Methods
        /// <summary>
        /// Get the list of cached Pokémon
        /// </summary>
        /// <returns>Returns the list of Pokémon in cache or null if the cache does not exist</returns>
        public static Pokedex? GetPokedex()
        {
            // Check if the pokemon entries have been cached
            if (File.Exists(PokemonsPath))
            {
                string pokemonsJson = File.ReadAllText(PokemonsPath);

                return Pokedex.ParsePokemons(pokemonsJson);
            }
            return null;
        }

        /// <summary>
        /// Download or update cached Pokemon list
        /// </summary>
        public static async Task RefreshCaches()
        {
            using HttpClient httpClient = new();
            string pokemonEntriesJson = await httpClient.GetStringAsync(PokemonsUri);

            // Load the cached pokemon entries
            var cachedPokedex = GetPokedex();
            PokemonEntry[]? pokemonEntries;

            try
            {   pokemonEntries = JsonSerializer.Deserialize<PokemonEntry[]>(pokemonEntriesJson); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.WriteLine(ex);
                return;
            }

            if (pokemonEntries != null)
            {
                List<Pokemon> pokemonList = new();

                // Asynchronously download Pokémon by generation
                Task<ICollection<Pokemon>>[] tasksList = new[]
                {
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[0..150]), // Gen 1
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[151..250]), // Gen 2
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[251..385]), // Gen 3
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[386..492]), // Gen 4
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[493..648]), // Gen 5
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[649..720]), // Gen 6
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[721..801]), // Gen 7
                    UpdatePokemons(httpClient, cachedPokedex, pokemonEntries[802..897]), // Gen 8
                };

                // Wait for all downloads to complete
                var results = await Task.WhenAll(tasksList);

                // Merge all Pokémon
                foreach (var result in results)
                    pokemonList.AddRange(result);

                // Save the updated cache is needed
                if (pokemonList.Count != 0)
                {
#if DEBUG           // Make the json is human readable for debugging
                    var options = new JsonSerializerOptions { WriteIndented = true };
#else               // Make the json is compact for saving space
                    var options = new JsonSerializerOptions { WriteIndented = false };
#endif
                    var pokemonsJson = JsonSerializer.Serialize(pokemonList, options);

                    File.WriteAllText(PokemonsPath, pokemonsJson);
                }
            }

        }

        /// <summary>
        /// Download Pokémons if they doesn't exist in the cache or are expired
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        /// <param name="cachedPokedex">Cached Pokedex</param>
        /// <param name="pokemonEntries">The Pokémons to download</param>
        /// <returns>Returns a list of Pokémon that doesn't exist in the cache or are obsolete</returns>
        private static async Task<ICollection<Pokemon>> UpdatePokemons(HttpClient httpClient, Pokedex? cachedPokedex, PokemonEntry[] pokemonEntries)
        {
            List<Pokemon> pokemonList = new();
            Pokemon? pokemon;

            // Cycle through all Pokémon entries
            foreach (var pokemonEntry in pokemonEntries)
            {
                // Get the updated Pokémon
                pokemon = await UpdatePokemon(httpClient, cachedPokedex, pokemonEntry);

                // Add if newer
                if (pokemon != null)
                    pokemonList.Add(pokemon);
            }

            // Returns the newest Pokémon
            return pokemonList;
        }

        /// <summary>
        /// Download a Pokémon if it doesn't exist in the cache or is out-of-date
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        /// <param name="cachedPokedex">Cached Pokedex</param>
        /// <param name="pokemonEntry">The Pokémon to download</param>
        /// <returns>Return Pokémon if it doesn't exist in the cache or is out-of-date, otherwise returns null</returns>
        private static async Task<Pokemon?> UpdatePokemon(HttpClient httpClient, Pokedex? cachedPokedex, PokemonEntry pokemonEntry)
        {
            Pokemon? pokemon = null;

            // If the Pokémon in cache is the same on the API side, we reuse it
            if (cachedPokedex != null && pokemonEntry.LastEdit == cachedPokedex[pokemonEntry.Id]?.LastEdit)
                pokemon = cachedPokedex[pokemonEntry.Id];
            // If the Pokémon is outdated we download the new one
            else
            {
                // Get the Pokémon
                string pokemonJson = await httpClient.GetStringAsync(pokemonEntry.Url);

                // Parse it
                try
                {   pokemon = JsonSerializer.Deserialize<Pokemon>(pokemonJson); }
                catch (Exception ex)
                {   Debug.WriteLine(ex); }
                finally { }
            }

            return pokemon;
        }
        #endregion
    }
}
