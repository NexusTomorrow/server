using NexusTomorrow.Game.Static.Pvp;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[Message(GameMessageOpcode.ServerUnitPvpStateChange)]
public class ServerUnitPvpStateChange : IWritable
{
    public uint UnitId { get; set; }
    public PvpState State { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(State, 3u);
    }
}
