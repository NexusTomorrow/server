using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's info message.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetInfoMessage returns it; the client raises GuildInfoMessage.
/// </remarks>
[WireMessage(GameMessageOpcode.Server048F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildInfoMessageChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's info message.
    /// </summary>
    [WideString] public string InfoMessage { get; set; } = "";
}
