using Godot;

namespace EscapedfromTime.Scenes.SplashScreen;

public partial class SplashScreen : Control
{
	public void OnTimerTimeout()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu/main_menu.tscn");
	}
}
