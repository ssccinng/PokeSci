namespace Poke.Core.Models;

public enum AbilityRating
{
    Detrimental = -1,
    Useless = 0,
    Ineffective = 1,
    Useful = 2,
    Effective = 3,
    VeryUseful = 4,
    Essential = 5,
}

public class Ability
{
    public int Id
    {
        get;
        set;
    }
    /// <summary>
    /// 特性名字
    /// </summary>
    public Text? Name
    {
        get;
        set;
    }
    /// <summary>
    /// 特性描述
    /// </summary>
    public Text? Description
    {
        get;
        set;
    }
    
    public AbilityRating Rating
    {
        get;
        set;
    }
    
    
    
    
}