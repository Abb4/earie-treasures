using Entities.Items;
using Entities.Player;
using Godot;
using utilities;

namespace Shared.Interactions;

public partial class KeyboardPlayerInteraction : Node
{
    [Export] public Area2D PlayerInteractionArea;

    [Export] public PlayerInventory PlayerInventory;
    [Export] public PlayerUi PlayerUi;

    public override void _Ready()
    {
        PlayerInteractionArea.AssertEditorPropertySet(nameof(PlayerInteractionArea));
        PlayerInventory.AssertEditorPropertySet(nameof(PlayerInventory));
        PlayerUi.AssertEditorPropertySet(nameof(PlayerUi));
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("interaction"))
        {
            foreach (Area2D overlappingArea in PlayerInteractionArea.GetOverlappingAreas())
            {
                if (overlappingArea is PlayerItemContainer lootedContainer)
                {
                    var interaction = Interaction<Area2D>.From(PlayerInteractionArea);

                    var interactionResult = lootedContainer.Interact(interaction);

                    if (interactionResult.IsFailure)
                    {
                        GD.Print($"Player attempted to interract with a lootable container but failed with the following error: {interactionResult.Error}");
                        continue;
                    }

                    PlayerItemContainerUi lootedContainerUi = interactionResult.Value;

                    PlayerInventory.RegisterLootedContainer(lootedContainer);

                    PlayerUi.DisplayLootedContainerUi(lootedContainerUi);

                    PlayerUi.DisplayLootedContainerUi(PlayerInventory.PlayerInventoryItemContainer.PlayerItemContainerUi);
                }
            }
        }
    }
}
