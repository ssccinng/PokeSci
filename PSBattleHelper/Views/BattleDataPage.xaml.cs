using Microsoft.UI.Xaml.Controls;

using PSBattleHelper.ViewModels;

namespace PSBattleHelper.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class BattleDataPage : Page
{
    public BattleDataViewModel ViewModel
    {
        get;
    }

    public BattleDataPage()
    {
        ViewModel = App.GetService<BattleDataViewModel>();
        InitializeComponent();
    }
}
