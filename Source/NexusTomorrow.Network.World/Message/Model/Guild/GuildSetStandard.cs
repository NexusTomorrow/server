using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets the guild's standard: the request GuildTypeLib's SetStandard sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client04CF, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class GuildSetStandard
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();

    /// <summary>
    /// The standard, laid out as a holomark.
    /// </summary>
    public GuildHolomark Holomark { get; private set; } = new();
}
