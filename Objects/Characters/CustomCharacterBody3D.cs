using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace EscapedfromTime.Objects.Characters;

public enum CustomAnimation
{
    Idle = 0,
    H1MeleeAttackChop,
    H1MeleeAttackSliceDiagonal,
    H1MeleeAttackSliceHorizontal,
    H1MeleeAttackStab,
    H1RangedAiming,
    H1RangedReload,
    H1RangedShoot,
    H1RangedShooting,
    H2MeleeAttackChop,
    H2MeleeAttackSlice,
    H2MeleeAttackSpin,
    H2MeleeAttackSpinning,
    H2MeleeAttackStab,
    H2MeleeIdle,
    H2RangedAiming,
    H2RangedReload,
    H2RangedShoot,
    H2RangedShooting,
    Block,
    BlockAttack,
    BlockHit,
    Blocking,
    Cheer,
    DeathA,
    DeathAPose,
    DeathB,
    DeathBPose,
    DodgeBackward,
    DodgeForward,
    DodgeLeft,
    DodgeRight,
    DualwieldMeleeAttackChop,
    DualwieldMeleeAttackSlice,
    DualwieldMeleeAttackStab,
    HitA,
    HitB,
    Interact,
    JumpFullLong,
    JumpFullShort,
    JumpIdle,
    JumpLand,
    JumpStart,
    LieDown,
    LieIdle,
    LiePose,
    LieStandUp,
    PickUp,
    RunningA,
    RunningB,
    RunningStrafeLeft,
    RunningStrafeRight,
    SitChairDown,
    SitChairIdle,
    SitChairPose,
    SitChairStandUp,
    SitFloorDown,
    SitFloorIdle,
    SitFloorPose,
    SitFloorStandUp,
    SpellcastLong,
    SpellcastRaise,
    SpellcastShoot,
    Spellcasting,
    Tpose,
    Throw,
    UnarmedIdle,
    UnarmedMeleeAttackKick,
    UnarmedMeleeAttackPunchA,
    UnarmedMeleeAttackPunchB,
    UnarmedPose,
    UseItem,
    WalkingA,
    WalkingB,
    WalkingBackwards,
    WalkingC
}

public static class CustomAnimationStringName
{
    private static readonly Dictionary<CustomAnimation, string> StringName = new()
    {
        { CustomAnimation.Idle, "Idle" },
        { CustomAnimation.H1MeleeAttackChop, "1H_Melee_Attack_Chop" },
        { CustomAnimation.H1MeleeAttackSliceDiagonal, "1H_Melee_Attack_Slice_Diagonal" },
        { CustomAnimation.H1MeleeAttackSliceHorizontal, "1H_Melee_Attack_Slice_Horizontal" },
        { CustomAnimation.H1MeleeAttackStab, "1H_Melee_Attack_Stab" },
        { CustomAnimation.H1RangedAiming, "1H_Ranged_Aiming" },
        { CustomAnimation.H1RangedReload, "1H_Ranged_Reload" },
        { CustomAnimation.H1RangedShoot, "1H_Ranged_Shoot" },
        { CustomAnimation.H1RangedShooting, "1H_Ranged_Shooting" },
        { CustomAnimation.H2MeleeAttackChop, "2H_Melee_Attack_Chop" },
        { CustomAnimation.H2MeleeAttackSlice, "2H_Melee_Attack_Slice" },
        { CustomAnimation.H2MeleeAttackSpin, "2H_Melee_Attack_Spin" },
        { CustomAnimation.H2MeleeAttackSpinning, "2H_Melee_Attack_Spinning" },
        { CustomAnimation.H2MeleeAttackStab, "2H_Melee_Attack_Stab" },
        { CustomAnimation.H2MeleeIdle, "2H_Melee_Idle" },
        { CustomAnimation.H2RangedAiming, "2H_Ranged_Aiming" },
        { CustomAnimation.H2RangedReload, "2H_Ranged_Reload" },
        { CustomAnimation.H2RangedShoot, "2H_Ranged_Shoot" },
        { CustomAnimation.H2RangedShooting, "2H_Ranged_Shooting" },
        { CustomAnimation.Block, "Block" },
        { CustomAnimation.BlockAttack, "Block_Attack" },
        { CustomAnimation.BlockHit, "Block_Hit" },
        { CustomAnimation.Blocking, "Blocking" },
        { CustomAnimation.Cheer, "Cheer" },
        { CustomAnimation.DeathA, "Death_A" },
        { CustomAnimation.DeathAPose, "Death_A_Pose" },
        { CustomAnimation.DeathB, "Death_B" },
        { CustomAnimation.DeathBPose, "Death_B_Pose" },
        { CustomAnimation.DodgeBackward, "Dodge_Backward" },
        { CustomAnimation.DodgeForward, "Dodge_Forward" },
        { CustomAnimation.DodgeLeft, "Dodge_Left" },
        { CustomAnimation.DodgeRight, "Dodge_Right" },
        { CustomAnimation.DualwieldMeleeAttackChop, "Dualwield_Melee_Attack_Chop" },
        { CustomAnimation.DualwieldMeleeAttackSlice, "Dualwield_Melee_Attack_Slice" },
        { CustomAnimation.DualwieldMeleeAttackStab, "Dualwield_Melee_Attack_Stab" },
        { CustomAnimation.HitA, "Hit_A" },
        { CustomAnimation.HitB, "Hit_B" },
        { CustomAnimation.Interact, "Interact" },
        { CustomAnimation.JumpFullLong, "Jump_Full_Long" },
        { CustomAnimation.JumpFullShort, "Jump_Full_Short" },
        { CustomAnimation.JumpIdle, "Jump_Idle" },
        { CustomAnimation.JumpLand, "Jump_Land" },
        { CustomAnimation.JumpStart, "Jump_Start" },
        { CustomAnimation.LieDown, "Lie_Down" },
        { CustomAnimation.LieIdle, "Lie_Idle" },
        { CustomAnimation.LiePose, "Lie_Pose" },
        { CustomAnimation.LieStandUp, "Lie_StandUp" },
        { CustomAnimation.PickUp, "PickUp" },
        { CustomAnimation.RunningA, "Running_A" },
        { CustomAnimation.RunningB, "Running_B" },
        { CustomAnimation.RunningStrafeLeft, "Running_Strafe_Left" },
        { CustomAnimation.RunningStrafeRight, "Running_Strafe_Right" },
        { CustomAnimation.SitChairDown, "Sit_Chair_Down" },
        { CustomAnimation.SitChairIdle, "Sit_Chair_Idle" },
        { CustomAnimation.SitChairPose, "Sit_Chair_Pose" },
        { CustomAnimation.SitChairStandUp, "Sit_Chair_StandUp" },
        { CustomAnimation.SitFloorDown, "Sit_Floor_Down" },
        { CustomAnimation.SitFloorIdle, "Sit_Floor_Idle" },
        { CustomAnimation.SitFloorPose, "Sit_Floor_Pose" },
        { CustomAnimation.SitFloorStandUp, "Sit_Floor_StandUp" },
        { CustomAnimation.SpellcastLong, "Spellcast_Long" },
        { CustomAnimation.SpellcastRaise, "Spellcast_Raise" },
        { CustomAnimation.SpellcastShoot, "Spellcast_Shoot" },
        { CustomAnimation.Spellcasting, "Spellcasting" },
        { CustomAnimation.Tpose, "T-Pose" },
        { CustomAnimation.Throw, "Throw" },
        { CustomAnimation.UnarmedIdle, "Unarmed_Idle" },
        { CustomAnimation.UnarmedMeleeAttackKick, "Unarmed_Melee_Attack_Kick" },
        { CustomAnimation.UnarmedMeleeAttackPunchA, "Unarmed_Melee_Attack_Punch_A" },
        { CustomAnimation.UnarmedMeleeAttackPunchB, "Unarmed_Melee_Attack_Punch_B" },
        { CustomAnimation.UnarmedPose, "Unarmed_Pose" },
        { CustomAnimation.UseItem, "Use_Item" },
        { CustomAnimation.WalkingA, "Walking_A" },
        { CustomAnimation.WalkingB, "Walking_B" },
        { CustomAnimation.WalkingBackwards, "Walking_Backwards" },
        { CustomAnimation.WalkingC, "Walking_C" }
    };

    public static string GetStringName(this CustomAnimation animation)
    {
        return StringName.TryGetValue(animation, out string stringName) ? stringName : animation.ToString();
    }
}

[GlobalClass]
public partial class CustomCharacterBody3D : CharacterBody3D
{
    [Export] public Node3D GlTransmissionFormatBinary;

    private AnimationPlayer _animationPlayer;
    private string _defaultAnimationStringName = ((CustomAnimation)0).GetStringName();
    public CustomAnimation CurrentAnimation { get; private set; } = 0;

    public override void _Ready()
    {
        Array<Node> glbChildren = GlTransmissionFormatBinary.GetChildren();
        _animationPlayer = glbChildren.OfType<AnimationPlayer>().FirstOrDefault();

        if (_animationPlayer is null)
        {
            GD.PushError("AnimationPlayer not found in GlTransmissionFormatBinary children.");
            throw new NullReferenceException("AnimationPlayer is required but was not found in GlTransmissionFormatBinary.");
        }

        _animationPlayer.Play(_defaultAnimationStringName);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        if (!IsOnFloor()) velocity += GetGravity() * (float)delta;

        Velocity = velocity;
        MoveAndSlide();
    }

    public void PlayAnimation(CustomAnimation animation, float blendTime = 0.2f)
    {
        if (CurrentAnimation == animation) return;

        CurrentAnimation = animation;
        _animationPlayer.Play(CurrentAnimation.GetStringName(), blendTime, 1.0f, false);
    }

    public void PlayAnimationThenPlayDefault(CustomAnimation animation, float blendTime = 0.2f)
    {
        if (CurrentAnimation == animation) return;

        CurrentAnimation = animation;
        string animationStringName = CurrentAnimation.GetStringName();
        _animationPlayer.Play(animationStringName, blendTime, 1.0f, false);
        _animationPlayer.Queue(_defaultAnimationStringName);
        _animationPlayer.SetBlendTime(animationStringName, _defaultAnimationStringName, blendTime);
    }
}
