using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterTimeControl : Node
{
	[ExportCategory("Component Properties")]
	[Export] public CharacterBody3D Character = null!;

	private Vector3 _initialPosition;
	private Vector3 _initialRotation;

	public override void _Ready()
	{
		_initialPosition = Character.GlobalPosition;
		_initialRotation = Character.GlobalRotation;
	}

	public override void _Input(InputEvent @event)
	{
		if (!@event.IsActionPressed("player_back_in_time")) return;
		Character.GlobalPosition = _initialPosition;
		Character.GlobalRotation = _initialRotation;
		Character.Velocity = Vector3.Zero;
	}
}
