using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Asks to join a chat channel: the request ChatSystemLib.JoinChannel sends, and its slash command.
/// </summary>
[WireMessage(GameMessageOpcode.Client01BA, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatJoinChannel
{
    /// <summary>
    /// The channel's type, an id in the ChatChannel table; 18 for a custom channel.
    /// </summary>
    [UInt(14)] public ushort ChannelType { get; private set; }

    /// <summary>
    /// A custom channel's name.
    /// </summary>
    [WideString] public string Name { get; private set; } = "";

    /// <summary>
    /// A custom channel's password.
    /// </summary>
    [WideString] public string Password { get; private set; } = "";

    /// <summary>
    /// The number the client gives the channel, which <see cref="ChatChannelJoined.Order"/> sends back.
    /// </summary>
    [UInt(32)] public uint Order { get; private set; }
}
