using Godot;

namespace EscapedfromTime.Scenes.Maps;

public partial class DungeonLevel : Node3D
{
	private SubViewportContainer _winMenu;

	public override void _Ready()
	{
		_winMenu = GetNode<SubViewportContainer>("WinMenu");
	}

	public void OnPlayerWin(Node body)
	{
		GD.Print($"Body {body.Name} entered the win room");
		if (!body.IsInGroup("player")) return;
		GD.Print("Player Win!");
		_winMenu.SetVisible(true);
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
}
