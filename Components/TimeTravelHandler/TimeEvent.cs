using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

public enum TimeEventType
{
    None,
    VelocityChange,
    RotationChange,
    BackToTime,
    PlayerInteract
}

public struct TimeEvent
{
    public TimeEvent() {}

    public TimeEventType Type = default;

    public float FloatValue = default;
    public Vector3 VectorValue = default;
    public Transform3D TransformValue = default;
}
