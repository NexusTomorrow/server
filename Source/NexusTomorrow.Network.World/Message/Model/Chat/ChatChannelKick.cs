using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Kicks a player from a chat channel: the request a channel's Kick call sends, and its slash command.
/// </summary>
/// <remarks>
/// The client sends it only for a name that passes its name check.
/// </remarks>
[WireMessage(GameMessageOpcode.Client01BD, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelKick
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; private set; } = new();

    /// <summary>
    /// The player to kick, by name.
    /// </summary>
    [WideString] public string PlayerName { get; private set; } = "";
}
