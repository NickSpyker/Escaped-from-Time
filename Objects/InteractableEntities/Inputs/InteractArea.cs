using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Inputs;

public partial class InteractArea : Node3D
{
	[Export] public Label3D Label = null!;

	[Signal] public delegate void PlayerInteractedEventHandler();

	private bool _isActionInProgress;

	public override void _Ready()
	{
		Label.SetVisible(false);
	}

	public override void _Input(InputEvent @event)
	{
		if (!Label.Visible) return;

		if (_isActionInProgress && @event.IsActionReleased("player_interacts"))
		{
			_isActionInProgress = false;
			Label.SetText("INTERACTION_MESSAGE_LABEL");
			return;
		}
		if (_isActionInProgress || !@event.IsActionPressed("player_interacts")) return;

		_isActionInProgress = true;
		Label.SetText("INTERACTION_IN_PROGRESS");

		EmitSignal(SignalName.PlayerInteracted);
	}

	/* Signal */ public void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player")) return;
		Label.SetVisible(true);
		_isActionInProgress = false;
		Label.SetText("INTERACTION_MESSAGE_LABEL");
	}

	/* Signal */ public void OnBodyExited(Node body)
	{
		if (!body.IsInGroup("player")) return;
		Label.SetVisible(false);
		_isActionInProgress = false;
		Label.SetText("INTERACTION_MESSAGE_LABEL");
	}
}
