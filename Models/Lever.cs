using Godot;

namespace EscapedfromTime.Models;

[GlobalClass]
public partial class Lever : Node3D
{
	private AnimationPlayer _animationPlayer;
	
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("off");
	}

	public void SetOn() => _animationPlayer.Play("on");

	public void SetOff() => _animationPlayer.Play("off");
}
