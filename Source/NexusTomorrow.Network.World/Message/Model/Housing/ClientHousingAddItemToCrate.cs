using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client04E7, Direction.ClientToServer)]
public sealed partial class Client04E7
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
