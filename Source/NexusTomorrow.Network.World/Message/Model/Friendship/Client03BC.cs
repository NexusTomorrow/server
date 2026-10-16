using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03BC, Direction.ClientToServer)]
public sealed partial class Client03BC
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(3)] public byte Unknown1 { get; private set; }
}
