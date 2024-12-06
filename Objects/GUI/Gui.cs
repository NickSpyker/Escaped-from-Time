using EscapedfromTime.Components.CharacterBehaviors;
using Godot;

namespace EscapedfromTime.Objects.GUI;

public partial class Gui : SubViewportContainer
{
	[Export] public CharacterHealthHandler CharacterHealth = null!;

	private ProgressBar _progressBar;

	public override void _Ready()
	{
		_progressBar = GetNode<ProgressBar>("SubViewport/Control/MarginContainer/Control/PVbar");
	}

	public override void _PhysicsProcess(double delta)
	{
		_progressBar.Value = CharacterHealth.Health;
	}
}
