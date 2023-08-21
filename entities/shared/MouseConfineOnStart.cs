using Godot;

using utilities;

namespace Components;

public partial class MouseConfineOnStart : Node
{
	private bool _activated = false;

	public override void _Input(InputEvent inputEvent)
	{
		if(!_activated)
		{
			InputExtensions.ConfineMouse();
			_activated = true;
		}
	}
}
