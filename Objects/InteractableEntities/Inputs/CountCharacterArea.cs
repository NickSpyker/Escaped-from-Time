using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Inputs;

public partial class CountCharacterArea : InputInteractableEntity
{
	[Signal] public delegate void NumberOfPlayersChangeEventHandler(uint count);

	private uint _count;

	/* Signal */ public void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player")) return;
		_count++;
		InteractEmitSignal(SignalName.NumberOfPlayersChange, _count);
	}

	/* Signal */ public void OnBodyExited(Node body)
	{
		if (!body.IsInGroup("player")) return;
		_count--;
		InteractEmitSignal(SignalName.NumberOfPlayersChange, _count);
	}
}
