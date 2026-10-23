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

// Dormant and read to Fields, a pair the registry marker rules refuse among the declared messages, so its rung alone would let it go
// and only the dormant check refuses it.
[WireMessage((GameMessageOpcode)0xFE03, Direction.ServerToClient)]
[Verified(Rung.Fields)]
[Dormant]
public sealed partial class GateDormantMessage
{
    [UInt(8)] public byte Known { get; set; }
}
