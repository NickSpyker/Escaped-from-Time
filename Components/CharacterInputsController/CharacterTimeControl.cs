using System;
using System.Linq;
using EscapedfromTime.Components.TimeTravelHandler;
using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterTimeControl : Node
{
	[ExportCategory("Component Properties")]
	[Export] public CharacterBody3D Character = null!;
	[Export] public CharacterTimeGhostRecorder TimeGhostRecorder = null!;

	private TimeMechanicsArea _timeMechanicsArea;

	private Vector3 _initialPosition;
	private Vector3 _initialRotation;

	public override void _Ready()
	{
		Node currentParentNode = GetParent();

		while (currentParentNode != null)
		{
			if (currentParentNode is TimeMechanicsArea timeLineArea)
			{
				_timeMechanicsArea = timeLineArea;
				_initialPosition = Character.GlobalPosition;
				_initialRotation = Character.GlobalRotation;
				return;
			}

			currentParentNode = currentParentNode.GetParent();
		}

		GD.PrintErr("No TimeLineArea parent found for CharacterTimeControl");
		throw new InvalidOperationException("CharacterTimeControl must be a child of a TimeLineArea node. Current parent hierarchy does not contain TimeLineArea.");
	}

	public override void _Input(InputEvent @event)
	{
		if (!@event.IsActionPressed("player_back_in_time")) return;
		Character.GlobalPosition = _initialPosition;
		Character.GlobalRotation = _initialRotation;
		Character.Velocity = Vector3.Zero;

		const string playerGhostScenePath = "res://Objects/Characters/Adventurers/player_knight_ghost.tscn";
		PackedScene playerGhostPackedScene = GD.Load<PackedScene>(playerGhostScenePath);

		CharacterBody3D newPlayerGhost = playerGhostPackedScene.Instantiate<CharacterBody3D>();

		_timeMechanicsArea.AddChild(newPlayerGhost);
		newPlayerGhost.GlobalPosition = _initialPosition;
		newPlayerGhost.GlobalRotation = _initialRotation;

		CharacterTimeGhostPlayer ghostPlayer = newPlayerGhost.GetChildren().OfType<CharacterTimeGhostPlayer>().FirstOrDefault();
		ghostPlayer?.LoadTimeEvents(TimeGhostRecorder.GetTimeEventsAndClear());

		_timeMechanicsArea.ReStart();
	}
}
