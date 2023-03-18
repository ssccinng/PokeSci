namespace Poke.Core.Models;

public class Battle
{
    
}

public interface IBattleOption
{
    
}


public interface IPlayerOption
{
    string? Name { get; set; }
    string? Avatar { get; set; }
    string? Rating { get; set; }
    PokemonSet[]? Team { get; set; } 
    long? Seed { get; set; }
}

public interface ITextObject {
    public string Desc { get; set; }
    public string ShortDesc { get; set; }
}

public interface IPlines {
    public string Activate { get; set; }
    public string AddItem { get; set; }
    public string Block { get; set; }
    public string Boost { get; set; }
    public string Cant { get; set; }
    public string ChangeAbility { get; set; }
    public string Damage { get; set; }
    public string End { get; set; }
    public string Heal { get; set; }
    public string Move { get; set; }
    public string Start { get; set;}
    public string Transform { get;set;}
}


public interface ITextFile : ITextObject {
    public string name{get;set;}
    public ModdedTextObject gen1{get;set;}
    // ... other properties omitted for brevity
}

public interface IMovePlines : IPlines {
    // ... other properties omitted for brevity
}

public interface AbilityText : ITextFile, IPlines {
    // ... other properties omitted for brevity
}

public interface MoveText : ITextFile, IMovePlines {}

public interface ItemText : ITextFile, IPlines {}

public interface PokedexText : ITextFile {}

public interface DefaultText {}

public interface ModdedTextObject : ITextObject, IPlines {}

public interface MoveEventMethods {
    // int? basePowerCallback(Battle battle, Pokemon pokemon, Pokemon target, ActiveMove move);
    /** Return true to stop the move from being used */
    // bool? beforeMoveCallback(Battle battle, Pokemon pokemon, Pokemon target, ActiveMove move);
    void beforeTurnCallback(Battle battle, Pokemon pokemon, Pokemon target);
    int? damageCallback(Battle battle, Pokemon pokemon, Pokemon target);
    void priorityChargeCallback(Battle battle, Pokemon pokemon);

    void onDisableMove(Battle battle, Pokemon pokemon);

    void onAfterHit(CommonHandlers.VoidSourceMove handler);
    // void onAfterSubDamage(Battle battle, int damage, Pokemon target, Pokemon source, ActiveMove move);
    void onAfterMoveSecondarySelf(CommonHandlers.VoidSourceMove handler);
    // void onAfterMoveSecondary(CommonHandlers.VoidMove handler);
    void onAfterMove(CommonHandlers.VoidSourceMove handler);
    int? onDamagePriority { get; set; }
    object onDamage(
        Battle battle,
        int damage,
        Pokemon target,
        Pokemon source
        // Effect effect
    );
}

public interface CommonHandlers {
    int? ModifierEffect(Battle battle, int relayVar, Pokemon target, Pokemon source, Effect effect);
    int? ModifierMove(Battle battle, int relayVar, Pokemon target, Pokemon source, ActiveMove move);
    object ResultMove { get; set; }
    object ExtResultMove { get; set; }
    // void VoidEffect(Battle battle, Pokemon target, Pokemon source, Effect effect);
    // void VoidMove(Battle battle, Pokemon target, Pokemon source, ActiveMove move);
    // int? ModifierSourceEffect(
    //     Battle battle,
    //     int relayVar,
    //     Pokemon source,
    //     Pokemon target,
    //     Effect effect
    // );
    // int? ModifierSourceMove(
    //     Battle battle,
    //     int relayVar,
    //     Pokemon source,
    //     Pokemon target,
    //     ActiveMove move
    // );
    // object ResultSourceMove { get; set; }
    // object ExtResultSourceMove { get; set; }
    // void VoidSourceEffect(Battle battle, Pokemon source, Pokemon target, Effect effect);
    // void VoidSourceMove(Battle battle, Pokemon source, Pokemon target, ActiveMove move);

    public class VoidSourceMove
    {
    }
}