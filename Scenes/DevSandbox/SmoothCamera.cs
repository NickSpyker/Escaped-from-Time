using Godot;

public partial class SmoothCamera : Node
{
	[Export] public Node3D Player;
	[Export] public SpringArm3D SpringArm;

	[Export] public float FollowSpeed = 5.0f;
	[Export] public Vector3 CameraOffset = new Vector3(0, 5, -10);

	private Vector3 _currentPosition;

	public override void _Process(double delta)
	{
		if (Player == null || SpringArm == null)
			return;

		Vector3 targetPosition = Player.GlobalTransform.Origin + CameraOffset;
		_currentPosition = _currentPosition.Lerp(targetPosition, FollowSpeed * (float)delta);

		SpringArm.GlobalTransform = new Transform3D(SpringArm.GlobalTransform.Basis, _currentPosition);
		SpringArm.LookAt(Player.GlobalTransform.Origin, Vector3.Up);
	}
}
