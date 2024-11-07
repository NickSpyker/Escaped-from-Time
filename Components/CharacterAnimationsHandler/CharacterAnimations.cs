using System;
using Godot;
using Godot.Collections;

namespace EscapedfromTime.Components.CharacterAnimationsHandler;

public enum CharacterAnimation
{
    Idle,
    Interact,
    Jump,
    Walk,
    Run,
    Attack,
    Block,
    BlockHit,
    Death,
    Resurrect
}

public static class CharacterAnimationStringNames
{
	private static readonly Dictionary<CharacterAnimation, string> AnimationEnumToString = new()
	{
		{ CharacterAnimation.Idle, "idle" },
		{ CharacterAnimation.Interact, "interact" },
		{ CharacterAnimation.Jump, "jump" },
		{ CharacterAnimation.Walk, "walk" },
		{ CharacterAnimation.Run, "run" },
		{ CharacterAnimation.Attack, "attack" },
		{ CharacterAnimation.Block, "block" },
		{ CharacterAnimation.BlockHit, "block_hit" },
		{ CharacterAnimation.Death, "death" },
		{ CharacterAnimation.Resurrect, "resurrect" }
	};

	public static string ToStringName(this CharacterAnimation animation)
	{
		if (AnimationEnumToString.TryGetValue(animation, out string animationStringName))
			return animationStringName;

		GD.PushError($"Invalid CharacterAnimation {animation}.");
		throw new ArgumentException($"Invalid CharacterAnimation {animation}.");
	}
}

[GlobalClass]
public partial class CharacterAnimations : Node
{
    [ExportCategory("Component Properties")]
    [Export] public AnimationTree AnimationTree;

    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
	    if (AnimationTree is not null)
	    {
		    _stateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
	    }
	    else
	    {
		    GD.PushError("No AnimationTree set.");
		    throw new Exception($"No AnimationTree set.");
	    }
    }

    public void Play(CharacterAnimation animation)
    {
        _stateMachine.Travel(animation.ToStringName());
    }
}
