using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Objects.TopDownCamera;
using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterMovements : Node
{
	[Export] public CharacterBody3D Character;
	[Export] public CharacterAnimations CharacterAnimations;
	[Export] public TopDownCamera SpringArm;


	[Export] public float WalkingSpeed = 5.0f;
	[Export] public float SprintSpeed = 10.0f;

	// [Export] public float JumpVelocity = 4.5f;
	// [Export] public float Gravity = -9.8f;



	// private Vector3 _velocity;
	 private float verticalInput;
	 private float horizontalInput;
	 private float moveAmount;


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		HandleMovementInput();
		HandleAllMovements();
	}

	private void HandleMovementInput()
	{
		Vector2 inputDir = Input.GetVector("player_move_left", "player_move_right", "player_move_forward", "player_move_backward");

		verticalInput = inputDir.Y;
		horizontalInput = inputDir.X;



		moveAmount = Mathf.Clamp(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput), 0.0f, 1.0f);

		if (moveAmount <= 0.5 && moveAmount > 0.0)
		{
			moveAmount = 0.5f;
		}
		else if (moveAmount > 0.5 && moveAmount <= 1.0)
		{
			moveAmount = 1.0f;
		}

		// GD.Print("Move Amount: " + moveAmount);
	}

	private void HandleAllMovements()
	{
		// if (Character.IsOnFloor())
		// {
			HandleGroundedMovement();
		// }
		// else
		// {
			// HandleAirborneMovement();
		// }
	}

	private void HandleGroundedMovement()
	{
		Vector3 forward = SpringArm.GlobalTransform.Basis.Z;
		forward.Y = 0;
		forward = forward.Normalized();

		Vector3 right = SpringArm.GlobalTransform.Basis.X;
		right.Y = 0;
		right = right.Normalized();

		Vector3 moveDirection = forward * verticalInput;
		moveDirection = moveDirection + right * horizontalInput;

		moveDirection = moveDirection.Normalized();

		moveDirection.Y = 0;

		if (moveAmount > 0.5f)
		{
			Character.Velocity = moveDirection * SprintSpeed;
		}
		else if (moveAmount <= 0.5f)
		{
			Character.Velocity = moveDirection * WalkingSpeed;
		}
		Character.LookAt(Character.GlobalTransform.Origin + moveDirection, Vector3.Up);
		Character.MoveAndSlide();
	}
}
