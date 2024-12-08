using Godot;

namespace EscapedfromTime.Scenes.MainMenu;

public partial class MainMenu : Control
{
	/* Signal */ public void OnButtonPressedTutorial()
	{
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/Maps/tutorial_dungeon_level.tscn");
	}

	/* Signal */ public void OnButtonPressedPlay()
	{
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/Maps/main_dugeon_level.tscn");
	}

	/* Signal */ public void OnButtonPressedDevSandbox()
	{
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/DevSandbox/dev_sandbox.tscn");
	}

	/* Signal */ public void OnButtonPressedSettings()
	{
	}

	/* Signal */ public void OnButtonPressedExit()
	{
		GetTree().Quit();
	}
}
