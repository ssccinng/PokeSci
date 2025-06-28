namespace Showdown;

public record SwitchData : ChooseData
{

    /// <summary>
    /// 要换上来的宝可梦在队伍里的位置
    /// </summary>
    public int PokeId
    {
        get; set;
    }

    public override string ToString()
    {
        if (IsPass) return "pass";
        return $"switch {PokeId}";
    }
}

public record SwitchNameData : ChooseData
{

    /// <summary>
    /// 要换上来的宝可梦在队伍里的位置
    /// </summary>
    public string Pokename
    {
        get; set;
    }

    public override string ToString()
    {
        if (IsPass) return "pass";
        return $"switch {Pokename}";
    }
}