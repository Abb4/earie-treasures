using Entities.Items;
using Godot;
using utilities;

namespace Entities.Player;
public partial class PlayerUi : PanelContainer
{
    [Export] public Container MainDisplayContainer;

    public override void _Ready()
    {
        MainDisplayContainer.AssertEditorPropertySet(nameof(MainDisplayContainer));
    }

    public void DisplayLootedContainerUi(PlayerItemContainerUi lootedContainerUi)
    {
        MainDisplayContainer.AddChild(lootedContainerUi);
        this.Show();
    }
}
