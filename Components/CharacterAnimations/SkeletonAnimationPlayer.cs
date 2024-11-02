using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace EscapedfromTime.Components.CharactersAnimations;

public enum SkeletonAnimation
{
    Idle,
    // TODO
}

public static class SkeletonAnimationStringNames
{
    private static readonly Dictionary<SkeletonAnimation, string> AnimationEnumToString = new()
    {
        { SkeletonAnimation.Idle, "Idle" },
        // TODO
    };

    public static string ToStringName(this SkeletonAnimation animation)
    {
        if (AnimationEnumToString.TryGetValue(animation, out string animationStringName))
            return animationStringName;

        GD.PushError($"Invalid SkeletonAnimation {animation}.");
        throw new ArgumentException($"The SkeletonAnimation {animation} is not valid.");
    }
}

[GlobalClass]
public partial class SkeletonAnimationPlayer : Node
{
    [Export] public Node3D GlTransmissionFormatBinary;

    private AnimationPlayer _animationPlayer;

    public SkeletonAnimation CurrentAnimation { get; private set; } = 0;
    private string _defaultAnimationStringName = ((SkeletonAnimation)0).ToStringName();

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

    public void PlayAnimation(SkeletonAnimation animation, float blendTime = 0.2f)
    {
        if (CurrentAnimation == animation) return;

        CurrentAnimation = animation;
        _animationPlayer.Play(CurrentAnimation.ToStringName(), blendTime);
    }

    public void PlayAnimationThenPlayDefault(SkeletonAnimation animation, float blendTime = 0.2f)
    {
        if (CurrentAnimation == animation) return;

        CurrentAnimation = animation;
        string animationStringName = CurrentAnimation.ToStringName();
        _animationPlayer.Play(animationStringName, blendTime);
        _animationPlayer.Queue(_defaultAnimationStringName);
        _animationPlayer.SetBlendTime(animationStringName, _defaultAnimationStringName, blendTime);
    }
}
