using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Takes the player out of a chat channel.
/// </summary>
/// <remarks>
/// For a channel the player may leave, the client raises ChatLeave and drops the channel.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01BF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelLeft
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// Why, a ChatChannelLeaveReason value: 1 kicked, 2 banned.
    /// </summary>
    [UInt(2)] public byte Reason { get; set; }
}
