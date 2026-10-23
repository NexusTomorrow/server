using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Passes a chat channel's ownership to another player: the request a channel's PassOwner call sends, and its slash command.
/// </summary>
[WireMessage(GameMessageOpcode.Client01CF, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelPassOwner
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; private set; } = new();

    /// <summary>
    /// The new owner, by name.
    /// </summary>
    [WideString] public string PlayerName { get; private set; } = "";
}
