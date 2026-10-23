using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A chat channel key, which the client registers as a message of its own; nothing sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Message01B6, Direction.Both)]
[Verified(Rung.Fields)]
[Dormant]
public sealed partial class ChatChannelKeyRecord
{
    /// <summary>
    /// The channel's type, an id in the ChatChannel table.
    /// </summary>
    [UInt(14)] public ushort Type { get; set; }

    /// <summary>
    /// The channel's id among the channels of its type.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }
}
