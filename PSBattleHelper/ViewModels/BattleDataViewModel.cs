using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using PSBattleHelper.Contracts.ViewModels;
using PSBattleHelper.Core.Contracts.Services;
using PSBattleHelper.Core.Models;

namespace PSBattleHelper.ViewModels;

public class BattleDataViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public BattleDataViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
