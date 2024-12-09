using Godot;

namespace EscapedfromTime.Objects.SoundEffect;

[GlobalClass]
public partial class PlayerSoundEffects : Node3D
{
	private AudioStreamPlayer _walking;
	private AudioStreamPlayer _running;
	private AudioStreamPlayer _jumping;
	private AudioStreamPlayer _landing;

	private AnimationPlayer _animationPlayer;
	private string _targetAnimation = "idling";
	private string _currentAnimation = "idling";

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_walking = GetNode<AudioStreamPlayer>("Walking");
		_running = GetNode<AudioStreamPlayer>("Running");
		_jumping = GetNode<AudioStreamPlayer>("Jumping");
		_landing = GetNode<AudioStreamPlayer>("Landing");
	}

	public override void _Process(double delta)
	{
		if (_animationPlayer.CurrentAnimation is not null && _animationPlayer.CurrentAnimation != "")
			_currentAnimation = _animationPlayer.CurrentAnimation;

		if (_targetAnimation == "idling")
		{
			_animationPlayer.Stop();
			_stopAllSounds();
			return;
		}

		if (_targetAnimation == _currentAnimation) return;
		_animationPlayer.Stop();
		_stopAllSounds();
		_animationPlayer.Play(_targetAnimation);
	}

	public void PlayIdling()
	{
		_animationPlayer.Stop();
		_stopAllSounds();
		_targetAnimation = "idling";
	}

	public void PlayWalking()
	{
		if (_targetAnimation == "walking") return;
		_animationPlayer.Stop();
		_stopAllSounds();
		_animationPlayer.Play("walking");
		_targetAnimation = "walking";
	}

	public void PlayRunning()
	{
		if (_targetAnimation == "running") return;
		_animationPlayer.Stop();
		_stopAllSounds();
		_animationPlayer.Play("running");
		_targetAnimation = "running";
	}

	public void PlayJumping()
	{
		_animationPlayer.Stop();
		_stopAllSounds();
		if (_targetAnimation == "jumping") return;
		_animationPlayer.Play("jumping");
		_targetAnimation = "jumping";
	}

	public void PlayLanding()
	{
		_animationPlayer.Stop();
		_stopAllSounds();
		if (_targetAnimation == "landing") return;
		_animationPlayer.Play("landing");
		_targetAnimation = "landing";
	}

	private void _stopAllSounds()
	{
		_walking.Playing = false;
		_walking.Stop();
		_running.Playing = false;
		_running.Stop();
		_jumping.Playing = false;
		_jumping.Stop();
		_landing.Playing = false;
		_landing.Stop();
	}
}
