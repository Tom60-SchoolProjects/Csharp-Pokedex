using Pokédex;

Console.WriteLine("Mise à jour de la listes des pokémons...");
await PokemonCache.RefreshCaches();
Console.WriteLine();

var pokedex = PokemonCache.GetPokedex();

if (pokedex == null)
    Console.WriteLine("Le Pokédex n'existe pas");
else
{
    var actions = new Actions(pokedex);

    actions.AfficherListePokemons();
    actions.AfficherPokemon(2);
    actions.AfficherPokemonsDeType("Fire");
    actions.AfficherPokemonsDeGeneration(3);
    actions.AfficherMoyennePoidsPokemonsDeTypeAcier();
}

Console.WriteLine();
Console.WriteLine("Appuyez sur une touche pour quitter");
Console.ReadKey();
