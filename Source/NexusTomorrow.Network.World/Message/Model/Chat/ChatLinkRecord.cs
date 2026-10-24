using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A chat link, which the client registers as a message of its own; nothing sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Message01B8, Direction.Both)]
[Verified(Rung.Purpose)]
[Dormant]
public sealed partial class ChatLinkRecord
{
    /// <summary>
    /// The link.
    /// </summary>
    public ChatLink Link { get; set; } = new();
}
