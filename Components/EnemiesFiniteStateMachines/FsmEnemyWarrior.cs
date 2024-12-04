using System;
using EscapedfromTime.Components.CharacterAnimationsHandler;
using Godot;

namespace EscapedfromTime.Components.EnemiesFiniteStateMachines;

[GlobalClass]
public partial class FsmEnemyWarrior : Node3D
{
    [ExportCategory("Component Properties")]
    [Export] public RayCast3D RayCast;
    [Export] public CharacterAnimations CharacterAnimations;
    [Export] public CharacterBody3D Skeleton;

    private CharacterBody3D _targetPlayer;

    private State _currentState = 0;
    private enum State
    {
        Spawning,
        Wander,
        CuriouslySearching,
        CheckIfTargetVisible,
        Attacking
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (_currentState)
        {
            case State.Wander: _wander(delta); break;
            case State.CuriouslySearching: _curiouslySearching(delta); break;
            case State.CheckIfTargetVisible: _checkIfTargetVisible(); break;
            case State.Attacking: _attacking(); break;
            case State.Spawning: default: break;
        }
    }

    // ---------- Events ----------

    public void OnSpawnTimerTimemout()
    {
        _currentState = State.Wander;
        CharacterAnimations.Play(CharacterAnimation.Idle);
    }

    public void OnFieldOfViewBodyEntered(Node body)
    {
        if (body is not CharacterBody3D character || !character.Name.ToString().StartsWith("Player")) return;
        _currentState = State.CuriouslySearching;
        _targetPlayer = character;
        CharacterAnimations.Play(CharacterAnimation.SkSearching);
    }

    public void OnFieldOfTargetBodyEntered(Node body)
    {
        if (body is not CharacterBody3D character || !character.Name.ToString().StartsWith("Player")) return;
        _currentState = State.CheckIfTargetVisible;
        _targetPlayer = character;
        CharacterAnimations.Play(CharacterAnimation.SkTaunt);
    }
    public void OnFieldOfTargetBodyExited(Node body)
    {
        if (body is null || _targetPlayer != body) return;
        _currentState = State.CuriouslySearching;
        RayCast.SetTargetPosition(new Vector3(0, 0, -30));
        CharacterAnimations.Play(CharacterAnimation.SkSearching);
    }

    // ---------- States Behaviors ----------

    // To Refactor
    private void _wander(double delta)
    {
    }

    private void _curiouslySearching(double delta)
    {
    }

    private void _checkIfTargetVisible()
    {
        if (_targetPlayer == null) return;
        
        Vector3 targetPlayerLocalPosition = ToLocal(_targetPlayer.GlobalPosition);
        RayCast.SetTargetPosition(targetPlayerLocalPosition + new Vector3(0, 1.5f, 0));

        if (!RayCast.CollideWithBodies || RayCast.GetCollider() is not CharacterBody3D character || character != _targetPlayer)
            return;

        _currentState = State.Attacking;
    }

    private void _attacking()
    {
        if (_targetPlayer == null) return;
        
        Vector3 targetPlayerLocalPosition = ToLocal(_targetPlayer.GlobalPosition);
        RayCast.SetTargetPosition(targetPlayerLocalPosition + new Vector3(0, 1, 0));

        if (!RayCast.CollideWithBodies || RayCast.GetCollider() is not CharacterBody3D character || character != _targetPlayer)
        {
            _currentState = State.CheckIfTargetVisible;
            return;
        }
    }
}
