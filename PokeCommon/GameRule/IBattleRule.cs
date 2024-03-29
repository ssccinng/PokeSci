﻿using PokeCommon.Interface;
using PokeCommon.Models;
using PokemonDataAccess.Models;

namespace PokeCommon.GameRule
{
    /// <summary>
    /// 规则设计得好好撕烤
    /// </summary>
    public interface IBattleRule
    {

        /// <summary>
        /// 对战引擎
        /// </summary>
        public IBattleEngine BattleEngine
        {
            get;
        }
        /// <summary>
        /// 队伍成员限制
        /// </summary>
        public int TeamMemberLimit
        {
            get;
        }
        /// <summary>
        /// 上场成员限制
        /// </summary>
        public int BattleMemberLimit
        {
            get;
        }
        /// <summary>
        /// 队伍可见性
        /// </summary>
        public bool TeamVisable
        {
            get;
        }
        /// <summary>
        /// 对战成员可见性
        /// </summary>
        public bool BattleMemberVisable
        {
            get;
        }

        public bool PreviewTeam
        {
            get;
        }
        /// <summary>
        /// 对战时间规则
        /// </summary>
        public TimeRule TimeRule
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Pokemon> PokemonBanList
        {
            get;
        }


        public bool IsTeamOk(List<GamePokemon> gamePokemons);
        public bool IsTeamOk(GamePokemonTeam gamePokemonTeam)
        {
            return IsTeamOk(gamePokemonTeam.GamePokemons);
        }


    }
}
