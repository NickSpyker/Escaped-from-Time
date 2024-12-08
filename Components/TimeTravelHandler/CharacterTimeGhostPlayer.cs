using System.Collections.Generic;
using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Components.CharacterBehaviors;
using EscapedfromTime.Helper;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class CharacterTimeGhostPlayer : Node
{
    [ExportCategory("Component Properties")]
    [Export] public CharacterBody3D Character;
    [Export] public CharacterAttackHandler AttackHandler = null!;
    [Export] public CharacterHealthHandler HealthHandler = null!;
    [Export] public CharacterAnimations Animations = null!;
    [Export] public float ActionsEndedGravity = -9.8f;

    private Dictionary<uint, List<TimeEvent>> _timeEvents = new();

    private TimeMechanicsArea _timeMechanicsArea;

    private Vector3 _initialPosition;
    private Vector3 _initialRotation;

    private bool _characterTimeGhostActionsEnded;

    public override void _Ready()
    {
        _initialPosition = Character.GlobalPosition;
        _initialRotation = Character.GlobalRotation;
        _timeMechanicsArea = TimeMechanicsHelper.GetTimeMechanicsAreaFrom(this);
        _timeMechanicsArea.ReStartTime += () => {
            Character.GlobalPosition = _initialPosition;
            Character.GlobalRotation = _initialRotation;
            _characterTimeGhostActionsEnded = false;
        };
    }

    public void LoadTimeEvents(Dictionary<uint, List<TimeEvent>> timeEvents)
    {
        _timeEvents = timeEvents;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_characterTimeGhostActionsEnded)
        {
            if (Character.IsOnFloor()) return;
            Vector3 velocity = Character.Velocity;
            velocity.Y += ActionsEndedGravity * (float)delta;
            Character.Velocity = velocity;
            Character.MoveAndSlide();
            return;
        }

        if (!_timeMechanicsArea.IsRunning) return;
        _timeEvents.TryGetValue(_timeMechanicsArea.T, out List<TimeEvent> timeEvents);
        timeEvents?.ForEach(timeEvent => {
            switch (timeEvent.Type)
            {
                case TimeEventType.VelocityChange:
                    Character.Velocity = timeEvent.VectorValue;
                    Character.MoveAndSlide();
                    break;
                case TimeEventType.RotationChange:
                    Character.GlobalTransform = timeEvent.TransformValue;
                    break;
                case TimeEventType.PlayerAttack:
                    Animations.Play(CharacterAnimation.Attack);
                    AttackHandler.Attack();
                    break;
                case TimeEventType.PlayerBlock:
                    bool isBlocking = timeEvent.BoolValue;
                    Animations.Play(isBlocking ? CharacterAnimation.Block : CharacterAnimation.Idle);
                    HealthHandler.ReduceDamage = isBlocking;
                    break;
                case TimeEventType.BackToTime:
                    _characterTimeGhostActionsEnded = true;
                    break;
                case TimeEventType.TriggerAnimation:
                    Animations.Play(timeEvent.CharacterAnimationValue);
                    break;
            }
        });
    }
}
