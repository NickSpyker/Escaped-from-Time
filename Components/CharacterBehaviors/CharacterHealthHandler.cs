using Godot;

namespace EscapedfromTime.Components.CharacterBehaviors;

[GlobalClass]
public partial class CharacterHealthHandler : Node
{
    [ExportCategory("Component Properties")]
    [Export] public float Health = 100f;
    [Export] public float PossibleDamageReduction = 0.7f;
    
    public bool IsAlive { get; private set; } = true;

    public bool ReduceDamage;

    public void ReceiveAttack(float damage)
    {
        if (!IsAlive) return;
        Health -= ReduceDamage ? (1 - PossibleDamageReduction) * damage : damage;

        if (Health > 0f) return;
        Health = 0f;
        IsAlive = false;
    }
}
