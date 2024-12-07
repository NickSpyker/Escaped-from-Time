using System;
using EscapedfromTime.Objects.TimePhysicsObjects;
using Godot;

namespace EscapedfromTime.Components.PhysicsBehaviors;

[GlobalClass]
public partial class CanMoveRigidBody : Node
{
    [ExportCategory("Component Properties")]
    [Export] public CharacterBody3D Character;
    [Export] public float CharacterMassInKg = 70f;

    public override void _PhysicsProcess(double delta)
    {
        for (int i = 0; i < Character.GetSlideCollisionCount(); i++)
        {
            KinematicCollision3D collision = Character.GetSlideCollision(i);
            if (collision.GetCollider() is not TimeMovableRigidBody rigidBody) continue;

            Vector3 normal = collision.GetNormal();

            const float upThreshold = 0.9f;
            if (normal.Dot(Vector3.Up) > upThreshold) continue;

            Vector3 pushDirection = _getPlanSignVector(-normal);

            rigidBody.ApplyForce(pushDirection * CharacterMassInKg);
        }
    }

    private Vector3 _getPlanSignVector(Vector3 vector)
    {
        return new Vector3(Math.Sign(vector.X), 0, Math.Sign(vector.Z));
    }
}
