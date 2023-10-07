using System.Collections.Generic;
using Godot;
using utilities;

namespace Entities.Items;

public partial class LootableContainerUi : Godot.Container
{
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

            itemUi.Show();

            // TODO forward item UI events (such as click) to events of this class

            this.AddChild(itemUi);
        }
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
