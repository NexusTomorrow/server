using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.Client03CD, Direction.ClientToServer)]
public sealed partial class Client03CD
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
