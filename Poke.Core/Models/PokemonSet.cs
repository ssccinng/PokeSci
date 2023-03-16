namespace Poke.Core.Models;


public class PokemonSet: IPokemonSet
{
    public string Name { get; set; }

    public string Species { get; set; }

    public string Item { get; set; }

    public string Ability { get; set; }

    public string[] Moves { get; set; }

    public string Nature { get; set; }

    public string Gender { get; set; }

    public SixDimension Evs { get; set; }

    public SixDimension Ivs { get; set; }

    public int Level { get; set; }

    public bool? Shiny { get; set; }

    public int? Happiness { get; set; }

    public string? Pokeball { get; set; }

    public string? HPType { get; set; }

    public int? DynamaxLevel { get; set; }

    public bool? Gigantamax { get; set; }

    public string? TeraType { get; set; }
}

public interface IPokemonSet
{
    /// <summary>
    /// Nickname. Should be identical to its base species if not specified
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Species name (including forme if applicable), e.g. "Minior-Red".
    /// This should always be converted to an id before use.
    /// </summary>
    string Species { get; set; }

    string Item { get; set; }

    string Ability { get; set; }

    string[] Moves { get; set; }

    string Nature { get; set; }
    string Gender { get; set; }
    SixDimension Evs { get; set; }
    SixDimension Ivs { get; set; }

    int Level { get; set; }
    bool? Shiny { get; set; }
    int? Happiness { get; set; }
    
    string? Pokeball { get; set; }
    
    /// <summary>
    /// 觉醒力量类型
    /// </summary>
    string? HPType { get; set; }
    int? DynamaxLevel { get; set; }
    bool? Gigantamax { get; set; }
    string? TeraType { get; set; }
}