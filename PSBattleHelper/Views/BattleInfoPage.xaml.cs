using Microsoft.UI.Xaml.Controls;

using PSBattleHelper.ViewModels;

namespace PSBattleHelper.Views;

public sealed partial class BattleInfoPage : Page
{
    public BattleInfoViewModel ViewModel
    {
        get;
    }

    public BattleInfoPage()
    {
        ViewModel = App.GetService<BattleInfoViewModel>();
        InitializeComponent();
    }
}
