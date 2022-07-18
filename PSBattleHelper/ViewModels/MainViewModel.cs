using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PSBattleHelper.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private static string _psId;
    private static string _psPwd;
    public static List<string> _msgs { get; set; } = new();
    public static PokePSCore.PSClient PSClient { get; set; }

    public string PSId
    {
        get => _psId;
        set => SetProperty(ref _psId, value);
    }

    public string Password
    {
        get => _psPwd;
        set => SetProperty(ref _psPwd, value);
    }

    public List<string> Msgs => _msgs;
    public MainViewModel()
    {
    }

    public async Task<bool> LogToPs()
    {
        PSClient = new(PSId, Password);
        await PSClient.ConnectAsync();
        await Task.Delay(500);
        var res= await PSClient.LoginAsync();
        await PSClient.ChatWithIdAsync("mooob", "临流");
        PSClient.ChatAction += (sender, args) =>
        {
            Msgs.Add($"{sender}: {Msgs}");
        };
        //await PSClient.SearchBattleAsync("gen8randombattle");
        return res;
    }
}
