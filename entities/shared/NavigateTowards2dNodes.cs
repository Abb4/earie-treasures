using Godot;
using Godot.Collections;
using utilities;

namespace Shared;

public partial class NavigateTowards2dNodes : Node
{
    [Export] public CharacterBody2D Parent;

    [Export] public NavigationAgent2D NavigationAgent2D;

    [Export] public Array<Node2D> Targets;

    [Export] public bool StopAfterLastTargetReached = true;

    [Export] public int MovementSpeed = 100;

    int currentTargetIndex = 0;

    bool navigationReady = false;

    bool navigationStarted = false;

    bool navigationFinished = false;

    public override void _Ready()
    {
        Parent = Parent.ToNodeResult(nameof(Parent))
            .CompensateFromParent(child: this)
            .ThrowIfValueNotSet();

        NavigationAgent2D = NavigationAgent2D.ToNodeResult(nameof(NavigationAgent2D))
            .CompensateFromChildren(parent: Parent)
            .ThrowIfValueNotSet();

        NavigationAgent2D.VelocityComputed += OnSafeVelocityComputed;

        // Make sure to not await during _Ready.
        // see more: https://docs.godotengine.org/en/stable/tutorials/navigation/navigation_introduction_2d.html#setup-for-2d-scene
        Callable.From(ActorSetup).CallDeferred();
    }

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync and create navigation map
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        navigationReady = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!navigationReady)
        {
            return;
        }

        // TODO handle unreachable case
        if (!navigationFinished)
        {
            if (NavigationAgent2D.IsNavigationFinished())
            {
                if (navigationStarted)
                {
                    currentTargetIndex += 1;
                }
                else
                {
                    currentTargetIndex = 0;
                    navigationStarted = true;
                }

                if (StopAfterLastTargetReached && currentTargetIndex == Targets.Count)
                {
                    navigationFinished = true;
                    return;
                }

                if (currentTargetIndex >= Targets.Count)
                {
                    currentTargetIndex = 0;
                }

                var currentTarget = Targets[currentTargetIndex];

                // TODO this is maybe very expensive to do every process tick, need cashing
                NavigationAgent2D.TargetPosition = currentTarget.GlobalPosition;
            }
            else
            {
                var currentTarget = Targets[currentTargetIndex];

                // TODO this is maybe very expensive to do every process tick, need cashing
                NavigationAgent2D.TargetPosition = currentTarget.GlobalPosition;

                Vector2 currentAgentPosition = Parent.GlobalTransform.Origin;
                Vector2 nextPathPosition = NavigationAgent2D.GetNextPathPosition();

                Vector2 newVelocity = (nextPathPosition - currentAgentPosition).Normalized();
                newVelocity *= MovementSpeed;

                if (NavigationAgent2D.AvoidanceEnabled)
                {
                    // moving with NavigationAgent avoidance requires different handling
                    NavigationAgent2D.Velocity = newVelocity;
                }
                else
                {
                    // without avoidance simply move Parent
                    MoveParent(newVelocity);
                }
            }
        }
    }

    private void OnSafeVelocityComputed(Vector2 safeVelocity)
    {
        MoveParent(safeVelocity);
    }

    private void MoveParent(Vector2 velocity)
    {
        Parent.Velocity = velocity;
        Parent.MoveAndSlide();
    }
}
