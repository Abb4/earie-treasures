using System;
using Entities.Items;
using Godot;

using utilities;

namespace Entities.Player;

public partial class PlayerInventory : Node
{
    [Export] public PlayerItemContainer PlayerInventoryItemContainer;

    public override void _Ready()
    {
        PlayerInventoryItemContainer.AssertEditorPropertySet(nameof(PlayerItemContainer));
    }

    internal void RegisterLootedContainer(PlayerItemContainer lootedContainer)
    {
        lootedContainer.PlayerItemClicked += OnLootedContainerItemClicked;
        PlayerInventoryItemContainer.PlayerItemClicked += OnInventoryItemClicked;
    }

    private void OnLootedContainerItemClicked(PlayerItem lootedItem, PlayerItemContainer lootedItemContainer)
    {
        lootedItemContainer.TransferItemTo(lootedItem, PlayerInventoryItemContainer);
    }

    private void OnInventoryItemClicked(PlayerItem playerItem, PlayerItemContainer playerItemContainer)
    {
        throw new NotImplementedException();
    }
}
