using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Components.CharacterBehaviors;
using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterInteractions : Node
{
	[ExportCategory("Component Properties")]
	[Export] public CharacterAttackHandler AttackHandler;
	[Export] public CharacterHealthHandler HealthHandler;
	[Export] public CharacterAnimations Animations;

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("player_attack"))
		{
			Animations.Play(CharacterAnimation.Attack);
			AttackHandler.Attack();
		}

		if (Input.IsActionJustPressed("player_block"))
		{
			Animations.Play(CharacterAnimation.Block);
			HealthHandler.ReduceDamage = true;
		}
		else if (Input.IsActionJustReleased("player_block"))
		{
			Animations.Play(CharacterAnimation.Idle);
			HealthHandler.ReduceDamage = false;
		}
	}
}
