using System;
using Godot;

namespace EscapedfromTime.Components.CharacterBehaviors;

[GlobalClass]
public partial class CharacterHealthHandler : Node
{
    [ExportCategory("Component Properties")]
    [Export] public float Health = 75f;
    [Export] public float HealthPassiveRecovery = 1f;
    [Export] public float PossibleDamageReduction = 0.75f;

    public bool IsAlive { get; private set; } = true;

    public bool ReduceDamage;

    public override void _PhysicsProcess(double delta)
    {
        if (Math.Abs(Health - 100f) < 0.001) return;
        Health += HealthPassiveRecovery * (float)delta;
        if (Health > 100f) Health = 100f;
    }

    public void ReceiveAttack(float damage)
    {
        if (!IsAlive) return;
        Health -= ReduceDamage ? (1 - PossibleDamageReduction) * damage : damage;

        if (Health > 0f) return;
        Health = 0f;
        IsAlive = false;
    }
}
