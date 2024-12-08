using System.Collections.Generic;
using EscapedfromTime.Helper;
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
        _timeMechanicsArea = TimeMechanicsHelper.GetTimeMechanicsAreaFrom(this);
        InteractableEntity.PlayerInteracted += _onInteractionStarted;
        InteractableEntity.PlayerStopInteraction += _onInteractionStoped;
        _timeMechanicsArea.ReStartTime += _onTimeRestart;
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

    private void _onInteractionStarted()
    {
        uint t = _timeMechanicsArea.T;
        if (!_timeEventsRecord.ContainsKey(t))
            _timeEventsRecord.Add(t, new List<TimeEvent>());

        _timeEventsRecord[t].Add(new TimeEvent {
            Type = TimeEventType.PlayerInteract,
            InteractDataValue = new InteractionData(InteractableEntity, InputInteractableEntity.SignalName.PlayerInteracted)
        });
    }

    private void _onInteractionStoped()
    {
        uint t = _timeMechanicsArea.T;
        if (!_timeEventsRecord.ContainsKey(t))
            _timeEventsRecord.Add(t, new List<TimeEvent>());

        _timeEventsRecord[t].Add(new TimeEvent {
            Type = TimeEventType.PlayerInteract,
            InteractDataValue = new InteractionData(InteractableEntity, InputInteractableEntity.SignalName.PlayerStopInteraction)
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
