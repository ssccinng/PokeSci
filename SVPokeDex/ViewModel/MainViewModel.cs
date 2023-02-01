using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SVPokeDex.Resources.Styles;

namespace SVPokeDex.ViewModel;
public partial class MainViewModel: ObservableObject
{
    [ObservableProperty]
    string text = "This is nothing left for me";
    static ResourceDictionary keyValuePairs;
    
    public static MainViewModel Instance
    {
    get; set; }
    public MainViewModel()
    {
        Instance = this;
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        keyValuePairs = mergedDictionaries.Last();
        //mergedDictionaries.Last();
    }

    public static void ChangeTheme(string mode)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        mergedDictionaries.Clear();
        if (mode == "朱")
        {
            mergedDictionaries.Add(new ScarletTheme());
        }
        else
        {
            mergedDictionaries.Add(new VioletTheme());

        }
        //mergedDictionaries.Add(keyValuePairs);
        Preferences.Set("ColorMode", mode);

    }
}
