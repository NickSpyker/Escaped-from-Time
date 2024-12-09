using Godot;

namespace EscapedfromTime.Scenes.MainMenu;

public partial class MainMenu : Control
{
	private Control _settings;

	public override void _Ready()
	{
		_settings = GetNode<Control>("SettingsMenu");
	}

	/* Signal */ public void OnButtonPressedPlay()
	{
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/Maps/dungeon_level.tscn");
	}

	/* Signal */ public void OnButtonPressedDevSandbox()
	{
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/DevSandbox/dev_sandbox.tscn");
	}

	/* Signal */ public void OnButtonPressedSettings()
	{
		_settings.SetVisible(true);
	}

	/* Signal */ public void OnButtonPressedExit()
	{
		GetTree().Quit();
	}
}
