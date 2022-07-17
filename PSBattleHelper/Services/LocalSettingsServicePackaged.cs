using System.Threading.Tasks;

using PSBattleHelper.Contracts.Services;
using PSBattleHelper.Core.Helpers;

using Windows.Storage;

namespace PSBattleHelper.Services;

public class LocalSettingsServicePackaged : ILocalSettingsService
{
    public async Task<T> ReadSettingAsync<T>(string key)
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
        {
            return await Json.ToObjectAsync<T>((string)obj);
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
    }
}
