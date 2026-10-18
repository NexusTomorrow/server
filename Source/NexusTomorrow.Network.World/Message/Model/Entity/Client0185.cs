using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client0185, Direction.ClientToServer)]
public sealed partial class Client0185
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
