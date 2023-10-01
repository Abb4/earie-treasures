using CSharpFunctionalExtensions;
using Godot;
using utilities;

namespace Shared.Interactions;

public partial class LootableContainer : Area2D, IInteractible<Area2D, LootableContainerUi>
{
    [Export] public PackedScene LootableContainerUiScene;
    private LootableContainerUi LootableContainerUi;

    public override void _Ready()
    {
        LootableContainerUiScene.AssertEditorPropertySet(nameof(LootableContainerUiScene));

        LootableContainerUi = Result.Try<LootableContainerUi, GameError>(
            () => LootableContainerUiScene.Instantiate<LootableContainerUi>(),
            errorHandler: NodeError.From)
            .Value;
    }

    public Result<LootableContainerUi, GameError> Interact(Interaction<Area2D> interaction)
    {
        return LootableContainerUi;
    }
}


public interface InteractionTrigger
{
}

public class KeydownInteractionTrigger
{
}