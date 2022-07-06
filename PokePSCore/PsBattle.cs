using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.VisualBasic.CompilerServices;
using PokeCommon.Models;

namespace PokePSCore;

public enum PlayerPos
{
    [Display(Name = "p1")]
    Player1,
    [Display(Name = "p2")]
    Player2,
}

public class PsBattle
{

    public Action<PsBattle> OnTeampreview;
    public Action<PsBattle> OnForceSwitch;
    public Action<PsBattle> OnChooseMove;

    /// <summary>
    /// 房间Id
    /// </summary>
    public string Tag { get; set; }
    /// <summary>
    /// 我是p1 还是p2
    /// </summary>
    public PlayerPos PlayerPos { get; set; }
    /// <summary>
    /// 玩家1名字
    /// </summary>
    public string Player1 { get; set; }
    public GamePokemonTeam GamePokemonTeam1 { get; set; } = new GamePokemonTeam();
    /// <summary>
    /// 玩家2名字
    /// </summary>
    public string Player2 { get; set; }
    public GamePokemonTeam GamePokemonTeam2 { get; set; } = new GamePokemonTeam();


    public GamePokemonTeam OppTeam => PlayerPos == PlayerPos.Player1 ? GamePokemonTeam2 : GamePokemonTeam1;

    /// <summary>
    /// 内部回合数，用于发送命令
    /// </summary>
    public int Turn { get; set; } = 0;
    public int idx = 1;

    public PsBattle(string tag)
    {
        Tag = tag;
    }
    public void UpdateOppTeam(string name, int lv = 50)
    {
        //OppTeam.GamePokemons.Any(s => s.MetaPokemon.)
        // 要getps的id
    }
    public void RefreshByRequest(string request)
    {
        var data = JsonDocument.Parse(request).RootElement;

        if (data.TryGetProperty("rqid", out var jsonId))
        {
            Turn = jsonId.GetInt32();
        }
        if (data.TryGetProperty("forceSwitch", out var jsongFSwitch))
        {
            // 需要换人
            OnForceSwitch?.Invoke(this);
        }
        if (data.TryGetProperty("side", out var side))
        {

        }
        if (data.TryGetProperty("active", out var active))
        {

        }
    }
}