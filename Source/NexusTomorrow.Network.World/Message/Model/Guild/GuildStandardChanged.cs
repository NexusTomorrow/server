using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's standard.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetStandard returns it; the client raises GuildStandard.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04D0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildStandardChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's standard, laid out as a holomark.
    /// </summary>
    public GuildHolomark Holomark { get; set; } = new();
}
