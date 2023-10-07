using Entities.Items;
using Entities.Player;
using Godot;
using utilities;

namespace Shared.Interactions;

public partial class KeyboardPlayerInteraction : Node
{
    [Export] public Area2D PlayerInteractionArea;

    [Export] public PlayerUi PlayerUi;

    public override void _Ready()
    {
        PlayerInteractionArea.AssertEditorPropertySet(nameof(PlayerInteractionArea));
        PlayerUi.AssertEditorPropertySet(nameof(PlayerUi));
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("interaction"))
        {
            foreach (Area2D overlappingArea in PlayerInteractionArea.GetOverlappingAreas())
            {
                if (overlappingArea is PlayerItemContainer playerItemContainer)
                {
                    var interaction = Interaction<Area2D>.From(PlayerInteractionArea);

                    var interactionResult = playerItemContainer.Interact(interaction);

                    if (interactionResult.IsFailure)
                    {
                        GD.Print($"Player attempted to interract with a lootable container but failed with the following error: {interactionResult.Error}");
                        continue;
                    }

                    PlayerItemContainerUi containerUi = interactionResult.Value;

                    PlayerUi.DisplayLootedContainerUi(containerUi);
                }
            }
        }
    }
}
