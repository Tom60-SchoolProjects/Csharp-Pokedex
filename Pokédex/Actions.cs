namespace Pokédex
{
    internal class Actions
    {
        #region Constructors
        public Actions(Pokedex pokedex)
        {
            this.pokedex = pokedex;
        }
        #endregion

        #region Variables
        Pokedex pokedex;
        #endregion


        #region Methods
        /// <summary>
        /// Affiche la liste des Pokémons
        /// </summary>
        public void AfficherListePokemons()
        {
            Console.WriteLine("Liste de tout les Pokémons :");

            foreach (var pokemon in pokedex.Pokemons)
                Console.Write($"{pokemon.Id} - {pokemon.Name?["fr"]}, ");

            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Affiche un Pokémon
        /// </summary>
        /// <param name="id">L'id du Pokémon</param>
        public void AfficherPokemon(int id)
        {
            var pokemon = pokedex[id];  // Récupère le Pokémon par son id

            Console.Write("Pokémon : ");

            if (pokemon == null)
                Console.Write("Ce Pokémon existe pas");
            else
                Console.Write($"{pokemon.Id} - {pokemon.Name?["fr"]}");

            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Affiche un Pokémon de type donnés
        /// </summary>
        /// <param name="type">Le type du Pokémon</param>
        public void AfficherPokemonsDeType(string type)
        {
            var pokemons = pokedex.Pokemons.Where(x => x.Types != null && x.Types.Contains(type)); // Récupère les Pokémon de type donnés

            Console.WriteLine($"Liste de tout les Pokémons de type {type} :");

            foreach (var pokemon in pokemons)
                Console.Write($"{pokemon.Id} - {pokemon.Name?["fr"]}, ");

            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Affiche des Pokémon de génération donnés
        /// </summary>
        /// <param name="generation">La généartion du Pokémon</param>
        public void AfficherPokemonsDeGeneration(int generation)
        {
            var pokemons = pokedex.Pokemons.Where(x => x.GetGeneration() == generation); // Récupère les Pokémon de génération donnés

            Console.WriteLine($"Liste de tout les Pokémons de génération {generation} :");

            foreach (var pokemon in pokemons)
                Console.Write($"{pokemon.Id} - {pokemon.Name?["fr"]}, ");

            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Affiche la moyenne des poids des Pokémon de type Acier
        /// </summary>
        public void AfficherMoyennePoidsPokemonsDeTypeAcier()
        {
            var pokemonsAcier = pokedex.Pokemons.Where(x => x.Types != null && x.Types.Contains("Steel")); // Récupère les Pokémon de type Acier
            var poidsMoyen = pokemonsAcier.Average(x => x.Weight); // Fait la moyenne

            Console.WriteLine($"Le poids moyen des Pokémon de type Acier est de {poidsMoyen:00.00}");
        }
        #endregion
    }
}
