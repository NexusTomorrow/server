using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Renames a guild.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetName returns the name; the client raises GuildName.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04AF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRenamed
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's new name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";
}
