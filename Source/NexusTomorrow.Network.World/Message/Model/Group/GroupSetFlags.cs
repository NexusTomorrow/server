using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0406, Direction.ClientToServer)]
public sealed partial class GroupSetFlags
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }
}
