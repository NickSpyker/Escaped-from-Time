using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Inputs;

public partial class PressToInteractArea : InputInteractableEntity
{
	[Export] public Label3D Label = null!;
	[Export] public bool ShowLabel = true!;

	private bool _canInteract;
	private bool _isActionInProgress;

	public override void _Ready()
	{
		Label.SetVisible(false);
	}

	public override void _Input(InputEvent @event)
	{
		if (!_canInteract) return;

		if (_isActionInProgress && @event.IsActionReleased("player_interacts"))
		{
			_isActionInProgress = false;
			if (ShowLabel) Label.SetText("INTERACTION_MESSAGE_LABEL");
			InteractEmitSignal(InputInteractableEntity.SignalName.PlayerStopInteraction);
			return;
		}
		if (_isActionInProgress || !@event.IsActionPressed("player_interacts")) return;

		_isActionInProgress = true;
		if (ShowLabel) Label.SetText("INTERACTION_IN_PROGRESS");

		InteractEmitSignal(InputInteractableEntity.SignalName.PlayerInteracted);
	}

	/* Signal */ public void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player")) return;
		Label.SetVisible(ShowLabel);
		_isActionInProgress = false;
		_canInteract = true;
		if (ShowLabel) Label.SetText("INTERACTION_MESSAGE_LABEL");
	}

	/* Signal */ public void OnBodyExited(Node body)
	{
		if (!body.IsInGroup("player")) return;
		Label.SetVisible(false);
		_isActionInProgress = false;
		_canInteract = false;
		if (ShowLabel) Label.SetText("INTERACTION_MESSAGE_LABEL");
	}
}
