using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SVPokeDex.ViewModel;
public partial class PokemonSearchViewModel: ObservableObject
{
    [ObservableProperty]
    public List<string> strings = new List<string>() { "1", "2", "3", "4", };
}
