using System.Collections.Generic;
using System.Linq;
using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Helper;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class CharacterTimeGhostRecorder : Node
{
	private readonly Dictionary<uint, List<TimeEvent>> _timeEvents = new();

	private TimeMechanicsArea _timeMechanicsArea;

	public override void _Ready()
	{
		_timeMechanicsArea = TimeMechanicsHelper.GetTimeMechanicsAreaFrom(this);
	}

	public void RecordMoveAndSlide(Vector3 velocity)
	{
		uint t = _timeMechanicsArea.T;
		if (!_timeEvents.ContainsKey(t)) _timeEvents.Add(t, new List<TimeEvent>());

		_timeEvents[t].Add(new TimeEvent { Type = TimeEventType.VelocityChange, VectorValue = velocity });
	}

	public void RecordGlobalTransform(Transform3D transform)
	{
		uint t = _timeMechanicsArea.T;
		if (!_timeEvents.ContainsKey(t)) _timeEvents.Add(t, new List<TimeEvent>());

		_timeEvents[t].Add(new TimeEvent { Type = TimeEventType.RotationChange, TransformValue = transform });
	}

	public void RecordPlayerAttack()
	{
		uint t = _timeMechanicsArea.T;
		if (!_timeEvents.ContainsKey(t)) _timeEvents.Add(t, new List<TimeEvent>());

		_timeEvents[t].Add(new TimeEvent { Type = TimeEventType.PlayerAttack });
	}

	public void RecordPlayerDefending(bool isDefending)
	{
		uint t = _timeMechanicsArea.T;
		if (!_timeEvents.ContainsKey(t)) _timeEvents.Add(t, new List<TimeEvent>());

		_timeEvents[t].Add(new TimeEvent { Type = TimeEventType.PlayerBlock, BoolValue = isDefending });
	}

	public void RecordAnimation(CharacterAnimation animation)
	{
		uint t = _timeMechanicsArea.T;
		if (!_timeEvents.ContainsKey(t)) _timeEvents.Add(t, new List<TimeEvent>());

		_timeEvents[t].Add(new TimeEvent { Type = TimeEventType.TriggerAnimation, CharacterAnimationValue = animation });
	}

	public Dictionary<uint, List<TimeEvent>> GetTimeEventsAndClear()
	{
		uint t = _timeMechanicsArea.T;
		if (!_timeEvents.TryGetValue(t, out List<TimeEvent> eventsList))
			_timeEvents.Add(t, new List<TimeEvent> { new TimeEvent { Type = TimeEventType.BackToTime } });
		else
			eventsList.Add(new TimeEvent { Type = TimeEventType.BackToTime });

		Dictionary<uint, List<TimeEvent>> result = _timeEvents.ToDictionary(
			entry => entry.Key,
			entry => entry.Value.Select(timeEvent => new TimeEvent {
				Type = timeEvent.Type,
				FloatValue = timeEvent.FloatValue,
				VectorValue = timeEvent.VectorValue,
				TransformValue = timeEvent.TransformValue
			}).ToList()
		);
		_timeEvents.Clear();
		return result;
	}
}
