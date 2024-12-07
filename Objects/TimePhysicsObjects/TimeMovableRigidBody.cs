using EscapedfromTime.Components.TimeTravelHandler;
using EscapedfromTime.Helper;
using Godot;

namespace EscapedfromTime.Objects.TimePhysicsObjects;

[GlobalClass]
public partial class TimeMovableRigidBody : RigidBody3D
{
	[ExportCategory("Object Properties")]
	[Export] public float MaxSpeed = 5.0f;
	[Export] public float MaxForce = 100.0f;

	private TimeMechanicsArea _timeMechanicsArea;
	private bool _resetAll;

	private Vector3 _initialPosition;
	private Vector3 _initialRotation;

	public override void _Ready()
	{
		_timeMechanicsArea = TimeMechanicsHelper.GetTimeMechanicsAreaFrom(this);
		_timeMechanicsArea.ReStartTime += _onTimeRestart;
		_initialPosition = GlobalPosition;
		_initialRotation = GlobalRotation;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_resetAll)
		{
			ApplyForce(Vector3.Zero);
			LinearVelocity = Vector3.Zero;
			AngularVelocity = Vector3.Zero;
			GlobalPosition = _initialPosition;
			GlobalRotation = _initialRotation;
			_resetAll = false;
			return;
		}

		if (LinearVelocity.Length() > MaxSpeed)
			LinearVelocity = LinearVelocity.Normalized() * MaxSpeed;

		if (ConstantForce.Length() > MaxForce)
			ConstantForce = ConstantForce.Normalized() * MaxForce;
	}

	private void _onTimeRestart()
	{
		_resetAll = true;
	}
}
