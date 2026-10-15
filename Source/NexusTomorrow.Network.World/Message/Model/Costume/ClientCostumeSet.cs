using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ClientCostumeSet)]
[WireMessage(GameMessageOpcode.ClientCostumeSet, Direction.ClientToServer)]
public sealed partial class ClientCostumeSet : IReadable
{
    [UInt(32)] public int Index { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Index = reader.ReadInt();
    }
}
