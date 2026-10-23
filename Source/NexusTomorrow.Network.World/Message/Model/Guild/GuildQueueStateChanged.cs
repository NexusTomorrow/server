using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's queue state.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetQueueState returns it; on a change the client raises GuildQueueStateChanged with the old and the new state.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04C3, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildQueueStateChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's queue state, a GuildQueueState value.
    /// </summary>
    [UInt(3)] public byte State { get; set; }
}
