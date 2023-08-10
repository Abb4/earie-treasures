using Godot;
using Godot.Collections;

using utilities;

namespace Shared;

public partial class NavMesh2DSeeker : CharacterBody2D
{
	[Export] public Array<Node2D> Targets;
	
	[Export] public bool StopAfterLastTargetReached = true;

	[Export] public int MovementSpeed = 100;
	
	public override void _Ready()
	{
		if(this.TryFindNodeInChildrenRecursively<NavigateTowards2dNodes>(out var navigationComponent))
		{
			navigationComponent.Targets = Targets;
			navigationComponent.MovementSpeed = MovementSpeed;
			navigationComponent.StopAfterLastTargetReached = StopAfterLastTargetReached;
		}
	}
}
