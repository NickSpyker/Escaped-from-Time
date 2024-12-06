using Godot;

namespace EscapedfromTime.Scenes.SplashScreen;

public partial class SplashScreen : Control
{
	public override void _Ready()
	{
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/MainMenu/main_menu.tscn");
	}
}
