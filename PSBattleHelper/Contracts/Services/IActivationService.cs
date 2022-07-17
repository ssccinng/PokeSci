using System.Threading.Tasks;

namespace PSBattleHelper.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
