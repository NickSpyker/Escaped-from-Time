using Godot;

namespace EscapedfromTime.Objects.TopDownCamera
{
	public partial class TopDownCamera : Node3D
	{
		[Export] public float Sensitivity = 1f;
		[Export] public float MaxRotationX = 45f;
		[Export] public float MinRotationX = -45f;
		[Export] public float MaxRotationY = 45f;
		[Export] public float MinRotationY = -45f;
		[Export] public CharacterBody3D Character;
		[Export] public float Distance = 10f;

		private Vector2 _lastMousePosition;
		private Vector2 _rotation;

		public override void _Ready()
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}

		public override void _Process(double delta)
		{
			Vector2 mouseDelta = Input.GetLastMouseVelocity();

			_rotation.X -= mouseDelta.Y * (Sensitivity / 1000.0f);
			_rotation.Y -= mouseDelta.X * (Sensitivity / 1000.0f);

			// _rotation.X = Mathf.Clamp(_rotation.X, MinRotationX, MaxRotationX);
			// _rotation.Y = Mathf.Clamp(_rotation.Y, MinRotationY, MaxRotationY);

			Vector3 targetPosition = Character.GlobalPosition;
			Vector3 offset = new Vector3(0, 0, Distance);
			offset = offset.Rotated(Vector3.Up, Mathf.DegToRad(_rotation.Y));
			offset = offset.Rotated(Vector3.Left, Mathf.DegToRad(_rotation.X));
			GlobalPosition = targetPosition + offset;
			LookAt(targetPosition, Vector3.Up);
		}
	}
}
