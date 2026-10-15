using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerEntityGroupAssociation)]
    public class ServerEntityGroupAssociation : IWritable
    {
        public uint UnitId { get; set; }
        public ulong GroupId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(GroupId);
        }
    }
}
