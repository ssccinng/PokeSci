using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokeUI3.MVVM.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PokeUI3.MVVM.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BSTToolsView : Page
    {
        public BSTToolsViewModel BSTToolsVM => DataContext as BSTToolsViewModel;
        public BSTToolsView()
        {
            this.InitializeComponent();
        }

        private void Spd1_5_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
