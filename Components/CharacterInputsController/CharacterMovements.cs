using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Objects.TopDownCamera;
using Godot;

namespace EscapedfromTime.Components.CharacterInputsController;

[GlobalClass]
public partial class CharacterMovements : Node
{
    [ExportCategory("Component Properties")]
    [Export] public CharacterBody3D Character;
    [Export] public CharacterAnimations CharacterAnimations;
    [Export] public TopDownCamera TopDownCamera;

    [ExportCategory("Component Settings")]
    [Export] public float Speed = 5.0f;
    [Export] public float JumpVelocity = 4.5f;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Character.Velocity;

        if (!Character.IsOnFloor())
            velocity += Character.GetGravity() * (float)delta;

        if (Input.IsActionJustPressed("ui_accept") && Character.IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            CharacterAnimations.Play(CharacterAnimation.Jump);
        }

        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Character.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
            CharacterAnimations.Play(CharacterAnimation.Walk);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Character.Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Character.Velocity.Z, 0, Speed);
            CharacterAnimations.Play(CharacterAnimation.Idle);
        }

        Character.Velocity = velocity;
        Character.MoveAndSlide();
    }
}