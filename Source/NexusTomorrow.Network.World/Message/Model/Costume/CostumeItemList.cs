using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Sets the player's unlocked costume items.
/// </summary>
/// <remarks>
/// The client files each item under its slot and drops an id it does not know.
/// </remarks>
[WireMessage(GameMessageOpcode.Server025A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CostumeItemList
{
    /// <summary>
    /// The unlocked items, by Item2 id.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> ItemIds { get; set; } = [];
}
