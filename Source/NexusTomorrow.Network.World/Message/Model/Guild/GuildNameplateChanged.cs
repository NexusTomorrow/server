using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets the guild the player shows on their nameplate.
/// </summary>
/// <remarks>
/// GuildTypeLib's IsNameplate is true for that guild alone; the client raises GuildNameplateChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04AC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildNameplateChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();
}
