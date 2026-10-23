using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Changes the stack count of an item in a guild bank tab.
/// </summary>
[WireMessage(GameMessageOpcode.Server0476, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildBankItemStackCountChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// The item's new stack count.
    /// </summary>
    [UInt(32)] public uint StackCount { get; set; }
}
