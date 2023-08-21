using Godot;

using utilities;

namespace Components;

public partial class KeyboardToggleMouseConfine : Node
{
	public override void _Input(InputEvent inputEvent)
	{
		if(Input.IsActionJustPressed("cancel"))
		{
			if (InputExtensions.IsMouseConfined())
			{
				InputExtensions.UnconfineMouse();
			} 
			else
			{
				InputExtensions.ConfineMouse();
			}
		}
	}
}
