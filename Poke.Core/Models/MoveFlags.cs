namespace Poke.Core.Models;

/// <summary>
/// 招式具体细节 可能需要解析
/// </summary>
public class MoveFlags
{
    public bool Bypasssub { get; set; } = false;

    public bool Bite { get; set; } = false;

    public bool Bullet { get; set; } = false;

    public bool Charge { get; set; } = false;

    public bool Contact { get; set; } = false;

    public bool Dance { get; set; } = false;

    public bool Defrost { get; set; } = false;

    public bool Distance { get; set; } = false;

    public bool Gravity { get; set; } = false;

    public bool Heal { get; set; } = false;

    public bool Mirror { get; set; } = false;

    public bool Allyanim { get; set; } = false;

    public bool Nonsky { get; set; } = false;
    
    public bool Powder { get; set; } = false;

    public bool Protect { get; set; } = false;

    public bool Pulse { get; set; } = false;

    public bool Punch { get; set; } = false;

    public bool Recharge { get; set; } = false;

    public bool Reflectable { get; set; } = false;

    public bool Slicing { get; set; } = false;

    public bool Snatch { get; set; } = false;

    public bool Sound { get; set; } = false;

    public bool Wind { get; set; } = false;
}

public interface IMoveFlags
{
    /// <summary>
    /// 是否无视替身
    /// Ignores a target's substitute.
    /// </summary>
    bool Bypasssub { get; set; }

    /// <summary>
    /// 强壮下颚威力*1.5
    /// Power is multiplied by 1.5 when used by a Pokemon with the Ability Strong Jaw.
    /// </summary>
    bool Bite { get; set; }

    /// <summary>
    /// 弹类节能
    /// Has no effect on Pokemon with the Ability Bulletproof.
    /// </summary>
    bool Bullet { get; set; }

    /// <summary>
    /// 
    /// The user is unable to make a move between turns.
    /// </summary>
    bool Charge { get; set; }

    /// <summary>
    /// 接触技能
    /// Makes contact.
    /// </summary>
    bool Contact { get; set; }

    /// <summary>
    /// 是否是起舞类技能
    /// When used by a Pokemon, other Pokemon with the Ability Dancer can attempt to execute the same move.
    /// </summary>
    bool Dance { get; set; }

    /// <summary>
    /// 解除冰冻
    /// Thaws the user if executed successfully while the user is frozen.
    /// </summary>
    bool Defrost { get; set; }

    /// <summary>
    /// 是否在三打中能够打到任何一个人
    /// // Can target a Pokemon positioned anywhere in a Triple Battle.
    /// </summary>
    bool Distance { get; set; }

    /// <summary>
    /// Prevented from being executed or selected during Gravity's effect.
    /// </summary>
    bool Gravity { get; set; }

    /// <summary>
    /// Prevented from being executed or selected during Heal Block's effect.
    /// </summary>
    bool Heal { get; set; }

    /// <summary>
    /// Can be copied by Mirror Move.
    /// </summary>
    bool Mirror { get; set; }

    /// <summary>
    /// 对队友使用是否有动画
    /// The move has an animation when used on an ally.
    /// </summary>
    bool Allyanim { get; set; }

    /// <summary>
    /// Prevented from being executed or selected in a Sky Battle.
    /// </summary>
    bool Nonsky { get; set; }

    /// <summary>
    /// 花粉类技能
    /// Has no effect on Pokemon which are Grass-type, have the Ability Overcoat, or hold Safety Goggles.
    /// </summary>
    bool Powder { get; set; }

    /// <summary>
    /// 保护类技能
    /// Blocked by Detect, Protect, Spiky Shield, and if not a Status move, King's Shield.
    /// </summary>
    bool Protect { get; set; }

    /// <summary>
    /// 波动类技能
    /// Power is multiplied by 1.5 when used by a Pokemon with the Ability Mega Launcher.
    /// </summary>
    bool Pulse { get; set; }

    /// <summary>
    /// 拳类技能
    /// Power is multiplied by 1.2 when used by a Pokemon with the Ability Iron Fist.
    /// </summary>
    bool Punch { get; set; }

    /// <summary>
    /// 使用成功后则下一回合不能行动
    /// If this move is successful
    /// the user must recharge on the following turn and cannot make a move.
    /// </summary>
    bool Recharge { get; set; }

    /// <summary>
    /// 是否可以被反弹
    /// Bounced back to the original user by Magic Coat or the Ability Magic Bounce.
    /// </summary>
    bool Reflectable { get; set; }

    /// <summary>
    /// Power is multiplied by 1.5 when used by a Pokemon with the Ability Sharpness.
    /// </summary>
    bool Slicing { get; set; }

    /// <summary>
    /// Can be stolen from the original user and instead used by another Pokemon using Snatch.
    /// </summary>
    bool Snatch { get; set; }

    /// <summary>
    /// 声音类技能
    /// Has no effect on Pokemon with the Ability Soundproof.
    /// </summary>
    bool Sound { get; set; }

    /// <summary>
    /// Activates the Wind Power and Wind Rider Abilities.
    /// </summary>
    bool Wind { get; set; }
}