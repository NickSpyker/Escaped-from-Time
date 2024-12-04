using System.Collections.Generic;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class TimeLineArea : Node3D
{
    public bool IsRunning { get; private set; }

    private Dictionary<uint, List<string>> _tickEvents = new();
    private uint _t;

    public override void _PhysicsProcess(double _)
    {
        if (IsRunning) _t += 1;
    }

    public void Reset()
    {
        _tickEvents.Clear();
        IsRunning = false;
        _t = 0;
    }

    public void ReStart()
    {
        IsRunning = false;
        _t = 0;
    }

    public void Start()
    {
        IsRunning = true;
    }

    public void Pause()
    {
        IsRunning = false;
    }

    public void AddEvent(string eventName)
    {
        _tickEvents[_t].Add(eventName);
    }
}
