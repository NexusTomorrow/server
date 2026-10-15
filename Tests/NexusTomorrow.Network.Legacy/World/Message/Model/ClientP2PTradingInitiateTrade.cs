using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingInitiateTrade)]
public class ClientP2PTradingInitiateTrade : IReadable
{
    public uint TargetUnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TargetUnitId = reader.ReadUInt();
    }
}
