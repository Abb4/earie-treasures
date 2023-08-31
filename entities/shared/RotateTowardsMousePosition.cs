using Godot;

using utilities;

namespace Shared.Controlds;

public partial class RotateTowardsMousePosition : Node
{
    [Export] public Node2D Parent;

    public override void _Ready()
    {
        Parent = Parent.ToNodeResult()
            .CompensateFromParent(this)
            .ThrowIfValueNotSet();
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (InputExtensions.IsMouseConfined() && inputEvent is InputEventMouseMotion)
        {
            Parent.LookAt(Parent.GetGlobalMousePosition());
        }
    }
}
