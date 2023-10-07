using CSharpFunctionalExtensions;
using Godot;
using Godot.Collections;
using Shared.Interactions;
using utilities;

namespace Entities.Items;

public partial class PlayerItemContainer : Area2D, IInteractible<Area2D, PlayerItemContainerUi>
{
    [Signal] public delegate void PlayerItemClickedEventHandler(PlayerItem playerItem, PlayerItemContainer playerItemContainer);

    [Export] public PackedScene PlayerItemContainerUiScene;
    private PlayerItemContainerUi PlayerItemContainerUi;

    [Export] public Array<PlayerItem> containerItems = new();

    public int MaximumContainerCapacity = 9;

    public override void _Ready()
    {
        PlayerItemContainerUiScene.AssertEditorPropertySet(nameof(PlayerItemContainerUiScene));

        PlayerItemContainerUi = Result.Try<PlayerItemContainerUi, GameError>(
            () => PlayerItemContainerUiScene.Instantiate<PlayerItemContainerUi>(),
            errorHandler: NodeError.From)
            .Value;

        PlayerItemContainerUi.ConfigureUiFromItems(containerItems);

        PlayerItemContainerUi.PadContainerUiWithEmptyItemSlots(MaximumContainerCapacity - containerItems.Count);

        PlayerItemContainerUi.PlayerItemClicked += OnPlayerItemClicked;

        PlayerItemContainerUi.Show();
    }

    private void OnPlayerItemClicked(PlayerItem playerItem)
    {
        EmitSignal(nameof(PlayerItemClicked), playerItem, this);
    }

    public Result<PlayerItemContainerUi, GameError> Interact(Interaction<Area2D> interaction)
    {
        return PlayerItemContainerUi;
    }
}
