using System;
using Godot;
using Array = Godot.Collections.Array;

namespace EscapedfromTime.Scenes.LoadingScreen;

public partial class LoadingScreen : Control
{
	[Export] public ProgressBar ProgressBar = null!;

	public static string TargetScenePath { get; private set; }

	private string _loadingPath;
	private Error _loadError;

	public static void LoadScene(Node currentScene, string targetScenePath)
	{
		TargetScenePath = targetScenePath;
		currentScene.GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, "res://Scenes/LoadingScreen/loading_screen.tscn");
	}

	public override void _Ready()
	{
		if (string.IsNullOrEmpty(TargetScenePath))
		{
			GD.PrintErr("No target scene specified");
			return;
		}

		GD.Print($"Change scene to {TargetScenePath}, starting to load...");

		ProgressBar.Value = 0.0;
		_loadingPath = TargetScenePath;
		_loadError = ResourceLoader.LoadThreadedRequest(TargetScenePath);

		if (_loadError != Error.Ok)
			GD.PrintErr($"Failed to start loading scene: {_loadError}");
	}

	public override void _Process(double delta)
	{
		if (string.IsNullOrEmpty(_loadingPath)) return;

		Array progress = new();
		ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_loadingPath, progress);

		switch (status)
		{
			case ResourceLoader.ThreadLoadStatus.InProgress:
				if (progress.Count > 0)
				{
					ProgressBar.Value += 0.01 * (progress[0].AsDouble() * 100.0 - ProgressBar.Value);
				}
				break;
			case ResourceLoader.ThreadLoadStatus.Loaded:
				if (ProgressBar.Value < 98.0)
				{
					ProgressBar.Value += 0.05 * (100.0 - ProgressBar.Value);
					return;
				}
				Resource resource = ResourceLoader.LoadThreadedGet(_loadingPath);
				if (resource is PackedScene scene)
					GetTree().ChangeSceneToPacked(scene);
				else
					GD.PrintErr("Failed to load scene: Resource is not a PackedScene");
				_loadingPath = null;
				break;
			case ResourceLoader.ThreadLoadStatus.Failed:
				GD.PrintErr("Failed to load scene");
				_loadingPath = null;
				break;
			case ResourceLoader.ThreadLoadStatus.InvalidResource:
				GD.PrintErr("Invalid resource");
				_loadingPath = null;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
