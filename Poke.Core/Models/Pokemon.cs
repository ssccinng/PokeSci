namespace Poke.Core.Models;

public class Pokemon
{
    public int Id
    {
        get;
        set;
    }
    /// <summary>
    /// 全国图鉴Id
    /// </summary>
    public int NationDexId
    {
        get;
        set;
    }
    /// <summary>
    /// 宝可梦名字
    /// </summary>
    public Text? Name
    {
        get;
        set;
    }

    /// <summary>
    /// 形态名字
    /// </summary>
    public Text? FormName
    {
        get;
        set;
    }
    /// <summary>
    /// 完整名字
    /// </summary>
    public Text? FullName
    {
        get;
        set;
    }
    /// <summary>
    /// 形态Id
    /// </summary>
    public int FormId
    {
        get;
        set;
    } = 0;
}