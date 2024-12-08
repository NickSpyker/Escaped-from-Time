using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Inputs;

public partial class InteractArea : InputInteractableEntity
{
	private uint _count;

	/* Signal */ public void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player")) return;
		_count++;
		InteractEmitSignal(InputInteractableEntity.SignalName.PlayerInteracted);
	}

	/* Signal */ public void OnBodyExited(Node body)
	{
		if (!body.IsInGroup("player")) return;
		_count--;
		if (_count == 0)
			InteractEmitSignal(InputInteractableEntity.SignalName.PlayerStopInteraction);
	}
}
