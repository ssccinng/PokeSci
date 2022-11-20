using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SVPokeDex.Resources.Styles;

namespace SVPokeDex.ViewModel;
public partial class SettingViewModel: ObservableObject
{
    // 利用首选项存储
    [ObservableProperty]
    //string _themeText;
    string _themeText = "朱";
    public SettingViewModel()
    {
        ThemeText = Preferences.Get("ColorMode", "朱");
    }
    [RelayCommand]
    public void ChangeTheme()
    {
        if (_themeText == "朱")
        {
            ThemeText = "紫";
            MainViewModel.ChangeTheme("紫");
        }
        else
        {
            ThemeText = "朱";
            MainViewModel.ChangeTheme("朱");
        }

    }

    

}
