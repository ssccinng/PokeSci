using System.Threading.Tasks;

namespace PSBattleHelper.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
