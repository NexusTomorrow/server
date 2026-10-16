using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client045E, Direction.ClientToServer)]
public sealed partial class Client045E
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
