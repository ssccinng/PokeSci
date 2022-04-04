using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PokemonDataAccess.Models;
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

namespace PokeUI3.MVVM.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PokeDexPage : Page
    {
        public PokeDexPage()
        {
            this.InitializeComponent();
        }
        public PokedexViewModel PokedexVM => (PokedexViewModel)DataContext;
        private void PokeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //MainWindow.Instance.ContentFrame.Navigate(typeof(PokeDetail));
        }

        private void PokeList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            MainWindow.Instance.ContentFrame.Navigate(typeof(PokeDetail));
            (MainWindow.Instance.ContentFrame.Content as Page).DataContext = new PokeDetailViewModel((listView.SelectedItem as Pokemon));
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var suitablePokes = new List<Pokemon>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var cat in (DataContext as PokedexViewModel).Pokemons)
                {
                    var found = splitText.All((key) =>
                    {
                        return cat.FullNameEng.ToLower().StartsWith(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(cat.FullNameEng);
                        suitablePokes.Add(cat);
                    }

                    found = splitText.All((key) =>
                    {
                        return cat.FullNameChs.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(cat.FullNameChs);
                        suitablePokes.Add(cat);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
                PokeList.ItemsSource = suitablePokes;
            }
        }
    }
}
