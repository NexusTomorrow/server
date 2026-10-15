using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerResurrectionUpdate)]
public class ServerResurrectionUpdate : IWritable
{
    public ResurrectionType ShowRezFlags { get; set; } // 8
    public bool HasCasterRezRequest { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ShowRezFlags, 8u);
        writer.Write(HasCasterRezRequest);
    }
}
