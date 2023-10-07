using Godot;
using utilities;

namespace Entities.Items;

public partial class PlayerItemUi : Container
{
    [Signal] public delegate void ItemClickedEventHandler(PlayerItem playerItem, PlayerItemUi playerItemUi);

    [Export] public TextureRect ItemImage;

    public PlayerItem PlayerItem;

    public override void _Ready()
    {
        ItemImage.AssertEditorPropertySet(nameof(ItemImage));
    }

    public void ConfigureUiFromItem(PlayerItem playerItem)
    {
        var itemImage = GD.Load<Texture2D>(playerItem.IconPath);

        ItemImage.Texture = itemImage;

        PlayerItem = playerItem;

        ItemImage.GuiInput += OnItemClicked;
    }

    private void OnItemClicked(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.IsPressed())
        {
        }
    }
}
