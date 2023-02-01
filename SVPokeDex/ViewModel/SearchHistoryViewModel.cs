using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SVPokeDex.ViewModel;
public sealed partial class SearchHistoryViewModel: ObservableObject
{
    public List<string> SearchHistory
    {
    get; set; }


}
