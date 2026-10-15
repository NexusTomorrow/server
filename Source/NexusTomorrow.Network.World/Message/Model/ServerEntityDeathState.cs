using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityDeathState)]
[WireMessage(GameMessageOpcode.ServerEntityDeathState, Direction.ServerToClient)]
public sealed partial class ServerEntityDeathState : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool Dead { get; set; }
    // doesn't seem to be used by the client
    [UInt(5)] public byte Reason { get; set; }
    [UInt(32)] public uint RezHealth { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Dead);
        writer.Write(Reason, 5u);
        writer.Write(RezHealth);
    }
}
