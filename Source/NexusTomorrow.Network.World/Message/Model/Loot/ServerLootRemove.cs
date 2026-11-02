using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

/// <summary>
/// Removes a loot entry; the client drops the loot with this id.
/// </summary>
[WireMessage(GameMessageOpcode.Server08A1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerLootRemove
{
    /// <summary>
    /// The id of the loot entry to remove.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint LootId { get; set; }
}
