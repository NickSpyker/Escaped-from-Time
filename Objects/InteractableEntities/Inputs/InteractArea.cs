using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Inputs;

public partial class InteractArea : Node3D
{
	[Export] public Label3D Label = null!;

	public override void _Ready()
	{
		Label.SetVisible(false);
	}

	/* Signal */ public void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("player")) Label.SetVisible(true);
	}

	/* Signal */ public void OnBodyExited(Node body)
	{
		if (body.IsInGroup("player")) Label.SetVisible(false);
	}
}
