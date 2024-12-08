using System;
using EscapedfromTime.Helper;
using Godot;

namespace EscapedfromTime.Objects.InteractableEntities.Outputs;

[GlobalClass]
public partial class PillarElevator : Node3D
{
	[Export] public uint MeterPerBody = 1;

	private const float PillarSize = 20;

	private Vector3 _initialPosition;

	private float _maxHeight;
	private float _targetHeight;

	public override void _Ready()
	{
		_initialPosition = GlobalPosition;
		_targetHeight = _initialPosition.Y;
		_maxHeight = _initialPosition.Y + PillarSize;
		TimeMechanicsHelper.GetTimeMechanicsAreaFrom(this).ReStartTime += () => {
			GlobalPosition = _initialPosition;
			_targetHeight = _initialPosition.Y;
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Math.Abs(_targetHeight - GlobalPosition.Y) < 0.001f) return;

		Vector3 newPosition = GlobalPosition;
		newPosition.Y = _lerpMove(newPosition.Y, _targetHeight, (float)delta);
		GlobalPosition = newPosition;
	}

	public void SetHeight(uint height) => _targetHeight = Math.Clamp(height * MeterPerBody + _initialPosition.Y, _initialPosition.Y, _maxHeight);

	public float _lerpMove(float from, float to, float t) => from + t * (to - from);
}
