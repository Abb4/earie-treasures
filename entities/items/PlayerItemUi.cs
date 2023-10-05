using Godot;
using utilities;

namespace Entities.Items;

public partial class PlayerItemUi : Container
{
	[Export] public TextureRect ItemImage;

    private PlayerItem playerItem;

	public override void _Ready()
	{
		ItemImage.AssertEditorPropertySet(nameof(ItemImage));
	}

	public void ConfigureUiFromItem(PlayerItem playerItem)
	{
		var itemImage = GD.Load<Texture2D>(playerItem.IconPath);

		ItemImage.Texture = itemImage;

        this.playerItem = playerItem;
	}
}
