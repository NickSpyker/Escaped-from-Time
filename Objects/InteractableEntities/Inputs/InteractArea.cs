using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Inputs;

public partial class InteractArea : InputInteractableEntity
{
	/* Signal */ public void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player")) return;
		InteractEmitSignal(InputInteractableEntity.SignalName.PlayerInteracted);
	}
	
	/* Signal */ public void OnBodyExited(Node body)
	{
		if (!body.IsInGroup("player")) return;
		InteractEmitSignal(InputInteractableEntity.SignalName.PlayerStopInteraction);
	}
}
