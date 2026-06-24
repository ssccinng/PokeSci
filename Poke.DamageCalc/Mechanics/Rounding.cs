namespace Poke.DamageCalc.Mechanics;

internal static class Rounding
{
    public static int PokeRound(double value)
    {
        var floor = Math.Floor(value);
        return value - floor > 0.5 ? (int)floor + 1 : (int)floor;
    }

    public static int ApplyMod(int value, int numerator, int denominator) =>
        PokeRound(value * numerator / (double)denominator);

    public static int ApplyRandom(int value, int roll) =>
        (int)Math.Floor(value * roll / 100.0);
}
