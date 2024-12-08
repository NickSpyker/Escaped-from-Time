using EscapedfromTime.Helper;
using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Outputs;

public partial class Door : StaticBody3D
{
	private AnimationPlayer _animationPlayer;

	public bool IsOpen { get; private set; }

	private string _lastAnimation;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		TimeMechanicsHelper.GetTimeMechanicsAreaFrom(this).ReStartTime += () => {
			_animationPlayer.Play("close_door");
			IsOpen = false;
		};
	}

	public void OpenDoor()
	{
		if (_lastAnimation != "open_door")
		{
			_animationPlayer.Play("open_door");
			_lastAnimation = "open_door";
		}
		IsOpen = true;
	}

	public void CloseDoor()
	{
		if (_lastAnimation != "close_door")
		{
			_animationPlayer.Play("close_door");
			_lastAnimation = "close_door";
		}
		IsOpen = false;
	}

	public void ToggleDoor()
	{
		IsOpen = !IsOpen;
		if (IsOpen) OpenDoor(); else CloseDoor();
	}
}
