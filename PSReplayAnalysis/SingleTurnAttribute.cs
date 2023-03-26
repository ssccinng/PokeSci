namespace PSReplayAnalysis;

// 生成一个SingleTurn特性
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SingleTurnAttribute : Attribute
{
    public SingleTurnAttribute()
    {
    }
}
