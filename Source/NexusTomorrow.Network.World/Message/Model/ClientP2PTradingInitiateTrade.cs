using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingInitiateTrade)]
[WireMessage(GameMessageOpcode.ClientP2PTradingInitiateTrade, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingInitiateTrade : IReadable
{
    [UInt(32)] public uint TargetUnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TargetUnitId = reader.ReadUInt();
    }
}
