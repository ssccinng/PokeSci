namespace Showdown;

public record ChooseData
{
    /// <summary>
    /// 是否跳过
    /// </summary>
    public bool IsPass { get; set; } = false;
}

public record TeamOrderData(string Order) : ChooseData;