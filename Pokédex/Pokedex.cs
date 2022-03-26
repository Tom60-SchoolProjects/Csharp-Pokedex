using System.Diagnostics;
using System.Text.Json;

namespace Pokédex
{
    [Serializable]
    public class Pokedex
    {
        #region Constructors
        public Pokedex(List<Pokemon> pokemons)
        {
            this.pokemons = pokemons;
        }
        #endregion

        #region Variables
        private readonly List<Pokemon> pokemons;
        #endregion

        #region Properties
        public List<Pokemon> Pokemons => pokemons;
        #endregion

        #region Methods
        /// <summary>
        /// Parse a json full of <see cref="Pokemon"/> into a <see cref="Pokedex"/>.
        /// </summary>
        /// <param name="json">Json string to parse</param>
        /// <returns>Return a <see cref="Pokedex"/>, is empty if an error occurred.</returns>
        public static Pokedex ParsePokemons(string json)
        {
            Pokemon[]? pokemons = null;

            try
            {   pokemons = JsonSerializer.Deserialize<Pokemon[]>(json); }
            catch (Exception ex)
            {   Debug.WriteLine(ex); }

            return pokemons == null ? new Pokedex(new List<Pokemon>()) : new Pokedex(pokemons.ToList());
        }

        /// <summary>
        /// Get a <see cref="Pokemon"/> by name.
        /// </summary>
        /// <param name="name"><see cref="Pokemon"/> name</param>
        /// <returns>Return a <see cref="Pokemon"/></returns>
        public Pokemon? this[string name] => pokemons.FirstOrDefault(x => x.Name != null && x.Name.ContainsValue(name));

        /// <summary>
        /// Get a <see cref="Pokemon"/> by id.
        /// </summary>
        /// <param name="id"><see cref="Pokemon"/> id</param>
        /// <returns>Return a <see cref="Pokemon"/></returns>
        public Pokemon? this[int id] => pokemons.FirstOrDefault(x => x.Id == id);
        #endregion
    }
}
