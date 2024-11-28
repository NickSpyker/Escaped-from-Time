using EscapedfromTime.Components.CharacterAnimationsHandler;
using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterMovements : Node
{
	[Export] public CharacterBody3D Character;
	[Export] public CharacterAnimations CharacterAnimations;
	[Export] public SpringArm3D SpringArm;

	[Export] public float Speed = 5.0f;
	[Export] public float JumpVelocity = 4.5f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Character.Velocity;

		if (!Character.IsOnFloor())
			velocity += Character.GetGravity() * (float)delta;

		if (Input.IsActionJustPressed("ui_accept") && Character.IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			CharacterAnimations.Play(CharacterAnimation.Jump);
		}

		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (inputDir != Vector2.Zero)
		{
			Vector3 forward = -SpringArm.GlobalTransform.Basis.Z;
			forward.Y = 0;
			forward = forward.Normalized();

			Vector3 right = SpringArm.GlobalTransform.Basis.X;
			right.Y = 0;
			right = right.Normalized();

			Vector3 direction = (right * inputDir.X + forward * inputDir.Y).Normalized();

			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;

			Character.LookAt(Character.GlobalTransform.Origin + direction, Vector3.Up);

			CharacterAnimations.Play(CharacterAnimation.Walk);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Character.Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Character.Velocity.Z, 0, Speed);

			CharacterAnimations.Play(CharacterAnimation.Idle);
		}

		Character.Velocity = velocity;
		Character.MoveAndSlide();
	}
}
