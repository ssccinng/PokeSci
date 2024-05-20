using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTeamAI
{
    public interface IAITeamBuilder
    {
        // 负责把已知信息传入AI，AI根据已知信息进行预测
        SimpleGamePokemonTeam PredictTeam(SimpleGamePokemonTeam team);
    }
}
