using Godot;

namespace EscapedfromTime.Scenes.GameOverMenu;

public partial class GameOverMenu : SubViewportContainer
{
	public override void _Ready() => ProcessMode = ProcessModeEnum.Always;

	/* Signal */ public void OnExitToMainMenu()
	{
		GetTree().Paused = false;
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/MainMenu/main_menu.tscn");
	}

	/* Signal */ public void OnExitToDesktop() => GetTree().Quit();
}
