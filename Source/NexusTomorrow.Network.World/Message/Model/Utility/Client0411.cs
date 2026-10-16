using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Client0411, Direction.ClientToServer)]
public sealed partial class Client0411
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
