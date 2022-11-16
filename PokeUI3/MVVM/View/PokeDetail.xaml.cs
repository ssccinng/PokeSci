using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using PokeUI3.MVVM.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PokeUI3.MVVM.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PokeDetail : Page
    {

        internal PokeDetailViewModel PokeDetailVM => DataContext as PokeDetailViewModel;
        public PokeDetail()
        {
            this.InitializeComponent();
        }

        private void ToBaseValueTool_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ContentFrame.Navigate(typeof(BSTToolsView));
            (MainWindow.Instance.ContentFrame.Content as Page).DataContext = new BSTToolsViewModel(PokeDetailVM.Pokemon);
        }

        private void qwe_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            PokeDetailVM.MyProperty = (int)(sender as Slider).Value;
            //www.HP = (int)(sender as Slider).Value;
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //PokeDetailVM.BSTViewChange();
        }
    }
}
