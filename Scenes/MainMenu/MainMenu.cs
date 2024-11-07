using Godot;

namespace EscapedfromTime.Scenes.MainMenu;

public partial class MainMenu : Control
{
	public void OnTimerTimeout()
	{
		GetTree().ChangeSceneToFile("res://Scenes/DevSandbox/dev_sandbox.tscn");
	}
}
