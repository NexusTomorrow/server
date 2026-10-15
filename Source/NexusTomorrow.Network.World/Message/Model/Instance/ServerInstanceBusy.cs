using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

// Prevents resurrection interaction and causes InstanceSetBusy event
[Message(GameMessageOpcode.ServerInstanceBusy)]
[WireMessage(GameMessageOpcode.ServerInstanceBusy, Direction.ServerToClient)]
public sealed partial class ServerInstanceBusy : IWritable
{
    [UInt(1)] public bool Busy { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Busy);
    }
}
