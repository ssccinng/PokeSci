namespace Poke.Core.Models;

public interface ILang
{
    /// <summary>
    /// 简体中文
    /// </summary>
    string? CHS
    {
        get;
        set;
    }
    /// <summary>
    /// 繁体中文
    /// </summary>
    string? CHT
    {
        get;
        set;
    }
    /// <summary>
    /// 英语
    /// </summary>
    string? ENG
    {
        get;
        set;
    }
    /// <summary>
    /// 日语
    /// </summary>
    string? JPN
    {
        get;
        set;
    }
    /// <summary>
    /// 法语
    /// </summary>
    string? FRA
    {
        get;
        set;
    }
    /// <summary>
    /// 韩语
    /// </summary>
    string? KOR
    {
        get;
        set;
    }
    /// <summary>
    /// 意大利语
    /// </summary>
    string? ITA
    {
        get;
        set;
    }
    /// <summary>
    /// 西班牙语
    /// </summary>
    string? SPA
    {
        get;
        set;
    }
    /// <summary>
    /// 德语
    /// </summary>
    string? GER
    {
        get;
        set;
    }
}