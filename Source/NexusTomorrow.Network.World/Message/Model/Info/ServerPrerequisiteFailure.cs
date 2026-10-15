using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

// One, some, or all of the parameters set. The strings will be concatenated.
[WireMessage(GameMessageOpcode.ServerPrerequisiteFailure, Direction.ServerToClient)]
public sealed partial class ServerPrerequisiteFailure
{
    [UInt(17)] public uint PrerequisiteId { get; set; }
    [UInt(9)] public uint PrerequisiteTypeId { get; set; }
    [UInt(15)] public uint LocalizedStringId { get; set; }
}
