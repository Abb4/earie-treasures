using System;
using Godot;
using utilities;

namespace Entities.Items;

public partial class PlayerItemUi : Control
{
    [Signal] public delegate void PlayerItemClickedEventHandler(PlayerItem playerItem);

    [Export] public TextureRect ItemImage;

    [Export] public Control CurrentStackAmountUiControl;
    [Export] public Label CurrentStackAmountUiLabel;

    public PlayerItem PlayerItem;

    public override void _Ready()
    {
        ItemImage.AssertEditorPropertySet(nameof(ItemImage));
    }

    public void ConfigureUiFromItem(PlayerItem playerItem)
    {
        var itemImage = GD.Load<Texture2D>(playerItem.GetItemIconPath());

        ItemImage.Texture = itemImage;

        PlayerItem = playerItem;

        ItemImage.GuiInput += OnPlayerItemClicked;
        playerItem.PlayerItemAttributeChanged += UpdateUiFromItem;

        UpdateUiFromItem(playerItem);
    }

    public void UpdateUiFromItem(PlayerItem playerItem)
    {
        if (playerItem.IsStackable && playerItem.CurrentStackAmount > 1)
        {
            CurrentStackAmountUiControl.Visible = true;
            CurrentStackAmountUiLabel.Text = playerItem.CurrentStackAmount.ToString();
        }
        else
        {
            CurrentStackAmountUiControl.Visible = false;
        }
    }

    private void OnPlayerItemClicked(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.IsPressed())
        {
            EmitSignal(nameof(PlayerItemClicked), PlayerItem);
        }
    }

    public override void _ExitTree()
    {
        PlayerItem.PlayerItemAttributeChanged -= UpdateUiFromItem;
        base._ExitTree();
    }
}
