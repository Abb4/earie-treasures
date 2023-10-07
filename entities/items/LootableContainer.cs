using CSharpFunctionalExtensions;
using Godot;
using Godot.Collections;
using Shared.Interactions;
using utilities;

namespace Entities.Items;

public partial class LootableContainer : Area2D, IInteractible<Area2D, LootableContainerUi>
{
    [Export] public PackedScene LootableContainerUiScene;
    private LootableContainerUi LootableContainerUi;

    [Export] public Array<PlayerItem> containerItems = new();

    public int MaximumContainerCapacity = 9;

    public override void _Ready()
    {
        LootableContainerUiScene.AssertEditorPropertySet(nameof(LootableContainerUiScene));

        LootableContainerUi = Result.Try<LootableContainerUi, GameError>(
            () => LootableContainerUiScene.Instantiate<LootableContainerUi>(),
            errorHandler: NodeError.From)
            .Value;

        LootableContainerUi.ConfigureUiFromItems(containerItems);

        LootableContainerUi.PadContainerUiWithEmptyItemSlots(MaximumContainerCapacity - containerItems.Count);

        // TODO subscribe to container UI events, remove items from container if necessary using their Guids

        LootableContainerUi.Show();
    }

    public Result<LootableContainerUi, GameError> Interact(Interaction<Area2D> interaction)
    {
        return LootableContainerUi;
    }
}