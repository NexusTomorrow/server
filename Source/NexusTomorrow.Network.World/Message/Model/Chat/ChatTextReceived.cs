using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// The text block of a chat line the client receives, which it registers as a message of its own; nothing sends it alone.
/// </summary>
[WireMessage(GameMessageOpcode.Message01C1, Direction.Both)]
[Verified(Rung.Purpose)]
[Dormant]
public sealed partial class ChatTextReceived
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
    /// True when the line comes from the other faction.
    /// </summary>
    [UInt(1)] public bool CrossFaction { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(16)] public ushort Unknown3 { get; set; }
}
