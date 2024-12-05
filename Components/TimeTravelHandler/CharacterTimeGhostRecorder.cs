using System;
using Godot;

namespace EscapedfromTime.Components.TimeTravelHandler;

[GlobalClass]
public partial class CharacterTimeGhostRecorder : Node
{
    [ExportCategory("Component Properties")]
    [Export] public CharacterBody3D Character;

    private TimeMechanicsArea _timeMechanicsArea;

    public override void _Ready()
    {
        Node currentNode = GetParent();

        while (currentNode != null)
        {
            if (currentNode is TimeMechanicsArea timeLineArea)
            {
                _timeMechanicsArea = timeLineArea;
                return;
            }

            currentNode = currentNode.GetParent();
        }

        GD.PrintErr("No TimeLineArea parent found for CharacterTimeGhostRecorder");
        throw new InvalidOperationException("CharacterTimeGhostRecorder must be a child of a TimeLineArea node. Current parent hierarchy does not contain TimeLineArea.");
    }

    public override void _PhysicsProcess(double delta)
    {
    }
}
