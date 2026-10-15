using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityStandState)]
public class ServerEntityStandState : IWritable
{
    public uint Guid { get; set; }
    public StandState StandState { get; set; }
    public uint EmoteId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(StandState, 4);
        writer.Write(EmoteId, 14);
    }
}
