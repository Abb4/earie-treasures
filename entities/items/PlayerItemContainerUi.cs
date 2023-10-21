using System;
using System.Collections.Generic;
using Godot;
using utilities;

namespace Entities.Items;

public partial class PlayerItemContainerUi : Godot.Container
{
    [Signal] public delegate void PlayerItemClickedEventHandler(PlayerItem playerItem);

    [Export] public PackedScene PlayerItemUiScene;
    [Export] public PackedScene EmptyPlayerItemUiScene;

    public override void _Ready()
    {
        PlayerItemUiScene.AssertEditorPropertySet(nameof(PlayerItemUiScene));
        EmptyPlayerItemUiScene.AssertEditorPropertySet(nameof(EmptyPlayerItemUiScene));
    }

    public void ConfigureUiFromItems(IReadOnlyList<PlayerItem> containerItems)
    {
        foreach (var item in containerItems)
        {
            AddItemUi(item);
        }
    }

    private void OnPlayerItemClicked(PlayerItem playerItem)
    {
        EmitSignal(nameof(PlayerItemClicked), playerItem);
    }

    public void PadContainerUiWithEmptyItemSlots(int maxContainerItemCount)
    {
        int paddingChildCount = maxContainerItemCount - this.GetChildCount();

        for (int i = 0; i < paddingChildCount; i++)
        {
            var emptyItemUi = EmptyPlayerItemUiScene.Instantiate<Control>();

            this.AddChild(emptyItemUi);
        }
    }

    internal void RemoveItemUi(PlayerItem playerItem)
    {
        foreach (var child in this.GetChildren())
        {
            if (child is PlayerItemUi itemUi)
            {
                if (itemUi.PlayerItem.GetInstanceId() == playerItem.GetInstanceId())
                {
                    itemUi.PlayerItemClicked -= OnPlayerItemClicked;
                    itemUi.Hide();
                    this.RemoveChild(child);
                    child.QueueFree();
                    break;
                }
            }
        }
    }

    internal void AddItemUi(PlayerItem playerItem, bool useFirstEmptySlot = false)
    {
        var itemUi = PlayerItemUiScene.Instantiate<PlayerItemUi>();

        itemUi.ConfigureUiFromItem(playerItem);

        if (!useFirstEmptySlot)
        {
            this.AddChild(itemUi);
        }
        else
        {
            for (int i = 0; i < this.GetChildCount(); i++)
            {
                var child = this.GetChild(i);

                if (child is not PlayerItemUi && child is Control emptyDisplay)
                {
                    emptyDisplay.AddSibling(itemUi);

                    emptyDisplay.Hide();
                    this.RemoveChild(emptyDisplay);
                    emptyDisplay.QueueFree();

                    break;
                }
            }
        }

        itemUi.PlayerItemClicked += OnPlayerItemClicked;
        itemUi.Show();
    }

    internal void AdjustPaddingIfNeeded(int maximumContainerCapacity)
    {
        if (this.GetChildCount() <= maximumContainerCapacity)
        {
            return;
        }

        foreach (var child in this.GetChildren())
        {
            if (child is not PlayerItemUi && child is Control childControl)
            {
                childControl.Hide();
                this.RemoveChild(childControl);
                childControl.QueueFree();
                break;
            }
        }
    }
}
