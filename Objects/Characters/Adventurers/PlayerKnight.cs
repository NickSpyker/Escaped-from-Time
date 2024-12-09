using EscapedfromTime.Components.CharacterBehaviors;
using Godot;

namespace EscapedfromTime.Objects.Characters.Adventurers;

public partial class PlayerKnight : CharacterBody3D
{
	[Export] public CharacterHealthHandler HealthHandler { get; set; } = null!;
	
	private SubViewportContainer _gameOverContainer;

	public override void _Ready()
	{
		_gameOverContainer = GetNode<SubViewportContainer>("GameOverMenu");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (HealthHandler.IsAlive || _gameOverContainer.Visible) return;
		_gameOverContainer.SetVisible(true);
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GetTree().Paused = true;
	}
}
