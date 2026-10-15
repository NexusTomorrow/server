using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Message is used to change the interval at which the client sends entity updates.
// This can be sent at any time but will get overwritten by the ServiceInstanceSettings message.

[WireMessage(GameMessageOpcode.ServerClientEntityUpdateInterval, Direction.ServerToClient)]
public sealed partial class ServerClientEntityUpdateInterval
{
    [UInt(32)] public uint ClientEntitySendUpdateInterval { get; set; }
}
