using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupFlagsChanged)]
public class ClientGroupFlagsChanged : IReadable
{
    public ulong GroupId { get; set; }

    public GroupFlags NewFlags { get; set; }

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        NewFlags = reader.ReadEnum<GroupFlags>(32u);
    }
}
