namespace PokePSCore;

public class SwitchData: ChooseData
{

    /// <summary>
    /// 要换上来的宝可梦在队伍里的位置
    /// </summary>
    public int PokeId { get; set; }

    public override string ToString()
    {
        if (IsPass) return "pass";
        return $"switch {PokeId}";
    }
}