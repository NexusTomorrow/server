using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ClientRealmInfoRequest)]
[WireMessage(GameMessageOpcode.ClientRealmInfoRequest, Direction.ClientToServer)]
public sealed partial class ClientRealmInfoRequest : IReadable
{
    [UInt(14)] public uint RealmId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        RealmId = reader.ReadUInt(14u);
    }
}
