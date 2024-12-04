using System.Linq;
using Godot;
using Godot.Collections;

namespace EscapedfromTime.Components.CharacterBehaviors;

[GlobalClass]
public partial class CharacterAttackHandler : Node
{
    [ExportCategory("Component Properties")]
    [Export] public Node3D BodyToIgnore;
    [Export] public float AttackDamage = 10f;

    private Dictionary<Rid, CharacterHealthHandler> _bodiesCloseToAttack = new();

    /* Signal */ public void AttackAreaBodyEntered(Node3D bodyNode)
    {
        if (bodyNode == BodyToIgnore || bodyNode is not CharacterBody3D body) return;

        Rid rid = body.GetRid();
        if (_bodiesCloseToAttack.ContainsKey(rid)) return;

        CharacterHealthHandler healthHandlerComponent = body.GetChildren().OfType<CharacterHealthHandler>().FirstOrDefault();
        if (healthHandlerComponent != null)
            _bodiesCloseToAttack.Add(rid, healthHandlerComponent);
    }

    /* Signal */ public void AttackAreaBodyExited(Node3D bodyNode)
    {
        if (bodyNode == BodyToIgnore || bodyNode is not CharacterBody3D body) return;

        _bodiesCloseToAttack.Remove(body.GetRid());
    }

    public void Attack()
    {
        foreach (CharacterHealthHandler bodyHealth in _bodiesCloseToAttack.Values)
            bodyHealth.ReceiveAttack(AttackDamage);
    }
}