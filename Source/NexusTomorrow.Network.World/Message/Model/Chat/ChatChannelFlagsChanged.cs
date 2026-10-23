using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Sets the player's standing in a chat channel.
/// </summary>
/// <remarks>
/// The client raises ChatFlag.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01B7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelFlagsChanged
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// The player's standing in the channel: bit 0 owner, bit 1 moderator.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
