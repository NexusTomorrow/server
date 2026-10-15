using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

// One, some, or all of the parameters set. The strings will be concatenated.
[Message(GameMessageOpcode.ServerPrerequisiteFailure)]
[WireMessage(GameMessageOpcode.ServerPrerequisiteFailure, Direction.ServerToClient)]
public sealed partial class ServerPrerequisiteFailure : IWritable
{
    [UInt(17)] public uint PrerequisiteId { get; set; }
    [UInt(9)] public uint PrerequisiteTypeId { get; set; }
    [UInt(15)] public uint LocalizedStringId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PrerequisiteId, 17u);
        writer.Write(PrerequisiteTypeId, 9u);
        writer.Write(LocalizedStringId, 15u);
    }
}
