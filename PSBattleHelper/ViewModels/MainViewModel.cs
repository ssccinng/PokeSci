using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PSBattleHelper.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private static string _psId;
    private static string _psPwd;
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
    public MainViewModel()
    {
    }

    public async Task<bool> LogToPs()
    {
        PSClient = new(PSId, Password);
        await PSClient.ConnectAsync();
        var res= await PSClient.LoginAsync();
        await PSClient.ChatWithIdAsync("mooob", "临流");
        //await PSClient.SearchBattleAsync("gen8randombattle");
        return res;
    }
}
