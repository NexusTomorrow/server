using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Client059C, Direction.ClientToServer)]
public sealed partial class Client059C
{
    [UInt(14)] public uint Unknown0 { get; private set; }
}
