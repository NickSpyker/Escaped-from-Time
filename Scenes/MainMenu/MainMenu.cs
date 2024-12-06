using Godot;

namespace EscapedfromTime.Scenes.MainMenu;

public partial class MainMenu : Control
{
	/* Signal */ public void OnButtonPressedDevSandbox()
	{
		LoadingScreen.LoadingScreen.LoadScene(this, "res://Scenes/DevSandbox/dev_sandbox.tscn");
	}
}
