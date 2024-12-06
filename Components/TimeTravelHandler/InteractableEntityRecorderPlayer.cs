using System;
using System.Collections.Generic;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class InteractableEntityRecorderPlayer : Node
{
    private readonly Dictionary<uint, List<TimeEvent>> _timeEventsToPlay = new();
    private readonly Dictionary<uint, List<TimeEvent>> _timeEventsRecord = new();

    private TimeMechanicsArea _timeMechanicsArea;

    public override void _Ready()
    {
        Node currentParentNode = GetParent();

        while (currentParentNode != null)
        {
            if (currentParentNode is TimeMechanicsArea timeLineArea)
            {
                _timeMechanicsArea = timeLineArea;
                _timeMechanicsArea.ReStartTime += _onTimeRestart;
                return;
            }

            currentParentNode = currentParentNode.GetParent();
        }

        GD.PrintErr("No TimeLineArea parent found for InteractableEntityRecorderPlayer");
        throw new InvalidOperationException("InteractableEntityRecorderPlayer must be a child of a TimeLineArea node. Current parent hierarchy does not contain TimeLineArea.");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_timeEventsToPlay.TryGetValue(_timeMechanicsArea.T, value: out List<TimeEvent> timeEvents)) return;
        foreach (TimeEvent timeEvent in timeEvents)
        {
            // TODO
        }
    }

    private void _onTimeRestart()
    {
        foreach ((uint t, List<TimeEvent> events) in _timeEventsRecord)
        {
            if (!_timeEventsToPlay.ContainsKey(t))
                _timeEventsToPlay[t] = new List<TimeEvent>();
            _timeEventsToPlay[t].AddRange(events);
        }
        _timeEventsRecord.Clear();
    }
}
