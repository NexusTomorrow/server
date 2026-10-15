using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ServerRealmInfoResponse, Direction.ServerToClient)]
public sealed partial class ServerRealmInfoResponse
{
    [UInt(32)] public uint RealmId { get; set; }
    [WideString] public string RealmName { get; set; }
}
