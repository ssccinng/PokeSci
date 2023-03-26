namespace PSReplayAnalysis;

// 生成一个定义成员初始值和递减方式的特性
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class DecreaseAttribute : Attribute
{
    public DecreaseAttribute(int initValue, int decreaseValue = -1, int maxValue = 999, int minValue = 0)
    {
        InitValue = initValue;
        DeltaValue = decreaseValue;
        MaxValue = maxValue;
        MinValue = minValue;
    }
    public int InitValue { get; set; }
    public int DeltaValue { get; set; }
    public int MaxValue { get; set; }

    public int MinValue { get; set; }

    // 还需要一个递减（？ 覆盖 切换等的方法
}