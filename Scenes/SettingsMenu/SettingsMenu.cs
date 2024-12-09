using Godot;

namespace EscapedfromTime.Scenes.SettingsMenu;

public partial class SettingsMenu : Control
{
	public override void _Ready()
	{
		string optionLangNodePath = "MarginContainer/TabContainer/SETTINGS_TAB_GENERAL/VBoxContainer/GridContainer/OptionButtonLanguage";
		OptionButton languageOption = GetNode<OptionButton>(optionLangNodePath);

		languageOption.Selected = TranslationServer.GetLocale() switch { "fr_FR" => 1, "ko_KO" => 2, _ => 0 };
	}

	public void OnSettingsClose() => SetVisible(false);

	public void OnLanguageChanged(int i) => TranslationServer.SetLocale(i switch { 1 => "fr", 2 => "ko", _ => "en" });
}
