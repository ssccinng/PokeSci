using Poke.DamageCalc.Mechanics;

namespace Poke.DamageCalc;

public sealed class DamageResult
{
    public DamageResult(
        Generation generation,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field field,
        IReadOnlyList<int> damage,
        ResultDescription description)
    {
        Generation = generation;
        Attacker = attacker;
        Defender = defender;
        Move = move;
        Field = field;
        Damage = damage;
        RawDescription = description;
    }

    public Generation Generation { get; }

    public Pokemon Attacker { get; }

    public Pokemon Defender { get; }

    public Move Move { get; }

    public Field Field { get; }

    public IReadOnlyList<int> Damage { get; }

    public ResultDescription RawDescription { get; }

    public (int Min, int Max) Range()
    {
        if (Damage.Count == 0)
        {
            return (0, 0);
        }

        return (Damage.Min(), Damage.Max());
    }

    public string FullDescription(string notation = "%", bool err = true) =>
        DescriptionFormatter.FullDescription(this, notation);

    public string MoveDescription(string notation = "%") =>
        DescriptionFormatter.MoveDescription(this, notation);

    public string Recovery(string notation = "%") => string.Empty;

    public string Recoil(string notation = "%") => string.Empty;

    public string KoChance(bool err = true) => DescriptionFormatter.KoChance(this);
}

public sealed class ResultDescription
{
    public string? AttackerItem { get; set; }

    public string? AttackerAbility { get; set; }

    public string? DefenderItem { get; set; }

    public string? DefenderAbility { get; set; }

    public string? Weather { get; set; }

    public string? Terrain { get; set; }

    public string? MoveType { get; set; }

    public int? MoveBasePower { get; set; }

    public bool IsCritical { get; set; }

    public bool IsProtected { get; set; }

    public bool IsBurned { get; set; }

    public bool IsReflect { get; set; }

    public bool IsLightScreen { get; set; }

    public bool IsAuroraVeil { get; set; }

    public bool IsFriendGuard { get; set; }

    public bool IsHelpingHand { get; set; }

    public string? AttackerTera { get; set; }

    public string? DefenderTera { get; set; }

    public double TypeEffectiveness { get; set; } = 1;
}
