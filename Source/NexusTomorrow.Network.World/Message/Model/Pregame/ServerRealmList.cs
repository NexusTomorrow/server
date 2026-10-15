using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NetworkMessage = NexusTomorrow.Network.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on RealmSelect screen.
[Message(GameMessageOpcode.ServerRealmList)]
[WireMessage(GameMessageOpcode.ServerRealmList, Direction.ServerToClient)]
public sealed partial class ServerRealmList : IWritable
{
    [UInt(64)] public ulong Unused { get; set; }
    [Array(CountBits = 32)] public List<RealmInfo> Realms { get; set; } = [];
    [Array(CountBits = 32)] public List<NetworkMessage> Messages { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused);
        writer.Write(Realms.Count);
        Realms.ForEach(s => s.Write(writer));
        writer.Write(Messages.Count);
        Messages.ForEach(s => s.Write(writer));
    }
}
