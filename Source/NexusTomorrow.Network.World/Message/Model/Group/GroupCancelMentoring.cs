using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client043B, Direction.ClientToServer)]
public sealed partial class GroupCancelMentoring
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
