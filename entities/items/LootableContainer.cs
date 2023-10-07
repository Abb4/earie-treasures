using System;
using CSharpFunctionalExtensions;
using Godot;
using Godot.Collections;
using Shared.Interactions;
using utilities;

namespace Entities.Items;

public partial class LootableContainer : Area2D, IInteractible<Area2D, LootableContainerUi>
{
    [Signal] public delegate void ItemClickedEventHandler(PlayerItem playerItem);

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

        LootableContainerUi.ItemClicked += OnItemClicked;

        LootableContainerUi.Show();
    }

    private void OnItemClicked(PlayerItem playerItem)
    {
        EmitSignal(nameof(ItemClicked), playerItem);
    }

    public Result<LootableContainerUi, GameError> Interact(Interaction<Area2D> interaction)
    {
        return LootableContainerUi;
    }
}
