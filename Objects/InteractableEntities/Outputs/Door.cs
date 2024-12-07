using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Outputs;

public partial class Door : StaticBody3D
{
	private AnimationPlayer _animationPlayer;

	public bool IsOpen { get; private set; }

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void OpenDoor()
	{
		_animationPlayer.Play("open_door");
		IsOpen = true;
	}

	public void CloseDoor()
	{
		_animationPlayer.Play("close_door");
		IsOpen = false;
	}

	public void ToggleDoor()
	{
		IsOpen = !IsOpen;
		if (IsOpen) OpenDoor(); else CloseDoor();
	}
}
