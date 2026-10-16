using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Message063A, Direction.ClientToServer)]
public sealed partial class Message063A
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
    [UInt(15)] public ushort Unknown1 { get; private set; }
}
