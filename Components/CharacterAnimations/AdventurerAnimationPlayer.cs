using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace EscapedfromTime.Components.CharactersAnimations;

public enum AdventurerAnimation
{
	Idle,
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

public static class AdventurerAnimationStringNames
{
	private static readonly Dictionary<AdventurerAnimation, string> AnimationEnumToString = new()
	{
		{ AdventurerAnimation.Idle, "Idle" },
		{ AdventurerAnimation.H1MeleeAttackChop, "1H_Melee_Attack_Chop" },
		{ AdventurerAnimation.H1MeleeAttackSliceDiagonal, "1H_Melee_Attack_Slice_Diagonal" },
		{ AdventurerAnimation.H1MeleeAttackSliceHorizontal, "1H_Melee_Attack_Slice_Horizontal" },
		{ AdventurerAnimation.H1MeleeAttackStab, "1H_Melee_Attack_Stab" },
		{ AdventurerAnimation.H1RangedAiming, "1H_Ranged_Aiming" },
		{ AdventurerAnimation.H1RangedReload, "1H_Ranged_Reload" },
		{ AdventurerAnimation.H1RangedShoot, "1H_Ranged_Shoot" },
		{ AdventurerAnimation.H1RangedShooting, "1H_Ranged_Shooting" },
		{ AdventurerAnimation.H2MeleeAttackChop, "2H_Melee_Attack_Chop" },
		{ AdventurerAnimation.H2MeleeAttackSlice, "2H_Melee_Attack_Slice" },
		{ AdventurerAnimation.H2MeleeAttackSpin, "2H_Melee_Attack_Spin" },
		{ AdventurerAnimation.H2MeleeAttackSpinning, "2H_Melee_Attack_Spinning" },
		{ AdventurerAnimation.H2MeleeAttackStab, "2H_Melee_Attack_Stab" },
		{ AdventurerAnimation.H2MeleeIdle, "2H_Melee_Idle" },
		{ AdventurerAnimation.H2RangedAiming, "2H_Ranged_Aiming" },
		{ AdventurerAnimation.H2RangedReload, "2H_Ranged_Reload" },
		{ AdventurerAnimation.H2RangedShoot, "2H_Ranged_Shoot" },
		{ AdventurerAnimation.H2RangedShooting, "2H_Ranged_Shooting" },
		{ AdventurerAnimation.Block, "Block" },
		{ AdventurerAnimation.BlockAttack, "Block_Attack" },
		{ AdventurerAnimation.BlockHit, "Block_Hit" },
		{ AdventurerAnimation.Blocking, "Blocking" },
		{ AdventurerAnimation.Cheer, "Cheer" },
		{ AdventurerAnimation.DeathA, "Death_A" },
		{ AdventurerAnimation.DeathAPose, "Death_A_Pose" },
		{ AdventurerAnimation.DeathB, "Death_B" },
		{ AdventurerAnimation.DeathBPose, "Death_B_Pose" },
		{ AdventurerAnimation.DodgeBackward, "Dodge_Backward" },
		{ AdventurerAnimation.DodgeForward, "Dodge_Forward" },
		{ AdventurerAnimation.DodgeLeft, "Dodge_Left" },
		{ AdventurerAnimation.DodgeRight, "Dodge_Right" },
		{ AdventurerAnimation.DualwieldMeleeAttackChop, "Dualwield_Melee_Attack_Chop" },
		{ AdventurerAnimation.DualwieldMeleeAttackSlice, "Dualwield_Melee_Attack_Slice" },
		{ AdventurerAnimation.DualwieldMeleeAttackStab, "Dualwield_Melee_Attack_Stab" },
		{ AdventurerAnimation.HitA, "Hit_A" },
		{ AdventurerAnimation.HitB, "Hit_B" },
		{ AdventurerAnimation.Interact, "Interact" },
		{ AdventurerAnimation.JumpFullLong, "Jump_Full_Long" },
		{ AdventurerAnimation.JumpFullShort, "Jump_Full_Short" },
		{ AdventurerAnimation.JumpIdle, "Jump_Idle" },
		{ AdventurerAnimation.JumpLand, "Jump_Land" },
		{ AdventurerAnimation.JumpStart, "Jump_Start" },
		{ AdventurerAnimation.LieDown, "Lie_Down" },
		{ AdventurerAnimation.LieIdle, "Lie_Idle" },
		{ AdventurerAnimation.LiePose, "Lie_Pose" },
		{ AdventurerAnimation.LieStandUp, "Lie_StandUp" },
		{ AdventurerAnimation.PickUp, "PickUp" },
		{ AdventurerAnimation.RunningA, "Running_A" },
		{ AdventurerAnimation.RunningB, "Running_B" },
		{ AdventurerAnimation.RunningStrafeLeft, "Running_Strafe_Left" },
		{ AdventurerAnimation.RunningStrafeRight, "Running_Strafe_Right" },
		{ AdventurerAnimation.SitChairDown, "Sit_Chair_Down" },
		{ AdventurerAnimation.SitChairIdle, "Sit_Chair_Idle" },
		{ AdventurerAnimation.SitChairPose, "Sit_Chair_Pose" },
		{ AdventurerAnimation.SitChairStandUp, "Sit_Chair_StandUp" },
		{ AdventurerAnimation.SitFloorDown, "Sit_Floor_Down" },
		{ AdventurerAnimation.SitFloorIdle, "Sit_Floor_Idle" },
		{ AdventurerAnimation.SitFloorPose, "Sit_Floor_Pose" },
		{ AdventurerAnimation.SitFloorStandUp, "Sit_Floor_StandUp" },
		{ AdventurerAnimation.SpellcastLong, "Spellcast_Long" },
		{ AdventurerAnimation.SpellcastRaise, "Spellcast_Raise" },
		{ AdventurerAnimation.SpellcastShoot, "Spellcast_Shoot" },
		{ AdventurerAnimation.Spellcasting, "Spellcasting" },
		{ AdventurerAnimation.Tpose, "T-Pose" },
		{ AdventurerAnimation.Throw, "Throw" },
		{ AdventurerAnimation.UnarmedIdle, "Unarmed_Idle" },
		{ AdventurerAnimation.UnarmedMeleeAttackKick, "Unarmed_Melee_Attack_Kick" },
		{ AdventurerAnimation.UnarmedMeleeAttackPunchA, "Unarmed_Melee_Attack_Punch_A" },
		{ AdventurerAnimation.UnarmedMeleeAttackPunchB, "Unarmed_Melee_Attack_Punch_B" },
		{ AdventurerAnimation.UnarmedPose, "Unarmed_Pose" },
		{ AdventurerAnimation.UseItem, "Use_Item" },
		{ AdventurerAnimation.WalkingA, "Walking_A" },
		{ AdventurerAnimation.WalkingB, "Walking_B" },
		{ AdventurerAnimation.WalkingBackwards, "Walking_Backwards" },
		{ AdventurerAnimation.WalkingC, "Walking_C" }
	};

	public static string ToStringName(this AdventurerAnimation animation)
	{
		if (AnimationEnumToString.TryGetValue(animation, out string animationStringName))
			return animationStringName;

		GD.PushError($"Invalid AdventurerAnimation {animation}.");
		throw new ArgumentException($"The AdventurerAnimation {animation} is not valid.");
	}
}

[GlobalClass]
public partial class AdventurerAnimationPlayer : Node
{
	[Export] public Node3D GlTransmissionFormatBinary;

	private AnimationPlayer _animationPlayer;

	public AdventurerAnimation CurrentAnimation { get; private set; } = 0;
	private string _defaultAnimationStringName = ((AdventurerAnimation)0).ToStringName();

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

	public void PlayAnimation(AdventurerAnimation animation, float blendTime = 0.2f)
	{
		if (CurrentAnimation == animation) return;

		CurrentAnimation = animation;
		_animationPlayer.Play(CurrentAnimation.ToStringName(), blendTime);
	}

	public void PlayAnimationThenPlayDefault(AdventurerAnimation animation, float blendTime = 0.2f)
	{
		if (CurrentAnimation == animation) return;

		CurrentAnimation = animation;
		string animationStringName = CurrentAnimation.ToStringName();
		_animationPlayer.Play(animationStringName, blendTime);
		_animationPlayer.Queue(_defaultAnimationStringName);
		_animationPlayer.SetBlendTime(animationStringName, _defaultAnimationStringName, blendTime);
	}
}
