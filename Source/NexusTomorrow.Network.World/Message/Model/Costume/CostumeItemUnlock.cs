using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Unlocks the item at an inventory location as a costume item.
/// </summary>
/// <remarks>
/// The client sends it only for an item of a costume slot's kind that is not already unlocked, and only when the player can
/// pay the price.
/// </remarks>
[WireMessage(GameMessageOpcode.Client090F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CostumeItemUnlock
{
    /// <summary>
    /// Where the item is.
    /// </summary>
    public ItemLocation Location { get; } = new();
}
