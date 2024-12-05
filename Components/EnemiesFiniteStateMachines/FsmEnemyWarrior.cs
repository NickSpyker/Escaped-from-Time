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
    [Export] public NavigationAgent3D NavigationAgent;

    private CharacterBody3D _targetedPlayer; // Skeleton favorite enemy <3
    
    // STATE DATA: Wander
    private float _moveToAnotherPlaceTimer;

    // STATE DATA: CuriouslySearching
    
    // STATE DATA: Attacking
    
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

    /* Signal */ public void OnSpawnTimerTimemout()
    {
        _currentState = State.Wander;
        CharacterAnimations.Play(CharacterAnimation.Idle);
    }

    /* Signal */ public void OnFieldOfViewBodyEntered(Node body)
    {
        if (body is not CharacterBody3D character || !character.IsInGroup("skeleton_enemy")) return;
        _currentState = State.CuriouslySearching;
        _targetedPlayer = character;
        CharacterAnimations.Play(CharacterAnimation.Searching);
    }

    /* Signal */ public void OnFieldOfTargetBodyEntered(Node body)
    {
        if (body is not CharacterBody3D character || !character.IsInGroup("skeleton_enemy")) return;
        _currentState = State.CheckIfTargetVisible;
        _targetedPlayer = character;
        CharacterAnimations.Play(CharacterAnimation.Taunt);
    }

    /* Signal */ public void OnFieldOfTargetBodyExited(Node body)
    {
        if (body is null || _targetedPlayer != body) return;
        _currentState = State.CuriouslySearching;
        RayCast.SetTargetPosition(new Vector3(0, 0, -30));
        CharacterAnimations.Play(CharacterAnimation.Searching);
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
        if (_targetedPlayer == null) return;
        
        Vector3 targetPlayerLocalPosition = ToLocal(_targetedPlayer.GlobalPosition);
        RayCast.SetTargetPosition(targetPlayerLocalPosition + new Vector3(0, 1.5f, 0));

        if (!RayCast.CollideWithBodies || RayCast.GetCollider() is not CharacterBody3D character || character != _targetedPlayer)
            return;

        _currentState = State.Attacking;
    }

    private void _attacking()
    {
        if (_targetedPlayer == null) return;
        
        Vector3 targetPlayerLocalPosition = ToLocal(_targetedPlayer.GlobalPosition);
        RayCast.SetTargetPosition(targetPlayerLocalPosition + new Vector3(0, 1, 0));

        if (!RayCast.CollideWithBodies || RayCast.GetCollider() is not CharacterBody3D character || character != _targetedPlayer)
        {
            _currentState = State.CheckIfTargetVisible;
            return;
        }
    }
}
