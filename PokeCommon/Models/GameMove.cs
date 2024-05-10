using PokemonDataAccess.Models;

namespace PokeCommon.Models
{
    /// <summary>
    /// 游戏中的技能
    /// </summary>
    public class GameMove
    {
        public Move? MetaMove { get; set; }
        public int MoveId => MetaMove?.MoveId ?? 0;
        public int PPMax
        {
            get; set;
        }
        public int PP
        {
            get; set;
        }
        public string NameChs => MetaMove?.Name_Chs ?? "";
        public string NameEng => MetaMove?.Name_Eng ?? "";
        public string NameJpn => MetaMove?.Name_Jpn ?? "";

        public GameMove(Move move)
        {
            MetaMove = move;
        }

        public GameMove()
        {
            
        }
    }
}
