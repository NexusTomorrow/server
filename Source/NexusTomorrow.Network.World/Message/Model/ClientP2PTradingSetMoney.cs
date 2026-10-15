using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingSetMoney)]
[WireMessage(GameMessageOpcode.ClientP2PTradingSetMoney, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingSetMoney : IReadable
{
    [UInt(32)] public ulong Credits { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Credits = reader.ReadUInt();
    }
}
