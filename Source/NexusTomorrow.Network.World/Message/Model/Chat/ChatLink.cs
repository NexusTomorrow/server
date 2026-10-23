using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link in a chat line: a range of the text and what it points at.
/// </summary>
/// <remarks>
/// The client splits the text at each link's range and gives each range the keys its payload makes.
/// </remarks>
[WireStruct]
public sealed partial class ChatLink
{
    /// <summary>
    /// The link's kind, which picks its payload.
    /// </summary>
    [UInt(4)] public byte Type { get; set; }

    /// <summary>
    /// Where the link starts in the text.
    /// </summary>
    [UInt(16)] public ushort Start { get; set; }

    /// <summary>
    /// Where the link ends in the text.
    /// </summary>
    [UInt(16)] public ushort End { get; set; }

    /// <summary>
    /// What the link points at, by its kind.
    /// </summary>
    [Union(nameof(Type))] public IChatLinkPayload Payload { get; set; } = new ChatLinkFlag0();
}
