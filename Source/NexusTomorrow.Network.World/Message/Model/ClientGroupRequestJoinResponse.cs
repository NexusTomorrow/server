using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupRequestJoinResponse, Direction.ClientToServer)]
public sealed partial class ClientGroupRequestJoinResponse
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(1)] public bool AcceptedRequest { get; set; }

    [WideString] public string InviteeName { get; set; }

    [UInt(1)] public bool Unknown3 { get; set; }
}
