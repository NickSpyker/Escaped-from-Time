using System;
using System.Collections.Generic;
using EscapedfromTime.Objects.InteractableEntities;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class InteractableEntityRecorderPlayer : Node
{
    [ExportCategory("Component Properties")]
    [Export] public InputInteractableEntity InteractableEntity = null!;

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
                InteractableEntity.PlayerInteracted += _onInteraction;
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
            if (timeEvent.Type is not TimeEventType.PlayerInteract) return;
            InteractionData interaction = timeEvent.InteractDataValue;
            interaction.InteractNodeValue.InteractEmitSignal(interaction.SignalName, interaction.Args);
        }
    }

    private void _onInteraction()
    {
        uint t = _timeMechanicsArea.T;
        if (!_timeEventsRecord.ContainsKey(t))
            _timeEventsRecord.Add(t, new List<TimeEvent>());

        _timeEventsRecord[t].Add(new TimeEvent {
            Type = TimeEventType.PlayerInteract,
            InteractDataValue = new InteractionData(InteractableEntity, InputInteractableEntity.SignalName.PlayerInteracted)
        });
    }

    private void _onTimeRestart()
    {
        foreach ((uint t, List<TimeEvent> events) in _timeEventsRecord)
        {
            if (!_timeEventsToPlay.ContainsKey(t))
                _timeEventsToPlay.Add(t, new List<TimeEvent>());
            _timeEventsToPlay[t].AddRange(events);
        }
        _timeEventsRecord.Clear();
    }
}
