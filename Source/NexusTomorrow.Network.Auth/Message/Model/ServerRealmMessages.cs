using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NetworkMessage = NexusTomorrow.Network.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[Message(GameMessageOpcode.ServerRealmMessages)]
[WireMessage(GameMessageOpcode.ServerRealmMessages, Direction.ServerToClient)]
public sealed partial class ServerRealmMessages : IWritable
{
    [Array(CountBits = 32)] public List<NetworkMessage> Messages { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Messages.Count);
        Messages.ForEach(m => m.Write(writer));
    }
}
