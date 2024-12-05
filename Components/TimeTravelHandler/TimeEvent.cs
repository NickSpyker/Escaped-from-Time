using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

public enum TimeEventType
{
    None,
    NewPosition,
    VelocityChange,
    RotationChange,
}

public struct TimeEvent
{
    public TimeEvent() {}

    public TimeEventType Type = default;
    public float FloatValue = default;
    public Vector3 VectorValue = default;
}
