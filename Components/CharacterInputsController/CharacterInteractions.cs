using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Components.CharacterBehaviors;
using EscapedfromTime.Components.TimeTravelHandler;
using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterInteractions : Node
{
	[ExportCategory("Component Properties")]
	[Export] public CharacterAttackHandler AttackHandler = null!;
	[Export] public CharacterHealthHandler HealthHandler = null!;
	[Export] public CharacterAnimations Animations = null!;
	[Export] public CharacterTimeGhostRecorder TimeGhostRecorder = null!;

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("player_attack"))
		{
			Animations.Play(CharacterAnimation.Attack);
			AttackHandler.Attack();
			TimeGhostRecorder.RecordPlayerAttack();
		}

		if (Input.IsActionJustPressed("player_block"))
		{
			Animations.Play(CharacterAnimation.Block);
			HealthHandler.ReduceDamage = true;
			TimeGhostRecorder.RecordPlayerDefending(true);
		}
		else if (Input.IsActionJustReleased("player_block"))
		{
			Animations.Play(CharacterAnimation.Idle);
			HealthHandler.ReduceDamage = false;
			TimeGhostRecorder.RecordPlayerDefending(false);
		}
	}
}
