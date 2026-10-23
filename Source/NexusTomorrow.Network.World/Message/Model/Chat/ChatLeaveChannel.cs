using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Leaves a chat channel: the request a channel's Leave call sends, and its slash command.
/// </summary>
[WireMessage(GameMessageOpcode.Client01BE, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatLeaveChannel
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; private set; } = new();
}
