namespace PSReplayAnalysis;

// 生成一个CanBeJieBang特性
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CanBeJieBangAttribute : Attribute
{
    public CanBeJieBangAttribute()
    {
    }
}
