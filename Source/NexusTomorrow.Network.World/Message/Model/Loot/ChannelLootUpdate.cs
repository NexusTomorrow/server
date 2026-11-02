using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

/// <summary>
/// Announces loot to a channel.
/// </summary>
/// <remarks>
/// The client builds an item from the id and count and fires the channel loot event.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00DD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChannelLootUpdate
{
    /// <summary>
    /// The loot's type or flags.
    /// </summary>
    [VerifiedMember, UInt(4)] public byte LootType { get; set; }

    /// <summary>
    /// The loot's count; when 0 the client ignores the message.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong Count { get; set; }

    /// <summary>
    /// The loot item, by item id.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ItemId { get; set; }
}