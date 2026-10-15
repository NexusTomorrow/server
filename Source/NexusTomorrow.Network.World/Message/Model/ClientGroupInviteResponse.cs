using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientGroupInviteResponse
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(1)] public bool Response { get; set; }
    [UInt(32)] public uint Unk1 { get; set; }
}
