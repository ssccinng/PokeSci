namespace Poke.DamageCalc;

public sealed class StatsTable
{
    public StatsTable()
    {
    }

    public StatsTable(int value)
    {
        Hp = value;
        Atk = value;
        Def = value;
        Spa = value;
        Spd = value;
        Spe = value;
    }

    public StatsTable(int hp, int atk, int def, int spa, int spd, int spe)
    {
        Hp = hp;
        Atk = atk;
        Def = def;
        Spa = spa;
        Spd = spd;
        Spe = spe;
    }

    public int Hp { get; set; }

    public int Atk { get; set; }

    public int Def { get; set; }

    public int Spa { get; set; }

    public int Spd { get; set; }

    public int Spe { get; set; }

    public int this[StatId stat]
    {
        get => stat switch
        {
            StatId.Hp => Hp,
            StatId.Atk => Atk,
            StatId.Def => Def,
            StatId.Spa => Spa,
            StatId.Spd => Spd,
            StatId.Spe => Spe,
            _ => throw new ArgumentOutOfRangeException(nameof(stat))
        };
        set
        {
            switch (stat)
            {
                case StatId.Hp:
                    Hp = value;
                    break;
                case StatId.Atk:
                    Atk = value;
                    break;
                case StatId.Def:
                    Def = value;
                    break;
                case StatId.Spa:
                    Spa = value;
                    break;
                case StatId.Spd:
                    Spd = value;
                    break;
                case StatId.Spe:
                    Spe = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat));
            }
        }
    }

    public StatsTable Clone() => new(Hp, Atk, Def, Spa, Spd, Spe);

    public StatsTable CloneWith(StatId stat, int value)
    {
        var clone = Clone();
        clone[stat] = value;
        return clone;
    }
}

public enum StatId
{
    Hp,
    Atk,
    Def,
    Spa,
    Spd,
    Spe
}
