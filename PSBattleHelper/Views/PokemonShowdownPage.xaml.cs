using Microsoft.UI.Xaml.Controls;

using PSBattleHelper.ViewModels;

namespace PSBattleHelper.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/
public sealed partial class PokemonShowdownPage : Page
{
    public PokemonShowdownViewModel ViewModel
    {
        get;
    }

    public PokemonShowdownPage()
    {
        ViewModel = App.GetService<PokemonShowdownViewModel>();
        InitializeComponent();

        ViewModel.WebViewService.Initialize(WebView);
    }
}
