using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientRapidTransport, Direction.ClientToServer)]
public sealed partial class ClientRapidTransport
{
    [UInt(14)] public ushort TaxiNode { get; private set; }
    // not sure about the name - some increment
    [UInt(32)] public uint Time { get; private set; }
}
