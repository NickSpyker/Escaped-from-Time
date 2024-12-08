using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Objects.InteractableEntities;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

public enum TimeEventType
{
    None,
    VelocityChange,
    RotationChange,
    BackToTime,
    PlayerInteract,
    PlayerAttack,
    PlayerBlock,
    TriggerAnimation
}

public struct InteractionData
{
    public InteractionData(InputInteractableEntity interactNode, StringName signal, params Variant[] args)
    {
        InteractNodeValue = interactNode;
        SignalName = signal;
        Args = args;
    }

    public readonly InputInteractableEntity InteractNodeValue = null!;
    public StringName SignalName = null!;
    public Variant[] Args = null!;
}

public struct TimeEvent
{
    public TimeEvent() {}

    public TimeEventType Type = default;

    public bool BoolValue = default;
    public float FloatValue = default;
    public Vector3 VectorValue = default;
    public Transform3D TransformValue = default;
    public InteractionData InteractDataValue = default;
    public CharacterAnimation CharacterAnimationValue = default;
}
