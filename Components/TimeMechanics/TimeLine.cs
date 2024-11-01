using Godot;

namespace EscapedfromTime.Components.TimeMechanics;

[GlobalClass]
public partial class TimeLine : Node
{
    private float _timer = 0f;
    
    public override void _PhysicsProcess(double delta)
    {
        _timer += 0.1f;
    }
    
    public void ResetTimer()
    {
        _timer = 0f;
    }
}
