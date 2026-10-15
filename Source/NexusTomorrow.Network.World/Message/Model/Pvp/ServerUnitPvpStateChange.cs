using NexusTomorrow.Game.Static.Pvp;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[Message(GameMessageOpcode.ServerUnitPvpStateChange)]
[WireMessage(GameMessageOpcode.ServerUnitPvpStateChange, Direction.ServerToClient)]
public sealed partial class ServerUnitPvpStateChange : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(3)] public PvpState State { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(State, 3u);
    }
}
