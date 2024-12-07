using Godot;

namespace EscapedfromTime.Objects.InteractableEntities;

[GlobalClass]
public partial class InputInteractableEntity : Node3D
{
    [Signal] public delegate void PlayerInteractedEventHandler();

    public void InteractEmitSignal(StringName signal, params Variant[] args)
    {
        EmitSignal(signal, args);
    }
}
