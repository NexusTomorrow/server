using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupRequestJoin)]
[WireMessage(GameMessageOpcode.ClientGroupRequestJoin, Direction.ClientToServer)]
public sealed partial class ClientGroupRequestJoin : IReadable
{
    [WideString] public string Name { get; set; }
    [WideString] public string RealmName { get; set; }

    public void Read(GamePacketReader reader)
    {
        Name = reader.ReadWideString();
        RealmName = reader.ReadWideString();
    }
}
