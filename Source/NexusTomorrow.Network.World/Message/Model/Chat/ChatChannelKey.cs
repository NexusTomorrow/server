using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A chat channel, by its type and its id within that type.
/// </summary>
[WireStruct]
public sealed partial class ChatChannelKey
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
