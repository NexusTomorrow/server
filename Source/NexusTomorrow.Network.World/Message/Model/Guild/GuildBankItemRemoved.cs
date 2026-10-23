using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Removes an item from a guild bank tab.
/// </summary>
/// <remarks>
/// The client clears the item's slot and raises GuildBankItem.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0470, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildBankItemRemoved
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; private set; }
}
