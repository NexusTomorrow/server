using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ClientRealmInfoRequest, Direction.ClientToServer)]
public sealed partial class ClientRealmInfoRequest
{
    [UInt(14)] public uint RealmId { get; private set; }
}
