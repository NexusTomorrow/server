using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client042B, Direction.ClientToServer)]
public sealed partial class Client042B
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }
}
