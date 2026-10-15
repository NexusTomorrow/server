using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info
{
    // One, some, or all of the parameters set. The strings will be concatenated.
    [Message(GameMessageOpcode.ServerPrerequisiteFailure)]
    public class ServerPrerequisiteFailure : IWritable
    {
        public uint PrerequisiteId { get; set; }
        public uint PrerequisiteTypeId { get; set; }
        public uint LocalizedStringId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PrerequisiteId, 17u);
            writer.Write(PrerequisiteTypeId, 9u);
            writer.Write(LocalizedStringId, 15u);
        }
    }
}
