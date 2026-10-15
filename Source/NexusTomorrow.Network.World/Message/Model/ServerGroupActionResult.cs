using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupActionResult)]
public class ServerGroupActionResult : IWritable
{
    public ulong GroupId { get; set; }
    public Identity Identity { get; set; }
    public GroupActionResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        Identity.Write(writer);
        writer.Write(Result, 32);
    }
}
