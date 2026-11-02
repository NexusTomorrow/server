using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

/// <summary>
/// An intercom channel the client has left; the client clears the channel and its members.
/// </summary>
[WireMessage(GameMessageOpcode.Server054A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerICCommChannelLeft
{
    /// <summary>
    /// The channel's id, looked up in the client's intercom table.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ChannelId { get; set; }
}
