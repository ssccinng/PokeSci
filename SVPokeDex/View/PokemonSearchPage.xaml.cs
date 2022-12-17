using SVPokeDex.ViewModel;

namespace SVPokeDex.View;

public partial class PokemonSearch : ContentPage
{
	public PokemonSearch(PokemonSearchViewModel pokemonSearchViewModel)
	{
		InitializeComponent();
        BindingContext = pokemonSearchViewModel;
	}
}