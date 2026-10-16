using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03A1, Direction.ClientToServer)]
public sealed partial class Client03A1
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
