using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// The text block of a chat line the client sends, which it registers as a message of its own; nothing sends it alone.
/// </summary>
[WireMessage(GameMessageOpcode.Message01C4, Direction.Both)]
[Verified(Rung.Purpose)]
[Dormant]
public sealed partial class ChatTextSent
{
    /// <summary>
    /// The line's text.
    /// </summary>
    [WideString] public string Text { get; set; } = "";

    /// <summary>
    /// The links in the text.
    /// </summary>
    [Array(CountBits = 5)] public List<ChatLink> Links { get; } = [];

    /// <summary>
    /// The line, by the slot the client files it under until the server answers.
    /// </summary>
    [UInt(16)] public ushort ChatId { get; set; }
}
