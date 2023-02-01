using SVPokeDex.ViewModel;

namespace SVPokeDex.View;

public partial class PokeDexPage : ContentPage
{
	public PokeDexPage(PokeDexViewModel pokeDexViewModel)
	{
        BindingContext = pokeDexViewModel;
		InitializeComponent();
	}

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

        (BindingContext as PokeDexViewModel).ChangeImage(e.SelectedItem as string);
    }
}