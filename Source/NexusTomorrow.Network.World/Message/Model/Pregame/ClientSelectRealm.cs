using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ClientSelectRealm)]
[WireMessage(GameMessageOpcode.ClientSelectRealm, Direction.ClientToServer)]
public sealed partial class ClientSelectRealm : IReadable
{
    [UInt(32)] public uint RealmId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        RealmId = reader.ReadUInt();
    }
}
