using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Entities.Items;
using Godot;
using utilities;

namespace Shared.Interactions;

public partial class LootableContainer : Area2D, IInteractible<Area2D, LootableContainerUi>
{
    [Export] public PackedScene LootableContainerUiScene;
    private LootableContainerUi LootableContainerUi;

    public List<PlayerItem> containerItems = new();

    public int MaximumContainerCapacity = 9;

    public override void _Ready()
    {
        LootableContainerUiScene.AssertEditorPropertySet(nameof(LootableContainerUiScene));

        LootableContainerUi = Result.Try<LootableContainerUi, GameError>(
            () => LootableContainerUiScene.Instantiate<LootableContainerUi>(),
            errorHandler: NodeError.From)
            .Value;

        AddDummyItems();

        LootableContainerUi.ConfigureUiFromItems(containerItems);

        LootableContainerUi.PadContainerUiWithEmptyItemSlots(MaximumContainerCapacity - containerItems.Count);

        // TODO subscribe to container UI events, remove items from container if necessary using their Guids

        LootableContainerUi.Show();
    }

    private void AddDummyItems()
    {
        containerItems.Add(new PlayerItem());
        containerItems.Add(new PlayerItem());
        containerItems.Add(new PlayerItem());
    }

    public Result<LootableContainerUi, GameError> Interact(Interaction<Area2D> interaction)
    {
        return LootableContainerUi;
    }
}