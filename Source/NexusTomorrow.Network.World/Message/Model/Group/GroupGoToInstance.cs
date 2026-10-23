using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0411, Direction.ClientToServer)]
public sealed partial class GroupGoToInstance
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
