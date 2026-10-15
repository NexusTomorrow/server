using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupSendReadyCheck, Direction.ServerToClient)]
public sealed partial class ServerGroupSendReadyCheck
{
    [UInt(64)] public ulong GroupId { get; set; }

    public Identity Invoker { get; set; }

    [WideString] public string Message { get; set; }
}
