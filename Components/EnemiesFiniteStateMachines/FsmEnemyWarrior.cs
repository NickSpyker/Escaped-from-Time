using Godot;

namespace EscapedfromTime.Components.EnemiesFiniteStateMachines;

[GlobalClass]
public partial class FsmEnemyWarrior : Node
{
    [ExportCategory("Component Properties")]
    [Export] public RayCast3D RayCast;

    private State _currentState = 0;
    private enum State
    {
        Spawning,
        Wander
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (_currentState)
        {
            case State.Wander: _wander(); break;
            case State.Spawning: default: break;
        }
    }

    public void OnSpawnTimerTimemout()
    {
        _currentState = State.Wander;
    }

    public void OnFieldOfViewBodyEntered(Node body)
    {
    }

    public void OnFieldOfTargetBodyEntered(Node body)
    {
    }

    private void _wander()
    {
    }
}
