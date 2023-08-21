using Godot;

using utilities;

namespace Components;

public partial class KeyboardToggleMouseCapture : Node
{
    public override void _Input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("cancel"))
        {
            if (InputExtensions.IsMouseCaptured())
            {
                InputExtensions.UncaptureMouse();
            }
            else
            {
                InputExtensions.CaptureMouse();
            }
        }
    }
}
