using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerGroupRequestJoinResponse)]
    public class ServerGroupRequestJoinResponse : IWritable
    {
        public ulong GroupId { get; set; }

        public GroupMember MemberInfo { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(GroupId);
            MemberInfo.Write(writer);
        }
    }
}
