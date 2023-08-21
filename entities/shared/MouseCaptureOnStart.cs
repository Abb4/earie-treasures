using Godot;

using utilities;

namespace Components;

public partial class MouseCaptureOnStart : Node
{
    private bool _activated = false;

    public override void _Input(InputEvent inputEvent)
    {
        if(!_activated)
        {
            InputExtensions.CaptureMouse();
            _activated = true;
        }
    }
}
