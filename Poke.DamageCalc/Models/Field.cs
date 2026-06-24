namespace Poke.DamageCalc;

public sealed class Field
{
    public string? Weather { get; set; }

    public string? Terrain { get; set; }

    public bool IsDoubles { get; set; }

    public bool IsMagicRoom { get; set; }

    public bool IsWonderRoom { get; set; }

    public Side AttackerSide { get; set; } = new();

    public Side DefenderSide { get; set; } = new();

    public Field Clone() => new()
    {
        Weather = Weather,
        Terrain = Terrain,
        IsDoubles = IsDoubles,
        IsMagicRoom = IsMagicRoom,
        IsWonderRoom = IsWonderRoom,
        AttackerSide = AttackerSide.Clone(),
        DefenderSide = DefenderSide.Clone()
    };
}

public sealed class Side
{
    public bool IsReflect { get; set; }

    public bool IsLightScreen { get; set; }

    public bool IsAuroraVeil { get; set; }

    public bool IsProtected { get; set; }

    public bool IsFriendGuard { get; set; }

    public bool IsHelpingHand { get; set; }

    public Side Clone() => new()
    {
        IsReflect = IsReflect,
        IsLightScreen = IsLightScreen,
        IsAuroraVeil = IsAuroraVeil,
        IsProtected = IsProtected,
        IsFriendGuard = IsFriendGuard,
        IsHelpingHand = IsHelpingHand
    };
}
