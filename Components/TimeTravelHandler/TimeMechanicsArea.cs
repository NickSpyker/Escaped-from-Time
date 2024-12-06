using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class TimeMechanicsArea : Node3D
{
	[Signal] public delegate void ReStartTimeEventHandler();

	public uint T { get; private set; }
	public bool IsRunning { get; private set; } = true;

	public override void _PhysicsProcess(double _)
	{
		if (IsRunning) T += 1;
	}

	public void Start() { IsRunning = true; }
	public void Pause() { IsRunning = false; }

	public void ReStart()
	{
		T = 0;
		EmitSignal(SignalName.ReStartTime);
	}
}
