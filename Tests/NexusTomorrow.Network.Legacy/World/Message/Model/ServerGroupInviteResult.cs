using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupInviteResult)]
public class ServerGroupInviteResult : IWritable
{
    public ulong GroupId { get; set; }
    public string Name { get; set; }
    public GroupInviteResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.WriteStringWide(Name);
        writer.Write(Result, 5);
    }
}
