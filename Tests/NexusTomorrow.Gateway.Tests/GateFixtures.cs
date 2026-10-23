using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Gateway.Tests;

// Our own fixture messages for the send gate, at opcodes no client message uses.
[WireMessage((GameMessageOpcode)0xFE01, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GatePurposeMessage
{
    [VerifiedMember, UInt(8)] public byte Known { get; set; }
    [UInt(8)] public byte Unread { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE02, Direction.ServerToClient)]
public sealed partial class GateUnreadMessage
{
    [VerifiedMember, UInt(8)] public byte Known { get; set; }
}
