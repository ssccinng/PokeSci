using PokeCommon.Interface;

namespace PokeCommon.GameRule
{
    public interface IGameRule
    {
        public IStatusCalc StatusCalc
        {
            get;
        }
    }
}
