using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

using PSBattleHelper.ViewModels;

namespace PSBattleHelper.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    private async void LogToPs_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        LogToPs.IsEnabled = false;
        if (!await ViewModel.LogToPs())
        {
            LogToPs.IsEnabled = true;
            LogToPs.Content = "登陆失败";
            await Task.Delay(1000);
            LogToPs.Content = "登陆";

        }
        else
        {
            LogToPs.Content = "登陆成功";
        }
    }

    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (MainViewModel.PSClient == null) return;
        //MainViewModel.PSClient.ChatAction += AddChatMessage;
    }

    private void Page_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (MainViewModel.PSClient == null) return;

        //MainViewModel.PSClient.ChatAction -= AddChatMessage;

    }

    public void AddChatMessage(string Id, string Msg)
    {
        pmList.Items.Add($"{Id}: {Msg}");
    }
}
