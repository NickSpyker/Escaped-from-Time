using System;
using System.Collections.Generic;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class CharacterTimeGhostPlayer : Node
{
    [ExportCategory("Component Properties")]
    [Export] public CharacterBody3D Character;
    [Export] public float ActionsEndedGravity = -9.8f;

    private Dictionary<uint, List<TimeEvent>> _timeEvents = new();

    private TimeMechanicsArea _timeMechanicsArea;

    private bool _characterTimeGhostActionsEnded;

    public override void _Ready()
    {
        Node currentParentNode = GetParent();

        while (currentParentNode != null)
        {
            if (currentParentNode is TimeMechanicsArea timeLineArea)
            {
                _timeMechanicsArea = timeLineArea;
                return;
            }

            currentParentNode = currentParentNode.GetParent();
        }

        GD.PrintErr("No TimeLineArea parent found for CharacterTimeGhostPlayer");
        throw new InvalidOperationException("CharacterTimeGhostPlayer must be a child of a TimeLineArea node. Current parent hierarchy does not contain TimeLineArea.");
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
                case TimeEventType.BackToTime:
                    _characterTimeGhostActionsEnded = true;
                    break;
            }
        });
    }
}
