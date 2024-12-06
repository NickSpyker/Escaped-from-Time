using Godot;

namespace EscapedfromTime.Objects.TopDownCamera;

public partial class TopDownCamera : Node3D
{
	[ExportCategory("Object Properties")]
	[Export] public Node3D Character = null!;
	[Export] public float LeftAndRightRotationSpeed = 0.1f;
	[Export] public float MaximumPivot = 10.0f;
	[Export] public float MinimumPivot = -60.0f;
	[Export] public float UpAndDownRotationSpeed = 0.1f;
	[Export] public Vector3 CameraOffset = new(0, 2, 0);
	[Export] public float CameraSmoothSpeed = 1.0f;

	private Vector3 _cameraVelocity = Vector3.Zero;
	private float _leftAndRightLookAngle;
	private float _upAndDownLookAngle;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		HandleFollowTarget(delta);
		HandleRotation(delta);
	}

	private void HandleFollowTarget(double delta)
	{
		Vector3 targetPosition = Character.GlobalTransform.Origin + CameraOffset;

		Vector3 smoothedPosition = _smoothDamp(GlobalTransform.Origin, targetPosition, ref _cameraVelocity, CameraSmoothSpeed, (float)delta);

		GlobalTransform = new Transform3D(Basis.Identity, smoothedPosition);
	}

	private void HandleRotation(double delta)
	{
		Vector2 mouseDelta = Input.GetLastMouseVelocity();

		mouseDelta.X *= -1;

		_leftAndRightLookAngle += mouseDelta.X * LeftAndRightRotationSpeed * (float)delta;

		_upAndDownLookAngle -= mouseDelta.Y * UpAndDownRotationSpeed * (float)delta;
		_upAndDownLookAngle = Mathf.Clamp(_upAndDownLookAngle, MinimumPivot, MaximumPivot);

		Quaternion leftRightRotation = Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(_leftAndRightLookAngle), 0));
		Quaternion upDownRotation = Quaternion.FromEuler(new Vector3(Mathf.DegToRad(_upAndDownLookAngle), 0, 0));

		Quaternion finalRotation = leftRightRotation * upDownRotation;

		Transform3D cameraTransform = GlobalTransform;
		cameraTransform.Basis = new Basis(finalRotation);
		GlobalTransform = cameraTransform;
	}

	private Vector3 _smoothDamp(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothTime, float deltaTime)
	{
		smoothTime = Mathf.Max(0.0001f, smoothTime);
		float omega = 2f / smoothTime;

		float x = omega * deltaTime;
		float exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);

		Vector3 deltaVector = target - current;
		Vector3 temp = (velocity + omega * deltaVector) * deltaTime;
		velocity = (velocity - omega * temp) * exp;

		Vector3 output = current + (deltaVector + temp) * exp;
		if (!((target - current).Dot(output - target) > 0f)) return output;
		output = target;
		velocity = Vector3.Zero;

		return output;
	}
}
