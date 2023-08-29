using Godot;
using System;

public partial class AutoplayAnimation : AnimationPlayer
{
	[Export] string animationToAutoplay = string.Empty;

	public override void _Ready()
	{
		PlayAnimation();
	}

	public void PlayAnimation()
	{
		if(!string.IsNullOrWhiteSpace(animationToAutoplay))
		{
			this.Play(animationToAutoplay);
		}
	}
}
