using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Changes the guild's holomark: the request GuildTypeLib's Modify sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client04A6, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildModifyHolomark
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();

    /// <summary>
    /// The new holomark.
    /// </summary>
    public GuildHolomark Holomark { get; private set; } = new();
}
