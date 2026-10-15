using NexusTomorrow.Network.Message;
using NexusTomorrow.Game.Static.Group;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerGroupLeave)]
    public class ServerGroupLeave : IWritable
    {
        public ulong GroupId { get; set; }
        public RemoveReason Reason { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(GroupId);
            writer.Write(Reason, 4u);
        }
    }
}
