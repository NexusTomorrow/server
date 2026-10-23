using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Adds a chat channel the player has joined.
/// </summary>
/// <remarks>
/// The client takes the channel only when its ChatChannel row allows it, names a zone channel after the zone, and raises ChatJoin.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01BC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelJoined
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// A custom channel's name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// The player's standing in a custom channel, by bit as <see cref="ChatChannelFlagsChanged.Flags"/>.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// The channel's number, as <see cref="ChatJoinChannel.Order"/> asked for it.
    /// </summary>
    [UInt(32)] public uint Order { get; set; }
}
