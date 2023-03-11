namespace Poke.Core.Models;

/// <summary>
/// 招式具体细节 可能需要解析
/// </summary>
public class MoveFlags
{
    /// <summary>
    /// Ignores a target's substitute.
    /// </summary>
    public bool Bypasssub { get; set; } = false;

    public bool Bite { get; set; } =
        false; // Power is multiplied by 1.5 when used by a Pokemon with the Ability Strong Jaw.

    public bool Bullet { get; set; } =
        false; // Has no effect on Pokemon with the Ability Bulletproof.

    public bool Charge { get; set; } =
        false; // The user is unable to make a move between turns.

    public bool Contact { get; set; } =
        false; // Makes contact.

    public bool Dance { get; set; } =
        false; // When used by a Pokemon, other Pokemon with the Ability Dancer can attempt to execute the same move.

    public bool Defrost { get; set; } =
        false; // Thaws the user if executed successfully while the user is frozen.

    public bool Distance { get; set; } =
        false; // Can target a Pokemon positioned anywhere in a Triple Battle.

    public bool Gravity { get; set; } =
        false; // Prevented from being executed or selected during Gravity's effect.

    public bool Heal { get; set; } =
        false; // Prevented from being executed or selected during Heal Block's effect.

    public bool Mirror { get; set; } =
        false; // Can be copied by Mirror Move.

    public bool Allyanim { get; set; } =
        false; // The move has an animation when used on an ally.

    public bool Nonsky { get; set; } =
        false; // Prevented from being executed or selected in a Sky Battle.

    public bool Powder { get; set; } =
        false; // Has no effect on Pokemon which are Grass-type, have the Ability Overcoat, or hold Safety Goggles.

    public bool Protect { get; set; } =
        false; // Blocked by Detect, Protect, Spiky Shield, and if not a Status move, King's Shield.

    public bool Pulse { get; set; } =
        false; // Power is multiplied by 1.5 when used by a Pokemon with the Ability Mega Launcher.

    public bool Punch { get; set; } =
        false; // Power is multiplied by 1.2 when used by a Pokemon with the Ability Iron Fist.

    public bool Recharge { get; set; } =
        false; // If this move is successful, the user must recharge on the following turn and cannot make a move.

    public bool Reflectable { get; set; } =
        false; // Bounced back to the original user by Magic Coat or the Ability Magic Bounce.

    public bool Slicing { get; set; } =
        false; // Power is multiplied by 1.5 when used by a Pokemon with the Ability Sharpness.

    public bool Snatch { get; set; } =
        false; // Can be stolen from the original user and instead used by another Pokemon using Snatch.

    public bool Sound { get; set; } =
        false; // Has no effect on Pokemon with the Ability Soundproof.

    public bool Wind { get; set; } =
        false; // Activates the Wind Power and Wind Rider Abilities.
}

public interface IMoveFlags
{
    bool Bypasssub
    {
        get;
        set;
    } // Ignores a target's substitute.


    bool Bite
    {
        get;
        set;
    } // Power is multiplied by 1.5 when used by a Pokemon with the Ability Strong Jaw.

    bool Bullet
    {
        get;
        set;
    } // Has no effect on Pokemon with the Ability Bulletproof.

    bool Charge
    {
        get;
        set;
    } // The user is unable to make a move between turns.

    bool Contact { get; set; } // Makes contact.

    bool
        Dance
    {
        get;
        set;
    } // When used by a Pokemon, other Pokemon with the Ability Dancer can attempt to execute the same move.

    bool
        Defrost
    {
        get;
        set;
    } // Thaws the user if executed successfully while the user is frozen.

    bool
        Distance
    {
        get;
        set;
    } // Can target a Pokemon positioned anywhere in a Triple Battle.

    bool
        Gravity
    {
        get;
        set;
    } // Prevented from being executed or selected during Gravity's effect.

    bool
        Heal
    {
        get;
        set;
    } // Prevented from being executed or selected during Heal Block's effect.

    bool Mirror
    {
        get;
        set;
    } // Can be copied by Mirror Move.

    bool Allyanim
    {
        get;
        set;
    } // The move has an animation when used on an ally.

    bool
        Nonsky
    {
        get;
        set;
    } // Prevented from being executed or selected in a Sky Battle.

    bool
        Powder
    {
        get;
        set;
    } // Has no effect on Pokemon which are Grass-type, have the Ability Overcoat, or hold Safety Goggles.

    bool
        Protect
    {
        get;
        set;
    } // Blocked by Detect, Protect, Spiky Shield, and if not a Status move, King's Shield.

    bool
        Pulse
    {
        get;
        set;
    } // Power is multiplied by 1.5 when used by a Pokemon with the Ability Mega Launcher.

    bool
        Punch
    {
        get;
        set;
    } // Power is multiplied by 1.2 when used by a Pokemon with the Ability Iron Fist.

    bool
        Recharge
    {
        get;
        set;
    } // If this move is successful, the user must recharge on the following turn and cannot make a move.

    bool
        Reflectable
    {
        get;
        set;
    } // Bounced back to the original user by Magic Coat or the Ability Magic Bounce.

    bool
        Slicing
    {
        get;
        set;
    } // Power is multiplied by 1.5 when used by a Pokemon with the Ability Sharpness.

    bool
        Snatch
    {
        get;
        set;
    } // Can be stolen from the original user and instead used by another Pokemon using Snatch.

    bool Sound
    {
        get;
        set;
    } // Has no effect on Pokemon with the Ability Soundproof.

    bool Wind
    {
        get;
        set;
    } // Activates the Wind Power and Wind Rider Abilities.
}