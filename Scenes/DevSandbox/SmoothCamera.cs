using Godot;
using EscapedfromTime.Objects.TopDownCamera;

public partial class SmoothCamera : Node
{
	[ExportCategory("Component Properties")]
	[Export] public CharacterBody3D Character;
	[Export] public TopDownCamera TopDownCamera;

	[ExportCategory("Component Settings")]
	[Export] public float FollowSpeed = 5.0f;
	// [Export] public Vector3 Offset = new Vector3(0, 1, 1);

	public override void _Process(double delta)
	{
		if (Character == null || TopDownCamera == null)
			return;

		Vector3 targetPosition = Character.GlobalTransform.Origin;
		Vector3 currentPosition = TopDownCamera.GlobalTransform.Origin;
		Vector3 newPosition = currentPosition.Lerp(targetPosition, FollowSpeed * (float)delta);

		TopDownCamera.GlobalTransform = new Transform3D(TopDownCamera.GlobalTransform.Basis, newPosition);
	}
}
