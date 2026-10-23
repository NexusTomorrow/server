using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Asks for a chat channel's members: the request a channel's RequestMembers call sends, and its slash command.
/// </summary>
/// <remarks>
/// The server answers with <see cref="ChatChannelMembers"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client01D2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelRequestMembers
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; private set; } = new();
}
