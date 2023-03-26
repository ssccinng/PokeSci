namespace PSReplayAnalysis;

// 生成 ChangeRefresh特性
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ChangeRefreshAttribute : Attribute
{
    public int InitValue { get; set; }

    public ChangeRefreshAttribute(int initValue = 0)
    {
        InitValue = initValue;
    }
}
