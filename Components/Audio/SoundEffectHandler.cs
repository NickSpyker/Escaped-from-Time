using Godot;
using System.Collections.Generic;

namespace EscapedfromTime.Components.Audio;

[GlobalClass]
public partial class SoundEffectHandler : Node
{
	[ExportCategory("Component Properties")]
	[Export] public AudioStreamPlayer3D AudioStreamPlayer { get; set; } = null!;

	private readonly Dictionary<string, AudioStream> _audioStreams = new();
	private readonly Dictionary<string, float> _volumeSettings = new();
	private readonly Dictionary<string, float> _pitchSettings = new();
	private string _currentState = "idle";

	public override void _Ready()
	{
		// Load audio streams
		_audioStreams["walk"] = GD.Load<AudioStreamPlaylist>("res://Resources/Sounds/WalkAudioStreamPlaylist.tres");
		_audioStreams["run"] = GD.Load<AudioStreamPlaylist>("res://Resources/Sounds/RunAudioStreamPlaylist.tres");
		_audioStreams["jump"] = GD.Load<AudioStream>("res://Assets/Sounds/Player/Footsteps/Stone/Stone Jump.wav");
		_audioStreams["land"] = GD.Load<AudioStream>("res://Assets/Sounds/Player/Footsteps/Stone/Stone Land.wav");

		// Set default volumes
		_volumeSettings["walk"] = -10.0f;
		_volumeSettings["run"] = -8.0f;
		_volumeSettings["jump"] = -5.0f;
		_volumeSettings["land"] = -5.0f;

		// Set default pitch scales (1.0 is normal speed)
		_pitchSettings["walk"] = 1.0f;
		_pitchSettings["run"] = 1.2f;
		_pitchSettings["jump"] = 1.0f;
		_pitchSettings["land"] = 1.0f;
	}

	public void PlayerIdle()
	{
		AudioStreamPlayer.Stop();
		_currentState = "idle";
	}

	public void PlayerWalk() => _playSound("walk");

	public void PlayerRun() => _playSound("run");

	public void PlayerJump() => _playSound("jump");

	public void PlayerLand() => _playSound("land");

	private void _playSound(string soundName)
	{
		if (!_audioStreams.ContainsKey(soundName))
		{
			GD.PrintErr($"Sound {soundName} not found!");
			return;
		}

		if (_currentState == soundName && AudioStreamPlayer.Playing) return;

		AudioStreamPlayer.Stream = _audioStreams[soundName];
		AudioStreamPlayer.VolumeDb = _volumeSettings[soundName];
		AudioStreamPlayer.PitchScale = _pitchSettings[soundName];

		_currentState = soundName;
		AudioStreamPlayer.Play();
	}
}
