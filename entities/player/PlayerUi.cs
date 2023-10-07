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

	public void DisplayLootableContainerUi(LootableContainerUi containerUi)
	{
		MainDisplayContainer.AddChild(containerUi);
		this.Show();
	}
}
