using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Sends a chat line on a channel: the request a channel's Send call and the chat input send, for every channel but whispers.
/// </summary>
/// <remarks>
/// The server answers by the line's slot, with <see cref="ChatSendAccepted"/> or <see cref="ChatSendResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client01C3, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ChatSend
{
    /// <summary>
    /// The channel.
    /// </summary>
    [VerifiedMember] public ChatChannelKey Channel { get; private set; } = new();

    /// <summary>
    /// The line's text.
    /// </summary>
    [VerifiedMember, WideString] public string Text { get; private set; } = "";

    /// <summary>
    /// The links in the text.
    /// </summary>
    [Array(CountBits = 5)] public List<ChatLink> Links { get; } = [];

    /// <summary>
    /// The client's id for the pending line; <see cref="ChatSendAccepted"/> names the line it accepts by it.
    /// </summary>
    [VerifiedMember, UInt(16)] public ushort ChatId { get; private set; }
}
