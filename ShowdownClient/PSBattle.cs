using PokeCommon.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Showdown
{
    public enum PlayerPosition
    {
        [Display(Name = "p1")]
        Player1,
        [Display(Name = "p2")]
        Player2,
    }

    public partial class PSBattle
    {

        public event Action<PSBattle> OnTeampreview;
        public event Action<PSBattle, bool[]> OnForceSwitch;
        public event Action<PSBattle> OnChooseMove;
        public event Action<PSBattle> RequestsAction;

        public Dictionary<string, object> Additions = [];
        public GamePokemonTeam MyTeamConfig { get; set; }

        public ShowdownClient Client; // 这个大概率不需要了

        public int Turn { get; set; } = 0;

        public int InitId = 0;

        /// <summary>
        /// 房间Id
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// 我是p1 还是p2
        /// </summary>
        public PlayerPosition PlayerPosition { get; set; }

        /// <summary>
        /// 玩家1名字
        /// </summary>
        public string Player1 { get; set; } = string.Empty;

        public List<PSBattlePokemon> GamePokemonTeam1 { get; set; } = new();

        /// <summary>
        /// 玩家2名字
        /// </summary>
        public string Player2 { get; set; } = string.Empty;

        public List<PSBattlePokemon> GamePokemonTeam2 { get; set; } = new();

        /// <summary>
        /// 暂时的 以后需要整合到对战队伍中去
        /// </summary>
        public bool[] Actives { get; set; } = new bool[6];

        public JsonElement[] ActiveStatus = new JsonElement[2];

        public List<PSBattlePokemon> OppTeam => PlayerPosition == PlayerPosition.Player1 ? GamePokemonTeam2 : GamePokemonTeam1;

        public List<PSBattlePokemon> MyTeam => PlayerPosition == PlayerPosition.Player1 ? GamePokemonTeam1 : GamePokemonTeam2;

        public PSBattlePokemon[] Side1 = new PSBattlePokemon[4];

        public PSBattlePokemon[] Side2 = new PSBattlePokemon[4];

        public List<string> BattleLogs { get; set; } = [];

        public List<string> TurnLogs { get; set; } = [];

        public List<BattleTurn> BattleTurns { get; set; } = [];

        public BattleTurn NowTurn { get; set; } = new();


        public PSBattlePokemon[] MySide => PlayerPosition == PlayerPosition.Player1 ? Side1 : Side2;
        public PSBattlePokemon[] OppSide => PlayerPosition == PlayerPosition.Player1 ? Side2 : Side1;

        public string MyName => PlayerPosition == PlayerPosition.Player1 ? Player1 : Player2;
        public string OppName => PlayerPosition == PlayerPosition.Player1 ? Player2 : Player1;

        public PSBattle(ShowdownClient client, string tag)
        {
            Client = client;
            Tag = tag;
            GamePokemonTeam1 = [null, null, null, null, null, null];
            GamePokemonTeam2 = [null, null, null, null, null, null];
            BattleData = BattleData with { BattleInfo = new SingleBattle(Tag) };
        }

        internal void NextTurn()
        {
            //var turn =  (BattleTurns.Count > 0) ? BattleTurns[^1].NextTurn() : new();

            BattleTurns.Add(NowTurn);
            NowTurn = NowTurn.NextTurn();
        }
    }
}