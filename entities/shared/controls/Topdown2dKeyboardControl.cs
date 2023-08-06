using Godot;

using utilities;

namespace Shared.Controlds;

public partial class Topdown2dKeyboardControl : Node2D
{
	[Export] public CharacterBody2D Parent;
	
	[Export] public int MovementSpeed = 100;

	public override void _Ready()
	{
		Parent = this.FindParentNodeIfNotSet(Parent);
	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector2.Zero;

		if (Input.IsActionPressed("move_up"))
		{
			direction.Y -= 1f;
		}
		
		if (Input.IsActionPressed("move_down"))
		{
			direction.Y += 1f;
		}

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1f;
		}

		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1f;
		}
		
		if (direction != Vector2.Zero)
		{
			direction = direction.Normalized();

			Parent.Velocity = direction * MovementSpeed;

			Parent.MoveAndSlide();
		}
	}
}
