using Godot;

namespace EscapedfromTime.Scenes.SettingsMenu;

public partial class SettingsMenu : Control
{
	public override void _Ready()
	{
		string optionLangNodePath = "MarginContainer/TabContainer/SETTINGS_TAB_GENERAL/VBoxContainer/GridContainer/OptionButtonLanguage";
		OptionButton languageOption = GetNode<OptionButton>(optionLangNodePath);

		languageOption.Selected = TranslationServer.GetLocale() switch
		{
			"fr_FR" => 1,
			"en_US" => 2,
			"it_IT" => 3,
			"de_DE" => 4,
			"ko_KR" => 5,
			"ja_JP" => 6,
			"zh_CN" => 7,
			"uk_UA" => 8,
			"ru_RU" => 9,
			_ => 0
		};
	}

	public void OnSettingsClose() => SetVisible(false);

	public void OnLanguageChanged(int i) => TranslationServer.SetLocale(i switch
	{
		1 => "fr",
		2 => "es",
		3 => "it",
		4 => "de",
		5 => "ko",
		6 => "ja",
		7 => "zh",
		8 => "uk",
		9 => "ru",
		_ => "en"
	});
}
