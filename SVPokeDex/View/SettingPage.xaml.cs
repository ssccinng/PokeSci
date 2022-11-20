using SVPokeDex.ViewModel;

namespace SVPokeDex.View;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingViewModel settingViewModel)
	{
        BindingContext = settingViewModel;
		InitializeComponent();
        //App.Current.
	}
}