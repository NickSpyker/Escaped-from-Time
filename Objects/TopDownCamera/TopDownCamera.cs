using Godot;

namespace EscapedfromTime.Objects.TopDownCamera
{
	public partial class TopDownCamera : Node3D
	{
		[Export] public Node3D Character;
		[Export] private Vector3 CameraOffset = new Vector3(0, 2, 0);


		[Export] private float CameraSmoothSpeed = 1.0f;
		[Export] private float leftAndRightRotationspeed = 0.1f;
		[Export] private float upAndDownRotationspeed = 0.1f;
		[Export] private float minimumPivot = -60.0f;
		[Export] private float maximumPivot = 10.0f;

		private Vector3 _cameraVelocity = Vector3.Zero;
		private float leftAndRightLookAngle;
		private float upAndDownLookAngle;

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
			if (Character == null) return;

			Vector3 targetPosition = Character.GlobalTransform.Origin + CameraOffset;

			Vector3 smoothedPosition = SmoothDamp(GlobalTransform.Origin, targetPosition, ref _cameraVelocity, CameraSmoothSpeed, (float)delta);

			GlobalTransform = new Transform3D(Basis.Identity, smoothedPosition);
		}

		private void HandleRotation(double delta)
		{
			Vector2 mouseDelta = Input.GetLastMouseVelocity();

			mouseDelta.X *= -1;


			leftAndRightLookAngle += mouseDelta.X * leftAndRightRotationspeed * (float)delta;

			upAndDownLookAngle -= mouseDelta.Y * upAndDownRotationspeed * (float)delta;
			upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

			Quaternion leftRightRotation = Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(leftAndRightLookAngle), 0));
			Quaternion upDownRotation = Quaternion.FromEuler(new Vector3(Mathf.DegToRad(upAndDownLookAngle), 0, 0));

			Quaternion finalRotation = leftRightRotation * upDownRotation;

			Transform3D cameraTransform = GlobalTransform;
			cameraTransform.Basis = new Basis(finalRotation);
			GlobalTransform = cameraTransform;
		}

		private Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothTime, float deltaTime)
		{
			float maxSpeed = Mathf.Inf;
			smoothTime = Mathf.Max(0.0001f, smoothTime);
			float omega = 2f / smoothTime;

			float x = omega * deltaTime;
			float exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);

			Vector3 deltaVector = target - current;
			Vector3 temp = (velocity + omega * deltaVector) * deltaTime;
			velocity = (velocity - omega * temp) * exp;

			Vector3 output = current + (deltaVector + temp) * exp;
			if ((target - current).Dot(output - target) > 0f)
			{
				output = target;
				velocity = Vector3.Zero;
			}

			return output;
		}
	}
}
