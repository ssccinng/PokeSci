using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.CompilerServices;

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
    /// <summary>
    /// 玩家2名字
    /// </summary>
    public string Player2 { get; set; }

    public int Turn { get; set; } = 0;
    public int idx = 1;

    public PsBattle(string tag)
    {
        Tag = tag;
    }
}