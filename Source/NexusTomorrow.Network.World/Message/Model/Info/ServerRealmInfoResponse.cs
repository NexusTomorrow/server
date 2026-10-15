using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerRealmInfoResponse)]
[WireMessage(GameMessageOpcode.ServerRealmInfoResponse, Direction.ServerToClient)]
public sealed partial class ServerRealmInfoResponse : IWritable
{
    [UInt(32)] public uint RealmId { get; set; }
    [WideString] public string RealmName { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RealmId);
        writer.WriteStringWide(RealmName);
    }
}
