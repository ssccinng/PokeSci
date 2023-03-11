

using System.ComponentModel.DataAnnotations.Schema;

namespace Poke.Core.Models;

/// <summary>
/// 道具
/// </summary>
public class Item
{
    public int ItemId
    {
        get; set;
    }
    /// <summary>
    /// 道具名字
    /// </summary>
    public Text? Name
    {
        get;
        set;
    }
    /// <summary>
    /// 道具描述 
    /// </summary>
    public Text? Description
    {
        get;
        set;
    }
    public ItemType Item_Type
    {
        get; set;
    }
}

public enum ItemType
{
    Abandon,
    Medicine,
    Carryorevolve,
    Ingredients,
    BattleItem,
    PokeBall,
    TMTR,
    Berry,
    ImportantItem,
    Others

}