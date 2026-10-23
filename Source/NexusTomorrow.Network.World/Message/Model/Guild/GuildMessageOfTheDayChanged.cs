using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's message of the day.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetMessageOfTheDay returns it; the client raises GuildMessageOfTheDay.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04A5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildMessageOfTheDayChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's message of the day.
    /// </summary>
    [WideString] public string MessageOfTheDay { get; set; } = "";
}
