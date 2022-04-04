using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PokeUI3.MVVM.View;
using PokeUI3.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PokeUI3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        public static MainWindow Instance { get; private set; }
        public Frame ContentFrame => contentFrame;
        Random _rnd = new Random();
        public MainWindow()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(PokeUI3.MVVM.View.PokeDexPage));
            (contentFrame.Content as Page).DataContext = (MainGrid.DataContext as MainViewModel).PokemonVM;

            Title = _rnd.Next(10) == 9 ? "临流揽镜曳双魂" : "PokeUI3";
            Instance = this;
            //var a = test.Icon;
        }

        private void PokeUINv_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var sItem = sender.SelectedItem as NavigationViewItem;
            switch (sItem.Tag)
            {
                case "PokeDexPage":
                    contentFrame.Navigate(typeof(PokeUI3.MVVM.View.PokeDexPage));

                    (contentFrame.Content as Page).DataContext = (MainGrid.DataContext as MainViewModel).PokemonVM;
                    break;
                default:
                    break;
            }
        }

        private void PokeUINv_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            contentFrame.GoBack();
            if (contentFrame.Content is PokeDexPage)
            {
                (contentFrame.Content as Page).DataContext = (MainGrid.DataContext as MainViewModel).PokemonVM;
            }
            if (contentFrame.Content is PokeDetail)
            {
                (contentFrame.Content as Page).DataContext = (MainGrid.DataContext as MainViewModel).PokemonVM;
            }

        }

    }
}
