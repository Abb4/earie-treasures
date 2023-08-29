
using Godot;
using static Godot.Mathf;
using Godot.Collections;
using System.Collections.Generic;

namespace Shared.Tools;

public class DrawnTarget
{
    public Node2D Node;
    public bool InsideCone = false;

    public bool CollisionObsticlesObstructingSight = false;

    public Vector2 CollisionObsticlePosition = new();
}

//[Tool] //TODO readd?
public partial class VisionRange2d : Node2D
{
    [Export] public float ConeRadius = 300f;

    [Export] public int ArcWidthDeg = 60;

    [Export] public Array<Node2D> Targets;

    public List<DrawnTarget> DrawnTargets = new();

    // public override void _Input(InputEvent inputEvent)
    // {
    //     if (InputExtensions.IsMouseConfined() && inputEvent is InputEventMouseMotion)
    //     {
    //         // Redraw gizmos on mouse motion
    //         // TODO move into extension function
    //         QueueRedraw();
    //     }
    // }

    public override void _Ready()
    {
        foreach(var target in Targets)
        {
            if(target is not null)
            {
                var drawnTarget = new DrawnTarget()
                {
                    Node = target,
                };

                DrawnTargets.Add(drawnTarget);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach(var drawnTarget in this.DrawnTargets)
        {
            // NOTE reset variables before checks
            // TODO make this cleaner
            drawnTarget.InsideCone = false;
            drawnTarget.CollisionObsticlesObstructingSight = false;

            var targetPosition = drawnTarget.Node.GlobalPosition;

            if(drawnTarget.Node is RayCast2D)
            {
                var raycast = drawnTarget.Node as RayCast2D;

                if(raycast.IsColliding())
                {
                    targetPosition = raycast.GetCollisionPoint();
                }
                else
                {
                    targetPosition = raycast.ToGlobal(raycast.TargetPosition);
                }
            }

            if(targetPosition.DistanceTo(this.GlobalPosition) < ConeRadius)
            {
                var targetAngle = this.GlobalPosition.AngleToPoint(targetPosition);

                // TODO rework this with redians to avoid all the degree conversions
                // I did it for easier debugging
                var mouseAngleDeg = (this.GlobalRotationDegrees);
                var targetAngleDeg = RadToDeg(targetAngle);

                var difference = Abs(targetAngleDeg - mouseAngleDeg);

                float width = DegToRad(ArcWidthDeg / 2);

                var degWidth = RadToDeg(width);

                // TODO I don't know how this works
                // it fixes the edge case, that target is in 2 quadrant (i.e. has angle ~-170) and mouse is in 3 quadrant (i.e. has angle ~170) (-> distance would be around 300, even though it should be way lower)
                if(difference > (360 - degWidth)) difference = Abs(difference-360);

                var targetDifference = degWidth;

                if (difference < targetDifference) // target is within arc
                {
                    drawnTarget.InsideCone = true;

                    // NOTE this is collision check using raycast, see https://docs.godotengine.org/en/stable/tutorials/physics/ray-casting.html
                    // NOTE this code needs to happen in  _PhysicsProcess, see https://docs.godotengine.org/en/stable/tutorials/physics/ray-casting.html#accessing-space
                    {
                        var spaceState = this.GetWorld2D().DirectSpaceState;

                        // NOTE this is collision mask specifically for level obsticles
                        // TODO fix this in https://github.com/Abb4/earie-treasures/issues/78
                        uint enemyCollisionMask = 0b00000000_00000000_00000000_00000001; 

                        var query = PhysicsRayQueryParameters2D.Create(this.GlobalPosition, targetPosition, enemyCollisionMask);

                        var result = spaceState.IntersectRay(query);

                        if (result.Count > 0)
                        {
                            // NOTE here we will access hit dictionary, for the structure of the dict, see https://docs.godotengine.org/en/stable/tutorials/physics/ray-casting.html#raycast-query

                            //var collisionObject = result["collider"].AsGodotObject() as TileMap;

                            drawnTarget.CollisionObsticlesObstructingSight = true;
                            drawnTarget.CollisionObsticlePosition = result["position"].AsVector2();
                        }
                    }
                }
            }
        }

        // NOTE this will trigger _Draw() call
        QueueRedraw();
    }

    public override void _Draw()
    {
        var center = new Vector2(0, 0); // NOTE draw functions are local space, this center variable is actually a drawing offset from local origin

        float width = DegToRad(ArcWidthDeg / 2);

        var oneTargetFullyVisible = false;

        foreach(var drawnTarget in this.DrawnTargets)
        {
            var colorLine = Colors.DarkGray;
            var lineWidth = -1f; // -1 is always thin, see docs of DrawLine

            if(drawnTarget.InsideCone)
            {
                colorLine = Colors.Green;
                lineWidth = 10f;

                if(!drawnTarget.CollisionObsticlesObstructingSight)
                {
                    oneTargetFullyVisible = true;
                }
            }


            // NOTE this is code for drawing the line, draw the line regardless of player positions
            var targetPosition = drawnTarget.Node.GlobalPosition;

            // FIXME duplicate code, see above in _Process
            if(drawnTarget.Node is RayCast2D)
            {
                var raycast = drawnTarget.Node as RayCast2D;

                if(raycast.IsColliding())
                {
                    targetPosition = raycast.GetCollisionPoint();
                }
                else
                {
                    targetPosition = raycast.ToGlobal(raycast.TargetPosition);
                }
            }

            if(!drawnTarget.CollisionObsticlesObstructingSight)
            { 
                // Draw single line to target
                var localTargetPosition = this.ToLocal(targetPosition); // NOTE: Draw-functions are local space

                if(drawnTarget.InsideCone)
                {
                    DrawLine(center, localTargetPosition, colorLine, lineWidth); // NOTE: draw line regardless of distance
                }
            }
            else
            {
                // Draw two lines to show obstruction

                // NOTE: Draw-functions are local space
                var obsticlePosition = this.ToLocal(drawnTarget.CollisionObsticlePosition);
                var localTargetPosition = this.ToLocal(targetPosition); 

                DrawLine(center, obsticlePosition, Colors.Purple, lineWidth);
                DrawLine(obsticlePosition, localTargetPosition, Colors.Red, lineWidth);
                DrawCircle(obsticlePosition, 10f, Colors.Red);
            }
        }


        // NOTE this is code for drawing the cone, draw it regardless of targets
        // TODO move this into function/extension method
        {
            var coneWidth = -1f; // -1 is always thin, see docs of DrawLine
            var colorCone = Colors.Red;

            if(oneTargetFullyVisible)
            {
                colorCone = Colors.Green;
                coneWidth = 5f;
            }

            float angleFrom = -width;
            float angleTo = width;

            DrawArc(center, ConeRadius, angleFrom, angleTo, 10, colorCone, coneWidth);

            // TODO code can be simplified
            float anglePoint = angleFrom;
            var outerPoint1 = new Vector2(Cos(anglePoint), Sin(anglePoint)) * ConeRadius;

            anglePoint = angleFrom + 2 * angleTo;
            var outerPoint2 = new Vector2(Cos(anglePoint), Sin(anglePoint)) * ConeRadius;

            // draw connecting lines on both sides of the cone to enemy
            DrawLine(center, outerPoint1, colorCone, coneWidth);
            DrawLine(center, outerPoint2, colorCone, coneWidth);
        }
    }

}

