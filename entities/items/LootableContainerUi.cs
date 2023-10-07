using System;
using System.Collections.Generic;
using Godot;
using utilities;

namespace Entities.Items;

public partial class LootableContainerUi : Godot.Container
{
    [Signal] public delegate void ItemClickedEventHandler(PlayerItem playerItem);

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
            var itemUi = PlayerItemUiScene.Instantiate<PlayerItemUi>();

            itemUi.ConfigureUiFromItem(item);

            itemUi.ItemClicked += OnItemClicked;

            itemUi.Show();

            this.AddChild(itemUi);
        }
    }

    private void OnItemClicked(PlayerItem playerItem)
    {
        EmitSignal(nameof(ItemClicked), playerItem);
    }

    public void PadContainerUiWithEmptyItemSlots(int padItemsCount)
    {
        for (int i = 0; i < padItemsCount; i++)
        {
            var emptyItemUi = EmptyPlayerItemUiScene.Instantiate<Control>();

            this.AddChild(emptyItemUi);
        }
    }
}
