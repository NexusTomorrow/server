using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0412, Direction.ClientToServer)]
public sealed partial class GroupSetInstanceDifficulty
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
}
