using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Reports one result for several costume items, as <see cref="CostumeItemResult"/> does for one.
/// </summary>
[WireMessage(GameMessageOpcode.Server0258, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CostumeItemResultList
{
    /// <summary>
    /// The result, for every item.
    /// </summary>
    [UInt(32)] public CostumeUnlockResult Result { get; set; }

    /// <summary>
    /// The items, by Item2 id.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> ItemIds { get; set; } = [];
}
