using EscapedfromTime.Components.CharacterAnimationsHandler;
using EscapedfromTime.Components.CharacterBehaviors;
using Godot;

namespace EscapedfromTime.Components.EnemiesFiniteStateMachines;

[GlobalClass]
public partial class FsmEnemyWarrior : Node3D
{
    [ExportCategory("Component Properties")]
    [Export] public RayCast3D RayCast = null!;
    [Export] public CharacterAnimations CharacterAnimations = null!;
    [Export] public CharacterBody3D Skeleton = null!;
    [Export] public CharacterAttackHandler CharacterAttackHandler = null!;
    [Export] public CharacterHealthHandler CharacterHealthHandler = null!;
    [Export] public float SkeletonMoveSpeed = 400f;

    private CharacterBody3D _targetedPlayer; // Skeleton favorite enemy <3

    private State _currentState = 0;
    private enum State
    {
        Spawning,
        Wander,
        CuriouslySearching,
        CheckIfTargetVisible,
        Attacking
    }
    
    private bool _canAttack;
    private bool _attackTimerEnded;
    private Timer _attackTimer;

    private bool _deadAnimationPlayed;

    public override void _Ready()
    {
        _attackTimer = GetNode<Timer>("AttackTimer");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CharacterHealthHandler.IsAlive)
        {
            if (!_deadAnimationPlayed)
                CharacterAnimations.Play(CharacterAnimation.Death);
            return;
        }
        switch (_currentState)
        {
            case State.Wander: _wander(); break;
            case State.CuriouslySearching: _curiouslySearching(); break;
            case State.CheckIfTargetVisible: _checkIfTargetVisible(); break;
            case State.Attacking: _attacking(delta); break;
            case State.Spawning: default: break;
        }
    }

    // ---------- Events ----------

    /* Signal */ public void OnSpawnTimerTimemout()
    {
        _currentState = State.Wander;
        CharacterAnimations.Play(CharacterAnimation.Idle);
    }

    /* Signal */ public void OnFieldOfViewBodyEntered(Node body)
    {
        if (body is not CharacterBody3D character || !character.IsInGroup("player")) return;
        _currentState = State.CuriouslySearching;
        _targetedPlayer = character;
        CharacterAnimations.Play(CharacterAnimation.Searching);
    }

    /* Signal */ public void OnFieldOfTargetBodyEntered(Node body)
    {
        if (body is not CharacterBody3D character || !character.IsInGroup("player")) return;
        _currentState = State.CheckIfTargetVisible;
        _targetedPlayer = character;
        CharacterAnimations.Play(CharacterAnimation.Taunt);
    }

    private uint _threshold = 10;
    /* Signal */ public void OnFieldOfTargetBodyExited(Node body)
    {
        if (body is null || _targetedPlayer != body || _threshold-- != 0) return;
        _threshold = 10;
        _currentState = State.CuriouslySearching;
        RayCast.SetTargetPosition(new Vector3(0, 0, -30));
        CharacterAnimations.Play(CharacterAnimation.Searching);
    }

    /* Signal */ public void OnPlayerInAttackArea(Node body)
    {
        if (body is not CharacterBody3D character || !character.IsInGroup("player")) return;
        _canAttack = true;
    }

    /* Signal */ public void OnPlayerOutAttackArea(Node body)
    {
        if (body is not CharacterBody3D character || !character.IsInGroup("player")) return;
        _canAttack = false;
    }

    /* Signal */ public void OnAttackTimerTimeout()
    {
        _attackTimerEnded = true;
    }

    // ---------- States Behaviors ----------

    // To Refactor
    private void _wander() { }

    private void _curiouslySearching() { }

    private void _checkIfTargetVisible()
    {
        if (_targetedPlayer == null) return;
        
        Vector3 targetPlayerLocalPosition = ToLocal(_targetedPlayer.GlobalPosition);
        RayCast.SetTargetPosition(targetPlayerLocalPosition + new Vector3(0, 1.5f, 0));

        if (!RayCast.CollideWithBodies || RayCast.GetCollider() is not CharacterBody3D character || character != _targetedPlayer)
            return;

        _currentState = State.Attacking;
    }

    private void _attacking(double delta)
    {
        if (_targetedPlayer == null) return;
        
        Vector3 targetPlayerLocalPosition = ToLocal(_targetedPlayer.GlobalPosition);
        RayCast.SetTargetPosition(targetPlayerLocalPosition + new Vector3(0, 1, 0));

        if (!RayCast.CollideWithBodies || RayCast.GetCollider() is not CharacterBody3D character || character != _targetedPlayer)
        {
            _currentState = State.CheckIfTargetVisible;
            return;
        }
        
        Skeleton.LookAt(_targetedPlayer.GlobalPosition + new Vector3(0, 1, 0), new Vector3(0, 1, 0));
        Skeleton.GlobalRotation = new Vector3(0, Skeleton.GlobalRotation.Y, 0);

        if (_canAttack)
        {
            if (!_attackTimerEnded) return;
            CharacterAnimations.Play(CharacterAnimation.Attack);
            CharacterAttackHandler.Attack();
            _attackTimer.Start(0);
            _attackTimerEnded = false;
            return;
        }

        Vector3 skeletonVelocity = (_targetedPlayer.GlobalPosition - Skeleton.GlobalPosition).Normalized();
        skeletonVelocity.Y = 0;
        CharacterAnimations.Play(skeletonVelocity != Vector3.Zero ? CharacterAnimation.Run : CharacterAnimation.Taunt);
        Skeleton.Velocity = skeletonVelocity * SkeletonMoveSpeed * (float)delta;
        Skeleton.MoveAndSlide();
    }
}
