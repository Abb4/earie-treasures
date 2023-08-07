using Godot;
using Godot.Collections;

namespace Shared;

public partial class NavMesh2DSeeker : CharacterBody2D
{
	[Export] public Array<Node2D> Targets;
	
	[Export] public bool StopAfterLastTargetReached = true;

	[Export] public int MovementSpeed = 100;
	
	[Export] public NavigateTowards2dNodes navigationComponent;
	
	public override void _Ready()
	{
		navigationComponent.Targets = Targets;		
		navigationComponent.MovementSpeed = MovementSpeed;		
		navigationComponent.StopAfterLastTargetReached = StopAfterLastTargetReached;		
	}
}
