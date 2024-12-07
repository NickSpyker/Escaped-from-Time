using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Components.TimeTravelHandler;
using EscapedfromTime.Objects.TopDownCamera;
using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterMovements : Node
{
	[ExportCategory("Component Properties")]
	[Export] public CharacterBody3D Character = null!;
	[Export] public CharacterAnimations CharacterAnimations = null!;
	[Export] public TopDownCamera SpringArm = null!;
	[Export] public CharacterTimeGhostRecorder TimeGhostRecorder = null!;

	[Export] public float JumpingMovementSpeed = 3.0f;
	[Export] public float JumpingAndRunningMovementSpeed = 7.0f;
	[Export] public float WalkingSpeed = 5.0f;
	[Export] public float SprintSpeed = 10.0f;
	[Export] public float JumpVelocity = 4.5f;
	[Export] public float Gravity = -9.8f;

	private Vector3 _velocity;
	private float _verticalInput;
	private float _horizontalInput;
	private float _moveAmount;

	private bool _isPlayerSprintingWhileJumping;

	public override void _PhysicsProcess(double delta)
	{
		HandleMovementInput();
		HandleAllMovements(delta);
	}

	private void HandleMovementInput()
	{
		Vector2 inputDir = Input.GetVector(
			"player_move_left",
			"player_move_right",
			"player_move_forward",
			"player_move_backward"
		);

		_verticalInput = inputDir.Y;
		_horizontalInput = inputDir.X;

		_moveAmount = Mathf.Clamp(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput), 0.0f, 1.0f);

		if (!Input.IsActionPressed("player_run"))
		{
			_moveAmount *= 0.5f;
		}

		if (_moveAmount <= 0.5 && _moveAmount > 0.0)
		{
			_moveAmount = 0.5f;
		}
		else if (_moveAmount > 0.5 && _moveAmount <= 1.0)
		{
			_moveAmount = 1.0f;
		}
	}

	private void HandleAllMovements(double delta)
	{
		if (Character.IsOnFloor())
		{
			HandleGroundedMovement();

			if (Input.IsActionJustPressed("player_jump"))
			{
				_isPlayerSprintingWhileJumping = _moveAmount > 0.5;
				_velocity.Y = JumpVelocity;
			}
		}
		else
		{
			HandleAirborneMovement(delta);
		}

		Character.Velocity = _velocity;
		TimeGhostRecorder.RecordMoveAndSlide(_velocity);
		Character.MoveAndSlide();
	}

	private void HandleGroundedMovement()
	{
		_velocity.X = 0;
		_velocity.Z = 0;

		Vector3 forward = SpringArm.GlobalTransform.Basis.Z;
		forward.Y = 0;
		forward = forward.Normalized();

		Vector3 right = SpringArm.GlobalTransform.Basis.X;
		right.Y = 0;
		right = right.Normalized();

		Vector3 moveDirection = forward * _verticalInput + right * _horizontalInput;
		moveDirection = moveDirection.Normalized();

		switch (_moveAmount)
		{
			case > 0.5f:
				_velocity.X = moveDirection.X * SprintSpeed;
				_velocity.Z = moveDirection.Z * SprintSpeed;

				CharacterAnimations.Play(CharacterAnimation.Run);
				break;
			case <= 0.5f and > 0.0f:
				_velocity.X = moveDirection.X * WalkingSpeed;
				_velocity.Z = moveDirection.Z * WalkingSpeed;

				CharacterAnimations.Play(CharacterAnimation.Walk);
				break;
			default:
				_velocity.X = 0;
				_velocity.Z = 0;

				CharacterAnimations.Play(CharacterAnimation.Idle);
				break;
		}

		SmoothLookAt(moveDirection);
	}

	private void SmoothLookAt(Vector3 targetDirection)
	{
		if (targetDirection.Length() == 0) return;

		Transform3D transform = Character.GlobalTransform;
		Vector3 currentForward = transform.Basis.Z.Normalized();
		Vector3 desiredForward = -targetDirection.Normalized();

		Vector3 interpolatedForward = currentForward.Lerp(desiredForward, 0.40f);
		interpolatedForward.Y = 0;
		interpolatedForward = interpolatedForward.Normalized();

		Vector3 up = Vector3.Up;
		Vector3 right = up.Cross(interpolatedForward).Normalized();
		Vector3 adjustedUp = interpolatedForward.Cross(right).Normalized();

		transform.Basis = new Basis(right, adjustedUp, interpolatedForward);
		TimeGhostRecorder.RecordGlobalTransform(transform);
		Character.GlobalTransform = transform;
	}

	private void HandleAirborneMovement(double delta)
	{
		_velocity.Y += Gravity * (float)delta;

		CharacterAnimations.Play(CharacterAnimation.Jump);

		Vector3 forward = SpringArm.GlobalTransform.Basis.Z;
		forward.Y = 0;
		forward = forward.Normalized();

		Vector3 right = SpringArm.GlobalTransform.Basis.X;
		right.Y = 0;
		right = right.Normalized();

		Vector3 moveDirection = forward * _verticalInput + right * _horizontalInput;
		moveDirection = moveDirection.Normalized();

		if (_isPlayerSprintingWhileJumping)
		{
			_velocity.X = moveDirection.X * JumpingAndRunningMovementSpeed;
			_velocity.Z = moveDirection.Z * JumpingAndRunningMovementSpeed;
		}
		else
		{
			_velocity.X = moveDirection.X * JumpingMovementSpeed;
			_velocity.Z = moveDirection.Z * JumpingMovementSpeed;
		}

		SmoothLookAt(moveDirection);
	}
}
