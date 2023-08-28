
using Godot;
using static Godot.Mathf;
using utilities;

namespace Shared.Tools;

[Tool]
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

        float width = DegToRad(ArcWidthDeg / 2);

        float angleFrom = -width;
        float angleTo = width;

        var color = new Color(1, 0, 0);

        DrawArc(center, radius, angleFrom, angleTo, 20, color);

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

            var localTargetPosition = this.ToLocal(targetPosition) ;

            var mouseAngle = this.GlobalPosition.AngleToPoint(targetPosition);

            //var difference = Abs(mouseAngle - this.GlobalRotation);

            var a = RadToDeg(mouseAngle) + 180;
            var b = (this.GlobalRotationDegrees + 180);

            GD.Print(a);
            GD.Print(b);

            var difference = Abs(a - b);

            var degWidth = RadToDeg(width);

            GD.Print(difference);
            GD.Print(degWidth);

            if (difference < degWidth) // target is within arc
            {
                enemyLineColor = Colors.Green;
            }

            DrawLine(center, localTargetPosition, enemyLineColor);
        }
    }
}

