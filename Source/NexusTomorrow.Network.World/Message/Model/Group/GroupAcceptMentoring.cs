using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client043E, Direction.ClientToServer)]
public sealed partial class GroupAcceptMentoring
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
}
