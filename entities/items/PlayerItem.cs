using System;
using Godot;

namespace Entities.Items;

public enum PlayerItemType
{
    GOLD
}

public partial class PlayerItem : Node
{
    [Export] public PlayerItemType ItemType = PlayerItemType.GOLD;

    [Export] public bool IsStackable = true;
    [Export] public int CurrentStackAmount = 20;
    [Export] public int MaximumStackAmount = 1000;

    [Signal] public delegate void PlayerItemAttributeChangedEventHandler(PlayerItem playerItem);

    public string GetItemIconPath()
    {
        return this.ItemType switch
        {
            PlayerItemType.GOLD => "res://assets/icons/gold_coins.svg",
            _ => throw new ArgumentException($"Unhandled value {this.ItemType} for {typeof(PlayerItemType).Name}"),
        };
    }

    private void OnPlayerItemAttributeChanged()
    {
        EmitSignal(nameof(PlayerItemAttributeChanged), this);
    }

    public PlayerItem Clone()
    {
        return new PlayerItem()
        {
            ItemType = this.ItemType,
            IsStackable = this.IsStackable,
            CurrentStackAmount = this.CurrentStackAmount,
            MaximumStackAmount = this.MaximumStackAmount,
        };
    }

    public bool TryAddToStackWithoutRemainder(int amount, out PlayerItem remainder)
    {
        int availableStackAmount = MaximumStackAmount - CurrentStackAmount;

        if (availableStackAmount >= amount)
        {
            CurrentStackAmount += amount;
            OnPlayerItemAttributeChanged();

            remainder = null;
            return true;
        }
        else
        {
            int remainderAmount = amount - availableStackAmount;

            remainder = this.Clone();

            remainder.CurrentStackAmount = remainderAmount;

            CurrentStackAmount = MaximumStackAmount;

            OnPlayerItemAttributeChanged();

            return false;
        }

    }
}