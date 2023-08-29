using Godot;
using System;

public partial class VisualizeRayCast2D : RayCast2D
{
	private Vector2 DrawPosition = new();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(this is RayCast2D)
		{
			var raycast = this as RayCast2D;

			if(raycast.IsColliding())
			{
				DrawPosition = ToLocal(raycast.GetCollisionPoint());
			}
			else
			{
				DrawPosition = raycast.TargetPosition;
			}
		}

		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawCircle(DrawPosition, radius: 10f, Colors.Red);
		DrawLine(from: Vector2.Zero, DrawPosition, Colors.Red, width: 3f);
	}
}
