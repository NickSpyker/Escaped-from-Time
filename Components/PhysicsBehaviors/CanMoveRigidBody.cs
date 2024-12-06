using Godot;

namespace EscapedfromTime.Components.PhysicsBehaviors;

[GlobalClass]
public partial class CanMoveRigidBody : Node
{
    [ExportCategory("Component Properties")]
    [Export] public CharacterBody3D Character;
    [Export] public float CharacterMassInKg = 70f;

    private Vector3 _velocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        if (Character.Velocity != Vector3.Zero)
            _velocity = Character.Velocity * CharacterMassInKg;

        for (int i = 0; i < Character.GetSlideCollisionCount(); i++)
        {
            KinematicCollision3D collision = Character.GetSlideCollision(i);
            if (collision.GetCollider() is RigidBody3D rigidBody)
            {
                rigidBody.ApplyForce(-collision.GetNormal() * _velocity);
            }
        }
    }
}
