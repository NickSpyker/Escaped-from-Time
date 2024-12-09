using Godot;

namespace EscapedfromTime.Scenes.WinMenu;

public partial class WinMenu : SubViewportContainer
{
	public override void _Ready() => ProcessMode = ProcessModeEnum.Always;

	/* Signal */ public void OnExitToMainMenu()
	{
		GetTree().Paused = false;
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/MainMenu/main_menu.tscn");
	}

	/* Signal */ public void OnExitToDesktop() => GetTree().Quit();
}
