using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupSendReadyCheck, Direction.ClientToServer)]
public sealed partial class ClientGroupSendReadyCheck
{
    [UInt(64)] public ulong GroupId { get; set; }

    [WideString] public string Message { get; set; }
}
