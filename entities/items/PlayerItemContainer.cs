using System;
using System.Linq;
using CSharpFunctionalExtensions;
using Godot;
using Godot.Collections;
using Shared.Interactions;
using utilities;

namespace Entities.Items;

public partial class PlayerItemContainer : Area2D, IInteractible<Area2D, PlayerItemContainerUi>
{
    [Signal] public delegate void PlayerItemClickedEventHandler(PlayerItem playerItem, PlayerItemContainer playerItemContainer);

    [Export] public PackedScene PlayerItemContainerUiScene;
    public PlayerItemContainerUi PlayerItemContainerUi;

    [Export] public Array<PlayerItem> ContainerItems = new();

    public int MaximumContainerCapacity = 9;

    public override void _Ready()
    {
        PlayerItemContainerUiScene.AssertEditorPropertySet(nameof(PlayerItemContainerUiScene));

        PlayerItemContainerUi = Result.Try<PlayerItemContainerUi, GameError>(
            () => PlayerItemContainerUiScene.Instantiate<PlayerItemContainerUi>(),
            errorHandler: NodeError.From)
            .Value;

        PlayerItemContainerUi.ConfigureUiFromItems(ContainerItems);

        PlayerItemContainerUi.PadContainerUiWithEmptyItemSlots(MaximumContainerCapacity);

        PlayerItemContainerUi.PlayerItemClicked += OnPlayerItemClicked;

        PlayerItemContainerUi.Show();
    }

    private void OnPlayerItemClicked(PlayerItem playerItem)
    {
        EmitSignal(nameof(PlayerItemClicked), playerItem, this);
    }

    public Result<PlayerItemContainerUi, GameError> Interact(Interaction<Area2D> interaction)
    {
        return PlayerItemContainerUi;
    }

    internal void RemovePlayerItemFromContainer(PlayerItem playerItem)
    {
        ContainerItems.Remove(playerItem);
        PlayerItemContainerUi.RemoveItemUi(playerItem);
        PlayerItemContainerUi.PadContainerUiWithEmptyItemSlots(MaximumContainerCapacity);
    }

    internal void AddPlayerItemToContainer(PlayerItem playerItem)
    {
        ContainerItems.Add(playerItem);
        PlayerItemContainerUi.AddItemUi(playerItem, useFirstEmptySlot: true);
        PlayerItemContainerUi.AdjustPaddingIfNeeded(MaximumContainerCapacity);
    }

    internal void TransferItemTo(PlayerItem lootedItem, PlayerItemContainer playerInventoryItemContainer)
    {
        this.RemovePlayerItemFromContainer(lootedItem);
        playerInventoryItemContainer.AddPlayerItemToContainer(lootedItem);
    }
}
