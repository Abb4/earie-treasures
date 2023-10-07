using Godot;
using utilities;

namespace Entities.Items;

public partial class PlayerItemUi : Container
{
    [Signal] public delegate void PlayerItemClickedEventHandler(PlayerItem playerItem);

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

        ItemImage.GuiInput += OnPlayerItemClicked;
    }

    private void OnPlayerItemClicked(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.IsPressed())
        {
            EmitSignal(nameof(PlayerItemClicked), PlayerItem);
        }
    }
}
