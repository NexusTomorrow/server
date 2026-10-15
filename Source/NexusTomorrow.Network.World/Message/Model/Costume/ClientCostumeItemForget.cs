using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ClientCostumeItemForget)]
[WireMessage(GameMessageOpcode.ClientCostumeItemForget, Direction.ClientToServer)]
public sealed partial class ClientCostumeItemForget : IReadable
{
    [UInt(18)] public uint Item2Id { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Item2Id = reader.ReadUInt(18u);
    }
}
