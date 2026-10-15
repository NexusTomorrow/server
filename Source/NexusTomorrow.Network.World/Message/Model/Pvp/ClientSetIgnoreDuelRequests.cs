using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.ClientSetIgnoreDuelRequests, Direction.ClientToServer)]
public sealed partial class ClientSetIgnoreDuelRequests
{
    [UInt(1)] public bool Ignore { get; private set; }
}
