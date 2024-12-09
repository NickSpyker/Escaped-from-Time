using Godot;

namespace EscapedfromTime.Scenes.PauseMenu;

public partial class PauseMenu : SubViewportContainer
{
	private Control _settingsMenu;

	private bool _paused;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		_settingsMenu = GetNode<Control>("SubViewport/SettingsMenu");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed("pause")) return;
		_paused = !_paused;
		_setPause(_paused);
	}

	/* Signal */ public void OnResume()
	{
		_paused = false;
		_setPause(_paused);
	}

	/* Signal */ public void OnSettings()
	{
		_settingsMenu.SetVisible(true);
	}

	/* Signal */ public void OnExitToMainMenu()
	{
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/MainMenu/main_menu.tscn");
	}

	/* Signal */ public void OnExitToDesktop() => GetTree().Quit();

	private void _setPause(bool paused)
	{
		SetVisible(paused);
		GetTree().Paused = paused;
		Input.MouseMode = paused ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
	}
}
