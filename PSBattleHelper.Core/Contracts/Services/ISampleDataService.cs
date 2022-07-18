using System.Collections.Generic;
using System.Threading.Tasks;

using PSBattleHelper.Core.Models;

namespace PSBattleHelper.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ISampleDataService
{
    Task<IEnumerable<SampleOrder>> GetGridDataAsync();
}
