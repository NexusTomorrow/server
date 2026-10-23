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

// Dormant and read to Fields, as a message the client reads only as another message's record may be, so its rung alone would let it go
// and only the dormant check refuses it.
[WireMessage((GameMessageOpcode)0xFE03, Direction.ServerToClient)]
[Verified(Rung.Fields)]
[Dormant]
public sealed partial class GateDormantMessage
{
    [UInt(8)] public byte Known { get; set; }
}

// Read to Fields and holding a dormant message as a member: the dormant mark is the record type's, and the send gate judges only the
// message sent.
[WireMessage((GameMessageOpcode)0xFE04, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GateHolderMessage
{
    [UInt(8)] public byte Count { get; set; }
    public GateDormantMessage Record { get; set; } = new();
}
