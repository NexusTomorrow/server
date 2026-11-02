using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

/// <summary>
/// Reports the result of an in-character comm channel message.
/// </summary>
/// <remarks>
/// The client replies ClientICCommChannelNotJoined when the channel is not joined; otherwise it applies the message to the channel members.
/// </remarks>
[WireMessage(GameMessageOpcode.Server054C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ICCommMessageResult
{
    /// <summary>
    /// The comm channel, by channel id.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ChannelId { get; set; }

    /// <summary>
    /// The message, by message id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MessageId { get; set; }

    /// <summary>
    /// The message's flags.
    /// </summary>
    [VerifiedMember, UInt(4)] public byte Flags { get; set; }
}