using System;
using Godot;

namespace EscapedfromTime.Components.TimeMechanics;

[GlobalClass]
public partial class TimeEntity : Node
{
    public Guid Id { get; private set; } = Guid.Empty;

    public override void _Ready()
    {
        Id = Guid.NewGuid();
    }
}
