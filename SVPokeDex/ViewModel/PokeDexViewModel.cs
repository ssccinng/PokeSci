using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SVPokeDex.Model;

namespace SVPokeDex.ViewModel;
public partial class PokeDexViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<SVPokemon> _svPokemons = new () { new(), new(), new(), };

    [ObservableProperty]
    ObservableCollection<string> aa = MainPage.imgs.ToObservableCollection();

    [ObservableProperty]
    string dexImage = "story_img_01.png";
    public PokeDexViewModel()
    {
        var res =  FileSystem.Current.OpenAppPackageFileAsync("PokemonData.json").Result;
        var aa = JsonDocument.Parse(res);
        var a = 1 + 1;
    }

    [RelayCommand]
    public void ChangeImage(string name)
    {
        DexImage= name;
    }
}
