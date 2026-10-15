using NexusTomorrow.Network.Message;
using NexusTomorrow.Game.Static.Group;

namespace NexusTomorrow.Network.World.Message.Model.Shared
{
    public class GroupMember : IWritable
    {
        public Identity MemberIdentity { get; set; } = new();
        public GroupMemberInfoFlags Flags { get; set; }
        public GroupCharacter Member { get; set; }
        public uint GroupIndex { get; set; }

        public void Write(GamePacketWriter writer)
        {
            MemberIdentity.Write(writer);
            writer.Write(Flags, 32);
            Member.Write(writer);
            writer.Write(GroupIndex);
        }
    }
}
