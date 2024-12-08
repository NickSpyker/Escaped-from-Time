using System;
using EscapedfromTime.Components.TimeTravelHandler;
using Godot;

namespace EscapedfromTime.Helper;

public static class TimeMechanicsHelper
{
    public static TimeMechanicsArea GetTimeMechanicsAreaFrom(Node node)
    {
        Node currentParentNode = node.GetParent();
        while (currentParentNode != null)
        {
            if (currentParentNode is TimeMechanicsArea timeLineArea) return timeLineArea;
            currentParentNode = currentParentNode.GetParent();
        }
        GD.PrintErr($"No TimeLineArea parent found for {node.GetName()}");
        throw new InvalidOperationException($"{node.GetName()} must be a child of a TimeLineArea node.");
    }
}
