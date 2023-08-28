
using Godot;
using static Godot.Mathf;
using utilities;

namespace Shared.Tools;

//[Tool] //TODO readd?
public partial class VisionRange2d : Node2D
{
    [Export] public int ArcWidthDeg = 60;

    [Export] public Node2D Target;

    // public override void _Input(InputEvent inputEvent)
    // {
    //     if (InputExtensions.IsMouseConfined() && inputEvent is InputEventMouseMotion)
    //     {
    //         // Redraw gizmos on mouse motion
    //         // TODO move into extension function
    //         QueueRedraw();
    //     }
    // }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        var center = new Vector2(0, 0);
        float radius = 300;

        ArcWidthDeg = 120; // TODO remove debug code

        float width = DegToRad(ArcWidthDeg / 2);

        float angleFrom = -width;
        float angleTo = width;

        var color = new Color(1, 0, 0);

        DrawArc(center, radius, angleFrom, angleTo, 10, color);

        float anglePoint = angleFrom;
        var p1 = new Vector2(Cos(anglePoint), Sin(anglePoint)) * radius;

        anglePoint = angleFrom + 2 * angleTo;
        var p2 = new Vector2(Cos(anglePoint), Sin(anglePoint)) * radius;

        DrawLine(center, p1, color);
        DrawLine(center, p2, color);

        if(Target is not null)
        {
            var enemyLineColor = Colors.DarkGray;

            var targetPosition = Target.GlobalPosition;

            var localTargetPosition = this.ToLocal(targetPosition);

            var targetAngle = this.GlobalPosition.AngleToPoint(targetPosition);

            var mouseAngleDeg = (this.GlobalRotationDegrees);
            var targetAngleDeg = RadToDeg(targetAngle);


            var difference = Abs(targetAngleDeg - mouseAngleDeg);

            if(difference > 300) difference = Abs(difference-360);

            var degWidth = RadToDeg(width);

            var targetDifference = degWidth;

            GD.Print($"difference: {difference}");
            GD.Print($"tar difference: {targetDifference}");

            if (difference < targetDifference) // target is within arc
            {
                enemyLineColor = Colors.Green;
            }

            DrawLine(center, localTargetPosition, enemyLineColor);
        }
    }
}

